using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks.Triggers;
using Unity.VisualScripting;
using UnityEngine;
using static GameEnums;
using Random = UnityEngine.Random;

public class SpawnerDataContext
{
    public StageData stageData;
    public List<UnitData> unitDatas;

}
public class StageSpawnerHandler : MonoBehaviour,
    IInitialize<SpawnerDataContext>,
    IEventRegist<UnitLifecycleInfo>,
    IEventHandlerProvider<int>
{
    [SerializeField] List<Spawner> _enemySpawners = new();
    private SpawnerDataContext _spawnerDataContext;

    
    public void Initialize(SpawnerDataContext context)
    {
        _spawnerDataContext = context;
        _enemySpawners = GameObject.FindObjectsOfType<Spawner>().ToList();
        for (int i = 0; i < _enemySpawners.Count; i++)
        {
            _enemySpawners[i].Initialize( new SpawnerDataContext(){
                stageData = context.stageData,
                unitDatas = context.unitDatas});
        }
    }
    
    public void RegistEnemys(int currentRound)
    {
        int currentAssignDropCoin = 0;

        var infoList = _spawnerDataContext.stageData.roundDatas[currentRound-1].enemyCountInfo;
        infoList.RemoveAll(info => info.Item3 == 0);
        var roundReward = _spawnerDataContext.stageData.roundDatas[currentRound - 1].clearRewardCoin;
        for (int i = 0; i < infoList.Count; i++)
        {
            Spawner spawner = FindSpawnerAtIndex(infoList[i].Item2);
            for (int j = 0; j < infoList[i].Item3; j++)
            {
                int num = roundReward - currentAssignDropCoin;
                int dropCoin = Random.Range(0, Mathf.Min(3, num+1));
                Debug.Log($"AddSpanwer : {infoList[i].Item2} : {_enemySpawners.Count}");
                currentAssignDropCoin += dropCoin;
                if (i == infoList.Count - 1 && j == infoList[i].Item3 - 1)
                {
                    if (currentAssignDropCoin != roundReward)
                    {
                        dropCoin = roundReward - currentAssignDropCoin;
                    }
                }
                spawner.RegistStandbyUnit(infoList[i].Item1, dropCoin);
            }
            //spawner.ShowNextUnits();
        }
    }
    public Spawner FindSpawnerAtIndex(int index)
    {
        return _enemySpawners.Find(s => s.Index == index);
    }
    
    public List<SpawnerTransformData> GetSpawnerTransformData()
    {
        List<SpawnerTransformData> spawnerTransformDatas = new();
        foreach (var enemySpawner in _enemySpawners)
        {
            spawnerTransformDatas.Add(new SpawnerTransformData()
            {
                SpawnerTransform = enemySpawner.transform,
                SpawnerIndex = enemySpawner.Index
            });
        }
        return spawnerTransformDatas;
    }
    

    public void CallbackEvent(int round)
    {
        RegistEnemys(round);
    }
    public Action RegistEvent(Action<UnitLifecycleInfo> callback)
    {
        for(int i = 0; i < _enemySpawners.Count; i++)
        {
            _enemySpawners[i].RegistEvent(callback);
        }

        return () => UnRegistEvent(callback);
    } 
    public void UnRegistEvent(Action<UnitLifecycleInfo> callback) 
    {
        for(int i = 0; i < _enemySpawners.Count; i++)
        {
            _enemySpawners[i].UnRegistEvent(callback);
        }
    }
    
    private Action _unRegistCallback;
    private void OnDestroy()
    {
        _unRegistCallback?.Invoke();
    }

    public void SetEventUnRegistCallback(Action callback)
    {
        _unRegistCallback += callback;
    }


}
