using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetUnitCache : MonoBehaviour
{
    public ITargetableUnit CachedTarget;

    void Awake()
    {
        CachedTarget = GetComponentInParent<ITargetableUnit>();
    }
}
