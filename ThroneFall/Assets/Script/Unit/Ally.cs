using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Unit
{
    private MeleeAllyAI _meleeAi;
    public bool isFollowing = false;
    public override void Initialize(UnitData unitData)
    {
        base.Initialize(unitData);
        
        if(TryGetComponent<MeleeAllyAI>(out var meleeUnitAI))
        {
            meleeUnitAI.Initialize(_move,_unitState,_attack);
            _meleeAi = meleeUnitAI;
        }
    }

    public override void Hit(AttackInfo attackInfo)
    {
        base.Hit(attackInfo);
        AudioController.instance.PlaySound("Unit_Hit", SoundConfig.SoundType.Effect);

    }

    public void OnFollow(bool isOn)
    {
        isFollowing = isOn;
        _meleeAi.isFollowing = isOn;
    }

    public override void Die()
    {
        base.Die();
    }

}
