using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
// public partial class StageInitializer
// {
//     private void OnSpawn(Unit unit)
//     {
//         unitManager.RegistUnit(unit);
//     }
//
//     private void OnChangeGameState(GameConfig.EGameState state)
//     {
//         switch (state)
//         {
//             case GameConfig.EGameState.None:
//                 break;
//             case GameConfig.EGameState.Waiting:
//                 stageSpanwerManager.RegistEnemys(Context.StageData, Context._gameRoundProvider.CurrentRound);
//                 townManager.SetActivePreTown(true);
//                 unitManager.ClearAllUnit();
//                 nextStageEnemyViewer.SetListViews(Context._gameRoundProvider.CurrentRound);
//                 break;
//             case GameConfig.EGameState.Combat:
//                 nextStageEnemyViewer.HideNextEnemysInfo();
//                 townManager.SetActivePreTown(false);
//                 break;
//             case GameConfig.EGameState.GameOver:
//                 break;
//             case GameConfig.EGameState.GameClear:
//                 break;
//             case GameConfig.EGameState.Result:
//                 break;
//             default:
//                 throw new ArgumentOutOfRangeException(nameof(state), state, null);
//         }
//     }
// }
