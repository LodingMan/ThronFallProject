// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using Cysharp.Threading.Tasks.Triggers;
// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.Serialization;
//
// public class StageInitializer : MonoBehaviour
// {
//      
//      public void Initialize(StageInitializationContext context)
//      { 
//          var townHandler = GetComponentInChildren<TownHandler>();
//          var unitManager = GetComponentInChildren<UnitHandler>();
//          var stageSpawnerHandler = GetComponentInChildren<StageSpawnerHandler>();
//          var nextStageEnemyViewer = GetComponentInChildren<NextStageEnemyViewer>();
//          var townClickHandler = GetComponentInChildren<TownInputHandler>();
//          var townSelectPressDurationTracker = GetComponentInChildren<PressDurationTracker>();
//          var townSelectPressGauge = GetComponentInChildren<TownPressGauge>();
//          var playerCreator = GetComponentInChildren<PlayerCreator>();
//          
//         stageSpawnerHandler.Initialize(
//             context.UnitDatas,
//             context.StageData,
//             new SpawnerDataContext()
//             {
//                 _gameStateProvider = context.GameState,
//                 _gameRoundProvider = context.GameRound,
//                 _unitDeathNotifier = unitManager,
//             });
//         townHandler.Initialize(
//             new TownManagerProviderContext()
//             {
//                 TownDatas = context.TownDatas,
//                 GameRoundProvider = context.GameRound,
//                 GameStateProvider = context.GameState,
//                 GameCoinProvider = context.GameCoin,
//                 SpawnerInitializer = stageSpawnerHandler,
//             }
//             );
//         townClickHandler.Initialize(townSelectPressDurationTracker);
//         townSelectPressDurationTracker.Initialize(townSelectPressGauge);
//         townSelectPressGauge.Initialize();
//         unitManager.Initialize();
//         playerCreator.Initialize(
//             context.UnitObjects.Find(u => u.Key == "Player").Unit as InGamePlayer,
//             context.UnitDatas.Find(u => u.UnitID == "Player"),
//             new GameUnitInitializeContext()
//             {
//                 UnitDeathNotifier = unitManager,
//                 GameStateProvider = context.GameState,
//             });
//         nextStageEnemyViewer.Initialize(
//             context.GameRound,
//             context.StageData
//             ,context.UnitDatas,
//             stageSpawnerHandler.GetSpawnerTransformData()
//             );
//         
//         context.EventRegisterContext.GameStateEventRegister.RegistEvent(townHandler.OnChangeGameState);
//         context.EventRegisterContext.GameStateEventRegister.RegistEvent(nextStageEnemyViewer.OnChangeState);
//         context.EventRegisterContext.GameStateEventRegister.RegistEvent(stageSpawnerHandler.OnChangeState);
//         context.EventRegisterContext.GameStateEventRegister.RegistEvent(unitManager.OnChangeGameState);
//         context.EventRegisterContext.GameCoinEventRegister.RegistEvent(townHandler.OnChangeGameCoin);
//         stageSpawnerHandler.RegistEvent(unitManager.RegistUnit);
//         unitManager.RegistUnitDathEvent(context.EnemyCount.CatchUnitDie);
//         townSelectPressDurationTracker.RegistPressEvent(() =>
//         {
//             if (townClickHandler.GetSelectedTown().TownType == GameConfig.ETownType.PreTown)
//             {
//                 PopupController.Instance.OpenPopup<PopupTownBuildSelect>("PopupTownSelect",
//                     (popup) =>
//                     {
//                         popup.Initialize(
//                             townClickHandler.GetSelectedTown(),
//                             townHandler.GetFlags(),
//                             townHandler.BuildTown);
//                         popup.OpenPopup();
//                     });    
//             }
//         });
//         townSelectPressDurationTracker.RegistClickEvent(() =>
//         {
//             if (townClickHandler.GetSelectedTown().TownType != GameConfig.ETownType.PreTown)
//             {
//                 PopupController.Instance.OpenPopup<PopupUnitInfo>("PopupUnitInfo",
//                     (popup) =>
//                     {
//                         popup.Initialize(townClickHandler.GetSelectedTown().GetUnitData());
//                         popup.OpenPopup();
//                     }); 
//             }
//         });
//
//     }
// }
