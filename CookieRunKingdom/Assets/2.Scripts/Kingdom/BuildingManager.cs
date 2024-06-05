using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    static public BuildingManager Instance;

    [SerializeField]
    private GameObject _storePanel;

    private void Awake()
    {
        Instance = this;
    }

    public void OnClickBuildingBtn()
    {
        _storePanel.SetActive(true);
    }
}
