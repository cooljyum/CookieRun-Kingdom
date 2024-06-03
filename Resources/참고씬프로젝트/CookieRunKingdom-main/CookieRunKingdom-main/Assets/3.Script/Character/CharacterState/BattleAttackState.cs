using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAttackState : BaseBattleState
{
    private CharacterBattleController _target = null;

    public BattleAttackState(BattleStateFactory factory, BaseController controller) : base(factory, controller)
    {
    }

    public void SetTarget(CharacterBattleController col)
    {
        _target = col;
    }
        

    public override void Enter()
    {
        _controller.BaseSkill.NormalAttack();
    }

    public override void Exit()
    {
        _target = null;
    }

    public override void Update()
    {
        if (_target != _controller.BaseSkill.DetectTarget())
        {
            _factory.ChangeState(EBattleState.BattleRunState);
        }
    }
}
