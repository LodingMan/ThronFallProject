using System;
using System.Collections;
using System.Collections.Generic;
using static GameConfig;
using UnityEngine;
using UnityEngine.AI;

public class LobbyPlayer : Unit
{
    PlayerInputHandler _playerInputHandler;

    protected void Awake() 
    {
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        var agent = GetComponent<NavMeshAgent>();
        if (SaveDataManager.SaveData.LastStandingPos != Vector3.zero)
        {
            agent.Warp(SaveDataManager.SaveData.LastStandingPos);
        }
    }

    private void Update()
    {
        if (SaveDataManager.SaveData.LastStandingPos != transform.position)
        {
            SaveDataManager.SetLastStandingPos(transform.position);
        }
    }


    private void Start()
    {
        Initialize(new UnitData(){Speed = 15f});
    }



    public override void Initialize(UnitData unitData)
    {
        base.Initialize(unitData);
        _playerInputHandler.Initialize(_move);
        EventRegistHelper.RegistCallbackEvent(_playerInputHandler, _move);

    }

    public override void Die()
    {
        base.Die();
        Destroy(this);
    }
}
