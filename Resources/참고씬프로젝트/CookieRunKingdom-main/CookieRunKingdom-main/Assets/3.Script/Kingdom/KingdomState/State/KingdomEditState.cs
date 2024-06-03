using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KingdomEditState : KingdomBaseState
{
    private BuildingController _currentBuilding = null;

    public KingdomEditState(KingdomStateFactory factory, KingdomManager manager) : base(factory, manager)
    {
    }

    public override void Enter()
    {
        _currentBuilding = null;
        GameManager.UI.PushUI(_manager.KingdomEditUI);
        _manager.KingdomGrid.gameObject.SetActive(true);
        _manager.buildingsInKingdom.ForEach(building => building.gameObject.SetActive(true));
    }

    public override void Exit()
    {
        GameManager.UI.ClearUI();
        _manager.BuildingCircleEditUI.HideUI();
        _manager.BuildingCircleEditUIInPreview.HideUI();
        _manager.KingdomGrid.gameObject.SetActive(false);

        // ³ª°¥ ¶§ previewTile ¾ø¾ÖÁà¾ß ÇØ
        BuildingPreviewTileObjectPool.instance.ResetPreviewTile();

        _manager.buildingsInKingdom.ForEach(building => building.gameObject.SetActive(false));
    }

    public override void Update()
    {

    }

    public override void OnClickStart()
    {
        base.OnClickStart();

        Vector2 currentPos = Vector2.zero;
        if (Mouse.current != null && Mouse.current.enabled)
            currentPos = Mouse.current.position.ReadValue();
        else if (Touchscreen.current != null && Touchscreen.current.enabled)
            currentPos = Touchscreen.current.position.ReadValue();

        RaycastHit2D rayHit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(currentPos), 100, 1 << LayerMask.NameToLayer("UI"));
        if (rayHit.collider && rayHit.transform.gameObject.layer == LayerMask.NameToLayer("UI"))
            return;

        rayHit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(currentPos), 100, 1 << LayerMask.NameToLayer("Building"));
        if (rayHit.collider)
        {
            _currentBuilding = rayHit.transform.GetComponent<BuildingController>();
            _manager.BuildingCircleEditUI.ButtonParent.SetActive(false);
            _manager.BuildingCircleEditUIInPreview.ButtonParent.SetActive(false);
            _manager.BuildingCircleEditUI.SetPrevBuilding(_currentBuilding);
            _manager.BuildingCircleEditUIInPreview.SetPrevBuilding(_currentBuilding);
            _currentBuilding.BuildingEditor.OnClickEditMode();

            if (_currentBuilding.BuildingEditor.IsInstance)
                _manager.BuildingCircleEditUI.SetBuilding(_currentBuilding, rayHit.transform, _camera.orthographicSize);
            else
                _manager.BuildingCircleEditUIInPreview.SetBuilding(_currentBuilding, rayHit.transform, _camera.orthographicSize);

            return;
        }

        rayHit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(currentPos), 100, 1 << LayerMask.NameToLayer("Ground"));
        if (rayHit.collider)
        {
            _currentBuilding = null;
            return;
        }
    }

    public override void OnClick()
    {
        base.OnClick();

        Vector2 currentPos = Vector2.zero;
        if (Mouse.current != null && Mouse.current.enabled)
            currentPos = Mouse.current.position.ReadValue();
        else if (Touchscreen.current != null && Touchscreen.current.enabled)
            currentPos = Touchscreen.current.position.ReadValue();

        RaycastHit2D rayHit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(currentPos), 100, 1 << LayerMask.NameToLayer("UI"));
        if (rayHit.collider && rayHit.transform.gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            MyButton button = rayHit.transform.GetComponent<MyButton>();
            if (button != null)
                button.OnClick();
            return;
        }

        // ¿Õ±¹¿¡ ¼³Ä¡µÈ ºôµùÀ» ´­·¶À» ¶§
        rayHit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(currentPos), 100, 1 << LayerMask.NameToLayer("Building"));

        if (!rayHit.collider)
        {
            _currentBuilding = null;
            return;
        }

        _currentBuilding = rayHit.transform.GetComponent<BuildingController>();

        if (_currentBuilding == null)
            return;

        _manager.BuildingCircleEditUI.ButtonParent.SetActive(false);
        _manager.BuildingCircleEditUIInPreview.ButtonParent.SetActive(false);
        _manager.BuildingCircleEditUI.SetPrevBuilding(_currentBuilding);
        _manager.BuildingCircleEditUIInPreview.SetPrevBuilding(_currentBuilding);
        _currentBuilding.BuildingEditor.OnClickEditMode();

        if (_currentBuilding.BuildingEditor.IsInstance)
            _manager.BuildingCircleEditUI.SetBuilding(_currentBuilding, rayHit.transform, _camera.orthographicSize);
        else
            _manager.BuildingCircleEditUIInPreview.SetBuilding(_currentBuilding, rayHit.transform, _camera.orthographicSize);
    }

    public override void OnDrag()
    {
        if(_currentBuilding == null)
        {
            base.OnDrag();
        }
        else
        {
            Vector2 currentPos = Vector2.zero;
            if (Mouse.current != null && Mouse.current.enabled)
                currentPos = Mouse.current.position.ReadValue();
            else if (Touchscreen.current != null && Touchscreen.current.enabled)
                currentPos = Touchscreen.current.position.ReadValue();

            Vector2 pos = _camera.ScreenToWorldPoint(currentPos);
            Vector3Int gridPos = _manager.Grid.WorldToCell(pos);
            _currentBuilding.transform.localPosition = _manager.Grid.CellToWorld(gridPos);

            _currentBuilding.BuildingEditor.UpdatePreviewTile();
        }
    }

    public override void OnWheel(InputAction.CallbackContext value)
    {
        base.OnWheel(value);
    }
}
