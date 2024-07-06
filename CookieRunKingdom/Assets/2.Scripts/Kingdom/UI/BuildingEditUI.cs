using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingEditUI : MonoBehaviour  //* 건물 편집 기능 원형 UI *//
{
    [SerializeField]
    private Transform _parentObject; //비활성화용 부모 오브젝트

    private bool _isRotated = false; //건물 이미지 회전 여부

    public void OnClickExitBtn()
    {
        Debug.Log("Exit Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick");
        // Exit 버튼 클릭 시 수행할 동작 추가
    }

    public void OnClickCheckBtn() //Edit-체크
    {
        Debug.Log("Check Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick");
        KingdomManager.Instance.OnClickConstructBuilding();
    }

    public void OnClickRotateBtn() //Edit-회전
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

    public void OnClickInfoBtn() //Edit-정보
    {
        Debug.Log("Info Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick");
        // Info 버튼 클릭 시 수행할 동작 추가
    }
}
