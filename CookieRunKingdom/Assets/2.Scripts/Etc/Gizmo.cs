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
            // 기지모의 색상을 설정
            Gizmos.color = GizmoColor;

            // 현재 오브젝트의 위치에 오프셋을 더하여 와이어프레임 네모를 그림
            Vector3 positionWithOffset = transform.position + GizmoOffset;
            Gizmos.DrawWireCube(positionWithOffset, GizmoSize);

            // Handles의 색상을 설정
            Handles.color = TextColor;

            // 텍스트를 그릴 위치를 계산
            Vector3 textPosition = positionWithOffset + TextOffset;

            // 텍스트를 그림
            Handles.Label(textPosition, GizmoText);

            //텍스트를 개체명으로 변경
            GizmoText = this.gameObject.name;
        }
    }

}
