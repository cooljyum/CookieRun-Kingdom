using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingEditUI : MonoBehaviour
{
    public void OnClickExitBtn()
    {
        Debug.Log("Exit Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick2");
        // Exit 버튼 클릭 시 수행할 동작 추가
    }

    public void OnClickCheckBtn()
    {
        Debug.Log("Check Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick2");
        KingdomManager.Instance.OnClickConstructBuilding();
    }

    public void OnClickRotateBtn()
    {
        Debug.Log("Rotate Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick2");
        // Rotate 버튼 클릭 시 수행할 동작 추가
    }

    public void OnClickInfoBtn()
    {
        Debug.Log("Info Btn Click!");
        SoundManager.Instance.PlayFX("BtnClick2");
        // Info 버튼 클릭 시 수행할 동작 추가
    }
}
