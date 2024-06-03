using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieBattleController : CharacterBattleController
{
    protected override void Dead()
    {
        base.Dead();
        BattleManager.instance.CheckGameOver();
    }

    public override void Disappear()
    {
        base.Disappear();
        gameObject.SetActive(false);
    }
}
