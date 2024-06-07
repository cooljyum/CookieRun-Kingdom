using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    // 네모의 색상을 설정
    public Color GizmoColor = Color.white;

    // 네모의 크기를 설정
    public Vector3 GizmoSize = new Vector3(1, 1, 1);

    // 네모의 오프셋을 설정
    public Vector3 GizmoOffset = Vector3.zero;

    // 표시할 텍스트를 설정
    public string GizmoText = "Gizmo Text";

    // 텍스트 오프셋을 설정
    public Vector3 TextOffset = new Vector3(0, 1, 0);

    // 텍스트 색상을 설정
    public Color TextColor = Color.white;

    // 기지모를 표시할지 여부를 설정
    public bool ShowGizmo = true;

    // 씬 뷰에서 기지모를 그립니다.
    void OnDrawGizmos()
    {
        if (ShowGizmo)
        {
            // 현재 오브젝트의 위치에 오프셋을 더하여 네모의 위치를 계산
            Vector3 positionWithOffset = transform.position + GizmoOffset;

            // 기지모의 회전 행렬을 계산
            Quaternion rotation = transform.rotation;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(positionWithOffset, rotation, Vector3.one);

            // 기지모의 색상을 설정
            Gizmos.color = GizmoColor;

            // 회전 행렬을 적용
            Gizmos.matrix = rotationMatrix;

            // 와이어프레임 네모를 그림
            Gizmos.DrawWireCube(Vector3.zero, GizmoSize);

            // 원래 행렬로 복원
            Gizmos.matrix = Matrix4x4.identity;

            // Handles의 색상을 설정
            GUIStyle style = new GUIStyle();
            style.normal.textColor = TextColor;

            // 텍스트를 그릴 위치를 계산
            Vector3 textPosition = positionWithOffset + TextOffset;

            // 텍스트를 그림
            Handles.Label(textPosition, GizmoText, style);

            // 텍스트를 개체명으로 변경
            if (string.IsNullOrEmpty(GizmoText))
            {
                GizmoText = gameObject.name;
            }
        }
    }
}
