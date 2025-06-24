using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

[Serializable]
public class ReferencePair
{
    public string Key;
    public AssetReferenceGameObject Reference;
}

[CreateAssetMenu(fileName = "ScriptableReference", menuName = "Scriptable/AssetReference")]
public class ReferenceScriptable : ScriptableObject
{
    public List<ReferencePair> ReferenceList;

    public ReferencePair FindStage(string key)
    {
        return ReferenceList.Find(s => s.Key == key);
    }
}
