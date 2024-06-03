using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCrowdControlState : BaseBattleState
{
    private Dictionary<ECCType, ICCCommand> _ccDictionary = new Dictionary<ECCType, ICCCommand>();
    private ECCType _currentCC;
    private Vector3 _dir;

    public BattleCrowdControlState(BattleStateFactory factory, BaseController controller) : base(factory, controller)
    {
        _ccDictionary[ECCType.KnockBack] = new KnockbackCommand(controller.CharacterBattleController);
        _ccDictionary[ECCType.airborne] = new AirborneCommand(controller.CharacterBattleController);
        _ccDictionary[ECCType.dragged] = new DraggedCommand(controller.CharacterBattleController);
    }

    public void SetCC(ECCType ccType, Vector3 dir = default(Vector3))
    {
        _controller.CharacterBattleController.CurrentCCType = ccType;
        _currentCC = ccType;
        _dir = dir;
    }

    public override void Enter()
    {
        _controller.CharacterAnimator.PlayAnimation(ECookieAnimation.BattleInactive);
        _ccDictionary[_currentCC].Execute(_dir);
    }

    public override void Exit()
    {
        _ccDictionary[_currentCC].Undo();
        _controller.CharacterBattleController.CurrentCCType = ECCType.none;
    }

    public override void Update()
    {
        if(_ccDictionary[_currentCC].IsFinish)
            _factory.ChangeState(EBattleState.BattleRunState);
    }
}
