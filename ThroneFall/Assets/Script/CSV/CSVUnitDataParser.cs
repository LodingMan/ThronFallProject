using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVUnitDataParser : IParser<UnitData>
{
    public List<UnitData> Parse(string[] lines)
    {
        List<UnitData> unitDatas = new();
        for (int i = 1; i < lines.Length; i++)
        {
            var coloms = lines[i].Split(",");
            unitDatas.Add(
                new UnitData()
                {
                    UnitID = coloms[0],
                    Tier = int.Parse(coloms[1]),
                    Hp = float.Parse(coloms[2]),
                    Speed = float.Parse(coloms[3]),
                    Damage = float.Parse(coloms[4]),
                    AttackCoolDown = float.Parse(coloms[5]),
                    AttackRange = float.Parse(coloms[6]),
                    UnitName = coloms[7],
                    IconName = coloms[8]
                });

        }
        return unitDatas;
    }
}