using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCreatorDataContext
{
    public List<UnitData> unitDatas;
}
public class PlayerCreator : MonoBehaviour,
    IInitialize<List<UnitData>>
{
    [SerializeField] private Transform trStart;
    [SerializeField] private InGamePlayer _player;
    private UnitData _playerData;
    public void Initialize(List<UnitData> unitDatas )
    {
        _playerData = unitDatas.Find(u => u.UnitID == "Player");
        SpawnPlayer();
    }
    public void SpawnPlayer()
    {
        StartCoroutine(SpawnPlayerWhenNavMeshReady());
    }
    
    private IEnumerator SpawnPlayerWhenNavMeshReady()
    {
        while (!IsNavMeshReady())
        {
            yield return null; 
        }

        Vector3 spawnPosition = trStart.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(spawnPosition, out hit, 10.0f, NavMesh.AllAreas))
        {
            var createdPlayer = Instantiate(_player, hit.position, Quaternion.identity,this.transform);
            createdPlayer.GetComponent<NavMeshAgent>().Warp(hit.position); 
            yield return null; // 한 프레임 기다리기
            createdPlayer.Initialize(_playerData);
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
}
