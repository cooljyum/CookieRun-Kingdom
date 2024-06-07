using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float _cameraSpeed = 3f;
    private Vector2 touchStart; // ��ġ ���� ����
    private Vector3 cameraStartPos; // ��ġ ���� �� ī�޶� ��ġ
    private float _zoomSpeed = 2f;
    private float _minZoom = 3f; // �ּ� �� ��
    private float _maxZoom = 10f; // �ִ� �� ��

    private void Update()
    {
        //����Ű �̵�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);

        transform.position += movement * _cameraSpeed * Time.deltaTime;

        //��ġ �̵�
        if (Input.touchCount == 1) // �ϳ��� ��ġ�� �����Ǿ��� ��
        {
            Touch touch = Input.GetTouch(0); // ù ��° ��ġ �Է��� ������

            if (touch.phase == TouchPhase.Began)
            {
                // ��ġ�� ���۵Ǹ� ��ġ ���� ��ġ�� ī�޶� ���� ��ġ�� ����
                touchStart = touch.position;
                cameraStartPos = transform.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // ��ġ �̵��� �����Ͽ� ī�޶� �̵�
                Vector2 touchDelta = touch.position - touchStart;
                Vector3 newCameraPos = cameraStartPos - new Vector3(touchDelta.x * _cameraSpeed * Time.deltaTime, touchDelta.y * _cameraSpeed * Time.deltaTime, 0);

                transform.position = newCameraPos;
            }
        }

        //�� ��/�ƿ�
        if (Input.GetKey(KeyCode.Q))
        {
            Camera.main.orthographicSize -= _zoomSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Camera.main.orthographicSize += _zoomSpeed * Time.deltaTime;
        }

        // �� �� ����
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, _minZoom, _maxZoom);
    }

}