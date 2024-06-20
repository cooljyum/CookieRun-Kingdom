using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera _camera;
    private float _targetZoom;
    private float _zoomSpeed = 5f; // �� �ӵ�

    [SerializeField]
    private float _minZoomSize = 5f;
    [SerializeField]
    private float _maxZoomSize = 7f;
    [SerializeField]
    private float _zoomInOutValueSize = 0.3f;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _targetZoom = _camera.orthographicSize; // ���� ī�޶��� �ܻ�����
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

        // Lerp�� ����Ͽ� ī�޶� ���� �ε巴�� ��ȭ��Ŵ
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetZoom, Time.deltaTime * _zoomSpeed);
    }

    //Set
    public void SetZoom(float newZoom)
    {
        _targetZoom = Mathf.Clamp(newZoom, _minZoomSize, _maxZoomSize); // �� ������ �ּ�/�ִ� ������ ����
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
