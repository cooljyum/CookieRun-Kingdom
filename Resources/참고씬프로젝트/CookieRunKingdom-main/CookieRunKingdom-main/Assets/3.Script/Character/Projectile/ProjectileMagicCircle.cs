using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProjectileMagicCircle : BaseProjectile
{
    [SerializeField] private DamageBox lighting;
    [SerializeField] private float timePerAttack = 1f;

    private Sequence seq;

    public override void Init(int damage, LayerMask targetLayer, Transform parent, Vector3 initPos, CharacterStat characterStat)
    {
        base.Init(damage, targetLayer, parent, initPos, characterStat);

        lighting.Init(damage, targetLayer, characterStat);
    }

    private void OnDisable()
    {
        seq.Kill();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        seq = DOTween.Sequence();
        seq.AppendInterval(timePerAttack)
           .AppendCallback(() => lighting.gameObject.SetActive(true))
           .AppendInterval(0.2f)
           .AppendCallback(() => lighting.gameObject.SetActive(false))
           .SetLoops(-1, LoopType.Incremental)
           .SetEase(Ease.Linear);
    }
}
