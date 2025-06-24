using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGameOver : BasePopup
{
    private Action ResetGameEvent;
    public void Initialize(Action resetGameEvent)
    {
        ResetGameEvent = resetGameEvent;
        
    }
    public void GoLobby()
    {
        SceneLoader.Instance.LoadSingleScene("LobbyScene");
        ClosePopup();
    }

    public void Restart()
    {
        ResetGameEvent?.Invoke();
        ClosePopup();
    }
}
