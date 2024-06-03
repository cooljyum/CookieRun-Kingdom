using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ¿¡½ºÇÁ·¹¼Ò¸À ÄíÅ°
public class Cookie0513Skill : BaseRangeSkill
{
    [SerializeField] private ProjectileCoffee _skillProjectile;

    private int _skillIndex = 0;

    public override void Init(BaseController controller)
    {
        base.Init(controller);
    }

    public override bool IsReadyToUseSkill()
    {
        return DetectTarget();
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
    }

    public override void NormalAttackEvent()
    {
        base.NormalAttackEvent();
    }

    public override void OnSkillEvent(int index)
    {
        _skillProjectile.gameObject.SetActive(true);
    }

    public override void SetLayer(LayerMask layer)
    {
        base.SetLayer(layer);
        _skillProjectile.Init(AttackPower / 5, layer, transform, new Vector3(4f, 2f, 0), _controller.CharacterStat);
    }

    public override bool UseSkill()
    {
        if(_skillIndex == 0)
        {
            PlayAnimation(animationName[_skillIndex++], false);
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
