using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupMenu : BasePopup
{
   [SerializeField] private GameObject LobbyMenu;
   [SerializeField] private GameObject InGameMenu;


   public void Initialize()
   {
       string sceneName = SceneManager.GetActiveScene().name;
        bool isLobby = sceneName == "LobbyScene";
        LobbyMenu.SetActive(isLobby);
        InGameMenu.SetActive(!isLobby);
   }

    public void OnClickTitle()
    {
        SceneLoader.Instance.LoadSingleScene("TitleScene");
        ClosePopup();
    }

    public void OnClickLobby()
    {
        SceneLoader.Instance.LoadSingleScene("LobbyScene");
        ClosePopup();
    }

    public void OnClickSetting()
    {
        PopupController.Instance.OpenPopup<PopupSetting>("PopupSetting", popup =>
        {
            popup.Initialize();
            popup.OpenPopup();
        });
    }

    public void OnClickRestart()
    {
        SceneLoader.Instance.StartLoadSceneAsync(
            "InGameScene", () =>
                FindObjectOfType<InGameComponentBinder>().StartBind(), true
        );
        ClosePopup();
    }
}
