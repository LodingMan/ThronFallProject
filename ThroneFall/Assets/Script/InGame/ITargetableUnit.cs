using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public interface ITargetableUnit
{
    void Hit(AttackInfo attackInfo);
    EUnitType GetUnitType { get; }
    bool GetTargetAble { get; }
    Vector3 GetPosition { get; }
}
