using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static GameEnums;

public class PlayerColliderEventHandler : MonoBehaviour,
    IInitialize,
    IEventHandlerProvider<InputInfo>
{
    [SerializeField] IInteractionAble _currentSelectable;
    private bool _isInteractPressing = false;
    private CombatStartInputHandler _combatStartInputHandler;
    [SerializeField] private GameObject callUpCircle;
    [SerializeField]private List<Ally> triggerInAllys = new();
    private bool _isCallUp = false;

    public void Initialize()
    {
        _combatStartInputHandler = FindObjectOfType<CombatStartInputHandler>();
        callUpCircle.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Town"))
        {
            if (_isInteractPressing) return;

            if (other.TryGetComponent<IInteractionAble>(out var obj1))
            {
                if (obj1.IsInteractable)
                {
                    obj1.OnTriggerIn();
                    _currentSelectable = obj1; 
                }
            } 
        }

        if (other.CompareTag("Coin"))
        {
            var coin = other.GetComponentInParent<GameCoin>();
            if (coin != null)
            {
                coin.ReturnCoin();
                AudioController.instance.PlaySound("CoinLoot", SoundConfig.SoundType.Effect);
            }
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (_isInteractPressing || _currentSelectable != null) return;
        if (other.CompareTag("Town"))
        {
            if (_isInteractPressing || _currentSelectable != null) return;

            if (other.TryGetComponent<IInteractionAble>(out var obj1))
            {
                if (obj1.IsInteractable)
                {
                    obj1.OnTriggerIn();
                    _currentSelectable = obj1; 
                }

            } 
        }
        if (other.CompareTag("Ally"))
        {
            if (_isCallUp)
            {
                if (other.TryGetComponent<Ally>(out var ally))
                {
                    if (!ally.isFollowing)
                    {
                        triggerInAllys.Add(ally);
                        ally.OnFollow(true);
                    }
                }
            }
        }
    }

    private Action<InputInfo> _lateOutAction;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Town"))
        {
            if (other.TryGetComponent<IInteractionAble>(out var obj1))
            {
                if (obj1.IsInteractable)
                {
                    if (!_isInteractPressing)
                    {
                        obj1.OnTriggerOut();
                        _currentSelectable = null;
                    }
                    else
                    {
                        _lateOutAction = (info) =>
                        {
                            obj1.OnTriggerOut();
                            _currentSelectable?.OnInteraction(info);
                            _currentSelectable = null;
                        };
                    } 
                }
            } 
        }
    }


    public void CallbackEvent(InputInfo info)
    {
        if (PopupController.Instance.isOpenPopup)
        {
            return;
        }

        if (info.actionName == "Interaction")
        {
            InteractionInput(info);
        }
        if (info.actionName == "CallUp")
        {
            CallupInput(info);
        }
    }

    private void InteractionInput(InputInfo info)
    {
        _isInteractPressing = info.inputType == GameEnums.EInputType.Press;

        if (info.inputType == GameEnums.EInputType.Down)
        {
            if (_currentSelectable == null)
            {
                //RoundStart관련

                _combatStartInputHandler?.OnInput(info);
            }
            else
            {
            }
        }
        if (info.inputType == GameEnums.EInputType.Press)
        {
            if (_currentSelectable == null)
            {
                //RoundStart관련
                _combatStartInputHandler?.OnInput(info);
            }
            else
            {
            }
        }
        if (info.inputType == GameEnums.EInputType.Up)
        {
            if (_currentSelectable == null)
            {
                //RoundStart관련
                _combatStartInputHandler?.OnInput(info);

            }
            if (_lateOutAction != null)
            {
                _lateOutAction.Invoke(info);
                _lateOutAction = null;
            }
            _isInteractPressing = false;
            _currentSelectable?.OnInteraction(info);
            _currentSelectable = null;
        }

        _currentSelectable?.OnInteraction(info);
    }

    private void CallupInput(InputInfo info)
    {
        if (info.inputType == GameEnums.EInputType.Down)
        {
            callUpCircle.SetActive(!_isCallUp);
            _isCallUp = !_isCallUp;

            if (!_isCallUp)
            {
                float spreadRadius = 1.5f;
                int count = triggerInAllys.Count;
                float angleStep = 360f / count;
                float currentAngle = 0f;

                foreach (var triggerInAlly in triggerInAllys)
                {
                    triggerInAlly.OnFollow(false);

                    float rad = currentAngle * Mathf.Deg2Rad;
                    Vector3 offset = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * spreadRadius;
                    Vector3 candidatePos = transform.position + offset;

                    // NavMesh 위인지 확인
                    if (NavMesh.SamplePosition(candidatePos, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                    {
                        triggerInAlly.GetComponent<NavMeshAgent>().Warp(hit.position);
                    }

                    currentAngle += angleStep;
                }

                triggerInAllys.Clear();
            }
        }

    }

    private Action _unRegistCallback;
    public void SetEventUnRegistCallback(Action callback)
    {
        _unRegistCallback = callback;
    }
    private void OnDestroy()
    {
        _unRegistCallback?.Invoke();
    }
}
