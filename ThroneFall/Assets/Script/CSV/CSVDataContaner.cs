using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
[Serializable]
public class ParserContext
{
    public CSVUnitDataParser unitDataParser = new();
    public CSVStageDataParser stageDataParser = new();
    public CSVTownDataParser townDataParser = new();
    public CSVStageUnitPoolDataParser stageUnitPoolDataParser = new();
}

[Serializable]
public class CSVDataContaner
{
    public List<UnitData> UnitDatas;
    public List<StageData> StageDatas;
    public List<TownData> TownDatas;
    public List<StageUnitPoolData> UnitPoolDatas;
    public void Initialize()
    {
        var context = new ParserContext();
        UnitDatas = context.unitDataParser.Parse(CSVLoader.Load("UnitData.csv"));
        for(int i = 0; i< GameConfig.MAX_STAGE; i++)
        {
            var stage = context.stageDataParser.Parse(CSVLoader.Load($"Stage{i}Data.csv"));
            stage.stage = i;
            StageDatas.Add(stage);
        }
        TownDatas = context.townDataParser.Parse(CSVLoader.Load("TownData.csv"));
        UnitPoolDatas = context.stageUnitPoolDataParser.Parse(CSVLoader.Load("StageUnitListData.csv"));
    }
}
