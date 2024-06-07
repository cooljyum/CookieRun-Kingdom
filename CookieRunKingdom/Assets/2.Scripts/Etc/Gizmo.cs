using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    // �׸��� ������ ����
    public Color GizmoColor = Color.white;

    // �׸��� ũ�⸦ ����
    public Vector3 GizmoSize = new Vector3(1, 1, 1);

    // �׸��� �������� ����
    public Vector3 GizmoOffset = Vector3.zero;

    // ǥ���� �ؽ�Ʈ�� ����
    public string GizmoText = "Gizmo Text";

    // �ؽ�Ʈ �������� ����
    public Vector3 TextOffset = new Vector3(0, 1, 0);

    // �ؽ�Ʈ ������ ����
    public Color TextColor = Color.white;

    // ������ ǥ������ ���θ� ����
    public bool ShowGizmo = true;

    // �� �信�� ������ �׸��ϴ�.
    void OnDrawGizmos()
    {
        if (ShowGizmo)
        {
            // ���� ������Ʈ�� ��ġ�� �������� ���Ͽ� �׸��� ��ġ�� ���
            Vector3 positionWithOffset = transform.position + GizmoOffset;

            // �������� ȸ�� ����� ���
            Quaternion rotation = transform.rotation;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(positionWithOffset, rotation, Vector3.one);

            // �������� ������ ����
            Gizmos.color = GizmoColor;

            // ȸ�� ����� ����
            Gizmos.matrix = rotationMatrix;

            // ���̾������� �׸� �׸�
            Gizmos.DrawWireCube(Vector3.zero, GizmoSize);

            // ���� ��ķ� ����
            Gizmos.matrix = Matrix4x4.identity;

            // Handles�� ������ ����
            GUIStyle style = new GUIStyle();
            style.normal.textColor = TextColor;

            // �ؽ�Ʈ�� �׸� ��ġ�� ���
            Vector3 textPosition = positionWithOffset + TextOffset;

            // �ؽ�Ʈ�� �׸�
            Handles.Label(textPosition, GizmoText, style);

            // �ؽ�Ʈ�� ��ü������ ����
            if (string.IsNullOrEmpty(GizmoText))
            {
                GizmoText = gameObject.name;
            }
        }
    }
}
