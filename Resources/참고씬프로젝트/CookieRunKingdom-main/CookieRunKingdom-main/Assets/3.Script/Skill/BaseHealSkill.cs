using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealSkill : BaseSkill
{
    public override void NormalAttackEvent()
    {
        // 가장 위급한 쿠키에게 회복을 해줍니다.

        List<CookieController> cookies = BattleManager.instance.CookieList;
        CharacterBattleController selectedCookie = cookies[0].CharacterBattleController;
        for (int i = 0; i < cookies.Count; i++)
            if(!selectedCookie.IsDead && selectedCookie.CurrentHp >= cookies[i].CharacterBattleController.CurrentHp)
                selectedCookie = cookies[i].CharacterBattleController;

        selectedCookie.ChangeCurrentHp(_controller.CharacterStat.hpStat.ResultStat / 10 + AttackPower, _controller.CharacterStat);
    }

    public override void OnSkillEvent(int index)
    {

    }

    public override bool UseSkill()
    {
        return true;
    }
}
