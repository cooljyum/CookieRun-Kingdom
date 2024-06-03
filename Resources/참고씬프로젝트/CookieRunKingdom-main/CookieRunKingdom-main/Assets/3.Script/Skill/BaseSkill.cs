using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public bool isSkillUse = false;

    // 스킬의 애니메이션 이름
    public string[] animationName;
    public string[] attackEvent;
    public string[] skillEvent;

    protected BaseController _controller;
    [SerializeField] protected DetectRange _detectAttackRange;

    protected int AttackPower => _controller.CharacterStat.attackStat.ResultStat;

    public virtual void Init(BaseController controller)
    {
        _controller = controller;
    }

    public virtual void SetLayer(LayerMask layer)
    {
        _detectAttackRange.Init(layer);
    }

    public virtual void NormalAttack()
    {
        _controller.CharacterAnimator.PlayAnimation(ECookieAnimation.BattleAttack);
    }

    // 기본 공격
    public abstract void NormalAttackEvent();

    // 스킬
    public abstract bool UseSkill();

    // 스킬 이벤트
    public abstract void OnSkillEvent(int index);

    // 스킬을 쓸 수 있으면 true, 아니면 false
    public virtual bool IsReadyToUseSkill()
    {
        return true;
    }

    public CharacterBattleController DetectTarget()
    {
        if (_detectAttackRange.enemies.Count != 0)
            return _detectAttackRange.enemies[0];
        return null;
    }

    protected void PlayAnimation(string animationName, bool isLoop = true)
    {
        _controller.CharacterAnimator.PlayAnimation(animationName, isLoop);
    }
}
