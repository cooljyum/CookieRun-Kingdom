using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float _cameraSpeed = 7f;
    private Vector2 touchStart; // ��ġ ���� ����
    private Vector3 cameraStartPos; // ��ġ ���� �� ī�޶� ��ġ
    private float _zoomSpeed = 5f; //0.1f; // Ű �� �ӵ� / ��ġ �� �ӵ� //*���� ����*//
    private float _minZoom = 5f; // �ּ� �� ��
    private float _maxZoom = 20f; // �ִ� �� ��

    private void Update()
    {
        //����Ű ��Ʈ�� - �̵�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);

        transform.position += movement * _cameraSpeed * Time.deltaTime;

        //����Ű ��Ʈ�� - �� ��/�ƿ�
        if (Input.GetKey(KeyCode.Q))
        {
            Camera.main.orthographicSize -= _zoomSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Camera.main.orthographicSize += _zoomSpeed * Time.deltaTime;
        }

        //��ġ ��Ʈ��
        if (Input.touchCount == 1) //-�̵�
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
        else if (Input.touchCount == 2) //-�� ��/�ƿ�
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                // ���� �����Ӱ� ���� �����ӿ��� �� �հ��� ������ �Ÿ� ���� ���
                float prevTouchDeltaMag = (touch1.position - touch1.deltaPosition - (touch2.position - touch2.deltaPosition)).magnitude;
                float touchDeltaMag = (touch1.position - touch2.position).magnitude;

                // �Ÿ� ���̸� ������� ����
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                Camera.main.orthographicSize += deltaMagnitudeDiff * _zoomSpeed;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, _minZoom, _maxZoom);
            }
        }

        // �� �� ����
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, _minZoom, _maxZoom);
    }
}