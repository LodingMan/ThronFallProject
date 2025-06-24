using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static GameEnums;

public class EnemyCountTracker : MonoBehaviour,
    IInitialize<StageData>,
    IEventRegist<EGameResult>,
    IMultiEventHandlerProvider
{
    private Action<EGameResult> _onEnemyClear;
    [SerializeField]private TMP_Text _lbRemainingEnemyCount;
    [SerializeField] private int _remainingEnemyCount;
    private StageData _stageData;
    private Dictionary<Type, Delegate> _eventCallBackDic = new();
    public Dictionary<Type, Delegate> GetEventProviderCallBackDic()
    {
        return _eventCallBackDic;
    }
    private int RemainingEnemyCount
    {
        get => _remainingEnemyCount;
        set
        {
            _remainingEnemyCount = value;
            _lbRemainingEnemyCount.text = $"Enemy : {_remainingEnemyCount.ToString()}";
        }
    }
    
    public void Initialize(StageData stageData)
    {
        _stageData = stageData;
        _eventCallBackDic[typeof(EGameState)] = new Action<EGameState>(GameStateChangeCallbackEvent);
        _eventCallBackDic[typeof(UnitLifecycleInfo)] = new Action<UnitLifecycleInfo>(EnemyDeadCallbackEvent);
        _eventCallBackDic[typeof(int)] = new Action<int>(GameRoundChangeCallbackEvent);
    }

    public void Reset()
    {
        _remainingEnemyCount = 0;
    }
    
    public void SetEnemyCount(int count)
    {
        RemainingEnemyCount = count;
    }

    public void SetActive(bool isActive)
    {
        _lbRemainingEnemyCount.gameObject.SetActive(isActive);
    }
    
    public void GameStateChangeCallbackEvent(EGameState value)
    {
        if (value == EGameState.Combat)
        {
            SetActive(true);
        }
        else
        {
            SetActive(false);
        }
    }

    public void GameRoundChangeCallbackEvent(int round)
    {
        RemainingEnemyCount = _stageData.roundDatas[round - 1].enemyCountInfo.Sum(info => info.Item3);
    }
    public void EnemyDeadCallbackEvent(UnitLifecycleInfo info)
    {
        if (info.unitLifecycleEventType == EUnitLifecycleEventType.Destroyed && info.unit is Enemy)
        {
            RemainingEnemyCount--;
        }
        if (RemainingEnemyCount <= 0)
        {
            _onEnemyClear?.Invoke(EGameResult.RoundClear);
        }
    }
    
    #region EventProvide
    Action _unRegistCallback;
    public void SetEventUnRegistCallback(Action callback)
    {
        _unRegistCallback = callback;
    }
    #endregion
    
    #region EventRegist
    public Action RegistEvent(Action<EGameResult> callback)
    {
        _onEnemyClear += callback;
        return () => UnRegistEvent(callback);
    }

    public void UnRegistEvent(Action<EGameResult> callback)
    {
        _onEnemyClear -= callback;
    }
    #endregion
    
    
    private void OnDestroy()
    {
        _unRegistCallback?.Invoke();
        _onEnemyClear = null;
    }

}
