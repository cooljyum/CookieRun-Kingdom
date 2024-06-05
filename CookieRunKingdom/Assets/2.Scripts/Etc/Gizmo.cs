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
            // �������� ������ ����
            Gizmos.color = GizmoColor;

            // ���� ������Ʈ�� ��ġ�� �������� ���Ͽ� ���̾������� �׸� �׸�
            Vector3 positionWithOffset = transform.position + GizmoOffset;
            Gizmos.DrawWireCube(positionWithOffset, GizmoSize);

            // Handles�� ������ ����
            Handles.color = TextColor;

            // �ؽ�Ʈ�� �׸� ��ġ�� ���
            Vector3 textPosition = positionWithOffset + TextOffset;

            // �ؽ�Ʈ�� �׸�
            Handles.Label(textPosition, GizmoText);

            //�ؽ�Ʈ�� ��ü������ ����
            GizmoText = this.gameObject.name;
        }
    }

}
