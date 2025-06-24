using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour,
    IInputSubscriber,
    IEventRegist<InputInfo>
{
    private Action<InputInfo> _onClickInfoEvent;
    private void Awake()
    {
        InputManager.RegisterInput(this);
    }
    IMoveDestProvider MoveDestProvider;
    public void Initialize(IMoveDestProvider moveDestProvider)
    {
        MoveDestProvider = moveDestProvider;
    }
    public void OnInput(InputInfo info)
    {
        if (PopupController.Instance.isOpenPopup) return;
        _onClickInfoEvent?.Invoke(info);
    }
    private void OnDestroy()
    {
        InputManager.UnRegisterInput(this);
    }
    
    public Action RegistEvent(Action<InputInfo> callback)
    {
        _onClickInfoEvent += callback;
        return () => UnRegistEvent(callback);
    }

    public void UnRegistEvent(Action<InputInfo> callback)
    {
        if (_onClickInfoEvent != null)
        {
            _onClickInfoEvent -= callback;
        }
    }
}
