using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class CommanderTown : Town 
{
    private CombatStartInputHandler _combatStartInputHandler;
    [SerializeField] public List<Town> ChildTowns = new();

    public override void Initialize(TownData townData)
    {
        base.Initialize(townData);
        _combatStartInputHandler = FindObjectOfType<CombatStartInputHandler>();
        foreach (var childTown in ChildTowns)
        {
            childTown.gameObject.SetActive(false);
        }
    }

    public override void TownCreate()
    {
        base.TownCreate();
        var obj = ObjectPooler.instance.GetObjectPool(SpawnEffect,transform.position);
        _combatStartInputHandler._isCommanderTownCreated = true;
        if (_gameState == EGameState.Waiting)
        {
            HideAndShowChildPreTown(true);
        }

        var arrow = GetComponentInChildren<GuideArrow>();
        if (arrow != null)
        {
            arrow.RequestNextPhase();
        }
    }
    
    public override void TownBreak()
    {
        base.TownBreak();
        var obj = ObjectPooler.instance.GetObjectPool(BreakEffect,transform.position);
    }
    
    public void HideAndShowChildPreTown(bool isShow)
    {
            for (int i = 0; i < ChildTowns.Count; i++)
            {
                if (FlagEnumHas(ChildTowns[i].GetCurrentTownState, ETownState.Disable))
                {
                    ChildTowns[i]?.gameObject.SetActive(isShow);
                }
            } 
    }

    public override void GameStateCallbackEvent(EGameState state)
    {
        base.GameStateCallbackEvent(state);
        if (state == EGameState.Waiting&& FlagEnumHas(_townState.GetCurrentState, ETownState.Enable))
        {
            HideAndShowChildPreTown(true);
        }
        else
        {
            HideAndShowChildPreTown(false);
        }
    }

}
