using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// 커스터드3세맛 쿠키
public class Cookie0514Skill : BaseHealSkill
{
    private int _skillIndex = 0;

    public override void NormalAttackEvent()
    {
        base.NormalAttackEvent();
    }

    public override void OnSkillEvent(int index)
    {
        if(index == 0)
        {
            CookieController[] cookies = BattleManager.instance.CookieList
                .Where(cookie => !cookie.CharacterBattleController.IsDead)
                .OrderBy(cookie => cookie.CharacterBattleController.CurrentHp).ToArray();

            Debug.Log(cookies.Length + " 명 회복합니다.");

            // Hp 제일 작은 2명 회복 및 보호막
            for(int i = 0; i < 2; i++)
                if(i < cookies.Length)
                    cookies[i].CharacterBattleController.ChangeCurrentHp(_controller.CharacterStat.hpStat.ResultStat / 10 + AttackPower, _controller.CharacterStat);
        }
    }

    // 우리 팀에 체력이 깎인 쿠키가 있으면 true
    public override bool IsReadyToUseSkill()
    {
        List<CookieController> cookies = BattleManager.instance.CookieList;
        foreach(CookieController cookie in cookies)
            if (!cookie.CharacterBattleController.IsDead && cookie.CharacterBattleController.CurrentHp != cookie.CharacterBattleController.MaxHp)
                return true;
        return false;
    }

    public override bool UseSkill()
    {
        if(_skillIndex == 0)
        {
            PlayAnimation(animationName[_skillIndex++]);
        }
        else if(_skillIndex != 0)
        {
            if(!_controller.CharacterAnimator.IsPlayingAnimation())
            {
                _skillIndex = 0;
                return false;
            }
        }
        return true;
    }
}
