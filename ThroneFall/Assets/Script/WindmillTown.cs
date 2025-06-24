using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class WindmillTown : Town
{
    private CombatStartInputHandler _combatStartInputHandler;
    [SerializeField] public List<FarmTown> ChildTowns = new();
    [SerializeField] private int _currentClearRewardCoin;
    [SerializeField] private int _currentCreateFarmCount;

    private bool isBreak;

    public override void Initialize(TownData townData)
    {
        base.Initialize(townData);
        _combatStartInputHandler = FindObjectOfType<CombatStartInputHandler>();
        foreach (var childTown in ChildTowns)
        {
            childTown.gameObject.SetActive(false);
            childTown.RegistCreateNotifyCallback(CreateFarm);
        }

        _currentClearRewardCoin = 1;
    }

    public override void TownCreate()
    {
        base.TownCreate();
        var obj = ObjectPooler.instance.GetObjectPool(SpawnEffect, transform.position);
        _combatStartInputHandler._isCommanderTownCreated = true;
        if (_gameState == EGameState.Waiting)
        {
            HideAndShowChildPreTown(true, 0);
        }
    }

    public override void TownBreak()
    {
        base.TownBreak();
        isBreak = true;
        var obj = ObjectPooler.instance.GetObjectPool(BreakEffect, transform.position);
    }

    private void CreateFarm()
    {
        _currentCreateFarmCount++;
        HideAndShowChildPreTown(true, _currentCreateFarmCount);
    }

    public void HideAndShowChildPreTown(bool isShow, int index)
    {
        if (index >= ChildTowns.Count) return;
        if (FlagEnumHas(_townState.GetCurrentState, GameEnums.ETownState.Enable))
        {
            if (FlagEnumHas(ChildTowns[index].GetCurrentTownState, GameEnums.ETownState.Disable))
            {
                ChildTowns[index].gameObject.SetActive(isShow);
            }
        }
    }
    public void HideAndShowAllChildPreTown(bool isShow)
    {
        if (FlagEnumHas(_townState.GetCurrentState, GameEnums.ETownState.Enable))
        {
            for (int i = 0; i < ChildTowns.Count; i++)
            {
                if (FlagEnumHas(ChildTowns[i].GetCurrentTownState, GameEnums.ETownState.Disable))
                {
                    ChildTowns[i].gameObject.SetActive(isShow);
                }
            }
        }
    }


    public override void GameStateCallbackEvent(GameEnums.EGameState state)
    {
        base.GameStateCallbackEvent(state);
        if (FlagEnumHas(_townState.GetCurrentState, GameEnums.ETownState.Enable))
        {
            if (state == GameEnums.EGameState.Waiting)
            {
                HideAndShowChildPreTown(true, _currentCreateFarmCount);
            }
            else
            {
                HideAndShowAllChildPreTown(false);
            }
        }
    }

    public override void GameResultCallbackEvent(EGameResult result)
    {
        base.GameResultCallbackEvent(result);
        if (result == EGameResult.RoundClear && FlagEnumHas(_townState.GetCurrentState, ETownState.Enable))
        {
            for (int i = 0; i < _currentClearRewardCoin; i++)
            {
                if(!isBreak)
                {
                    _gameCoinHandler.CreateCoin(_trReturnCoin.position);
                }
            }
            isBreak = false;
        }
    }
}
