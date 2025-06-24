using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;
public class GameState : MonoBehaviour,
    IEventRegist<EGameState>,
    IMultiEventHandlerProvider,
    IInitialize
{
    [SerializeField]private EGameState _State;
    public EGameState GetCurrentState => _State;

    Action<EGameState> _onChangeState;
    private Dictionary<Type, Delegate> _eventProviderDic = new();
    public Dictionary<Type, Delegate> GetEventProviderCallBackDic()
    {
        return _eventProviderDic;
    }

    public void Initialize()
    {
        _State = EGameState.Waiting;
        _eventProviderDic[typeof(EGameState)] = new Action<EGameState>(GameStateCallback);
        _eventProviderDic[typeof(EGameResult)] = new Action<EGameResult>(GameResultCallback);
    }

    public void Reset()
    {
        SetState(EGameState.Waiting);
    }

    public Action RegistEvent(Action<EGameState> onChangeState)
    {
        _onChangeState += onChangeState;
        _onChangeState?.Invoke(GetCurrentState);
        return () => UnRegistEvent(onChangeState);
    }
    public void UnRegistEvent(Action<EGameState> onChangeState)
    {
        _onChangeState -= onChangeState;
    }

    private void SetState(EGameState state)
    {
        if (_State == state) return;
        if (_State == EGameState.GameClear || _State == EGameState.GameOver) return;
        
        _State = state;
        _onChangeState?.Invoke(_State);
        GameStateEventBus.Publish(_State);
        Debug.Log($"ChangeState : {state}");
    }
    public EGameState GetState() => _State;

    private void OnDestroy()
    {
        _unregistCallback?.Invoke();
        _onChangeState = null;
    }


    public void GameStateCallback(EGameState value)
    {
        SetState(value);
    }
    public void GameResultCallback(EGameResult value)
    {
        switch (value)
        {
            case EGameResult.None:
                break;
            case EGameResult.RoundClear:
            case EGameResult.GameStart:
                SetState(EGameState.Waiting);
                break;
            case EGameResult.CombatStart:
                SetState(EGameState.Combat);
                break;
            case EGameResult.GameClear:
                SetState(EGameState.GameClear);
                break;
            case EGameResult.GameOver:
                SetState(EGameState.GameOver);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(value), value, null);
        }
        
    }
    
   private Action _unregistCallback;


   public void SetEventUnRegistCallback(Action callback)
    {
        _unregistCallback = callback;
    }

}
