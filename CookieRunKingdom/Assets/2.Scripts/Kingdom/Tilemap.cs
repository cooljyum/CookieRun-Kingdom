using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour
{
    private Collider2D _collider; //건설 시 활용하는 충돌 콜라이더

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
        if (Input.GetMouseButtonDown(0)) //클릭한 타일 위치에 건물 위치 고정
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

    private void LoadBuildings() //저장한 빌딩 로드
    {
        List<(int, Vector2)> buildingDatas = GameManager.Instance.LoadBuildings();
        for (int i = 0; i < buildingDatas.Count; i++)
        {
            GameObject buildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/Map/Building");
            GameObject buildingObj = Instantiate(buildingPrefab, transform);
            Building building = buildingObj.GetComponent<Building>();
            building.Build(DataManager.Instance.GetBuildingData(buildingDatas[i].Item1), buildingDatas[i].Item2);
        }
    }

    public void ConstructBuilding() //체크 버튼 클릭 시 호출 -> 건물 짓기
    {
        GameObject buildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/Map/Building");
        GameObject buildingObj = Instantiate(buildingPrefab, transform);
        Building building = buildingObj.GetComponent<Building>();
        building.Build(DataManager.Instance.GetBuildingData(KingdomManager.Instance.SelectedBuildingData.Key), KingdomManager.Instance.SelectedPosition);
    }
}
