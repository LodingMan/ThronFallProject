using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public interface IGameRoundProvider
{
    int CurrentRound { get; }

    void NotifyNextRound();
}
public interface IGameCoinProvider
{
    int GetCurrentCoin { get; }
    void SetCoin(int coin);
}

public interface IStageLifeCycleProvider
{
    void RestartStage();
}

public interface ITransformProvider
{
    Transform GetStartTransform { get; }
}
public interface IGameStateProvider
{ 
    EGameState GetCurrentState { get; }
    void NotifyCombatStart();
    void NotifyWaitStart();
    void NotifyGameOver();
    void NotifyGameClear();
}
public interface IUnitStateProvider
{
    public void AddUnitState(EUnitState state);
    public void RemoveUnitState(EUnitState state);
    EUnitState GetCurrentState { get; }
}
public interface ITownStateProvider
{
    public void AddTownState(ETownState state);
    public void RemoveTownState(ETownState state);
    ETownState GetCurrentState { get; }
    
}

public interface IState
{
    public bool IsDead { get; }
    public bool CanBeAttack();
    public void StartAttack();
    public void CompleteAttack();

    public bool CanBeMove();
    public void StartMove();
    public void StopMove();



}
