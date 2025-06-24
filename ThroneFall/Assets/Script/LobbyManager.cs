using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Call;
    [FormerlySerializedAs("SekectStageObjects")] public List<GameObject> SelectStageObjects = new();
    
    
    private void Start()
    {
        Call = this;
        Refresh();
        if (SaveDataManager.SaveData.IsJoinTutorials==false)
        {
            JoinInGame(0);
            SaveDataManager.SaveData.IsJoinTutorials = true;
            SaveDataManager.Save();
        }
        else
        {
            AudioController.instance.PlaySound("Bgm_Lobby", SoundConfig.SoundType.Bgm);
        }

    }

    public void JoinInGame(int stage)
    {
        GameConfig.CurrentSelectStage = stage;
        SceneLoader.Instance.StartLoadSceneAsync("InGameScene", () =>
        {
             FindObjectOfType<InGameComponentBinder>().StartBind();
        }, true);
    }

    public void Refresh()
    {
        RefreshSelectStageObj();
    }

    public void RefreshSelectStageObj()
    {
        for (int i = 1; i <= GameConfig.MAX_STAGE-1; i++)
        {
            if (i <= SaveDataManager.SaveData.LastClearStage + 1)
            {
                SelectStageObjects[i - 1].SetActive(true);
            }
            else
            {
                SelectStageObjects[i - 1].SetActive(false);
            }
        }
    }

    public void ClickStageTown(int stage)
    {
        PopupController.Instance.OpenPopup<PopupSelectStageConfirm>("PopupSelectStageConfirm",
            popup =>
            {
                popup.Initialize(stage,JoinInGame);
                popup.OpenPopup();
            });
    }
}
