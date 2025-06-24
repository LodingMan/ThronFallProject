using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitHandler : MonoBehaviour,
    IInitialize,
    IEventHandlerProvider<UnitLifecycleInfo>
{
    [SerializeField]List<Unit> EnableAllUnits = new List<Unit>(); //Player를 제외환 활성화 되어있는 모든 유닛 
    public void Initialize()
    {
    }

    public void RegistUnit(Unit unit)
    {
        EnableAllUnits.Add(unit);
    }
    public void UnRegistUnit(Unit unit)
    {
        EnableAllUnits.Remove(unit);
    }
    public void ClearAllUnit()
    {
        foreach (var unit in EnableAllUnits)
        {
            var pool = unit.GetComponent<PoolObject>();
            if (pool != null)
            {
                ObjectPooler.ReturnPool(pool);   
            }
            else
            {
                Destroy(unit);
            }
        }
        EnableAllUnits.Clear();
    }
    public void NotifyUnitDeath(Unit unit)
    {
        UnRegistUnit(unit);
        StartCoroutine(WaitForUnitDeath(unit));
    }
    private IEnumerator WaitForUnitDeath(Unit unit)
    {
        yield return new WaitForSeconds(1f);
        var pool = unit.GetComponent<PoolObject>();
        if(pool != null)
        {
            ObjectPooler.ReturnPool(pool);
        }
        else
        {
            Destroy(unit);
        }
    }

    public void CallbackEvent(UnitLifecycleInfo value)
    {
        if(value.unitLifecycleEventType == GameEnums.EUnitLifecycleEventType.Destroyed)
        {
            NotifyUnitDeath(value.unit);
            //UnRegistUnit(value.unit);
        }
        else if(value.unitLifecycleEventType == GameEnums.EUnitLifecycleEventType.Created)
        {
            RegistUnit(value.unit);
        }
    }

    private Action _unRegistCallback;
    public void SetEventUnRegistCallback(Action callback)
    {
        _unRegistCallback = callback;
    }
    private void OnDestroy()
    {
        _unRegistCallback?.Invoke();
    }
}
