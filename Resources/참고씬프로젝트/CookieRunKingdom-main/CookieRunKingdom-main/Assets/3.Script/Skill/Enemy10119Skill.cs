using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy10119Skill : BaseMeleeSkill
{
    // 적은 기본공격을 몇번 이상하면 스킬을 사용하게 하자
    private int _skillCoolTime = 5;
    private int _currentSkillCount = 0;

    private int _skillIndex = 0;

    public override void Init(BaseController controller)
    {
        base.Init(controller);
    }

    public override bool IsReadyToUseSkill()
    {
        return base.IsReadyToUseSkill();
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
    }

    public override void NormalAttackEvent()
    {
        if (_controller.CharacterBattleController.CheckState(EBattleState.BattleSkillState))
        {
            OnSkillEvent(0);
        }
        else
        {
            base.NormalAttackEvent();
            _currentSkillCount++;
        }

        if(_currentSkillCount >= _skillCoolTime)
        {
            StartCoroutine(CoSKill());
            _currentSkillCount = 0;
        }
    }

    public override void OnSkillEvent(int index)
    {
        for(int i = 0; i < _detectAttackRange.enemies.Count; i++)
        {
            CharacterBattleController enemy = _detectAttackRange.enemies[i];
            if(!enemy.IsDead)
            {
                enemy.SetCC(ECCType.airborne, Vector3.up * 3f);
                enemy.ChangeCurrentHp(-AttackPower * 2, _controller.CharacterStat);
            }
        }
    }

    public override void SetLayer(LayerMask layer)
    {
        base.SetLayer(layer);
    }

    public override bool UseSkill()
    {
        if(_skillIndex == 0)
        {
            PlayAnimation(animationName[_skillIndex++], false);
        }
        else if(_skillIndex == 1)
        {
            if(!_controller.CharacterAnimator.IsPlayingAnimation())
            {
                PlayAnimation(animationName[_skillIndex++], false);
            }
        }
        else if(_skillIndex == 2)
        {
            if(!_controller.CharacterAnimator.IsPlayingAnimation())
            {
                _skillIndex = 0;
                return false;
            }
        }
        return true;
    }

    private IEnumerator CoSKill()
    {
        yield return new WaitUntil(() => !_controller.CharacterAnimator.IsPlayingAnimation());

        if(!_controller.CharacterBattleController.IsDead)
            _controller.CharacterBattleController.ChangeState(EBattleState.BattleSkillState);
    }
}
