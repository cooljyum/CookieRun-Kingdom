using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBtn : MonoBehaviour
{
    public BuildingData BuildingData;

    private GameObject _activeImage;

    private void Awake()
    {
        _activeImage = transform.parent.GetChild(4).gameObject;
    }

    public void OnClickBuildingBtn() //�ǹ� ����
    {
        print("Btn Click");
        KingdomManager.Instance.SelectBuilding(this);
    }

    public void SetInActive(bool isActive) //���� �� �ǹ� ��ư ��Ȱ��ȭ
    {
        _activeImage.SetActive(isActive);
    }
}
