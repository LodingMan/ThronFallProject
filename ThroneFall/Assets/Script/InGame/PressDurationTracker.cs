using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPressDurationReceiver
{
    void StartPressing();
    void CancelPress();
    void Tick();
}

public class PressDurationTracker : MonoBehaviour,
    IPressDurationReceiver,
    IInputEventRegister
{
    public float PressThreshold = 0.2f;
    private float holdDuration = 2f;
    private float pressStartTime = 0f;
    
    private bool isPressing = false;
    IPressProgressProvider PressProgressProvider;
    
    Action OnPressComplete;
    private Action OnClick;

    public void Initialize(IPressProgressProvider pressProgressProvider)
    {
        PressProgressProvider = pressProgressProvider;
    }
    
    public void StartPressing()
    {
        isPressing = true;
        pressStartTime = Time.time; 
    }
    public void Tick()
    {
        if (!isPressing) return;
        
        float elapsed = Time.time - pressStartTime;
        if (elapsed == 0)
        {
            PressProgressProvider.StartPress();
        }
        PressProgressProvider.SetPressProgress(elapsed);

        if (elapsed >= holdDuration)
        {
            isPressing = false;
            OnPressComplete?.Invoke(); 
        }
    }
    public void CancelPress()
    {
        float elapsed = Time.time - pressStartTime;
        if (elapsed < PressThreshold)
        {
            OnClick?.Invoke(); 
        }
        
        isPressing = false;
        pressStartTime = 0f;
        PressProgressProvider.SetPressProgress(0f);
    }
    
    public void RegistPressEvent(Action onPressComplete)
    {
        OnPressComplete -= onPressComplete;
        OnPressComplete += onPressComplete;    
    }

    public void UnRegistPressEvent(Action onPressComplete)
    {
        OnPressComplete -= onPressComplete;
    }

    public void RegistClickEvent(Action onClick)
    {
        OnClick -= onClick;
        OnClick += onClick;      
    }

    public void UnRegistClickEvent(Action onClick)
    {
        OnClick -= onClick;
    }
}
