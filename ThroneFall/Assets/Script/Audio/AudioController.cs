using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static SoundConfig;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioController : MonoBehaviour,
    IAudioService
{
    public static IAudioService instance;

   public AssetReferenceT<SoundLibrary> refSoundLibrary;
    private SoundLibrary soundLibrary;
    private Dictionary<string, AudioSource> dicAudioSrc = new();
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        if(instance == null)
        {
            instance = this;
        }
        Initialize();
    }

    public void Initialize()
    {
        initAudio();
    }

    private Task<OperationResult<SoundLibrary>> loadTask;
    private void initAudio()
    {
        if (this.gameObject.transform.childCount == 0)
        {
            string[] typeNames = Enum.GetNames(typeof(SoundConfig.SoundType));
            for(int i=0; i<typeNames.Length - 1; i++)
            {
                GameObject obj = new GameObject(typeNames[i]);
                obj.transform.SetParent(this.transform);
                var src = obj.AddComponent<AudioSource>();
                dicAudioSrc.Add(typeNames[i], src);
            }
        }
        SoundConfig.StartVolume();
        loadTask = AddressablesManager.LoadAssetAsync(refSoundLibrary);
    }


     public async Task<bool> PlaySound(string key, SoundConfig.SoundType soundType, bool isLoop = false)
     {
         var tcs = new TaskCompletionSource<bool>();
         if (soundType == SoundType.Bgm)
         {
             Stop(SoundType.Bgm);
         }
        try
        {
            if (soundLibrary == null)
            {
                await loadTask;
                if (loadTask.Result.Succeeded)
                {
                    {
                        soundLibrary = loadTask.Result.Value;
                        tcs.SetResult(true);
                        Play();
                    }
                }
                else
                {
                    Debug.LogWarning("SoundLibrary is Not Loaded");
                    tcs.SetResult(false); 
                }
            }
            else
            {
                Play();
                tcs.SetResult(true);
            }
            return await tcs.Task;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Not Found Audio Clip.");
            return false;
        }
        void Play()
        {
            AudioClip clip = soundLibrary.GetClip(key, soundType);
            var src = dicAudioSrc[soundType.ToString()];
            src.clip = clip;
            switch (soundType)
            {
                case SoundType.Bgm:
                    src.volume = BgmVolume;
                    break;
                case SoundType.Effect:
                case SoundType.Effect2:
                    src.volume = EffectVolume;
                    break;
                case SoundType.Count:
                case SoundType.Max:
                default:
                    break;
            }
            src.mute = isMute;
            if (isLoop)
            {
                src.loop = isLoop;
                src.Play();
            }
            else
            {
                src.loop = false;
                src.PlayOneShot(clip);
            }
            Debug.Log($"PlaySound {key}");
        }
    }

     public void SetVolume(SoundType soundType, float value)
     {
         dicAudioSrc[soundType.ToString()].volume = value;
     }
     
     public void Stop(SoundConfig.SoundType soundType)
     {
         var src = dicAudioSrc[soundType.ToString()];
         Debug.Log($"StopSound {soundType}");
         
         src?.Stop();
     }

}

