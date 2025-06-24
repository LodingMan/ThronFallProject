using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundPairAsset", menuName = "Scriptable/SoundPair")]
public class SoundPairSet : ScriptableObject
{
    [Serializable]
    public class SoundPair
    {
        public string key;
        public AudioClip clip;
    }

    [SerializeField] private SoundConfig.SoundType soundType;
    [SerializeField] private List<SoundPair> pairList = new();

    public AudioClip GetClip(string key)
    {
        var item = pairList.Find(pair => pair.key == key);
        return item?.clip;
    }

    public AudioClip GetClip(Enum value)
    {
        return GetClip(value.ToString());
    }
    
    public AudioClip GetClip(int value)
    {
        return GetClip(value.ToString());
    }

    public bool TryGetClip(out AudioClip clip, string key)
    {
        clip = pairList.Find(p => p.key == key).clip;
        return clip != null;
    }
    
    public AudioClip GetClipWithIndex(int index)
    {
        if (0 <= index && index < pairList.Count)
        {
            return pairList[index] == null ? null : pairList[index].clip;
        }
        else
        {
            return null;
        }
    }

    public SoundConfig.SoundType GetSoundType => this.soundType;


}