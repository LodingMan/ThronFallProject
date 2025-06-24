using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static GameEnums;

public class UnitLifecycleInfo
{
    public Unit unit;
    public EUnitLifecycleEventType unitLifecycleEventType;
}

public class Spawner : MonoBehaviour,
    IInitialize<SpawnerDataContext>,
    IEventRegist<UnitLifecycleInfo>
{
    public ListQueue<UnitData> _standbyUnits = new();
    private SpawnerDataContext _context;
    [SerializeField] private EGameState _currentGameState;

    public int Index;
    public bool isInitialized = false;

    private Action<UnitLifecycleInfo> _onUnitLifecycleEvent;


    public float CoolDown = 5f;
    public float lastSpawnTime;
    private void Update()
    {
        if (isInitialized == false) return;
        if (Time.time - lastSpawnTime >= CoolDown)
        {
            if (_standbyUnits.Count != 0)
            {
                if (_currentGameState == EGameState.Combat)
                {
                    CreateUnit(_standbyUnits.Dequeue());
                }
                lastSpawnTime = Time.time;

            }
        }
    }

    public void Initialize(SpawnerDataContext context)
    {
        _context = context;
        GameStateEventBus.RegistEvent(CallbackEvent);
        isInitialized = true;
    }

    public void Reset()
    {
        _standbyUnits.Clear();
    }

    public void ShowNextUnits() //옮겨야됨
    {
        List<EnemyCountData> items = new();
        for (int i = 0; i < _standbyUnits.Count; i++)
        {
            string iconName = _standbyUnits[i].IconName;
            int index = items.FindIndex(s => s.EnemyIconName == iconName);
            if (index == -1)
            {
                items.Add(new EnemyCountData() { EnemyIconName = iconName, Count = 0 });
                ++items[items.Count - 1].Count;
            }
            else
            {
                ++items[index].Count;
            }
        }
    }


    public void RegistStandbyUnit(string unitID, int dropCoin)
    {
        var unitData = _context.unitDatas.Find(m => m.UnitID == unitID);
        var copiedData = unitData.Clone();
        copiedData.DropCoin = dropCoin;
        _standbyUnits.Enqueue(copiedData);
    }

    public void CreateUnit(UnitData unitData)
    {
        Debug.Log($"startCreateUnit{unitData.UnitID}");

        StartCoroutine(SpawnUnitWhenNavMeshReady(unitData));
    }

    private IEnumerator SpawnUnitWhenNavMeshReady(UnitData unitData)
    {
        Unit createUnit;

        while (!IsNavMeshReady())
        {
            yield return null; 
        }
        NavMeshHit hit;

        if (NavMesh.SamplePosition(this.transform.position, out hit, 10.0f, NavMesh.AllAreas))
        {
            createUnit = ObjectPooler.instance.GetObjectPool(unitData.UnitID, hit.position, this.transform).GetComponent<Unit>();
            createUnit.GetComponent<NavMeshAgent>().Warp(hit.position); 
            createUnit.RegistEvent(_onUnitLifecycleEvent);
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


    public Action RegistEvent(Action<UnitLifecycleInfo> callback)
    {
        _onUnitLifecycleEvent += callback;
        return () => UnRegistEvent(callback);
    }

    public void UnRegistEvent(Action<UnitLifecycleInfo> callback)
    {
        _onUnitLifecycleEvent -= callback;
    }

    public void CallbackEvent(EGameState value)
    {
        _currentGameState = value;
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
