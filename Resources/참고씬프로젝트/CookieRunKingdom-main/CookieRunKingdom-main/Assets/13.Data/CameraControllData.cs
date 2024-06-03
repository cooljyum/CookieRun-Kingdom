using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CameraControllData : ScriptableObject
{
    [Header("수치들")]
    [SerializeField] private float _cameraMoveSpeed = 150f;
    [SerializeField] private float _cameraZoomSpeed = 0.01f;
    [SerializeField] private float _cameraZoomMin = 10f, _cameraZoomMax = 20f;
    [SerializeField] private Vector4 _cameraBorder = new Vector4(-9, 70, -40, 30);
    [SerializeField] private float _cameraBuildingZoom = 4.5f;
    [SerializeField] private Vector3 _cameraBuildingZoomOffset = new Vector3(1, -0.5f, -10f);


    // 프로퍼티
    public float CameraMoveSpeed => _cameraMoveSpeed;
    public float CameraZoomSpeed => _cameraZoomSpeed;
    public float CameraZoomMin => _cameraZoomMin;
    public float CameraZoomMax => _cameraZoomMax;
    public Vector4 CameraBorder => _cameraBorder;
    public float CameraBuildingZoom => _cameraBuildingZoom;
    public Vector3 CameraBuildingZoomOffset => _cameraBuildingZoomOffset;
}
