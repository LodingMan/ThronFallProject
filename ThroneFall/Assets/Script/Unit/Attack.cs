using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using static GameEnums;

[Serializable]
public struct AttackData
{
    public float Damage;
    public float CoolDown;
    public float Range;
}
public class Attack : MonoBehaviour,
    IAttackProvider
{
    public enum EAttackType
    {
        melee,
        range,
    }
     public EAttackType _attackType;
    [SerializeField]private bool _isAttackPossable = false;
    private string _attackEffName = string.Empty;
    [SerializeField]private Transform _trAttackEffect;
    [SerializeField] private AttackData _attackData;
    [SerializeField] private PoolObject _meleeAttackObject;
    [SerializeField] private PoolObject _rangeAttackObject;

    private IState _objectState;
    
    public LayerMask enemyLayer;

    [SerializeField]private float lastAttackTime = 0f;
    public void Initialize(AttackData attackData, IState state)
    {
        _objectState = state;
        _attackData = attackData;
    }
    public void OnAttackStart(ITargetableUnit target, Collider targetCollider)
    {
        Debug.Log("Attack");

        if (!_objectState.CanBeAttack()) return;
        
        switch (_attackType)
        {
            case EAttackType.melee:
                    var meleeObj = ObjectPooler.instance.GetObjectPool(_meleeAttackObject,_trAttackEffect.position).GetComponent<MeleeObject>();

                    //meleeObj.transform.position = trAttackEffect.position;//this.transform.position;
                    meleeObj.transform.LookAt(target.GetPosition);
                    AudioController.instance.PlaySound("Sword_Slash", SoundConfig.SoundType.Effect);
                    meleeObj.OnStart(target, new AttackInfo()
                    {
                        AttackType = _attackType,
                        Damage = _attackData.Damage,
                    }, targetCollider, OnCompleteAttack);
                break;
            case EAttackType.range:
                var rangeObj = ObjectPooler.instance.GetObjectPool(_rangeAttackObject, _trAttackEffect.position).GetComponent<RangeObject>();
                //rangeObj.transform.position = trAttackEffect.position;//this.transform.position;
                if (target != null)
                {
                    rangeObj.transform.LookAt(target.GetPosition);
                    AudioController.instance.PlaySound("Arrow_Launch", SoundConfig.SoundType.Effect);
                    rangeObj.OnStart(target, new AttackInfo()
                    {
                        AttackType = _attackType,
                        Damage = _attackData.Damage,
                    }, targetCollider, OnCompleteAttack); 
                }
                break;
            default:
                break;
        }
    }

    private Collider[] Enemies = new Collider[30];
    void Update()
    {
        int enemyCount = Physics.OverlapSphereNonAlloc(transform.position, _attackData.Range, Enemies, enemyLayer);
        ITargetableUnit target = null;
        float minDist = float.MaxValue;
        _isAttackPossable = false;
        Collider col = null;
        Collider closestCol = null;


        for (int i = 0; i < enemyCount; i++)
        {
            col = Enemies[i];
            if (col == null) continue;

            ITargetableUnit unit = null;

            if (col.TryGetComponent<TargetUnitCache>(out var cache))
            {
                unit = cache.CachedTarget;
            }
            else if (col.TryGetComponent<ITargetableUnit>(out var direct))
            {
                unit = direct;
            }

            if (unit == null || !unit.GetTargetAble)
                continue;

            float dist = Vector3.SqrMagnitude(unit.GetPosition - transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                target = unit;
                closestCol = col;
                _isAttackPossable = true;
            }
        }

        if (target != null && Time.time - lastAttackTime >= _attackData.CoolDown)
        {
            OnAttackStart(target, closestCol);
            lastAttackTime = Time.time;
        }
    }

    public void OnCompleteAttack()
    {
        _objectState.CompleteAttack();
        _isAttackPossable = false;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackData.Range);
    }


    public float GetAttackRange()
    {
        return _attackData.Range;
    }
}
