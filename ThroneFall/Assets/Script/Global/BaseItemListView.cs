using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BaseItemListView<TData, TItem> : MonoBehaviour where TItem : BaseItemListItem<TData>
{
    public List<TItem> ItemList;
    public AssetReferenceGameObject ItemReference;
    public Transform ItemParent;


    protected Action<TData> Callback;
    public virtual void SetItems(List<TData> dataList ,Action<TData> callback = null)
    {
        Callback = callback;
        foreach (var item in ItemList)
        {
            Destroy(item.gameObject);
        }
        ItemList.Clear();

        foreach (var data in dataList)
        {
            var obj = AddressablesManager.GetAsset<GameObject>(ItemReference);
            var item = Instantiate(obj, ItemParent).GetComponent<TItem>();
            item.SetData(data,callback);
            ItemList.Add(item);
        }

    }

    public virtual void ClearItems()
    {
        foreach (var item in ItemList)
        {
            Destroy(item.gameObject);
        }
        ItemList.Clear();
    }

}
