using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemListItem<TData> : MonoBehaviour
{
    protected Action<TData> Callback;
    public virtual void SetData(TData data, Action<TData> callback = null)
    {
        Callback = callback;
    }
}
