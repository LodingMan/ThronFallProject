using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using static GameConfig;
using static GameEnums;

public interface ITownDataProvider
{
    TownData GetTownData();
}

public class TownHandler : MonoBehaviour,
    IInitialize<List<TownData>>,
    IMultiEventRegist,
    IMultiEventHandlerProvider
{
    [SerializeField] private List<Town> _gameTowns = new();
    private List<TownSelectData> TownSelectFlags = new();
    private List<TownData> _townDatas;
    //private ScriptableUnits ScriptableTowns;

    private int _recvCurrentCoin;
    
    private Dictionary<Type, Delegate> _townHandlerEventRegistDic = new();
    private Dictionary<Type, Delegate> _townHandlerProviderDic = new();
    public Dictionary<Type, Delegate> GetEventProviderCallBackDic()
    {
        return _townHandlerProviderDic;
    }


    public void Initialize(List<TownData> townDatas)
    { 
        _townHandlerProviderDic[typeof(EGameState)] = new Action<EGameState>(OnChangeGameState);
        
        _townDatas = townDatas;
        _gameTowns.Clear();
        _gameTowns = FindObjectsOfType<Town>().ToList();

        foreach (var gameTown in _gameTowns)
        {
            TownData townData = townDatas.Find(t => t.TownID == gameTown.townID);
            if (townData != null)
            {
                gameTown.Initialize(townData);
                gameTown.RegistEvent(TownBreak);
            }
        }
    }

    public void OnChangeGameState(GameEnums.EGameState state)
    {
        switch (state)
        {
            case EGameState.None:
                break;
            case EGameState.Waiting:
                SetActivePreTown(true);
                break;
            case EGameState.Combat:
                SetActivePreTown(false);
                break;
            case EGameState.GameOver:
                break;
            case EGameState.GameClear:
                break;
            case EGameState.Result:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
    
    public void TownBreak(Town selectTown)
    {
        if (selectTown == null) return;

        if (selectTown is CommanderTown commander)
        {
            if (_townHandlerEventRegistDic.TryGetValue(typeof(EGameResult), out var dic) &&
                dic is Action<EGameResult> resultAction)
            {
                resultAction.Invoke(EGameResult.GameOver);
            }
        }
    }
    
    public void SetActivePreTown(bool isActive)
    {
        foreach (var town in _gameTowns)
        {
            if (town.GetTargetAble == true)
            {
                
            }
        }
    }

    //private Action OnBuildTown;
    // public void RegistEvent(Action onChange)
    // {
    //     OnBuildTown += onChange;
    // }
    //
    // public void UnRegistEvent(Action onChange)
    // {
    //     OnBuildTown -= onChange;
    // }
    
    #region EventProvide
    public void StateChangeCallback(EGameState value)
    {
        OnChangeGameState(value);
    }
    public void CoinChangeCallback(int coin)
    {
        _recvCurrentCoin = coin;
    }

    Action _unRegistCallback;
    public void SetEventUnRegistCallback(Action callback)
    {
        _unRegistCallback = callback;
    }
    #endregion
    private void OnDestroy()
    {
        _unRegistCallback?.Invoke();
        _townHandlerProviderDic.Clear();
        _townHandlerEventRegistDic.Clear();
    }

    #region EventRegist
    public Action RegistEvent<T>(Action<T> callback)
    {
        var key = typeof(T);
        if (!_townHandlerEventRegistDic.ContainsKey(key))
            _townHandlerEventRegistDic[key] = null;

        if (_townHandlerEventRegistDic.TryGetValue(typeof(T), out var del) && del is Action<T> existing)
        {
            _townHandlerEventRegistDic[key] =  existing + callback;
        }
        else
        {
            _townHandlerEventRegistDic[key] = callback;
        }
        return () => UnRegistEvent<T>(callback);
    }

    public void UnRegistEvent<T>(Action<T> callback)
    {
        var key = typeof(T);
        if (!_townHandlerEventRegistDic.ContainsKey(key)) return;

        if (_townHandlerEventRegistDic.TryGetValue(typeof(T), out var del) && del is Action<T> existing)
        {
            _townHandlerEventRegistDic[key] =  existing - callback;
        }
    }

    #endregion

}
