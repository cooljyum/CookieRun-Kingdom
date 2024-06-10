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

    public void OnClickBuildingBtn() //건물 선택
    {
        print("Btn Click");
        KingdomManager.Instance.SelectBuilding(this);
    }

    public void SetInActive(bool isActive) //상점 내 건물 버튼 비활성화
    {
        _activeImage.SetActive(isActive);
    }
}
