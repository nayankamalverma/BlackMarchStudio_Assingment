using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private ObstacleData obstacleData;
    [SerializeField] private GameObject obstaclePrebab;
    [Space]
    [SerializeField] private GridController gridController;

    private int gridSize;
    private void Start()
    {
        gridSize = gridController.GetGridSize();
        InstantiateObstacles();
    }

    private void InstantiateObstacles()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                int index = x * gridSize + y;
                TileInfo tile = gridController.GeTileInfo(x, y);
                if (obstacleData.blockedTiles[index])
                {
                    GameObject obj = Instantiate(obstaclePrebab, tile.transform);
                    tile.SetIsWalkable(false);
                    obj.transform.parent = null;
                }
                else
                {
                    tile.SetIsWalkable(true);
                }
            }
        }
    }
}
