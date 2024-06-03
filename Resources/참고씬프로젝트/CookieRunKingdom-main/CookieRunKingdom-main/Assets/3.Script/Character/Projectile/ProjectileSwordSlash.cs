using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSwordSlash : BaseProjectile
{
    public override void Init(int damage, LayerMask targetLayer, Transform parent, Vector3 initPos, CharacterStat characterStat)
    {
        _rigid = GetComponent<Rigidbody2D>();
        _damageBox = GetComponent<DamageBox>();
        _parent = parent;
        _initPos = initPos;

        if (_damageBox != null)
            _damageBox.Init(damage, targetLayer, characterStat, _isPersistence, ECCType.KnockBack, Utils.Dir.normalized * 0.4f);
    }
}
