using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfoUI : MonoBehaviour
{
    public void OnClickOkayBtn() //Info-Ȯ��
    {
        print("OkayBtn Click");

    }

    public void OnClickExitBtn() //Info-������
    {
        print("ExitBtn Click");
        gameObject.SetActive(false);
    }
}
