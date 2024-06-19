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
        LoadBuildings(); //*빌딩저장됨*//
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //클릭한 타일 위치에 빌딩 짓기
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_collider.bounds.Contains(mousePos))
            {
                KingdomManager.Instance.SelectedBuilding.transform.position = mousePos;
            }
        }
    }

    private void LoadBuildings() //저장한 빌딩 로드
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
