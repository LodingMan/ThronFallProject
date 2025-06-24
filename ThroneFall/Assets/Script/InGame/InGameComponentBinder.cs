using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class InGameComponentBinder : BaseComponentBinder
{
    public async override UniTask StartBind()
    {
        base.StartBind();
        GameState gameState = Find<GameState>("GameState");
        GameCoinHandler gameCoinHandler = Find<GameCoinHandler>("GameCoinHandler");
        GameRound gameRound = Find<GameRound>("GameRound");
        EnemyCountTracker enemyCountTracker = Find<EnemyCountTracker>("EnemyCountTracker");
        GameResultHandler gameResultHandler = Find<GameResultHandler>("GameResultHandler");
        StageLifeCycleHandler stageLifeCycleHandler = Find<StageLifeCycleHandler>("StageLifeCycleHandler");
        StageSpawnerHandler stageSpawnerHandler = Find<StageSpawnerHandler>("StageSpawnerHandler");
        TownHandler townHandler = Find<TownHandler>("TownHandler");
        UnitHandler unitHandler = Find<UnitHandler>("UnitHandler");
        CombatStartInputHandler combatStartInputHandler = Find<CombatStartInputHandler>("CombatStartInputHandler");
        CombatStartPressGauge combatStartPressGauge = Find<CombatStartPressGauge>("CombatStartPressGauge");
        NextStageEnemyViewer nextStageEnemyViewer = Find<NextStageEnemyViewer>("NextStageEnemyViewer");
        InGameInitializer inGameInitializer = Find<InGameInitializer>("InGameInitializer");
        InGameEventRegister inGameEventRegister = Find<InGameEventRegister>("InGameEventRegister");
        Debug.Log("BindComp");
        
        await inGameInitializer.InitializeAll(
            new InGameDataContainer()
            {
                unitDatas = MainController.Instance.CSVDataContaner.UnitDatas,
                townDatas = MainController.Instance.CSVDataContaner.TownDatas,
                stageData = MainController.Instance.CSVDataContaner.StageDatas.Find(s => s.stage == GameConfig.CurrentSelectStage), 
            },
            new BindedObjectsContainer()
            {
                gameState = gameState, 
                gameCoinHandler = gameCoinHandler,
                gameRound = gameRound,
                gameResultHandler = gameResultHandler,
                enemyCountTracker = enemyCountTracker,
                combatStartInputHandler = combatStartInputHandler,
                combatStartPressGauge = combatStartPressGauge,
                
                stageLifeCycleHandler = stageLifeCycleHandler,
                stageSpawnerHandler = stageSpawnerHandler,
                townHandler = townHandler,
                unitHandler = unitHandler,
                nextStageEnemyViewer = nextStageEnemyViewer,
                
                
            });
        Debug.Log("Initlaize Comp");
        
        
        inGameEventRegister.RegisterEvent(new InGameEventRegistContext()
        {
            GameRoundEventRegist = gameRound,
            GameResultEventRegist = gameResultHandler,
            GameStateEventRegist = gameState,
            StageSpawnerHandlerEventRegist = stageSpawnerHandler,
            TownHandlerEventRegist = townHandler,
            CombatStartEventRegist = combatStartInputHandler,
            EnemyCountTrackerEventRegist = enemyCountTracker,
    
            GameResultHandlerProvider = gameResultHandler,
            GameRoundHandlerProvider = gameRound,
            EnemyCountTrackerProvider = enemyCountTracker,
            UnitHandlerProvider = unitHandler,
            TownHandlerProvider = townHandler,
            CombatStartHandlerProvider = combatStartInputHandler,
            CombatStartPressGaugeProvider = combatStartPressGauge,
            NextEnemyViewerProvider = nextStageEnemyViewer ,
            GameStateProvider = gameState,
            StageSpawnerHandlerProvider = stageSpawnerHandler
        });
        Debug.Log("EventRegistComp");
    }

}