using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleController : CharacterBattleController
{
    protected override void Dead()
    {
        base.Dead();
        BattleManager.instance.CheckGameClear();
    }


    public override void Disappear()
    {
        base.Disappear();
        Destroy(gameObject);
    }
}
