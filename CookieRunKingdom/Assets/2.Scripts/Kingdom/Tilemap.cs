using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour
{
    private Collider2D _collider;
    //private List<Building> _buildings = new List<Building>();

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        LoadBuildings();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Ŭ���� Ÿ�� ��ġ�� �ǹ� ��ġ ����
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_collider.bounds.Contains(mousePos))
            {
                if (KingdomManager.Instance.SelectedBuildingData == null) return;

                KingdomManager.Instance.SetSnappedPosition();
                KingdomManager.Instance.IsBuildingFixed = true;
            }
        }
    }

    private void LoadBuildings() //������ ���� �ε�
    {
        List<(int, Vector2)> buildingDatas = GameManager.Instance.LoadBuildings();
        for (int i = 0; i < buildingDatas.Count; i++)
        {
            GameObject buildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/Map/Building");
            GameObject buildingObj = Instantiate(buildingPrefab, transform);
            Building building = buildingObj.GetComponent<Building>();
            building.Build(DataManager.Instance.GetBuildingData(buildingDatas[i].Item1), buildingDatas[i].Item2);

            //_buildings.Add(building);
        }
    }

    public void ConstructBuilding() //üũ ��ư Ŭ�� �� ȣ�� -> �ǹ� ����
    {
        GameObject buildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/Map/Building");
        GameObject buildingObj = Instantiate(buildingPrefab, transform);
        Building building = buildingObj.GetComponent<Building>();
        building.Build(DataManager.Instance.GetBuildingData(KingdomManager.Instance.SelectedBuildingData.Key), KingdomManager.Instance.SelectedPosition);
    }
}
