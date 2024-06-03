using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBattleState
{
    protected BattleStateFactory _factory;
    protected BaseController _controller;

    public BaseBattleState(BattleStateFactory factory, BaseController controller)
    {
        _factory = factory;
        _controller = controller;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
}
