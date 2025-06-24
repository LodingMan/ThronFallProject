using System;
using System.Collections.Generic;
using Microlight.MicroBar;
using UnityEngine;
using static GameEnums;

public class Health : MonoBehaviour, IHealthProvider
{
    [SerializeField] private MicroBar HpBar;
    public Vector3 GetPosition => transform.position;
    [SerializeField]
    private float viewHP;
    public float CurrentHP
    {
        get => HpBar.CurrentValue;
        private set
        {
            if (HpBar != null)
            {
               HpBar.UpdateBar(value);
               //SetActive(true);
               viewHP = value;
               if (HpBar.HPPercent == 0)
               {
                   SetActive(false);
               }
            }
        }
    }

    private void Awake()
    { 
        HpBar = GetComponentInChildren<MicroBar>();
    }
    
    public void Initialize(float maxHp)
    {
        if(HpBar != null)
        {
            HpBar.Initialize(maxHp);
            HpBar.gameObject.SetActive(false);
        }
    }

    public void SetFullHealth()
    {
        CurrentHP = HpBar.MaxValue;
    }

    public void SetHP(float hp)
    {
        CurrentHP = hp;
    }
    public void PlusHP(float hp)
    {
        CurrentHP += hp;
    }
    public void MinusHP(float hp)
    {
        CurrentHP -= hp;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
        }
    }

    public void SetActive(bool isActive)
    {
        HpBar.gameObject.SetActive(isActive);
    }


}
