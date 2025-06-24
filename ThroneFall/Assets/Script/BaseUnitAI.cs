using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class BaseUnitAI : MonoBehaviour
{
    public LayerMask enemyLayer;
    protected ITargetableUnit target;
    protected Collider targetCollider;
    public float targetDetectionRange;
    public float fullDetectionRange;
    protected bool isInitilaized = false;

    protected IMoveDestProvider MoveDestProvider;
    protected IUnitStateProvider UnitStateProvider;
    protected IAttackProvider _attackProvider;

    private float targetScanInterval = 0.5f;
    private float scanTimer = 0f;

    void Update()
    {
        scanTimer += Time.deltaTime;
        if (scanTimer >= targetScanInterval)
        {
            scanTimer = 0f;
            OnUpdate();
        }
    }
    protected virtual void OnUpdate()
    {
    }

    private Collider[] enemies = new Collider[30];
    private int enemiesCount;
    protected void FindNewTarget()
    {
        Array.Clear(enemies, 0, enemies.Length);

        int countInRange = Physics.OverlapSphereNonAlloc(transform.position, targetDetectionRange, enemies, enemyLayer);
        int countTotal = countInRange;

        if (countInRange == 0)
            countTotal = Physics.OverlapSphereNonAlloc(transform.position, fullDetectionRange, enemies, enemyLayer);

        enemiesCount = countTotal;
        var (bestTarget, bestCol) = GetBestTarget(targetDetectionRange);
        target = bestTarget;
        targetCollider = bestCol;

        if (target == null)
            MoveDestProvider?.NotifyStopMove();
    }


    private (ITargetableUnit, Collider) GetBestTarget(float attackRange)
    {
        List<(ITargetableUnit unit, Collider col, float dist, int priority)> candidates = new();

        for (int i = 0; i < enemiesCount; i++)
        {
            Collider col = enemies[i];
            if (col == null) continue;

            ITargetableUnit unit = null;
            if (col.TryGetComponent<ITargetableUnit>(out var direct))
            {
                unit = direct;
            }
            else if (col.TryGetComponent<TargetUnitCache>(out var cache))
            {
                unit = cache.CachedTarget;
            }
            
            if (unit == null || !unit.GetTargetAble)
                continue;

            float dist = Vector3.Distance(transform.position, unit.GetPosition);
            EUnitType type = unit.GetUnitType; 

            int priority = 99; // 낮을수록 우선

            bool inRange = dist <= attackRange;

            if (inRange && type == EUnitType.Unit)
                priority = 0;
            else if (inRange && type == EUnitType.Town)
                priority = 1;
            else if (!inRange && type == EUnitType.Town)
                priority = 2;
            else if (!inRange && type == EUnitType.Unit)
                priority = 3;

            candidates.Add((unit, col, dist, priority));
        }
        if (candidates.Count == 0)
            return (null, null);

        candidates.Sort((a, b) =>
        {
            int cmp = a.priority.CompareTo(b.priority);
            if (cmp != 0) return cmp;
            return a.dist.CompareTo(b.dist);
        });

        var best = candidates[0];
        return (best.unit, best.col);
    }


    protected void MoveToTarget()
    {
        MoveDestProvider?.SetMoveDest(targetCollider.transform.position);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fullDetectionRange);
    }


}
