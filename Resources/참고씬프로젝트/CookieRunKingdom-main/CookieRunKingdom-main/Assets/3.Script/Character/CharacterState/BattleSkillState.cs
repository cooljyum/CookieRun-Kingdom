using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬 버튼 눌러서 스킬 발동되면 이 상태로 이동
public class BattleSkillState : BaseBattleState
{
    private float _currentTime = 0f;

    public BattleSkillState(BattleStateFactory factory, BaseController controller) : base(factory, controller)
    {
    }

    public override void Enter()
    {
/*        if (_controller.CharacterBattleController.CookieBundle != null)
            _controller.CharacterBattleController.CookieBundle.ActiveMove(true);*/

        _currentTime = 0f;
        _controller.BaseSkill.UseSkill();
    }

    public override void Exit()
    {
/*        if (_controller.CharacterBattleController.CookieBundle != null)
            _controller.CharacterBattleController.CookieBundle.ActiveMove(false);*/
    }

    public override void Update()
    {
        if(!_controller.BaseSkill.UseSkill())
        {
            if (BattleManager.instance.IsBattleOver)
                _factory.ChangeState(EBattleState.BattleIdleState);
            else
                _factory.ChangeState(EBattleState.BattleRunState);
        }
    }
}
