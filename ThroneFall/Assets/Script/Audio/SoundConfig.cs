using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class SoundConfig
    {
        //private static string[] _bgmSourcePath = null; 
        
        // public static void InitBgmSourcePath() 
        // {
        //     _bgmSourcePath = new string[(int)BGM_LIST.COUNT]; 
        //
        //     _bgmSourcePath[(int)BGM_LIST.LOBBY] = "Sound/Bgm/Bgm_Lobby"; 
        //     _bgmSourcePath[(int)BGM_LIST.BATTLE_STAGE] = "Sound/Bgm/Bgm_Stage_Battle"; 
        //     _bgmSourcePath[(int)BGM_LIST.FREET] = "Sound/Bgm/Bgm_Freet"; 
        // }
        public static bool isMute = false;
        public static float BgmVolume = 0.5f;
        public static float EffectVolume = 0.5f;


        public static void StartVolume()
        {
            BgmVolume = PlayerPrefs.GetFloat("BgmVolume",0.5f);
            EffectVolume = PlayerPrefs.GetFloat("EffectVolume",0.5f);
            SetVolume(SoundType.Bgm, BgmVolume);
            SetVolume(SoundType.Effect, EffectVolume);
        }

        public static void SetVolume(SoundType soundType, float value)
        {
            switch (soundType)
            {
                case SoundType.Bgm:
                    BgmVolume = value;
                    PlayerPrefs.SetFloat("BgmVolume", value);
                    AudioController.instance.SetVolume(SoundType.Bgm,value);

                    break;
                case SoundType.Effect:
                case SoundType.Effect2:
                    EffectVolume = value;
                    PlayerPrefs.SetFloat("EffectVolume", value);
                    AudioController.instance.SetVolume(SoundType.Effect,value);
                    AudioController.instance.SetVolume(SoundType.Effect2,value);
                    break;
                case SoundType.Count:
                    break;
                case SoundType.Max:
                    break;
                default:
                    break;
            }

        }

        public enum SoundType
        {
            Bgm = 0,
            Effect,
            Effect2,
            Count,
            Max
        }
    
        public enum BGM_LIST
        {
            INTRO = -1,
            LOBBY,
            INGAME,
        }
    } 