using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBtn : MonoBehaviour
{
    [SerializeField]
    public BuildingData BuildingData;

    public void OnClickBuildingBtn()
    {
        print("Btn Click");
        KingdomManager.Instance.SelectBuilding(this);
    }
}
