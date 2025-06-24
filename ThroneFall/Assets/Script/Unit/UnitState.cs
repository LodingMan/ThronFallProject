using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static GameEnums;



public class UnitState : MonoBehaviour, 
    IUnitStateProvider,
    IEventRegist<EUnitState>,
    IState
{
    [SerializeField] private EUnitState _state;
    private Action<EUnitState> _onChangeUnitStateEvent;


    public EUnitState GetCurrentState => _state;

    public void AddUnitState(EUnitState state)
    {
        if (FlagEnumHas(_state, EUnitState.Die))
        {
            return;
        }
        FlagEnumAdd(ref _state,state);
        _onChangeUnitStateEvent?.Invoke(_state);

    }

    public void RemoveUnitState(EUnitState state)
    {
        if (FlagEnumHas(_state, EUnitState.Die))
        {
            return;
        } 
        if (FlagEnumHas(_state, state))
        {
            FlagEnumRemove(ref _state, state);
        }
        _onChangeUnitStateEvent?.Invoke(_state);
    }


    public void Initialize()
    {
        FlagEnumClear(ref _state);
    }
    
    public bool IsDead { get; }
    public bool CanBeAttack()
    {
        if (FlagEnumHas(_state,EUnitState.Die))
        {
            return false;
        }
        return true; 
    }

    public void StartAttack()
    {
        if (FlagEnumHas(_state,EUnitState.Die))
        {
            return;
        }
        AddUnitState(EUnitState.Attack);
    }

    public void CompleteAttack()
    {
        RemoveUnitState(EUnitState.Attack);
    }

    public bool CanBeMove()
    {
        if (FlagEnumHas(_state,EUnitState.Die) || FlagEnumHas(_state,EUnitState.Attack))
        {
            return false;
        }
        return true;
    }

    public void StartMove()
    {
        if (FlagEnumHas(_state,EUnitState.Die))
        {
            return;
        }
        AddUnitState(EUnitState.Move);
    }

    public void StopMove()
    {
        if (FlagEnumHas(_state,EUnitState.Die))
        {
            return;
        }
        RemoveUnitState(EUnitState.Move);
    }

    
    public Action RegistEvent(Action<EUnitState> callback)
    {
        _onChangeUnitStateEvent += callback;
        return () => UnRegistEvent(callback);
    }

    public void UnRegistEvent(Action<EUnitState> callback)
    {
        if (_onChangeUnitStateEvent != null)
        {
            _onChangeUnitStateEvent -= callback;
        }
    }

    public void Clear()
    {
        FlagEnumClear(ref _state);
    }
}
