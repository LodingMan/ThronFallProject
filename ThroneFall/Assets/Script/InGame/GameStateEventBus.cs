using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public static class GameStateEventBus
{ 
    private static Action<EGameState> _onChangeState;
    
    public static Action RegistEvent(Action<EGameState> onChangeState)
    {
        _onChangeState -= onChangeState;
        _onChangeState += onChangeState;
        return () => UnRegistEvent(onChangeState);
    }
    public static void UnRegistEvent(Action<EGameState> onChangeState)
    {
        _onChangeState -= onChangeState;
    }

    public static void Publish(EGameState state)
    {
        _onChangeState?.Invoke(state);
    }
}
