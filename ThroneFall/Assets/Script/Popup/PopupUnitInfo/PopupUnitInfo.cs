using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static GameConfig;

public class PopupUnitInfo : BasePopup
{
    public UnitData SelectUnitData;
    public Image imgTown;
    public TMP_Text lbTownName;
    public TMP_Text lbHP;

    public TMP_Text lbNotAttackTown;
    public TMP_Text lbDamage;
    public TMP_Text lbAttackSpeed;
    public TMP_Text lbAttackRange;
    public TMP_Text lbTier;
    
    public override void OpenPopup()
    {
        base.OpenPopup();
    }

    public void Initialize(UnitData selectUnitData)
    {
        imgTown.sprite = AddressablesManager.GetAsset<Sprite>(selectUnitData.IconName);
        SelectUnitData = selectUnitData;
        lbTownName.text = selectUnitData.UnitName;
        lbTier.text = $"Tier : {selectUnitData.Tier.ToString()}";
        lbHP.text =$"HP : {selectUnitData.Hp}";
        
        
        if(selectUnitData.Damage > 0 && selectUnitData.AttackRange > 0)
        {
            lbAttackRange.gameObject.SetActive(true);
            lbAttackRange.text = $"Range : {selectUnitData.AttackRange.ToString()}";
            lbDamage.gameObject.SetActive(true);
            lbDamage.text = $"Damage : {selectUnitData.Damage.ToString()}";
            lbAttackSpeed.gameObject.SetActive(true);
            lbAttackSpeed.text = $"AttackSpeed : {selectUnitData.AttackCoolDown.ToString()}";
        }
        else
        {
            lbNotAttackTown.gameObject.SetActive(true);
            lbAttackRange.gameObject.SetActive(false);
            lbDamage.gameObject.SetActive(false);
            lbAttackSpeed.gameObject.SetActive(false);
        }
        
    }

    public override void ClosePopup()
    {
        base.ClosePopup();
    }


}
