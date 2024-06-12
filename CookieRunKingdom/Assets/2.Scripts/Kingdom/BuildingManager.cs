using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private Collider2D _collider;
    private Building _building;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        GameObject buildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/Building");
        GameObject buildingObj = Instantiate(buildingPrefab, transform);
        _building = buildingObj.GetComponent<Building>();

        buildingObj.transform.localPosition = Vector3.zero;
        buildingObj.SetActive(false);
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
                    _building.Build(DataManager.Instance.GetBuildingData(key), mousePos);
            }
        }

        //디버깅용
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _building.Build(DataManager.Instance.GetBuildingData(101), new Vector3(0, -10, 0));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _building.Build(DataManager.Instance.GetBuildingData(201), new Vector3(10, -10, 0));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _building.Build(DataManager.Instance.GetBuildingData(301), new Vector3(20, -10, 0));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _building.Build(DataManager.Instance.GetBuildingData(401), new Vector3(30, -10, 0));
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _building.Build(DataManager.Instance.GetBuildingData(501), new Vector3(40, -10, 0));
        }
    }
}
