using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    public GameObject[] gridCellPrefab;

    public Vector3 gridOffset = Vector3.zero;
    // private GridCell[,] grid;

    void Start()
    {
        GenerateGrid();
        CenterGrid();
    }

    void GenerateGrid()
    {
        // grid = new GridCell[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                GameObject cell = Instantiate(gridCellPrefab[(x + z) % 2], new Vector3(x, 0, z), Quaternion.identity);
                cell.transform.parent = transform;
                // grid[x, y] = cell.GetComponent<GridCell>();
            }
        }
    }
    
    void CenterGrid()
    {
        // Calculate the center based on grid size
        Vector3 centerPosition = new Vector3((gridWidth - 1) / 2.0f, 0, (gridHeight - 1) / 2.0f);
        transform.position = -centerPosition + gridOffset;  // Move the grid so it's centered
    }
}
