using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class InGameEventRegistContext
{
    public IMultiEventRegist GameRoundEventRegist;
    public IEventRegist<EGameResult> GameResultEventRegist;
    public IEventRegist<EGameResult> EnemyCountTrackerEventRegist;
    public IEventRegist<EGameState> GameStateEventRegist;
    public IEventRegist<UnitLifecycleInfo> StageSpawnerHandlerEventRegist;
    public IMultiEventRegist TownHandlerEventRegist;
    public IMultiEventRegist CombatStartEventRegist;
    
    public IMultiEventHandlerProvider GameStateProvider;
    public IMultiEventHandlerProvider GameResultHandlerProvider;
    public IEventHandlerProvider<EGameResult> GameRoundHandlerProvider;
    public IMultiEventHandlerProvider EnemyCountTrackerProvider;
    public IEventHandlerProvider<UnitLifecycleInfo> UnitHandlerProvider;
    public IMultiEventHandlerProvider TownHandlerProvider;
    public IEventHandlerProvider<EGameState> CombatStartHandlerProvider;
    public IEventHandlerProvider<InputInfo> CombatStartPressGaugeProvider;
    public IMultiEventHandlerProvider NextEnemyViewerProvider;
    public IEventHandlerProvider<int> StageSpawnerHandlerProvider;
}
public class InGameEventRegister : MonoBehaviour
{
    public void RegisterEvent(InGameEventRegistContext context)
    {
        EventRegistHelper.RegistCallbackEvent(context.GameResultEventRegist, context.GameRoundHandlerProvider);
        EventRegistHelper.RegistCallbackEvent(context.GameResultEventRegist, context.GameStateProvider);

        EventRegistHelper.RegistCallbackEvent<EGameResult>(context.GameRoundEventRegist, context.GameResultHandlerProvider);
        EventRegistHelper.RegistCallbackEvent<int>(context.GameRoundEventRegist,context.NextEnemyViewerProvider);
        EventRegistHelper.RegistCallbackEvent<int>(context.GameRoundEventRegist, context.EnemyCountTrackerProvider);
        EventRegistHelper.RegistCallbackEvent(context.GameRoundEventRegist, context.StageSpawnerHandlerProvider);

        
        EventRegistHelper.RegistCallbackEvent(context.GameStateEventRegist, context.EnemyCountTrackerProvider);
        EventRegistHelper.RegistCallbackEvent(context.GameStateEventRegist, context.NextEnemyViewerProvider);
        EventRegistHelper.RegistCallbackEvent(context.GameStateEventRegist,context.TownHandlerProvider);
        EventRegistHelper.RegistCallbackEvent(context.GameStateEventRegist, context.CombatStartHandlerProvider);

        EventRegistHelper.RegistCallbackEvent(context.StageSpawnerHandlerEventRegist, context.EnemyCountTrackerProvider);
        EventRegistHelper.RegistCallbackEvent(context.StageSpawnerHandlerEventRegist, context.UnitHandlerProvider);
        
        EventRegistHelper.RegistCallbackEvent(context.CombatStartEventRegist, context.CombatStartPressGaugeProvider);
        EventRegistHelper.RegistCallbackEvent<EGameResult>(context.CombatStartEventRegist, context.GameResultHandlerProvider);
        
        EventRegistHelper.RegistCallbackEvent<EGameResult>(context.TownHandlerEventRegist, context.GameResultHandlerProvider);
        EventRegistHelper.RegistCallbackEvent<EGameState>(context.TownHandlerEventRegist, context.GameStateProvider);
        
        EventRegistHelper.RegistCallbackEvent(context.EnemyCountTrackerEventRegist, context.GameResultHandlerProvider);
    }
}
