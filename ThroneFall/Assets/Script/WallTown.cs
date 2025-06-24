using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTown : Town
{
    public override void Initialize(TownData townData)
    {
        base.Initialize(townData);
    }

    public override void TownCreate()
    {
        base.TownCreate();

        var obj = ObjectPooler.instance.GetObjectPool(SpawnEffect, transform.position);
        obj.transform.position = transform.position;//this.transform.position;
    }
    public override void TownBreak()
    {
        base.TownBreak();
        var obj = ObjectPooler.instance.GetObjectPool(BreakEffect, transform.position);
        obj.transform.position = transform.position;//this.transform.position;
    }
}
