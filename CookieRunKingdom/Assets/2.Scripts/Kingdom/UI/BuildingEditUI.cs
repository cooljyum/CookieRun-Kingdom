using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingEditUI : MonoBehaviour
{
    [SerializeField]
    private Transform _parentObject;

    private bool _isRotated = false;

    public void OnClickExitBtn()
    {
        Debug.Log("Exit Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick");
        // Exit 버튼 클릭 시 수행할 동작 추가
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

    public void OnClickInfoBtn()
    {
        Debug.Log("Info Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick");
        // Info 버튼 클릭 시 수행할 동작 추가
    }
}
