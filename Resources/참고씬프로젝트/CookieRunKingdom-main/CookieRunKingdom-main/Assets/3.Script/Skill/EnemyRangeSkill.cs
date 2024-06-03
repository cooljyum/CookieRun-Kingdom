using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeSkill : BaseRangeSkill
{
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
        StartCoroutine(CoFire());
    }

    public override void NormalAttackEvent()
    {
        base.NormalAttackEvent();
    }

    public override void OnSkillEvent(int index)
    {
        base.OnSkillEvent(index);
    }

    public override void SetLayer(LayerMask layer)
    {
        base.SetLayer(layer);
    }

    public override bool UseSkill()
    {
        return base.UseSkill();
    }

    private IEnumerator CoFire()
    {
        float currentTime = 0;
        float AnimationTime = _controller.CharacterAnimator.GetIntervalAnimation();

        while(true)
        {
            if (_controller.CharacterBattleController.IsDead)
                yield break;

            if (!_controller.CharacterBattleController.CheckState(EBattleState.BattleAttackState))
                yield break;

            currentTime += Time.deltaTime;
            if (currentTime >= AnimationTime / 2)
            {
                currentTime = 0;
                NormalAttackEvent();
            }

            yield return null;
        }
    }
}
