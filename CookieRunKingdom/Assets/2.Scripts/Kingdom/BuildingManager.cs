using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private Collider2D _collider;
    private List<Building> _buildings = new List<Building>();

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //클릭한 타일 위치에 빌딩 짓기
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_collider.bounds.Contains(mousePos))
            {
                int key = KingdomManager.Instance.Building();

                if (key != 0)
                {
                    //새로운 빌딩을 생성하고 리스트에 추가
                    GameObject buildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/Map/Building");
                    GameObject buildingObj = Instantiate(buildingPrefab, transform);
                    Building newBuilding = buildingObj.GetComponent<Building>();
                    newBuilding.Build(DataManager.Instance.GetBuildingData(key), mousePos);

                    _buildings.Add(newBuilding);
                }
            }
        }
    }
}
