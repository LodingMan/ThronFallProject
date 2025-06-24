using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVStageUnitPoolDataParser : IParser<StageUnitPoolData>
{
    public List<StageUnitPoolData> Parse(string[] lines)
    {
        List<StageUnitPoolData> unitPoolDatas = new();
        string[] unitNames = lines[0].Split(",");
        for (int i = 1; i < lines.Length; i++)
        {
            var coloms = lines[i].Split(",");
            StageUnitPoolData stageUnitPoolData = new();
            stageUnitPoolData.Stage =  int.Parse(coloms[0]);
            for(int j = 1; j < unitNames.Length; j++)
            {
                stageUnitPoolData.unitPoolDatas.Add(new SpawnUnitData()
                {
                    UnitID = unitNames[j],
                    isSpawn = Convert.ToBoolean(int.Parse(coloms[j])),
                });
                Debug.Log($"UnitPoolData : {stageUnitPoolData.Stage}/{unitNames[j]}/{coloms[j]}");
            } 
            unitPoolDatas.Add(stageUnitPoolData);
        }
        return unitPoolDatas;
    }

}
