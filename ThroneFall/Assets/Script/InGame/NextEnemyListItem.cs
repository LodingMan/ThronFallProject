using System;

using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class NextEnemyListItem : BaseItemListItem<EnemyCountData>
{
    public Image ItemImage;
    public TMP_Text lbCount;
    public int Count;
    
    public override void SetData(EnemyCountData data, Action<EnemyCountData> callback)
    {
        base.SetData(data,callback);
        ItemImage.sprite = AddressablesManager.GetAsset<Sprite>(data.EnemyIconName);
        lbCount.text = data.Count.ToString();
    }
}
