using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KingdomAdventureState : KingdomBaseState
{
    // 들어올 시 배틀쿠키들 생성해주고 이동해주기
    private CookieBundleInAdventure _cookieBundle = null;

    public KingdomAdventureState(KingdomStateFactory factory, KingdomManager manager) : base(factory, manager)
    {

    }

    public override void Enter()
    {
        _manager.CurrentCameraControllerData = _manager.CameraContrllInStageData;

        GameManager.UI.PushUI(_manager.KingdomAdventureUI);
        if (_cookieBundle == null)
        {
            _cookieBundle = GameObject.FindObjectOfType<CookieBundleInAdventure>();
        }

        _cookieBundle.transform.localPosition = GameManager.Game.battlePosition;
        _cookieBundle.CookieParent.localPosition = _cookieBundle.transform.localPosition;

        _camera.orthographicSize = 10;
        _camera.transform.position = new Vector3(_cookieBundle.transform.position.x, _cookieBundle.transform.position.y, -10f);
    }

    public override void Exit()
    {
        _manager.CurrentCameraControllerData = _manager.CameraControllInKingdomData;
        GameManager.UI.ClearUI();
    }

    public override void Update()
    {

    }

    public override void OnClick()
    {
        base.OnClick();

        if(DetectUI())
            return;

        _cookieBundle.OnMove();
    }

    public override void OnDrag()
    {
        base.OnDrag();
    }

    public override void OnWheel(InputAction.CallbackContext value)
    {
        base.OnWheel(value);
    }
}
