using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using static GameEnums;
using Object = UnityEngine.Object;

public class MeleeEnemyAI : BaseUnitAI
{

    public void Initialize(IMoveDestProvider moveDestProvider, IUnitStateProvider unitStateProvider,IAttackProvider attackProvider)
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
        if (FlagEnumHas(UnitStateProvider.GetCurrentState, EUnitState.Attack))
        {
            MoveDestProvider.NotifyStopMove();
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
        //MoveDestProvider.NotifyResumeMove();

        FindNewTarget();
        if (target != null && target.GetTargetAble)
        {
            MoveToTarget(); 
            transform.LookAt(new Vector3(target.GetPosition.x, this.transform.position.y,
                target.GetPosition.z)); 
            //Debug.Log($"UnitAILookAt Name: {gameObject.name} Target: {target.UnitTransform.name} Position: {target.UnitTransform.position} State : {UnitStateProvider?.GetCurrentState}");
        }
        else
        {
            MoveDestProvider?.NotifyStopMove();
            //Unit.SetUnitState(EUnitState.Idle);
        }
    }

}
