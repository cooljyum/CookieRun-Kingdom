using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float _cameraSpeed = 5f;
    private Vector2 touchStart; // 터치 시작 지점
    private Vector3 cameraStartPos; // 터치 시작 시 카메라 위치
    private float _zoomSpeed = 5f;
    private float _minZoom = 5f; // 최소 줌 값
    private float _maxZoom = 20f; // 최대 줌 값

    private void Update()
    {
        //방향키 이동
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);

        transform.position += movement * _cameraSpeed * Time.deltaTime;

        //터치 이동
        if (Input.touchCount == 1) // 하나의 터치가 감지되었을 때
        {
            Touch touch = Input.GetTouch(0); // 첫 번째 터치 입력을 가져옴

            if (touch.phase == TouchPhase.Began)
            {
                // 터치가 시작되면 터치 시작 위치와 카메라 시작 위치를 저장
                touchStart = touch.position;
                cameraStartPos = transform.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // 터치 이동을 감지하여 카메라 이동
                Vector2 touchDelta = touch.position - touchStart;
                Vector3 newCameraPos = cameraStartPos - new Vector3(touchDelta.x * _cameraSpeed * Time.deltaTime, touchDelta.y * _cameraSpeed * Time.deltaTime, 0);

                transform.position = newCameraPos;
            }
        }

        //줌 인/아웃
        if (Input.GetKey(KeyCode.Q))
        {
            Camera.main.orthographicSize -= _zoomSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Camera.main.orthographicSize += _zoomSpeed * Time.deltaTime;
        }

        // 줌 값 제한
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, _minZoom, _maxZoom);
    }

}