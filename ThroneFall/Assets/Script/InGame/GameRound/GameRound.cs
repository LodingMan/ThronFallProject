using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using static GameConfig;
using static GameEnums;

public class GameRoundProviderContext
{
    public IGameStateProvider _gameStateProvider;
}

public class GameRound : MonoBehaviour,
    IInitialize<int>,
    IMultiEventRegist, 
    IEventHandlerProvider<EGameResult>
{
    [SerializeField] private TMP_Text _lbRound;
    [SerializeField] private int _currentRound;
    private int _maxStageRound;

    private Dictionary<Type, Delegate> _eventDic = new(); //Dictionary<Type,Action<object>>();


    
    public int CurrentRound
    {
        get => _currentRound;
        private set
        {
            _currentRound = value;
            _lbRound.text = $"Round : {_currentRound.ToString()}";
            if (_eventDic.TryGetValue(typeof(int), out var del2) && del2 is Action<int> intAction)
            {
                intAction.Invoke(_currentRound);
            }
        }
    }
    //private IGameStateEventRegister Register;

    public void Initialize(int maxRound)
    {
        _maxStageRound = maxRound;
        CurrentRound = 1;

    }
    public void CallbackEvent(EGameResult value)
    {
        if (value == EGameResult.RoundClear)
        {
            NextRound();
        }
    }

    private void NextRound()
    {
        if(CurrentRound >= _maxStageRound)
        {
            if (_eventDic.TryGetValue(typeof(EGameResult), out var del) && del is Action<EGameResult> stringAction)
            {
                stringAction.Invoke(EGameResult.GameClear);
            }
            return;
        }
        CurrentRound++;

    }
    public Action RegistEvent<T>(Action<T> callback)
    {
        var key = typeof(T);
        if (!_eventDic.ContainsKey(key))
            _eventDic[key] = null;
        if (_eventDic.TryGetValue(typeof(T), out var del) && del is Action<T> existing)
        {
            _eventDic[key] =  existing + callback;
        }
        else
        {
            _eventDic[key] = callback;
        }
        if(typeof(T) == typeof(int))
        {
            if (_eventDic.TryGetValue(typeof(int), out var intDel) && intDel is Action<int> intAction)
            {
                intAction.Invoke(_currentRound);
            }
        }
        return () => UnRegistEvent<T>(callback);
    }

    public void UnRegistEvent<T>(Action<T> callback)
    {
        var key = typeof(T);
        if (!_eventDic.ContainsKey(key)) return;

        if (_eventDic.TryGetValue(typeof(T), out var del) && del is Action<T> existing)
        {
            _eventDic[key] =  existing - callback;
        }
        
    }

    
    private Action _unRegistCallback;
    private void OnDestroy()
    {
        _eventDic.Clear();
        _unRegistCallback?.Invoke();
    }
    public void SetEventUnRegistCallback(Action callback)
    {
        _unRegistCallback = callback;
    }



}
