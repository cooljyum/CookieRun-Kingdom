using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleIdleState : BaseBattleState
{
    private float _currentTime = 0f;

    public BattleIdleState(BattleStateFactory factory, BaseController controller) : base(factory, controller)
    {
    }

    public override void Enter()
    {
        _currentTime = 0f;
        _controller.CharacterAnimator.PlayAnimation(ECookieAnimation.BattleIdle);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
