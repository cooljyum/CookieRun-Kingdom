using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMapData
{
    public Tilemap[,] gridData;

    public GridMapData(Tilemap[,] gridMapData)
    {
        this.gridData = gridMapData;
    }

    public bool CheckGrid(int x, int y)
    {
        return gridData[y, x].isEmpty;
    }

    public void VisualizeGridMapData()
    {
        for(int i = 0; i < gridData.GetLength(1); i++)
            for(int j = 0; j < gridData.GetLength(0); j++)
                if(gridData[j, i] != null)
                    gridData[j, i].Visualize();
    }
}

