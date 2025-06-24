using System;
using UnityEngine.Serialization;

[Serializable]
public class TownData
{
    public string TownID;
    public int Tier;
    public float Hp;
    //public float Speed;
    public float Damage;
    public float AttackCoolDown;
    public float AttackRange;
    public string UnitName;
    public string IconName;
    public string Infomation;


    public int Price;

    public static TownData SampleTownData()
    {
        return new TownData()
        {
            Tier = 1,
            Hp = 100,
            Damage = 15,
            AttackCoolDown = 3,
            UnitName = "Warrior1",
            IconName = "TownImg_Commander",
        };
    }
    
    public TownData()
    {
    }

    public TownData(TownData Data)
    {
        TownID = Data.TownID;
        Tier = Data.Tier;
        Hp = Data.Hp;
        //Speed = Data.Speed;
        Damage = Data.Damage;
        AttackCoolDown = Data.AttackCoolDown;
        AttackRange = Data.AttackRange;
        UnitName = Data.UnitName;
        IconName = Data.IconName;
        Price = Data.Price;
    }
}