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
        LoadBuildings(); //*���������*//
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Ŭ���� Ÿ�� ��ġ�� ���� ����
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_collider.bounds.Contains(mousePos))
            {
                if (KingdomManager.Instance.SelectedBuilding != null)
                {
                    KingdomManager.Instance.SetSnappedPosition();
                    KingdomManager.Instance.SetSelectedBuilding(null); //���� ����
                }
            }
        }
    }

    private void LoadBuildings() //������ ���� �ε�
    {
        List<(int, Vector2)> buildingDatas = GameManager.Instance.LoadBuildings();
        foreach (var buildingData in buildingDatas)
        {
            GameObject buildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/Map/Building");
            GameObject buildingObj = Instantiate(buildingPrefab, transform);
            Building building = buildingObj.GetComponent<Building>();
            building.Build(DataManager.Instance.GetBuildingData(buildingData.Item1), buildingData.Item2);

            //_buildings.Add(building);
        }
    }
}
