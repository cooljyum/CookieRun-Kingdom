using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingEditUI : MonoBehaviour
{
    public void OnClickExitBtn()
    {
        Debug.Log("Exit Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick");
        // Exit ��ư Ŭ�� �� ������ ���� �߰�
    }

    public void OnClickCheckBtn()
    {
        Debug.Log("Check Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick");
        KingdomManager.Instance.OnClickConstructBuilding();
    }

    public void OnClickRotateBtn()
    {
        Debug.Log("Rotate Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick");
        // Rotate ��ư Ŭ�� �� ������ ���� �߰�
    }

    public void OnClickInfoBtn()
    {
        Debug.Log("Info Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick");
        // Info ��ư Ŭ�� �� ������ ���� �߰�
    }
}
