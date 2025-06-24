// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using static GameEnums;
//
// public class TownInputHandler : MonoBehaviour,
//     IInputSubscriber,
//     IInitialize,
//     IEventRegist<InputInfo>
// {
//     private List<TownSelectData> _currentTownSelectFlag;
//     private Action<InputInfo> _onInput;
//     private Town selectedTown;
//     private bool _isPressing_0 = false;
//     
//     private void Awake()
//     {
//         InputManager.RegisterInput(this);
//     }
//
//     private bool CheckLayer(InputInfo info)
//     {
//         return info.layer != LayerMask.NameToLayer("Town") && info.layer != LayerMask.NameToLayer("PreTown");
//     }
//
//     public void Initialize() 
//     {
//     }
//     public void OnDown_0(InputInfo info)
//     {
//         if (CheckLayer(info))
//         {
//             return;
//         }
//         _isPressing_0 = true;
//         _onInput?.Invoke(info);
//     }
//     public void OnUp_0(InputInfo info)
//     {
//         _isPressing_0 = false;
//         _onInput?.Invoke(info);
//         
//     }
//     public void OnPress_0(InputInfo info)
//     {
//         if (!_isPressing_0 || CheckLayer(info))
//         {
//             info.inputType = EInputType.Up_0;
//             OnUp_0(info);
//             return;
//         }
//         if (info.layer != LayerMask.NameToLayer("Town") && info.layer != LayerMask.NameToLayer("PreTown"))
//         {
//             info.inputType = EInputType.Up_0;
//             OnUp_0(info);
//             return;
//         }
//         _onInput?.Invoke(info);
//     }
//
//     public void OnClick_0(InputInfo info)
//     {
//         if (CheckLayer(info))
//         {
//             return;
//         }
//         _onInput?.Invoke(info);
//     }
//     
//     public void OnDown_1(InputInfo layer)
//     {
//     }
//
//     public void OnUp_1(InputInfo layer)
//     {
//     }
//
//     public void OnPress_1(InputInfo layer)
//     {
//     }
//
//     public void OnClick_1(InputInfo layer)
//     {
//     }
//     
//     public Action RegistEvent(Action<InputInfo> callback)
//     {
//         _onInput += callback;
//         return () => UnRegistEvent(callback);
//     }
//
//     public void UnRegistEvent(Action<InputInfo> callback)
//     {
//         _onInput -= callback;
//     }
//     
//     private void OnDestroy()
//     {
//         InputManager.UnRegisterInput(this);
//     }
// }
