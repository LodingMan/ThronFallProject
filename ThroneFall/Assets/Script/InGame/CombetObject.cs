using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CombetObject : MonoBehaviour
{
    protected ITargetableUnit TargetUnit;
    protected AttackInfo AttackInfo;
    public PoolObject PoolObject;
    public float hitApplyDelay;
    public Action OnComplete;
    public virtual void OnStart(ITargetableUnit unit, AttackInfo attackInfo, Collider targetCollider, Action onComplete)
    {
        OnComplete = onComplete;
        PoolObject = GetComponent<PoolObject>();
    }

    public virtual void OnCompleteAttack()
    {
        //Debug.Log($"HitComplete : {TargetUnit.name} , {AttackInfo.Damage}");
        OnComplete.Invoke();
        ObjectPooler.ReturnPool(PoolObject);

        return;
    }
}
