using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookie0002Skill : BaseMeleeSkill
{
    [SerializeField] protected DetectRange _detectSkillRange;
    private List<CharacterBattleController> targets => _detectSkillRange.enemies;
    private int _skillIndex = 0;
    private float _currentTime = 0;

    private int _currentCount = 0;
    private int _attackCount = 6;
    private float _duration = 3f;

    public override void Init(BaseController controller)
    {
        base.Init(controller);
    }

    public override void SetLayer(LayerMask layer)
    {
        base.SetLayer(layer);
        _detectSkillRange.Init(layer);
    }

    public override void NormalAttackEvent()
    {
        base.NormalAttackEvent();
    }

    public override void OnSkillEvent(int index)
    {

    }

    public override bool IsReadyToUseSkill()
    {
        return _detectSkillRange.enemies.Count != 0;
    }

    // false면 스킬 종료
    public override bool UseSkill()
    {
        // 딸쿠는 그냥 돌면서 때려야 해
        if(_skillIndex == 0)
        {
            PlayAnimation(animationName[_skillIndex++]);
        }
        else 
        {
            _currentTime += Time.deltaTime;

            if(_currentTime >= _duration * _currentCount / _attackCount)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    targets[i].ChangeCurrentHp(-AttackPower, _controller.CharacterStat);
                }
                _currentCount++;
            }

            if(_currentTime >= _duration)
            {
                _currentTime = 0;
                _skillIndex = 0;
                _currentCount = 0;
                return false;
            }
        }
        return true;
    }
}
