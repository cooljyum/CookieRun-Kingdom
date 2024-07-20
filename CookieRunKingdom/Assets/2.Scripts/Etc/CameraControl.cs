using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float _cameraSpeed = 7f;
    private Vector2 touchStart; // 터치 시작 지점
    private Vector3 cameraStartPos; // 터치 시작 시 카메라 위치
    private float _zoomSpeed = 5f; //0.1f; // 키 줌 속도 / 터치 줌 속도 //*추후 수정*//
    private float _minZoom = 5f; // 최소 줌 값
    private float _maxZoom = 20f; // 최대 줌 값

    private void Update()
    {
        //방향키 컨트롤 - 이동
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);

        transform.position += movement * _cameraSpeed * Time.deltaTime;

        //방향키 컨트롤 - 줌 인/아웃
        if (Input.GetKey(KeyCode.Q))
        {
            Camera.main.orthographicSize -= _zoomSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Camera.main.orthographicSize += _zoomSpeed * Time.deltaTime;
        }

        //터치 컨트롤
        if (Input.touchCount == 1) //-이동
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
        else if (Input.touchCount == 2) //-줌 인/아웃
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                // 이전 프레임과 현재 프레임에서 두 손가락 사이의 거리 차이 계산
                float prevTouchDeltaMag = (touch1.position - touch1.deltaPosition - (touch2.position - touch2.deltaPosition)).magnitude;
                float touchDeltaMag = (touch1.position - touch2.position).magnitude;

                // 거리 차이를 기반으로 수행
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                Camera.main.orthographicSize += deltaMagnitudeDiff * _zoomSpeed;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, _minZoom, _maxZoom);
            }
        }

        // 줌 값 제한
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, _minZoom, _maxZoom);
    }
}