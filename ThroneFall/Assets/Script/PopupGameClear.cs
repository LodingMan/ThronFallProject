using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGameClear : BasePopup
{
    public Action OnClickJoinLobby;
    public Action OnClickRestart;

    public void Initialize(Action resetGameAction)
    {
        OnClickRestart = resetGameAction;
    }
    public override void OpenPopup()
    {
        base.OpenPopup();
    }
    public void GoLobby()
    {
        SceneLoader.Instance.LoadSingleScene("LobbyScene");
        ClosePopup();    
    }

    public void Restart()
    {
        OnClickRestart?.Invoke();
        ClosePopup();
    }
}
