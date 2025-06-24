using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IInitialize
{
    void Initialize();
}
public interface IInitialize<T>
{
    void Initialize(T value);
}
public interface IAsyncInitialize
{
    UniTask Initialize();
}
public interface IAsyncInitialize<T>
{
    UniTask Initialize(T value);
}
