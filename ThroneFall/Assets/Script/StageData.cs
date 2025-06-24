using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class StageData
{
     public int stage;
     public List<RoundData> roundDatas = new();
    
    public int GetRoundReward(int round)
    {
        if (round <= 0 || round >= roundDatas.Count)
        {
            return 0;
        }
        RoundData findData = roundDatas.Find(r=> r.round == round);
        if (findData == null)
        {
            return 0;
        }

        return findData.clearRewardCoin;
    }
    public int GetRoundEnemyCount(int round)
    {
        if (round < 0 || round > roundDatas.Count)
        {
            return 0;
        }
        int enemyCount = 0;
        foreach (var tuple in roundDatas[round -1].enemyCountInfo)
        {
            enemyCount += tuple.Item3;
        }
        return enemyCount;
    }
    public List<(string ,int)> GetRoundEnemyInfo(int round, int spawnerIndex)
    {
        var  findEnemys = roundDatas[round -1].enemyCountInfo.FindAll(s => s.Item2 == spawnerIndex);
        List<(string, int)> enemyInfo = new();
        foreach (var findEnemy in findEnemys)
        {
            if (findEnemy.Item3 != 0)
            {
                enemyInfo.Add((findEnemy.Item1, findEnemy.Item3));
            }
        }
        return enemyInfo;
    }
}

[Serializable]
public class RoundData
{
    public int round;
    public int clearRewardCoin;
    public List<Tuple<string, int, int>> enemyCountInfo = new(); //enemyID, enemyIndex, enemyCount
    
    //public Dictionary<string, int> dicUnitCount = new();
}
