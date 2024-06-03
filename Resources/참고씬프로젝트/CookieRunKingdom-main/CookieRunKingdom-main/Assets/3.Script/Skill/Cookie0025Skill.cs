using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 마법사맛 쿠키
public class Cookie0025Skill : BaseRangeSkill
{
    [SerializeField] private ProjectileMagicCircle _skillProjectile;

    private int _skillIndex = 0;

    public override void Init(BaseController controller)
    {
        base.Init(controller);
    }

    public override void SetLayer(LayerMask layer)
    {
        base.SetLayer(layer);
        _skillProjectile.Init(AttackPower / 2, layer, transform, new Vector3(3f, 1.5f, 0), _controller.CharacterStat);
    }

    public override void NormalAttackEvent()
    {
        base.NormalAttackEvent();
    }

    public override void OnSkillEvent(int index)
    {
        if(index == 0)
        {
            // 투사체 발사!
            Vector3 dir = _controller.CharacterBattleController.IsForward ? Utils.Dir : -Utils.Dir;
            _skillProjectile.ShootProjectile(dir.normalized);
            _skillProjectile.gameObject.SetActive(true);
        }
    }

    public override bool IsReadyToUseSkill()
    {
        return DetectTarget();
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
