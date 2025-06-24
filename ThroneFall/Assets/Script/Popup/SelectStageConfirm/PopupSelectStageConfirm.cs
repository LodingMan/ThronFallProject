using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopupSelectStageConfirm : BasePopup
{
    public StageInfoListView StageInfoListView;

    private StageUnitPoolData StageUnitPoolData;
    //public List<TownSelectData> SelectableTownFlag = new();

    public override void OpenPopup()
    {
        base.OpenPopup();
    }

    private Action<int> JoinGameEvent;
    public void Initialize(int stageNumber, Action<int> joinGameEvent)
    {
        JoinGameEvent = joinGameEvent;
        StageUnitPoolData = MainController.Instance.CSVDataContaner.UnitPoolDatas.Find(s => s.Stage == stageNumber);
        StageInfoListView.SetItems(StageUnitPoolData.unitPoolDatas);
    }

    public void OnClickJoinInGame()
    {
        JoinGameEvent?.Invoke(StageUnitPoolData.Stage);
        //LobbyManager.Call.JoinInGame(StageUnitPoolData.Stage);
        ClosePopup();
    }
}
