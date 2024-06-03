using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDeadState : BaseBattleState
{
    private float currentTime = 0;

    public BattleDeadState(BattleStateFactory factory, BaseController controller) : base(factory, controller)
    {
    }

    public override void Enter()
    {
        currentTime = 0f;
        _controller.CharacterAnimator.PlayAnimation(ECookieAnimation.Dead);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        currentTime += Time.deltaTime;

        Vector3 dir = _controller.CharacterBattleController.IsForward ? new Vector3(-1f, 1f, 0f).normalized : new Vector3(1f, 1f, 0f).normalized;
        _controller.transform.position += dir * Time.deltaTime * 25f;

        if (currentTime >= 2)
        {
            _controller.CharacterBattleController.Disappear();
        }
    }
}
