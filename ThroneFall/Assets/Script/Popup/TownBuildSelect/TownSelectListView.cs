using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TownSelectListView : BaseItemListView<TownSelectData, TownSelectListItem>
{
    public override void SetItems(List<TownSelectData> dataList, Action<TownSelectData> callback = null)
    {
        base.SetItems(dataList,callback);
    }
}
