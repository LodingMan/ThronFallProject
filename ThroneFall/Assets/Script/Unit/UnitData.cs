using System;
using UnityEngine.Serialization;

[Serializable]
public class UnitData
{
    public string UnitID;
    public int Tier;
    public float Hp;
    public float Speed;
    public float Damage;
    public float AttackCoolDown;
    public float AttackRange;
    public string UnitName;
    public string IconName;
    
    public int DropCoin;


    public static UnitData SampleUnitData()
    {
        return new UnitData()
        {
            Tier = 1,
            Hp = 100,
            Speed = 15,
            Damage = 15,
            AttackCoolDown = 3,
            UnitName = "Warrior1",
            
        };
    }
    public UnitData()
    {
    }

    public UnitData(UnitData Data)
    {
        UnitID = Data.UnitID;
        Tier = Data.Tier;
        Hp = Data.Hp;
        Speed = Data.Speed;
        Damage = Data.Damage;
        AttackCoolDown = Data.AttackCoolDown;
        AttackRange = Data.AttackRange;
        UnitName = Data.UnitName;
        IconName = Data.IconName;
        DropCoin = Data.DropCoin;
    }

    public UnitData Clone() => (UnitData)this.MemberwiseClone();

}