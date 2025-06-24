using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationEventCallback : MonoBehaviour
{
    public Unit unit;
    public float delay;

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
    }
    
    public void DieEvent()
    {
        StartCoroutine(StartDieCallback());
    }
    private IEnumerator StartDieCallback()
    {
        yield return new WaitForSeconds(delay);
        unit.DieAnimationEnd();
    }

}
