using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static  SoundConfig;

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "Scriptable/SoundLibrary")]
public class SoundLibrary : ScriptableObject
{
    [SerializeField] private List<SoundPairSet> SoundBundleList = new();
    public SoundPairSet GetPairSet(SoundType type) => SoundBundleList.Find(p => p.GetSoundType == type);
    
    public bool TryGetPairSet(out SoundPairSet pairSet, SoundType type)
    {
        pairSet = SoundBundleList.Find(p => p.GetSoundType == type);
        return pairSet != null;
    }
    
    public SoundPairSet GetPairSetWithIndex(int index)
    {
        if (0 <= index && index < SoundBundleList.Count)
        {
            return SoundBundleList[index] == null ? null : SoundBundleList[index];
        }
        else
        {
            return null;
        }
    }

    public AudioClip GetClip(string key)
    {
        foreach (var bundle in SoundBundleList)
        {
            if (bundle.TryGetClip(out AudioClip clip, key))
            {
                return clip;
            }
        }

        return null;
    }

    public AudioClip GetClip(string key, SoundType type)
    {
        var pair = GetPairSet(type);
        return pair.GetClip(key);
    }

    public int Count => SoundBundleList.Count;



}