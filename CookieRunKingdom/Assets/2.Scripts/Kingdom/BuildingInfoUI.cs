using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfoUI : MonoBehaviour
{
    public void OnClickOkayBtn() //Info-확인
    {
        print("OkayBtn Click");

    }

    public void OnClickExitBtn() //Info-나가기
    {
        print("ExitBtn Click");
        gameObject.SetActive(false);
    }
}
