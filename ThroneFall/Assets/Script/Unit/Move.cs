using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using static GameEnums;


public class Move : MonoBehaviour,
    IMoveDestProvider,
    IEventHandlerProvider<InputInfo>
{
    private bool _isInitliazed = false;
    [FormerlySerializedAs("Dest")] public Vector3 dest;
    [SerializeField]private NavMeshAgent _agent;
    public bool _isWantMove = true;
    private bool _hasArrivedLastFrame = false;
    private float _unitSpeed;
    private IState _objectState;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Initialize(float unitSpeed, IState objectState)
    {
        _objectState = objectState;
        dest = transform.position;
        _unitSpeed = unitSpeed;
        _agent.speed = _unitSpeed;
        _isInitliazed = true;
    }

    bool HasArrived()
    {
        if (_agent == null) return true;
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                return true;
        }
        return false;
    }
    private void Update()
    {
        if (this == null || _agent == null) return;
        if (!_isInitliazed) return;
        if (_objectState.IsDead)
        {
            StopMove();
            return;
        }
        bool isArrived = HasArrived();
    
        if (!_hasArrivedLastFrame && isArrived)
        {
            // 도착한 "순간"에만 한 번 호출됨
            _objectState.StopMove();
        }

        _hasArrivedLastFrame = isArrived;
    }
    public void StopMove()
    {
        _agent.isStopped = true; // 이동 정지
        _agent.ResetPath();      // 현재 경로 삭제
        _objectState.StopMove();
    }
    

    public void SimpleMove()
    {
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenDir = (mousePos - playerScreenPos).normalized;
        Vector3 worldDir = Camera.main.transform.TransformDirection(new Vector3(screenDir.x, 0, screenDir.y));
        worldDir.y = 0f;
        worldDir.Normalize();

        Quaternion lookRot = Quaternion.LookRotation(worldDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, _agent.angularSpeed * Time.deltaTime);
        
        _agent.Move(worldDir * _agent.speed * Time.deltaTime);
        _objectState.StartMove();
    }

    public void OnChangeDest()
    {
        if (!_isWantMove) return;
        if (!_agent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent가 NavMesh 위에 있지 않습니다!");
            return;
        }
       
        if(_objectState.IsDead)
        {
            StopMove();
            return;
        }
        NavMeshHit hit;
        if (NavMesh.SamplePosition(dest, out hit, 100.0f, NavMesh.AllAreas))
        {
            dest = hit.position; // 가장 가까운 NavMesh 위치로 이동
            _isWantMove = true;
            _objectState.StopMove();
            _agent.stoppingDistance = 1f; // 목표와의 최소 거리 설정
            _agent.isStopped = false;
            _agent.SetDestination(dest);
            _objectState.StartMove();
        }
        else
        {
            StopMove();
        }
    }

    public void SetMoveDest(Vector3 dest)
    {
        this.dest = dest;
        OnChangeDest();
    }

    public void NotifyStopMove()
    {
        StopMove();
    }

    public void NotifyPauseMove()
    {
        _isWantMove = false;
        _agent.speed = 0;
    }
    public void NotifyResumeMove()
    {
        if (_isWantMove == false)
        {
            _isWantMove = true;
            _agent.speed = _unitSpeed;
        }
    }
    
    public void InputCallbackEvent(InputInfo info)
    {
        if (info.actionName == "Move" && info.inputType == EInputType.Press)
        {
            SimpleMove();
        }

        if (info.actionName == "Move" && info.inputType == EInputType.Up)
        {
            StopMove();
        }
    }

    public void CallbackEvent(InputInfo info)
    {
        InputCallbackEvent(info);
    }

    private void OnDestroy()
    {
        _unRegistCallback?.Invoke();
    }

    Action _unRegistCallback;
    public void SetEventUnRegistCallback(Action callback)
    {
        _unRegistCallback = callback;
    }
}
