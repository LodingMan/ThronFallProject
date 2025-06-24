using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnUnitData
{
    public string UnitID;
    public bool isSpawn;
}
[Serializable]
public class StageUnitPoolData
{
    public int Stage;
    public List<SpawnUnitData> unitPoolDatas = new();
}
