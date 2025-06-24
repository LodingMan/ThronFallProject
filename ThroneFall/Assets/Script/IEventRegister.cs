using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public interface IEventRegister
{
    void RegistEvent(Action onChange);
    void UnRegistEvent(Action onChange); 
}

public interface IUnitStateEventRegister
{
    void RegistEvent(Action<EUnitState> onChangeState);
    void UnRegistEvent(Action<EUnitState> onChangeState); 
}

public interface IUnitDathEventRegister
{
    void RegistUnitDathEvent(Action<Unit> onUnitDath);
    void UnRegistUnitDathEvent(Action<Unit> onUnitDath);
}
public interface IGameStateEventRegister
{
    void RegistEvent(Action<EGameState> onChangeState);
    void UnRegistEvent(Action<EGameState> onChangeState);
}

public interface IGameRoundEventRegister
{
    void RegistEvent(Action<int> onChangeRound);
    void UnRegistEvent(Action<int> onChangeRound);
}
public interface IEnemyTrackerEventRegister
{
    void RegistEvent(Action<int> onChangeEnemyCount);
    void UnRegistEvent(Action<int> onChangeEnemyCount);
}
public interface IGameCoinEventRegister
{
    void RegistEvent(Action<int> onChangeGameCoin);
    void UnRegistEvent(Action<int> onChangeGameCoin);
}
public interface IUnitSpawnEventRegister
{
    void RegistEvent(Action<Unit> onSpawnUnit);
    void UnRegistEvent(Action<Unit> onSpawnUnit);
}

public interface IPressEventRegister
{
    void RegistPressEvent(Action onPressComplete);
    void UnRegistPressEvent(Action onPressComplete);
}
public interface IClickEventRegister
{
    void RegistClickEvent(Action onClick);
    void UnRegistClickEvent(Action onClick);
}

public interface IInputEventRegister : IPressEventRegister, IClickEventRegister
{
    
}

public class EventRegisterContext
{
    public IGameStateEventRegister GameStateEventRegister;
    public IGameRoundEventRegister GameRoundEventRegister;
    public IEnemyTrackerEventRegister EnemyTrackerEventRegister;
    public IGameCoinEventRegister GameCoinEventRegister;
}
