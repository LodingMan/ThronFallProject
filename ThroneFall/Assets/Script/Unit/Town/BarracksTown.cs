using System;
using System.Collections;
using System.Collections.Generic;
using Microlight.MicroBar;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BarracksTown : Town
{
    //public Spawner unitSpawner;
    [SerializeField] private int _spawnCoolDown;
    private float currentProgress;

    private UnitData _townUnitData;
    [SerializeField] private string _townUnitID;
    [SerializeField]private List<Unit> _spawnUnits = new();
    private ObjectPooler _pooler;
    
    [SerializeField] private List<Transform> _defaultSpawnPoints = new();
    private Action<UnitLifecycleInfo> _onUnitLifecycleEvent;
    private int _spawnAbleCount = 4;
    
    private void Update()
    {
        if(_spawnUnits.Count < _spawnAbleCount)
        {
            currentProgress += Time.deltaTime;
            if (currentProgress >= _spawnCoolDown)
            {
                currentProgress = 0f;
                if (_townUnitData != null)
                {
                    CreateUnit(_townUnitData, _defaultSpawnPoints[_spawnUnits.Count].position);
                }
            }
        }
        else
        {
            currentProgress = 0f;
        }
    }

    public override void Initialize(TownData townData)
    {
        base.Initialize(townData);
    }
    
    public override void TownCreate()
    {
        base.TownCreate();
        _townUnitData = MainController.Instance.CSVDataContaner.UnitDatas.Find(u => u.UnitID == _townUnitID);
        if (_spawnAbleCount > _defaultSpawnPoints.Count)
        {
            return;
        }
        for (int i = _spawnUnits.Count; i < _spawnAbleCount; i++)
        {
            CreateUnit(_townUnitData, _defaultSpawnPoints[i].position);
        }

        var arrow = GetComponentInChildren<GuideArrow>();
        if (arrow != null)
        {
            arrow.RequestNextPhase();
        }
    }
    public override void TownBreak()
    {
        base.TownBreak();

    }

    public void BarracksUnitLifecycleEvent(UnitLifecycleInfo info)
    {
        if (info.unitLifecycleEventType == GameEnums.EUnitLifecycleEventType.Created)
        {
            _spawnUnits.Add(info.unit);
        }
        else if (info.unitLifecycleEventType == GameEnums.EUnitLifecycleEventType.Destroyed)
        {
            StartCoroutine(WaitForUnitDeath(info.unit));
            
            _spawnUnits.Remove(info.unit);
        }
    }
    
    public void CreateUnit(UnitData unitData,Vector3 spawnPoint)
    {
        Debug.Log($"startCreateUnit{unitData.UnitID}");
        StartCoroutine(SpawnUnitWhenNavMeshReady(unitData, spawnPoint));
    }
    
    private IEnumerator SpawnUnitWhenNavMeshReady(UnitData unitData,Vector3 spawnPoint)
    {
        Unit createUnit;

        // NavMesh가 로드될 때까지 대기
        while (!IsNavMeshReady())
        {
            yield return null; // 다음 프레임까지 대기
        }
        // NavMesh 위의 유효한 위치 찾기
        NavMeshHit hit;
        
        if (NavMesh.SamplePosition(spawnPoint, out hit, 10.0f, NavMesh.AllAreas))
        {
            createUnit = ObjectPooler.instance.GetObjectPool(unitData.UnitID,hit.position,this.transform).GetComponent<Unit>();
            createUnit.GetComponent<NavMeshAgent>().Warp(hit.position); // NavMesh 위로 강제 이동
            createUnit.RegistEvent(BarracksUnitLifecycleEvent);
            createUnit.Initialize(unitData);
        }
        else
        {
            Debug.LogError("NavMesh 위에서 생성할 수 있는 유효한 위치를 찾을 수 없음!");
        }
    }
    private bool IsNavMeshReady()
    {
        return NavMesh.CalculateTriangulation().vertices.Length > 0;
    }
    
    private IEnumerator WaitForUnitDeath(Unit unit)
    {
        yield return new WaitForSeconds(1f);
        var pool = unit.GetComponent<PoolObject>();
        if(pool != null)
        {
            ObjectPooler.ReturnPool(pool);
        }
        else
        {
            Destroy(unit);
        }
    }
}
