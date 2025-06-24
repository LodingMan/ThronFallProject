using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IAudioService
{
    public Task<bool> PlaySound(string key, SoundConfig.SoundType soundType, bool isLoop = false);
    public void SetVolume(SoundConfig.SoundType soundType, float value);
    public void Stop(SoundConfig.SoundType soundType);
}
