using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static GameEnums;

public class RangeUnitAI : BaseUnitAI
{
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
        if (FlagEnumHas(UnitStateProvider.GetCurrentState,EUnitState.Die))
        {
            MoveDestProvider.NotifyStopMove();
            return;
        }
        if (FlagEnumHas(UnitStateProvider.GetCurrentState,EUnitState.Attack))
        {
            MoveDestProvider.NotifyStopMove();
            return;
        }

        if ((UnityEngine.Object)target != null && target.GetTargetAble)
        {
            Vector3 closestPoint = targetCollider.ClosestPoint(transform.position);
            float distanceToTargetSurface = Vector3.Distance(transform.position, closestPoint);
            if (distanceToTargetSurface < _attackProvider.GetAttackRange())
            {
                MoveDestProvider.NotifyStopMove();
                return;
            } 
        }

        //MoveDestProvider.NotifyResumeMove();
        FindNewTarget();
        if (target != null && UnitStateProvider?.GetCurrentState != EUnitState.Die)
        {
            MoveToTarget(); 
            transform.LookAt(new Vector3(target.GetPosition.x, this.transform.position.y,
                target.GetPosition.z)); 
        }
        else
        {
            MoveDestProvider.NotifyStopMove();
         
        }
    }
}
