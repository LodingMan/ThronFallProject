using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    private void Awake()
    {
        AudioController.instance.PlaySound("Bgm_Title", SoundConfig.SoundType.Bgm);
    }

    public void OnClickNewGame()
    {
        SaveDataManager.ClearSave();
        SceneManager.LoadScene(2);
    }
    public void OnclickLoadGame()
    {
        SaveDataManager.Load();
        SceneLoader.Instance.StartLoadSceneAsync("LobbyScene", () =>
        {
            
        }, true);

        //SceneManager.LoadScene(2);

    }

    public void OnClickSetting()
    {
        PopupController.Instance.OpenPopup<PopupSetting>("PopupSetting", (popup) =>
        {
            popup.Initialize();
            popup.OpenPopup();
        });
    }

    public void OnClickExitGame()
    {
        Application.Quit();
    }
}