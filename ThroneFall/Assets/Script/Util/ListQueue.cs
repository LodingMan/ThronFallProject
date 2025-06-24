using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ListQueue<T>
{
    public List<T> list = new();

    public void Enqueue(T value)
    {
        list.Add(value);
    }
    
    public T Dequeue()
    {
        T value = list[list.Count - 1];
        list.Remove(value);
        return value;
    }

    public void Clear()
    {
        list.Clear();
    }
    
    public int Count => list.Count;
    public T this[int index] => list[index];


}
