using System;
using System.Collections;
using System.Collections.Generic;
using static GameConfig;
using UnityEngine;

public class InGamePlayer : Unit
{
    PlayerInputHandler _playerInputHandler;
    PlayerColliderEventHandler _playerColliderEventHandler;
    protected void Awake()
    {
        var camFollow = Camera.main.GetComponent<CameraFollow>().followTarget;
        if (camFollow == null)
        {
            Camera.main.GetComponent<CameraFollow>().followTarget = this.transform;
        }
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _playerColliderEventHandler = GetComponentInChildren<PlayerColliderEventHandler>();
        _gameCoinHandler = FindObjectOfType<GameCoinHandler>();
    }

    
    public override void Initialize(UnitData unitData)
    {
        base.Initialize(unitData);
        var camFollow = Camera.main.GetComponent<CameraFollow>().followTarget;
        Camera.main.GetComponent<CameraFollow>().followTarget = this.transform;
        _playerInputHandler.Initialize(_move);
        _playerColliderEventHandler.Initialize();
        
        EventRegistHelper.RegistCallbackEvent(_playerInputHandler, _move);
        EventRegistHelper.RegistCallbackEvent(_playerInputHandler, _playerColliderEventHandler);
    }

    public override void Hit(AttackInfo attackInfo)
    {
        base.Hit(attackInfo);
        AudioController.instance.PlaySound("Player_Hit", SoundConfig.SoundType.Effect);
    }


    public override void Die()
    {
        base.Die();
        Destroy(this);
    }
}
