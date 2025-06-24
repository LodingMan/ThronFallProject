using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : Unit
{
    public override void Initialize(UnitData unitData)
    {
        base.Initialize(unitData);
        if (TryGetComponent<RangeUnitAI>(out var rangeUnitAI))
        {
            rangeUnitAI.Initialize(_move,_unitState,_attack);
        }
        if(TryGetComponent<MeleeEnemyAI>(out var meleeUnitAI))
        {
            meleeUnitAI.Initialize(_move,_unitState,_attack);
        }
    }

    public override void Hit(AttackInfo attackInfo)
    {
        base.Hit(attackInfo);
        AudioController.instance.PlaySound("Unit_Hit", SoundConfig.SoundType.Effect);

    }

    public override void Die()
    {
        base.Die();
    }
}
