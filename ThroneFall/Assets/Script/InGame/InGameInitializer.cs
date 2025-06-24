using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;


public class InGameDataContainer
{
    public List<UnitData> unitDatas;
    public List<TownData> townDatas;
    public StageData stageData;
}
public class BindedObjectsContainer
{
    public IInitialize combatStartInputHandler;
    public IInitialize combatStartPressGauge;
    public IInitialize<StageData> enemyCountTracker;
    public IInitialize<int> gameRound;
    public IInitialize gameState;
    public IInitialize<StageData> gameCoinHandler;
    public IInitialize gameResultHandler;
    
    public IAsyncInitialize<List<UnitData>> stageLifeCycleHandler;
    public IInitialize<SpawnerDataContext> stageSpawnerHandler;
    public IInitialize<List<TownData>> townHandler;
    public IInitialize unitHandler;
    public IInitialize<EnemyViewerDataContext> nextStageEnemyViewer;
}
public class InGameInitializer : MonoBehaviour
{
    public async UniTask InitializeAll(InGameDataContainer inGameData, BindedObjectsContainer objects)
    {
        Initialize1(inGameData, objects);
        await objects.stageLifeCycleHandler.Initialize(inGameData.unitDatas);
        Initialize2(inGameData, objects);
    }
    
    public void Initialize1(InGameDataContainer inGameData, BindedObjectsContainer objects)
    {
        objects.combatStartPressGauge.Initialize();
        objects.combatStartInputHandler.Initialize();
        objects.enemyCountTracker.Initialize(inGameData.stageData);
        objects.gameRound.Initialize(inGameData.stageData.roundDatas.Count);
        objects.gameState.Initialize();
        objects.gameResultHandler.Initialize();
    }
    public void Initialize2(InGameDataContainer inGameData, BindedObjectsContainer objects)
    {
        objects.gameCoinHandler.Initialize(inGameData.stageData);
        objects.townHandler.Initialize(inGameData.townDatas);
        objects.unitHandler.Initialize();
        objects.stageSpawnerHandler.Initialize(new SpawnerDataContext()
        {
            stageData = inGameData.stageData,
            unitDatas = inGameData.unitDatas
        });
        objects.nextStageEnemyViewer.Initialize(new EnemyViewerDataContext()
        {
            unitDatas = inGameData.unitDatas,
            stageData = inGameData.stageData,
        });

    }

   
    
}
