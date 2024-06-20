using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera _camera;
    private float _targetZoom;
    private float _zoomSpeed = 5f; // 줌 속도

    [SerializeField]
    private float _minZoomSize = 5f;
    [SerializeField]
    private float _maxZoomSize = 7f;
    [SerializeField]
    private float _zoomInOutValueSize = 0.3f;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _targetZoom = _camera.orthographicSize; // 현재 카메라의 줌사이즈
    }

    private void Update()
    {
        if (BattleManager.Instance.IsOnBattle)
        {
            if (_targetZoom >= _minZoomSize)
            {
                ZoomIn(_zoomInOutValueSize);
            }
        }
        else 
        {
            if (_targetZoom <= _maxZoomSize)
            {
                ZoomOut(_zoomInOutValueSize);
            }
            else 
            {
                SetZoom(_maxZoomSize);
            }
        }

        // Lerp를 사용하여 카메라 줌을 부드럽게 변화시킴
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetZoom, Time.deltaTime * _zoomSpeed);
    }

    //Set
    public void SetZoom(float newZoom)
    {
        _targetZoom = Mathf.Clamp(newZoom, _minZoomSize, _maxZoomSize); // 줌 레벨을 최소/최대 값으로 제한
    }

    //ZoomIn
    public void ZoomIn(float amount)
    {
        SetZoom(_targetZoom - amount);
    }

    //Zoom Out
    public void ZoomOut(float amount)
    {
        SetZoom(_targetZoom + amount);
    }
}
