using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private GameObject gridCubePrefab;

    [SerializeField] private float spacing = 1f;

    [SerializeField] private int gridSize = 10;

    private TileInfo[,] tileInfoList;

    private void Awake()
    {
        tileInfoList = new TileInfo[gridSize, gridSize];
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(y * spacing, 0f, x * spacing);
                GameObject cubeInstance = Instantiate(gridCubePrefab, position, Quaternion.identity, transform);

                TileInfo info = cubeInstance.GetComponent<TileInfo>();
                if (info != null)
                {
                    info.SetCoordinates(x, y);
                    tileInfoList[x, y] = info;
                }
                else
                {
                    Debug.LogWarning(
                        $"GridManagerController: TileInfo component missing on prefab instance at ({x},{y}).");
                }
            }
        }
    }

    public int GetGridSize() => gridSize;

    public Transform GetTileTransform(int x, int y)
    {
        return tileInfoList[x, y].gameObject.transform;
    }

    public TileInfo GeTileInfo(int x, int y)
    {
        return tileInfoList[x, y];
    }

    public Vector3 GetTileWorldPosition(int x, int y)
    {
        if (IsValidCoordinate(x, y))
        {
            return new Vector3(y * spacing, 0f, x * spacing);
        }

        return Vector3.zero;
    }

    public bool IsValidCoordinate(int x, int y)
    {
        return x >= 0 && x < gridSize && y >= 0 && y < gridSize;
    }

    public List<TileInfo> GetAdjacentTiles(int x, int y)
    {
        List<TileInfo> adjacentTiles = new List<TileInfo>();

        // Define the 4 directions: up, down, left, right
        int[] deltaX = { 0, 0, -1, 1 };
        int[] deltaY = { -1, 1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int newX = x + deltaX[i];
            int newY = y + deltaY[i];

            // Check if the coordinate is valid and the tile is not occupied
            if (IsValidCoordinate(newX, newY))
            {
                TileInfo tileInfo = tileInfoList[newX, newY];
                if (tileInfo != null && !tileInfo.IsOccupied)
                {
                    adjacentTiles.Add(tileInfo);
                }
            }
        }

        return adjacentTiles;
    }
}