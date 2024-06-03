using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBattleState
{
    BattleIdleState,
    BattleRunState,
    BattleAttackState,
    BattleDeadState,
    BattleSkillState,
    BattleCrowdControlState,  // CC±â
}

public class BattleStateFactory
{
    private BaseController _controller;
    private Dictionary<EBattleState, BaseBattleState> _dictionary = new Dictionary<EBattleState, BaseBattleState>();
    private  BaseBattleState _currentState = null;

    public BaseBattleState CurrentState => _currentState;

    public BattleIdleState BattleIdle { get; private set; }
    public BattleRunState BattleRun { get; private set; }
    public BattleAttackState BattleAttack { get; private set; }
    public BattleDeadState BattleDead { get; private set; }
    public BattleSkillState BattleSkill { get; private set; }
    public BattleCrowdControlState BattleCC { get; private set; }

    public BattleStateFactory(BaseController controller)
    {
        _controller = controller;
        Init();
    }

    private void Init()
    {
        BattleIdle = new BattleIdleState(this, _controller);
        BattleRun = new BattleRunState(this, _controller);
        BattleAttack = new BattleAttackState(this, _controller);
        BattleDead = new BattleDeadState(this, _controller);
        BattleSkill = new BattleSkillState(this, _controller);
        BattleCC = new BattleCrowdControlState(this, _controller);

        _dictionary[EBattleState.BattleIdleState] = BattleIdle;
        _dictionary[EBattleState.BattleRunState] = BattleRun;
        _dictionary[EBattleState.BattleAttackState] = BattleAttack;
        _dictionary[EBattleState.BattleDeadState] = BattleDead;
        _dictionary[EBattleState.BattleSkillState] = BattleSkill;
        _dictionary[EBattleState.BattleCrowdControlState] = BattleCC;

        ChangeState(EBattleState.BattleIdleState);
    }

    public void ChangeState(EBattleState state)
    {
        if(_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = _dictionary[state];

        _currentState.Enter();
    }

    public bool CheckState(EBattleState state)
    {
        return _currentState == _dictionary[state];
    }
}
