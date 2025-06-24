using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using static GameConfig;


public class PopupTownBuildSelect : BasePopup
{

    public Town SelectPreTown;
    TownSelectListView TownSelectListView;
    public List<TownSelectData> SelectableTownFlag = new();
    Action<Town,TownData> OnSelectComplete;

    private void Awake()
    {
        TownSelectListView = GetComponentInChildren<TownSelectListView>();
    }

    public override void OpenPopup()
    {
        base.OpenPopup();
    }

    public void Initialize(Town town, List<TownSelectData> selectableTownFlag, Action<Town,TownData> onSelectComplete = null)
    {
        //var towns = InGameManager.Call.Stage.townManager.GameTowns;
        //SetTownFlag(towns);
        SelectPreTown = town;
        SelectableTownFlag = selectableTownFlag;
        TownSelectListView.SetItems(SelectableTownFlag,TownSelectComplete); //TownDatas 도 필요할겉같음. 
        OnSelectComplete = onSelectComplete;
        
    }
    
    public void TownSelectComplete(TownSelectData selectData)
    {
        OnSelectComplete?.Invoke(SelectPreTown, selectData.TownData);
        ClosePopup();
    }
    
    public override void ClosePopup()
    {
        base.ClosePopup();
    }


}
