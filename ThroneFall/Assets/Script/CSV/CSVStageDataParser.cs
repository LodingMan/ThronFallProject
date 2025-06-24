using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVStageDataParser : ISingleParser<StageData>
{
    public StageData Parse(string[] lines)
    {
        var titles = lines[0].Split(",");
        var stage = new StageData(); 
        for (int j = 1; j < lines.Length; j++)
        {
            var coloms = lines[j].Split(",");

            RoundData rd = new();
            rd.round = int.Parse(coloms[0]);
            for (int k = 1; k < titles.Length-1; k++)
            {
                var enemyNameAndIndex = titles[k].Split("/");
                string enemyID = enemyNameAndIndex[0];
                int enemyIndex = int.Parse(enemyNameAndIndex[1]);
                var enemyCount = int.Parse(coloms[k]);
                rd.enemyCountInfo.Add(new Tuple<string,int,int>(enemyID, enemyIndex, enemyCount));
            }
            rd.clearRewardCoin = int.Parse(coloms[titles.Length-1]);
            stage.roundDatas.Add(rd);
        }
        return stage;
    }

}