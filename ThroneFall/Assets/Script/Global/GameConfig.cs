using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public static class GameConfig
{
    
    public const int MAX_STAGE = 3;
    public const int MAX_COIN = 1000;
    public const int GAME_START_COIN = 8;
    public const float TOWN_BUY_DURATION = 3f;
    public const float COMBAT_START_DURATION = 1.5f;
    public static int CurrentSelectStage = 0;
    
    public static readonly Type NoParamAction = typeof(Action);


    public static int ResolutionIndex = -1;
    public static List<Resolution> GameResolutionDatas = new List<Resolution>()
    {
        new Resolution(){width = 1920, height = 1080},
        new Resolution(){width = 1280, height = 720},
    };

    public static void StartResolutionSetting()
    {
        var index = GameResolutionDatas.FindIndex(r =>
        {
            return r.width == Screen.currentResolution.width && r.height == Screen.currentResolution.height;
        });

        ResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", index);
        bool isFullScreen = PlayerPrefs.GetInt("FullScreen", 0) != 0;
        
        if (index == -1)
        {
            Debug.Log("해상도 오류");
            ResolutionIndex = 0;
            PlayerPrefs.SetInt("ResolutionIndex", 0);
            Screen.SetResolution(GameResolutionDatas[ResolutionIndex].width, GameResolutionDatas[ResolutionIndex].height,isFullScreen);
        }
        else
        {
            Screen.SetResolution(GameResolutionDatas[ResolutionIndex].width, GameResolutionDatas[ResolutionIndex].height,isFullScreen);
        }

    }

    public static void SetResolution(int index)
    {
        bool isFullScreen = PlayerPrefs.GetInt("FullScreen", 0) != 0;
        PlayerPrefs.SetInt("ResolutionIndex",index);
        Screen.SetResolution(GameResolutionDatas[index].width, GameResolutionDatas[index].height,isFullScreen);
        ResolutionIndex = index;
    }


    public static Action<T> GetDicAction<T>(Dictionary<Type, Delegate> dictionary)
    {
        if (dictionary.TryGetValue(typeof(T), out var del) && del is Action<T> action)
        {
            return action;
        }
        return null;
    }



    

    
    public static string GetTownImageString(ETownType type)
    {
        switch (type)
        {
            case ETownType.PreTown:
                return "TownImg_PreTown";
            case ETownType.Commander:
                return "TownImg_Commander";
            case ETownType.Turret:
                return "TownImg_Turret";
            default:
                return "None";
        }
    }
}
