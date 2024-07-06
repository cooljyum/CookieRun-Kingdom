using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingEditUI : MonoBehaviour  //* �ǹ� ���� ��� ���� UI *//
{
    [SerializeField]
    private Transform _parentObject; //��Ȱ��ȭ�� �θ� ������Ʈ

    private bool _isRotated = false; //�ǹ� �̹��� ȸ�� ����

    public void OnClickExitBtn()
    {
        Debug.Log("Exit Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick");
        // Exit ��ư Ŭ�� �� ������ ���� �߰�
    }

    public void OnClickCheckBtn() //Edit-üũ
    {
        Debug.Log("Check Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick");
        KingdomManager.Instance.OnClickConstructBuilding();
    }

    public void OnClickRotateBtn() //Edit-ȸ��
    {
        Debug.Log("Rotate Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick");
        if (_isRotated)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        _isRotated = !_isRotated;
    }

    public void OnClickInfoBtn() //Edit-����
    {
        Debug.Log("Info Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick");
        // Info ��ư Ŭ�� �� ������ ���� �߰�
    }
}
