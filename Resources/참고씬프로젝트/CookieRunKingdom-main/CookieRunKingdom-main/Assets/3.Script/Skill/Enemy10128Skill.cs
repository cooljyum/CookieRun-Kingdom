using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy10128Skill : BaseMeleeSkill
{
    // 적은 기본공격을 몇번 이상하면 스킬을 사용하게 하자
    private int _skillCoolTime = 6;
    private int _currentSkillCount = 0;

    private int _skillIndex = 0;

    // 0 회복,  1 쎈공격
    private int _skillPattern = 0;

    public override void Init(BaseController controller)
    {
        base.Init(controller);
        GameManager.Sound.PlayBgm(EBGM.bossBattle);
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

        if (_currentSkillCount >= _skillCoolTime)
        {
            _controller.CharacterBattleController.ChangeState(EBattleState.BattleSkillState);
            _currentSkillCount = 0;
        }
    }

    public override void OnSkillEvent(int index)
    {
        for (int i = 0; i < _detectAttackRange.enemies.Count; i++)
        {
            CharacterBattleController enemy = _detectAttackRange.enemies[i];
            if (!enemy.IsDead)
            {
                enemy.SetCC(ECCType.KnockBack, -Utils.Dir * 2);
                enemy.ChangeCurrentHp(-AttackPower * 3, _controller.CharacterStat);
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
            if (!_controller.CharacterAnimator.IsPlayingAnimation())
            {
                if (Random.Range(0, 3) == 0)
                    _skillPattern = 0;
                else
                    _skillPattern = 1;

                PlayAnimation(animationName[_skillPattern], false);
                _skillIndex++;

                // 회복
                if (_skillPattern == 0)
                    _controller.CharacterBattleController.ChangeCurrentHp(_controller.CharacterStat.hpStat.ResultStat / 5 * AttackPower, _controller.CharacterStat);
            }
        }
        else if(_skillIndex == 1)
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
