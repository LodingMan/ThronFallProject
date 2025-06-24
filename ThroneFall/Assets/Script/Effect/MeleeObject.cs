using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeObject : CombetObject
{
    public ParticleSystem particle;

    private Action OnCompleteCallback;
    public override void OnStart(ITargetableUnit target, AttackInfo attackInfo, Collider targetCollider,Action onComplete)
    {
        base.OnStart(target, attackInfo, targetCollider,onComplete);
        TargetUnit = target;
        AttackInfo = attackInfo;
        OnCompleteCallback = onComplete;
        StartCoroutine(Attack(target, attackInfo));
    }

    IEnumerator Attack(ITargetableUnit unit, AttackInfo attackInfo)
    {
        particle.Play();
        yield return new WaitForSeconds(hitApplyDelay);
        unit.Hit(attackInfo);
        //unit.Hit(this.GetComponent<MeleeObject>(),attackInfo);
        OnCompleteAttack();
    }

    public override void OnCompleteAttack()
    {
        base.OnCompleteAttack();
        OnCompleteCallback.Invoke();
    }

}
