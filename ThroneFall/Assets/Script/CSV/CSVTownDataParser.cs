using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVTownDataParser : IParser<TownData>
{
    public List<TownData> Parse(string[] lines)
    {
        List<TownData> townDatas = new();
        for (int i = 1; i < lines.Length; i++)
        {
            var coloms = lines[i].Split(",");
            townDatas.Add(
                new TownData()
                {
                    TownID = coloms[0],
                    Tier = int.Parse(coloms[1]),
                    Hp = float.Parse(coloms[2]),
                    Damage =float.Parse(coloms[3]), 
                    AttackCoolDown = float.Parse(coloms[4]),
                    AttackRange = float.Parse(coloms[5]),
                    Price = int.Parse(coloms[6]),
                    UnitName = coloms[ 7],
                    IconName = coloms[8],
                    Infomation = coloms[9],
                });
        }
        return townDatas;
    }
}
