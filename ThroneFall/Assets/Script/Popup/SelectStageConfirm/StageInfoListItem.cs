using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class StageInfoListItem : BaseItemListItem<SpawnUnitData>
{
    public UnitData UnitData;
    public Image UnitImage;
    public TMP_Text UnitName;

    public override void SetData(SpawnUnitData data, Action<SpawnUnitData> callback = null)
    {
        base.SetData(data);
        if (data.isSpawn)
        {
            gameObject.SetActive(true);
            UnitData = MainController.Instance.CSVDataContaner.UnitDatas.Find(u => u.UnitID == data.UnitID);
            var spr = AddressablesManager.GetAsset<Sprite>(UnitData.IconName);
            UnitImage.sprite = spr;
            UnitName.text = UnitData.UnitName;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    public void OnClick()
    {
        PopupController.Instance.OpenPopup<PopupUnitInfo>("PopupUnitInfo", (popup) =>
        {
            popup.Initialize(UnitData);
            popup.OpenPopup();
        });
        
    }

}
