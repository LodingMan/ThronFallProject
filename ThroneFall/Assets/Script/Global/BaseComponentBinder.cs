using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BaseComponentBinder : MonoBehaviour
{
    protected T Find<T>(string name) where T : Component
    {
        var go = GameObject.Find(name);
        return go?.GetComponent<T>();
    }

    protected T FindInChildren<T>(string name) where T : Component
    {
        var go = GameObject.Find(name);
        return go?.GetComponentInChildren<T>();
    }

    public async virtual UniTask StartBind()
    {
        
    }
    
}
