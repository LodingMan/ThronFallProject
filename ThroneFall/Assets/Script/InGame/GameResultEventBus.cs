using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public static class GameResultEventBus
{
    private static Action<EGameResult> _onChangeState;

    public static Action RegistEvent(Action<EGameResult> onChangeState)
    {
        _onChangeState -= onChangeState;
        _onChangeState += onChangeState;
        return () => UnRegistEvent(onChangeState);
    }
    public static void UnRegistEvent(Action<EGameResult> onChangeState)
    {
        _onChangeState -= onChangeState;
    }

    public static void Publish(EGameResult state)
    {
        _onChangeState?.Invoke(state);
    }

}
