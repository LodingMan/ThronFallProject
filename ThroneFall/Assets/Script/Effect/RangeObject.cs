using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RangeObject : CombetObject
{
    
    public ParticleSystem particle;
    public TrailRenderer trail;

    public float HitApplyDelay;
     public float MaxAirTime;
    public float Speed;
    public float MaxHeight;

    private Collider _targetCollider;

    public override void OnStart(ITargetableUnit unit, AttackInfo attackInfo,Collider targetCollider, Action onComplete)
    {
        base.OnStart(unit, attackInfo, targetCollider, onComplete);
        trail.Clear();
        TargetUnit = unit;
        AttackInfo = attackInfo;
        _targetCollider = targetCollider;
        //StartCoroutine(Attack(unit, attackInfo));
        
        Vector3 targetPosition = unit.GetPosition + Vector3.up * 1.5f;
        Launch(unit, Speed, MaxHeight,MaxAirTime);
    }
    IEnumerator Attack(ITargetableUnit unit, AttackInfo attackInfo)
    {
        if (particle != null)
        {
            particle.Play();

        }
        yield return new WaitForSeconds(HitApplyDelay);
        unit.Hit(attackInfo);
        //unit.Hit(this.GetComponent<MeleeObject>(),attackInfo);
        OnCompleteAttack();
    }
    
    public void Launch(ITargetableUnit target, float speed, float maxHeight, float maxAirTime)
    {
        StartCoroutine(TrackTargetRoutine(target, speed, maxHeight, maxAirTime));
    }

private IEnumerator TrackTargetRoutine(ITargetableUnit target, float speed, float height, float maxAirTime)
{
    float elapsedTime = 0f;
    Vector3 startPos = transform.position;
    Vector3 initialTargetPos = _targetCollider.transform.position;
    
    Vector3 flatStart = new Vector3(startPos.x, 0f, startPos.z);
    Vector3 flatInitialTarget = new Vector3(initialTargetPos.x, 0f, initialTargetPos.z);

    float initialDistance = Vector3.Distance(flatStart, flatInitialTarget);

    Vector3 previousToTarget = initialTargetPos - startPos;

    while (elapsedTime < maxAirTime)
    {
        if (target == null)
        {
            OnCompleteAttack();
            yield break;
        }

        elapsedTime += Time.deltaTime;
        Vector3 currentPos = transform.position;
        Vector3 targetPos = _targetCollider.transform.position;
        Vector3 toTarget = targetPos - currentPos;
        Vector3 flatToTarget = new Vector3(toTarget.x, 0f, toTarget.z);
        Vector3 flatDirection = flatToTarget.sqrMagnitude > 0.0001f 
            ? flatToTarget.normalized 
            : Vector3.zero;
        
        Vector3 horizontalMove = flatDirection * speed * Time.deltaTime;
        Vector3 nextPos = currentPos + horizontalMove;

        float currentDistance = flatToTarget.magnitude;
        float progress = initialDistance > 0.01f 
            ? Mathf.Clamp01(1f - (currentDistance / initialDistance)) 
            : 1f; 
        
        float arcY = height * (1f - Mathf.Pow(2f * progress - 1f, 2f));
        nextPos.y = Mathf.Lerp(startPos.y, targetPos.y, progress) + arcY;

        transform.position = nextPos;

        if (flatDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        Vector3 newToTarget = targetPos - nextPos;
        float distanceToTarget = newToTarget.magnitude;
        if (Vector3.Dot(previousToTarget, newToTarget) < 0f || distanceToTarget <= 0.5f)
        {
            StartCoroutine(Attack(target, AttackInfo));
            yield break;
        }

        previousToTarget = newToTarget;
        yield return null;
    }

    OnCompleteAttack();
}


    public override void OnCompleteAttack()
    {
        base.OnCompleteAttack();
        trail.Clear();
        
    }
    
}
