using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class TownSelectListItem : BaseItemListItem<TownSelectData>
{
    private Button _button;
    private Image _image;
    [SerializeField] private TMP_Text _lbTownName;
    [SerializeField] private TMP_Text _lbTownPrice;
    private TownSelectData itemData;

    private void Awake()
    {
        var popup = GetComponentInParent<PopupTownBuildSelect>();
    }

    public override void SetData(TownSelectData data, Action<TownSelectData> callback)
    {
        base.SetData(data,callback);
        itemData = data;
        _button = GetComponent<Button>();
        _image = GetComponentInChildren<Image>();
        
        _button.interactable = data.IsSelectable;

        var spr = AddressablesManager.GetAsset<Sprite>(data.TownData.IconName);
        _image.sprite = spr;
        _lbTownName.text = data.TownData.UnitName;
        _lbTownPrice.text = $"{data.TownData.Price}Coin";
        
    }
    public void RegistAction()
    {
        
    }
    
    public void OnClick()
    {
        Callback.Invoke(itemData);
    }
    
}
