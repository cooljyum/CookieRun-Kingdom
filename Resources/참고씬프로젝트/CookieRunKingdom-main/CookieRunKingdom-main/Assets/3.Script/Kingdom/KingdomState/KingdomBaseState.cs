using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class KingdomBaseState
{
    protected KingdomStateFactory _factory;
    protected KingdomManager _manager;
    protected Camera _camera;

    private Vector2 prevPos = Vector3.zero;


    private GameObject _ui_canvas;
    private GraphicRaycaster _ui_raycaster;
    private PointerEventData _click_data;
    private List<RaycastResult> _click_results;
    private bool _isinit = false;



    public KingdomBaseState(KingdomStateFactory factory, KingdomManager manager)
    {
        _factory = factory;
        _manager = manager;

        _camera = Camera.main;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();

    public virtual void OnWheel(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            float increment = value.ReadValue<Vector2>().y * _manager.CurrentCameraControllerData.CameraZoomSpeed;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + increment,
                _manager.CurrentCameraControllerData.CameraZoomMin, _manager.CurrentCameraControllerData.CameraZoomMax);
        }
    }

    public virtual void OnClickStart()
    {
        if (DetectUI()) return;
    }

    public virtual void OnClick()
    {
        if (DetectUI()) return; 
    }

    public virtual void OnDrag()
    {
        if (DetectUI()) return;

        Vector2 currentPos = Vector2.zero;
        if(Mouse.current != null && Mouse.current.enabled)
            currentPos = Mouse.current.position.ReadValue();
        else if(Touchscreen.current != null && Touchscreen.current.enabled)
            currentPos = Touchscreen.current.position.ReadValue();

        if (currentPos.Equals(prevPos))
            return;

        Vector3 dir = (currentPos - prevPos).normalized;

        Vector3 targetPos = _camera.transform.position - dir * _manager.CurrentCameraControllerData.CameraMoveSpeed * Time.deltaTime;
        float targetPosX = Mathf.Clamp(targetPos.x,
            _manager.CurrentCameraControllerData.CameraBorder.x, _manager.CurrentCameraControllerData.CameraBorder.y);
        float targetPosY = Mathf.Clamp(targetPos.y,
            _manager.CurrentCameraControllerData.CameraBorder.z, _manager.CurrentCameraControllerData.CameraBorder.w);

        _camera.transform.position = new Vector3(targetPosX, targetPosY, _camera.transform.position.z);
        prevPos = currentPos;
    }

    public virtual void OnDragEnd()
    {
        if (DetectUI()) return;
    }


    protected bool DetectUI()
    {
        if (!_isinit)
        {
            _ui_canvas = GameObject.Find("KingdomCanvas");
            _isinit = true;
            _ui_raycaster = _ui_canvas.GetComponent<GraphicRaycaster>();
            _click_data = new PointerEventData(EventSystem.current);
            _click_results = new List<RaycastResult>();
        }

        _click_data.position = Vector2.zero;
        if (Mouse.current != null && Mouse.current.enabled)
            _click_data.position = Mouse.current.position.ReadValue();
        else if (Touchscreen.current != null && Touchscreen.current.enabled)
            _click_data.position = Touchscreen.current.position.ReadValue();

        _click_results.Clear();

        _ui_raycaster.Raycast(_click_data, _click_results);

        if (_click_results.Count != 0)
            return true;
        return false;
    }
}
