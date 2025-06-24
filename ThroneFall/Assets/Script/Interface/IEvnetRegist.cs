using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public interface IEventRegist 
{
    Action RegistEvent(Action callback);
    void UnRegistEvent(Action callback);
}
public interface IEventRegist<T>
{
    Action RegistEvent(Action<T> callback);
    void UnRegistEvent(Action<T> callback);
}

public interface IMultiEventRegist
{
    Action RegistEvent<T>(Action<T> callback);
    void UnRegistEvent<T>(Action<T> callback);
}

public interface IEventHandlerProvider
{
    void CallbackEvent();
}
public interface IEventHandlerProvider<T>
{
    void CallbackEvent(T value);
    void SetEventUnRegistCallback(Action callback);

}
public interface IMultiEventHandlerProvider
{
    Dictionary<Type, Delegate> GetEventProviderCallBackDic();
    void SetEventUnRegistCallback(Action callback);
}