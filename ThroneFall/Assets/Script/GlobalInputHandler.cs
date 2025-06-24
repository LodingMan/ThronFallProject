using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalInputHandler : MonoBehaviour, IInputSubscriber
{
    void Start()
    {
        InputManager.RegisterInput(this);
    }

    public void OnInput(InputInfo info)
    {
        switch (info.inputType)
        {
            case GameEnums.EInputType.None:
                break;
            case GameEnums.EInputType.Down:
                if (info.actionName == "Cancel")
                {
                    if (PopupController.Instance.isOpenPopup)
                    {
                        PopupController.Instance.CloseLastOpenPopup();
                    }
                    else
                    {
                        string sceneName = SceneManager.GetActiveScene().name;
                        if (sceneName == "TitleScene")
                        {
                            PopupController.Instance.OpenPopup<PopupSetting>("PopupSetting", popup =>
                            {
                                popup.Initialize();
                                popup.OpenPopup();
                            });
                        }
                        else
                        {
                            PopupController.Instance.OpenPopup<PopupMenu>("PopupMenu", popup =>
                            {
                                popup.Initialize();
                                popup.OpenPopup();
                            });
                        }

                    }
                }
                break;
            case GameEnums.EInputType.Press:
                break;
            case GameEnums.EInputType.Up:
                break;
            default:
                break;
        }
    }
}
