using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager instance = null;
    public static GridManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GridManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "SingletonController";
                    instance = go.AddComponent<GridManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    [SerializeField] private Grid grid;

    public Vector2Int startPoint { get; private set; }
    public Grid Grid => grid;
    public GridMapData tileGridData { get; private set; }
    public GridMapData buildingGridData { get; private set; }


    public void SetGridMapData(GridMapData tileGridData, GridMapData buildingGridData, Vector2Int startPoint)
    {
        this.tileGridData = tileGridData;
        this.buildingGridData = buildingGridData;
        this.startPoint = startPoint;
    }

    public bool ValidTileCheck(int gridX, int gridY)
    {
        gridX -= startPoint.x;
        gridY -= startPoint.y;

        if (gridY >= buildingGridData.gridData.GetLength(0) || gridY < 0 ||
            gridX >= buildingGridData.gridData.GetLength(1) || gridX < 0)
            return false;

        Tilemap tile = buildingGridData.gridData[gridY, gridX];

        if(tile == null)
            return false;
        if(tile.isEmpty)
            return true;
        else
            return false;
    }

    public void UpdateTile(int gridX, int gridY, bool isEmpty)
    {
        gridX -= startPoint.x;
        gridY -= startPoint.y;

        Tilemap tile = buildingGridData.gridData[gridY, gridX];

        if (tile == null)
            return;

        tile.isEmpty = isEmpty;
    }

    public Vector3 ReturnEmptyTilePosition()
    {
        while(true)
        {
            int randomX = Random.Range(0, buildingGridData.gridData.GetLength(1));
            int randomY = Random.Range(0, buildingGridData.gridData.GetLength(0));

            if (buildingGridData.gridData[randomY, randomX] == null)
                continue;
            if(buildingGridData.gridData[randomY, randomX].isEmpty)
            {
                Tilemap tile = buildingGridData.gridData[randomY, randomX];
                return new Vector3(tile.x, tile.y, 0f);
            }
        }
    }
}
