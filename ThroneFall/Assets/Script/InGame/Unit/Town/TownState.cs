using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static GameEnums;



public class TownState : MonoBehaviour, 
    ITownStateProvider,
    IState
{
    [SerializeField]private ETownState _state = new();
    public ETownState GetCurrentState => _state;
    public bool IsDead => _state == ETownState.Break;
    public bool CanBeAttack()
    {
        if (FlagEnumHas(_state , ETownState.Break))
        {
            return false;
        }
        if (FlagEnumHas(_state , ETownState.Disable))
        {
            return false;
        }
        AddTownState(ETownState.Attack);
        return true;
        
    }

    public void StartAttack()
    {
        if (FlagEnumHas(_state , ETownState.Break))
        {
            return;
        }
        AddTownState(ETownState.Attack);
    }

    public void CompleteAttack()
    {
        RemoveTownState(ETownState.Attack);
    }

    public bool CanBeMove()
    {
        return false;
    }

    public void StartMove()
    {
    }

    public void StopMove()
    {
    }

    public void Initialize()
    {
        Clear();
        AddTownState(ETownState.Disable);
    }

    public void AddTownState(ETownState state)
    {
        if (state != ETownState.Break && FlagEnumHas(state, ETownState.Break))
        {
            return;
        }
        if (!FlagEnumHas(_state,state))
        {
            FlagEnumAdd(ref _state,state);
        }
    }

    public void RemoveTownState(ETownState state)
    {
        if (state != ETownState.Break && FlagEnumHas(state, ETownState.Break))
        {
            return;
        } 
        if (FlagEnumHas(_state, state))
        {
            FlagEnumRemove(ref _state, state);
        }
    }

    public void Clear()
    {
        FlagEnumClear(ref _state);
    }
    
}
