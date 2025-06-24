using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyCountProvider
{
    int GetEnemyCount { get; }
    void NotifyEnemyDie();
    void CatchUnitDie(Unit unit);

}
