using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ¸¶µé·» ÄíÅ°
public class Cookie0511Skill : BaseMeleeSkill
{
    [SerializeField] private BaseProjectile _projectile;

    private int _skillIndex = 0;
    private bool _isRange = false;

    private Coroutine _coSkill = null;

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
        if(_isRange)
        {
            PlayAnimation(animationName[1]);
        }
        else
        {
            base.NormalAttack();
        }
    }

    public override void NormalAttackEvent()
    {
        base.NormalAttackEvent();
    }

    public override void OnSkillEvent(int index)
    {
        base.OnSkillEvent(index);

        if(index == 0)
        {
            Debug.Log("ºûÀÇ °¡È£!");
        }
        else
        {
            Vector3 dir = _controller.CharacterBattleController.IsForward ? Utils.Dir : -Utils.Dir;
            _projectile.ShootProjectile(dir.normalized);
            _projectile.gameObject.SetActive(false);
            _projectile.gameObject.SetActive(true);
        }
    }

    public override void SetLayer(LayerMask layer)
    {
        base.SetLayer(layer);
        _projectile.Init(AttackPower, layer, transform, Vector3.up * 0.8f, _controller.CharacterStat);
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
                if (_coSkill != null)
                    StopCoroutine(_coSkill);
                _coSkill = StartCoroutine(CoSkill());

                _skillIndex = 0;
                return false;
            }
        }

        return true;
    }

    private IEnumerator CoSkill()
    {
        _isRange = true;

        yield return new WaitForSeconds(7f);

        PlayAnimation(animationName[2], false);
        while(true)
        {
            yield return null;

            if (BattleManager.instance.IsBattleOver)
                yield break;

            if (!_controller.CharacterAnimator.IsPlayingAnimation())
                break;
        }

        _controller.CharacterBattleController.ChangeState(EBattleState.BattleRunState);
        _isRange = false;
    }
}
