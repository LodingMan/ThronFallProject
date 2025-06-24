using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstancePanelTownInfo : MonoBehaviour
{
    [SerializeField]private TMP_Text lbTownName;
    [SerializeField]private TMP_Text lbTownInfomation;
    [SerializeField]private TMP_Text lbTownPrice;
    [SerializeField]private TMP_Text lbTownHp;
    [SerializeField]private Transform _trEnable;
    [SerializeField]private Transform _trDisable;

    private void Awake()
    {
        transform.position = _trDisable.position;
    }

    public void SetInfo(TownData townData)
    {
        lbTownName.text = townData.UnitName;
        lbTownPrice.text = "Coin : " + townData.Price.ToString();
        lbTownHp.text = "HP : " + townData.Hp.ToString();
        lbTownInfomation.text = townData.Infomation;
    }
    
    public void SetActivePanel(bool isActive)
    {
        if (isActive)
        {
            transform.position = _trEnable.position;
        }
        else
        {
            transform.position = _trDisable.position;
        }
    }
    

}
