// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Serialization;
// using static GameEnums;
//
// public class TownPressGauge : MonoBehaviour,
//     IInitialize,
//     IEventHandlerProvider<InputInfo>
// {
//     [FormerlySerializedAs("MousePressProgress")] [SerializeField]private MousePressProgress _mousePressProgress;
//     
//     private const float holdDuration = 2f; 
//     private Vector3 clickPos;
//     float _pressProgress = 0f;
//     
//     public float CurrentPressProgress
//     {
//         get => _pressProgress;
//         private set
//         {
//             _pressProgress = value;
//             if(value >= 1f)
//             {
//                 _pressProgress = 0f;
//                 OnPressComplete?.Invoke();
//             }
//         }
//     }
//     
//     Action OnPressComplete;
//     public void Initialize()
//     {
//
//     }
//
//     public void SetPressProgress(float time)
//     {
//         CurrentPressProgress = time/holdDuration;
//         _mousePressProgress?.SetProgress(CurrentPressProgress);
//     }
//
//     public void StartPress()
//     {
//         AudioController.instance.PlaySound("Town_BuildPopup", SoundConfig.SoundType.Effect2);
//         Debug.Log("Town_BuildPopup");
//     }
//     
//
//     public void CallbackEvent(InputInfo value)
//     {
//         if(value.layer != LayerMask.NameToLayer("Town") && value.layer != LayerMask.NameToLayer("PreTown"))
//         {
//             _mousePressProgress.SetActive(false);
//             return;
//         }
//         if (value.inputType == EInputType.Down_0)
//         {
//             _mousePressProgress.SetPosition(Input.mousePosition);
//         }
//         
//         if (value.inputType == EInputType.Press_0)
//         {
//             _mousePressProgress.SetPosition(Input.mousePosition);
//             SetPressProgress(value.duration);
//         }
//
//         if (value.inputType == EInputType.Up_0)
//         {
//             _mousePressProgress.SetActive(false);
//         }
//     }
//
//     private Action _unRegistCallback;
//     public void OnDestroy()
//     {
//         _unRegistCallback?.Invoke();
//     }
//     public void SetEventUnRegistCallback(Action callback)
//     {
//         _unRegistCallback = callback;
//     }
// }
