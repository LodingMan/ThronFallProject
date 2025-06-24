using System;
using System.Collections;
using System.Collections.Generic;
using Microlight.MicroBar;
using UnityEngine;
using UnityEngine.Serialization;
using static GameEnums;


public class Unit : MonoBehaviour,
    ITargetableUnit,
    IEventRegist<UnitLifecycleInfo>
{
    public string unitID;
    protected EGameState _gameState;
    protected GameCoinHandler _gameCoinHandler;
    protected UnitState _unitState;
    protected Move _move;
    protected Attack _attack;
    protected Health _health;
    protected UnitAnimation _unitAnimation;
    protected UnitData _unitData;
    public PoolObject _pool;
    protected Collider _collider;
    
    protected Action<UnitLifecycleInfo> _onUnitLifecycleEvent;


    public bool _isTargetAble = false;
    public EUnitType GetUnitType => EUnitType.Unit;
    public bool GetTargetAble => _isTargetAble;
    public Vector3 GetPosition => transform.position;


    protected virtual void Start()
    {
        _gameCoinHandler = FindObjectOfType<GameCoinHandler>();
    }

    public virtual void Initialize(UnitData unitData)
    {
        _unitData = unitData;
        _unitState = GetComponent<UnitState>();
        if (_unitState == null)
        {
            Debug.LogWarning("unitState is NULL");
            return;
        }
        _unitState?.Initialize();

        _unitAnimation = GetComponentInChildren<UnitAnimation>();
        if (_unitAnimation != null)
        {
            _unitAnimation?.Initialize();
            EventRegistHelper.RegistCallbackEvent(_unitState,_unitAnimation);
        }
        if(TryGetComponent<Health>(out var unitHealth))
        {
            _health = unitHealth;
            _health.Initialize(_unitData.Hp);
            _health.SetActive(true);

        }
        if (TryGetComponent<Move>(out var move))
        {
            _move = move;
            _move.Initialize(_unitData.Speed, _unitState);
        }
        if (TryGetComponent<Attack>(out var attack))
        {
            _attack = attack;
            attack.Initialize(new AttackData()
            {
                Damage = _unitData.Damage,
                CoolDown = unitData.AttackCoolDown,
                Range = unitData.AttackRange,
            },_unitState);
        }
        if (TryGetComponent<PoolObject>(out var poolObject))
        {
            _pool = poolObject;
        }
        GameStateEventBus.RegistEvent(GameStateCallbackEvent);
        _onUnitLifecycleEvent?.Invoke(new UnitLifecycleInfo()
        {
            unit = this,
            unitLifecycleEventType = EUnitLifecycleEventType.Created,
        });
        _collider = GetComponent<Collider>();
        _collider.enabled = true;
        _isTargetAble = true;
    }
    
    public virtual void Hit(AttackInfo attackInfo)
    {
        if (_unitState.GetCurrentState != EUnitState.Die)
        {
            _health.MinusHP(attackInfo.Damage);
            if (_health.CurrentHP <= 0)
            {
                _health.SetHP(0);
                _health.SetActive(false);
                _unitState.Clear();
                _isTargetAble = false;
                _unitState.AddUnitState(EUnitState.Die);
            }
            Debug.Log($"OnHit : {attackInfo.AttackType}, {attackInfo.Damage}"); 
        }    
    }

    public virtual void Die()
    {
        if (_move != null)
        {
            _move.StopMove();
        }
        _onUnitLifecycleEvent?.Invoke(new UnitLifecycleInfo()
        {
            unit = this,
            unitLifecycleEventType = EUnitLifecycleEventType.Destroyed,
        });


        for (int i = 0; i < _unitData.DropCoin; i++)
        {
            _gameCoinHandler.CreateCoin(transform.position);
        }
        _collider.enabled = false;
        _onUnitLifecycleEvent = null;
    }

    public void DieAnimationEnd()
    {
        Die();
    }

    public UnitData GetUnitData()
    {
        return _unitData;
    }

    public void GameStateCallbackEvent(EGameState state)
    {
        _gameState = state;
    }

    public Action RegistEvent(Action<UnitLifecycleInfo> callback)
    {
        _onUnitLifecycleEvent += callback;
        return ()=> UnRegistEvent(callback);
    }

    public void UnRegistEvent(Action<UnitLifecycleInfo> callback)
    {
        _onUnitLifecycleEvent -= callback;
    }
}
