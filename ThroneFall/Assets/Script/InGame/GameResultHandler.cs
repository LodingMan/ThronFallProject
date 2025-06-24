using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class GameResultHandler : MonoBehaviour,
    IInitialize,
    IEventRegist<EGameResult>,
    IMultiEventHandlerProvider
{
    private int _maxRound;
    private Action<EGameResult> _onGameResult;
    
    private Dictionary<Type, Delegate> _eventProviderDic = new();
    public Dictionary<Type, Delegate> GetEventProviderCallBackDic()
    {
        return _eventProviderDic;
    }
    public void Initialize()
    {
        _eventProviderDic[typeof(EGameResult)] = new Action<EGameResult>(GameResultCallbackEvent);
    }

    public void GameResultCallbackEvent(EGameResult value)
    {
        switch (value)
        {
            case EGameResult.CombatStart:
                NotifyCombatStart();
                break;
            case EGameResult.RoundClear:
                NotifyRoundClear();
                break;
            case EGameResult.GameStart:
                NotifyGameStart();
                break;
            case EGameResult.GameOver:
                NotifyGameOver();
                break;
            case EGameResult.GameClear:
                NotifyGameClear();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(value), value, null);
        }
        GameResultEventBus.Publish(value);

    }

    public void NotifyRoundClear()
    {
        _onGameResult.Invoke(EGameResult.RoundClear);
        AudioController.instance.Stop(SoundConfig.SoundType.Bgm);
        AudioController.instance.PlaySound("Round_Clear", SoundConfig.SoundType.Effect);
    }
    public void NotifyCombatStart()
    {
        _onGameResult.Invoke(EGameResult.CombatStart);
        AudioController.instance.Stop(SoundConfig.SoundType.Bgm);
        AudioController.instance.PlaySound("Combat_Start", SoundConfig.SoundType.Effect);
        AudioController.instance.PlaySound("Bgm_InGame", SoundConfig.SoundType.Bgm);

    }
    public void NotifyGameStart()
    {
        _onGameResult.Invoke(EGameResult.GameStart);
    }
    public void NotifyGameOver()
    {
        _onGameResult.Invoke(EGameResult.GameOver);
        PopupController.Instance.OpenPopup<PopupGameOver>("PopupGameOver",
            (popup) =>
            {
                popup.Initialize(() =>
                {
                    SceneLoader.Instance.StartLoadSceneAsync(
                        "InGameScene", () =>
                            FindObjectOfType<InGameComponentBinder>().StartBind()
                        , true
                    );
                });
                popup.OpenPopup();
                AudioController.instance.Stop(SoundConfig.SoundType.Bgm);
                AudioController.instance.PlaySound("GameOver", SoundConfig.SoundType.Effect);

            });
        
    }
    public void NotifyGameClear()
    {
        _onGameResult.Invoke(EGameResult.GameClear);
        PopupController.Instance.OpenPopup<PopupGameClear>("PopupGameClear",
            (popup) =>
            {
                popup.Initialize(()=>
                {
                    SceneLoader.Instance.StartLoadSceneAsync(
                        "InGameScene",()=>        
                        FindObjectOfType<InGameComponentBinder>().StartBind()
                        , true
                    );
                });
                popup.OpenPopup();
                AudioController.instance.Stop(SoundConfig.SoundType.Bgm);
                AudioController.instance.PlaySound("GameClear", SoundConfig.SoundType.Effect);
            });
        SaveDataManager.SetLastClearStage(GameConfig.CurrentSelectStage);
        if (GameConfig.CurrentSelectStage == 0)
        {
            SaveDataManager.SaveData.IsJoinTutorials = true;
            SaveDataManager.Save();
        }
    }


    public Action RegistEvent(Action<EGameResult> callback)
    {
        _onGameResult += callback;
        return ()=> UnRegistEvent(callback);
    }

    public void UnRegistEvent(Action<EGameResult> callback)
    {
        _onGameResult -= callback;
    }

    private Action _unRegistCallback;
    private void OnDestroy()
    {
        _unRegistCallback?.Invoke();
    }
    public void SetEventUnRegistCallback(Action callback)
    {
        _unRegistCallback = callback;
    }
}
