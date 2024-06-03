using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KingdomPlayState : KingdomBaseState
{
    public KingdomPlayState(KingdomStateFactory factory, KingdomManager manager) : base(factory, manager)
    {
    }

    public override void Enter()
    {
        GameManager.UI.PushUI(_manager.KingdomPlayUI);
    }

    public override void Exit()
    {
        GameManager.UI.ClearUI();
    }

    public override void Update()
    {

    }

    public override void OnClick()
    {

    }

    public override void OnDrag()
    {

    }

    public override void OnWheel(InputAction.CallbackContext value)
    {

    }
}
