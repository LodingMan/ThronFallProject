using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameEnums;
using UnityEngine.EventSystems;

public class CombatStartInputHandler : MonoBehaviour,
    IInputSubscriber,
    IInitialize,
    IMultiEventRegist,
    IEventHandlerProvider<EGameState>
{
    private Dictionary<Type, Delegate> _eventDic = new Dictionary<Type, Delegate>();
    private EGameState _currentState;
    private bool _isPressing_0 = false;
    public bool _isCommanderTownCreated = false;

    private void Awake()
    {
        //InputManager.RegisterInput(this);
    }

    public void Initialize()
    {
    }

    public void CallbackEvent(EGameState value)
    {
        _currentState = value;
    }

    private Action _unRegistCallback;
    private void OnDestroy()
    {
        _unRegistCallback?.Invoke();
        InputManager.UnRegisterInput(this);

    }
    public void SetEventUnRegistCallback(Action callback)
    {
        _unRegistCallback = callback;
    }

    public void OnInput(InputInfo info)
    {
        if (PopupController.Instance.isOpenPopup)
        {
            return;
        }
        if (_currentState == EGameState.Waiting && _isCommanderTownCreated)
        {
            if (_eventDic.TryGetValue(typeof(InputInfo), out var del1) && del1 is Action<InputInfo> inputAction)
            {
                inputAction.Invoke(info);
            }

            if (info.inputType == EInputType.Down)
            {
                AudioController.instance.PlaySound("CombatCall", SoundConfig.SoundType.Effect2);
            }
            if (info.inputType == EInputType.Press)
            {
                if (info.deltaTime > GameConfig.COMBAT_START_DURATION)
                {
                    if(_eventDic.TryGetValue(typeof(EGameResult), out var del2)&& del2 is Action<EGameResult> stateAction)
                    {
                        stateAction.Invoke(EGameResult.CombatStart);
                        AudioController.instance.PlaySound("CombatCallComplete", SoundConfig.SoundType.Effect);
                    }
                }
            }
        }
    }

    public Action RegistEvent<T>(Action<T> callback)
    {
        var key = typeof(T);
        if (!_eventDic.ContainsKey(key))
            _eventDic[key] = null;

        if (_eventDic.TryGetValue(typeof(T), out var del) && del is Action<T> existing)
        {
            _eventDic[key] =  existing + callback;
        }
        else
        {
            _eventDic[key] = callback;
        }
        
        return () => UnRegistEvent<T>(callback);
    }

    public void UnRegistEvent<T>(Action<T> callback)
    {
        var key = typeof(T);
        if (!_eventDic.ContainsKey(key)) return;

        if (_eventDic.TryGetValue(typeof(T), out var del) && del is Action<T> existing)
        {
            _eventDic[key] =  existing - callback;
        }
        
    }
}
