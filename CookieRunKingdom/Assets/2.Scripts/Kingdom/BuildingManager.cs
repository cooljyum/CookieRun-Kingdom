using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private GameObject _buildingPrefab;
    private Collider2D _collider;
    private Building _building;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _buildingPrefab = Resources.Load<GameObject>("Prefabs/Kingdom/Building");

        GameObject buildingObj = Instantiate(_buildingPrefab, transform);
        buildingObj.transform.localPosition = Vector3.zero;
        _building = buildingObj.GetComponent<Building>();
        buildingObj.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_collider.bounds.Contains(mousePos))
            {
                int key = KingdomManager.Instance.Building();

                if (key != 0)
                    _building.Build(DataManager.Instance.GetBuildingData(key));
            }
        }
    }
}
