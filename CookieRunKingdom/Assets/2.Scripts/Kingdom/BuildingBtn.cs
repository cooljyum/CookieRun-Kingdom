using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBtn : MonoBehaviour
{
    [SerializeField]
    public BuildingData BuildingData;

    public void OnClickBtn()
    {
        KingdomManager.Instance.SelectBuilding(this);
    }
}
