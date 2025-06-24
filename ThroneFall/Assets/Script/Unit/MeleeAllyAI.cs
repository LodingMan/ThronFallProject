using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using static GameEnums;
using Object = UnityEngine.Object;

public class MeleeAllyAI : BaseUnitAI
{
    public bool isFollowing;
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Initialize(IMoveDestProvider moveDestProvider, IUnitStateProvider unitStateProvider, IAttackProvider attackProvider)
    {
        MoveDestProvider = moveDestProvider;
        UnitStateProvider = unitStateProvider;
        _attackProvider = attackProvider;
        isInitilaized = true;
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (!isInitilaized) return;
        if (FlagEnumHas(UnitStateProvider.GetCurrentState, EUnitState.Die))
        {
            MoveDestProvider.NotifyStopMove();
            return;
        }
        if (FlagEnumHas(UnitStateProvider.GetCurrentState, EUnitState.Attack))
        {
            MoveDestProvider.NotifyPauseMove();
            return;
        }

        if ((UnityEngine.Object)target != null && target.GetTargetAble)
        {
            var closestPoint = targetCollider.ClosestPoint(transform.position);
            float distanceToTargetSurface = Vector3.Distance(transform.position, closestPoint);
            if (distanceToTargetSurface < _attackProvider.GetAttackRange())
            {
                MoveDestProvider.NotifyStopMove();
                return;
            }
        }

        MoveDestProvider.NotifyResumeMove();

        if (isFollowing)
        {
            FindPlayer();
            MoveToTarget();
            return;
        }

        FindNewTarget();
        if (target != null && target.GetTargetAble)
        {
            MoveToTarget();
            transform.LookAt(new Vector3(target.GetPosition.x, this.transform.position.y,
                target.GetPosition.z));
        }
        else
        {
            MoveDestProvider?.NotifyStopMove();
        }
    }

    private void FindPlayer()
    {
        if (_player.TryGetComponent<ITargetableUnit>(out var unit))
        {
            target = unit;
        }

        if (_player.TryGetComponent<Collider>(out var collider))
        {
            targetCollider = collider;
        }
        
    }



}
