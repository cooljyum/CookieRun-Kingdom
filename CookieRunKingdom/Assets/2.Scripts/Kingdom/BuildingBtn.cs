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

    public void OnClickBuildingBtn()
    {
        print("Btn Click");
        KingdomManager.Instance.SelectBuilding(this);
    }

    public void SetActive(bool isActive)
    {
        _activeImage.SetActive(isActive);
    }
}
