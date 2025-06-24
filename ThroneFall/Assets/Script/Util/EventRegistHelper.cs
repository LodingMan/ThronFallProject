using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventRegistHelper
{
    public static void RegistCallbackEvent<T>(IEventRegist<T> eventRegist, IEventHandlerProvider<T> registCallbackClass)
    {
        registCallbackClass.SetEventUnRegistCallback(eventRegist.RegistEvent(registCallbackClass.CallbackEvent));
    }
    public static void RegistCallbackEvent<T>(IEventRegist<T> eventRegist, IMultiEventHandlerProvider registCallbackClass)
    {
        if (registCallbackClass.GetEventProviderCallBackDic().TryGetValue(typeof(T), out var del) &&
            del is Action<T> callback)
        {
            registCallbackClass.SetEventUnRegistCallback(eventRegist.RegistEvent(callback));
        }
    }
    public static void RegistCallbackEvent<T>(IMultiEventRegist eventRegist, IEventHandlerProvider<T> registCallbackClass)
    {
        registCallbackClass.SetEventUnRegistCallback(eventRegist.RegistEvent<T>(registCallbackClass.CallbackEvent));
    }
    public static void RegistCallbackEvent<T>(IMultiEventRegist eventRegist, IMultiEventHandlerProvider registCallbackClass)
    {
        if (registCallbackClass.GetEventProviderCallBackDic().TryGetValue(typeof(T), out var del) &&
            del is Action<T> callback)
        {
            registCallbackClass.SetEventUnRegistCallback(eventRegist.RegistEvent(callback));
        }
    }}
