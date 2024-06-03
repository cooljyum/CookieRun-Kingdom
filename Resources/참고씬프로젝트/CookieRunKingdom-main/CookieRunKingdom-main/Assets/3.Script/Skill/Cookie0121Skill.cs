using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cookie0121Skill : BaseRangeSkill
{
    private float CurrentSkillTime => _controller.CharacterAnimator.GetIntervalAnimation();
    private int _skillIndex = 0;
    private float _currentTime = 0;
    private int _attackCount = 4;
    private bool _isSkill = false;

    [SerializeField] protected DetectRange _detectedSkilRange;
    [SerializeField] protected DetectRange _skillRange;

    private Vector3 _originPos = Vector3.zero;
    private CharacterBattleController _target;

    public override void NormalAttackEvent()
    {
        base.NormalAttackEvent();
    }

    public override void SetLayer(LayerMask layer)
    {
        base.SetLayer(layer);
        _detectedSkilRange.Init(layer);
        _skillRange.Init(layer);
    }

    public override void OnSkillEvent(int index)
    {

    }

    // 스킬 범위에 감지가 되면 그 때 사용할 수 있음
    public override bool IsReadyToUseSkill()
    {
        return _detectedSkilRange.enemies.Count != 0;
    }

    public override bool UseSkill()
    {
        if (_skillIndex == 0)
        {
            _originPos = transform.position;

            PlayAnimation(animationName[_skillIndex++], false);

            // 타겟 정하기
            foreach (CharacterBattleController target in _detectedSkilRange.enemies)
            {
                if (_target == null)
                    _target = target;
                else
                {
                    if (Vector3.Distance(transform.position, _target.transform.position) <
                        Vector3.Distance(transform.position, target.transform.position))
                    {
                        _target = target;
                    }
                }
            }
        }
        else if (_skillIndex == 1 && !_isSkill)
        {
            if (!_controller.CharacterAnimator.IsPlayingAnimation())
            {
                transform.position = _target.transform.position;
                PlayAnimation(animationName[_skillIndex], false);
                _isSkill = true;
            }
        }
        else if(_skillIndex == 1 && _isSkill)
        {
            if(_controller.CharacterAnimator.IsPlayingAnimation())
            {
                _currentTime += Time.deltaTime;
                if (_currentTime >= CurrentSkillTime / _attackCount)
                {
                    _currentTime = 0;

                    foreach (CharacterBattleController target in _skillRange.enemies)
                    {
                        target.ChangeCurrentHp(-AttackPower / 2, _controller.CharacterStat);
                    }
                }
            }
            else
            {
                _skillIndex++;
            }
        }
        else if(_skillIndex == 2)
        {
            if (!_controller.CharacterAnimator.IsPlayingAnimation())
            {
                transform.position = _originPos;
                PlayAnimation(animationName[_skillIndex++], false);
            }
        }
        else if(_skillIndex == 3)
        {
            if (!_controller.CharacterAnimator.IsPlayingAnimation())
            {
                _currentTime = 0;
                _skillIndex = 0;
                _isSkill = false;
                return false;
            }
        }
        return true;
    }
}
