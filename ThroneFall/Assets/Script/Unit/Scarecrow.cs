using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : Enemy
{
    protected override void Start()
    {
        base.Start();
        Initialize(MainController.Instance.CSVDataContaner.UnitDatas.Find(u => u.UnitID == "Enemy_Scarecrow_Melee"));
        _unitData.DropCoin = 8;
    }

    public override void Hit(AttackInfo attackInfo)
    {
        base.Hit(attackInfo);
        if (_health.CurrentHP == 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        base.Die();
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DORotate(new Vector3(90, 0, 0), 0.3f));
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            Destroy(this.gameObject);
        });

        var arrow = GetComponentInChildren<GuideArrow>();
        if (arrow != null)
        {
            arrow.RequestNextPhase();
        }
    }
}
