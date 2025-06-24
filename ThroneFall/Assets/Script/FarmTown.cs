using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class FarmTown :Town 
{
    [SerializeField] private int currentClearRewardCoin;
    private bool isBreak;

    public override void Initialize(TownData townData)
    {
        base.Initialize(townData);

        currentClearRewardCoin = 1;
        GameResultEventBus.RegistEvent(GameResultCallbackEvent);
    }

    public override void TownCreate()
    {
        base.TownCreate();
        var obj = ObjectPooler.instance.GetObjectPool(SpawnEffect, transform.position);
        onFarmCreated.Invoke();
    }

    public override void TownBreak()
    {
        base.TownBreak();
        var obj = ObjectPooler.instance.GetObjectPool(BreakEffect, transform.position);
        isBreak = true;
    }

    public void GameResultCallbackEvent(EGameResult result)
    {
        if (result == EGameResult.RoundClear && FlagEnumHas(_townState.GetCurrentState,ETownState.Enable))
        {
            for (int i = 0; i < currentClearRewardCoin; i++)
            {
                if (!isBreak)
                {
                    _gameCoinHandler.CreateCoin(_trReturnCoin.position);
                }
            }

            isBreak = false;
        }
    }

    private Action onFarmCreated;
    public void RegistCreateNotifyCallback(Action action)
    {
        onFarmCreated += action;
    }
}
