using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap tilemapPrefab;
    [SerializeField] private Transform parent;

    [SerializeField] private Grid grid;
    [SerializeField] private LayerMask detectedLayer;
    
    private Tilemap[,] mapData;
    private int startOrder;

    public void Generate()
    {
        Vector3Int start = grid.WorldToCell(Utils.MapStartPoint);
        Vector3Int end = grid.WorldToCell(Utils.MapEndPoint);

        int minX = start.x > end.x ? end.x : start.x;
        int maxX = start.x > end.x ? start.x : end.x;
        int minY = start.y > end.y ? end.y : start.y;
        int maxY = start.y > end.y ? start.y : end.y;

        GenerateGround(new Vector2Int(minX, minY), new Vector2Int(maxX, maxY));
    }

    private void GenerateGround(Vector2Int start, Vector2Int end)
    {
        mapData = new Tilemap[end.y - start.y + 1, end.x - start.x + 1];
        for (int y = end.y; y >= start.y; y--)
        {
            for (int x = end.x; x >= start.x; x--)
            {
                Vector3 gridPosition = grid.CellToWorld(new Vector3Int(x, y, 0));
                RaycastHit2D hit = Physics2D.Raycast(gridPosition, Vector2.zero, 0f, detectedLayer);

                if (hit.collider != null)
                {
                    Tilemap tile = Instantiate(tilemapPrefab, parent);
                    tile.SetInfo(gridPosition.x, gridPosition.y, startOrder);
                    mapData[y - start.y, x - start.x] = tile;
                }
            }
        }

        GridMapData tileGridData = new GridMapData(mapData);
        GridMapData buildingGridData = new GridMapData(mapData);

        GridManager.Instance.SetGridMapData(tileGridData, buildingGridData, new Vector2Int(start.x, start.y));
    }
}
