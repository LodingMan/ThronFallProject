using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static GameEnums;

public class CombatStartPressGauge : MonoBehaviour,
    IInitialize,
    IEventHandlerProvider<InputInfo>
{
    [SerializeField]private MousePressProgress _mousePressProgress;
    private Vector3 _clickPos;
    private float _pressProgress = 0f;
    
    public float CurrentPressProgress
    {
        get => _pressProgress;
        private set
        {
            _pressProgress = value;
            if(_pressProgress >= 1f)
            {
                _pressProgress = 0f;
            }
        }
    }
    Action OnPressComplete;
    public void Initialize()
    {
    }
    

    public void SetPressProgress(float time)
    {
        CurrentPressProgress = time / GameConfig.COMBAT_START_DURATION;
        _mousePressProgress?.SetProgress(CurrentPressProgress);
    }
    public void StartPress()
    {
        Debug.Log("CombatCall");
    }

    public void CallbackEvent(InputInfo value)
    {
        if (value.inputType == EInputType.Down)
        {
            StartPress();
        }
        
        SetPressProgress(value.deltaTime);
    }

    private Action _unRegistCallback;
    public void OnDestroy()
    {
        _unRegistCallback?.Invoke();
    }
    public void SetEventUnRegistCallback(Action callback)
    {
        _unRegistCallback = callback;
    }
}
