using System.Collections.Generic;
using Assets.Scripts.Utilities;
using TMPro;
using UnityEngine;

public class PlayerController : IAI
{
    [SerializeField] private Vector2Int spawnPos;
    [SerializeField] private TextMeshProUGUI coordinateText;
    [SerializeField] private LayerMask tileLayerMask;

    public bool isPlayersTurn { get; private set; } = true;
    private bool pathFound = false;
    public List<TileInfo> adjacentTile { get; private set; } = new List<TileInfo>();
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        TileInfo tile = gridController.GeTileInfo(spawnPos.x, spawnPos.y);
        transform.position = tile.transform.position;
        tile.SetIsOccupied(true);
        currentPos.x = tile.X;
        currentPos.y = tile.Y;
    }

    private void Update()
    {
        if (!isPlayersTurn) return;
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        // Cast a ray from the mouse position into the scene
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,100f,tileLayerMask))
        {
            // Check if the hit object has a TileInfo component
            TileInfo cubeInfo = hit.collider.GetComponent<TileInfo>();
            if (cubeInfo != null && !isMoving)
            {
                // Update UI text with the cube coordinates

                FindPath(cubeInfo);
                if (Input.GetMouseButton(0) && pathFound){
                    StartCoroutine(MoveAlongPath(currentPath));
                    FindAdjacentTile(cubeInfo);
                    isPlayersTurn = false;
                }
                return;
            } 
        }
            // If no cube hovered, clear the text
            coordinateText.text = string.Empty;
    }

    private void FindAdjacentTile(TileInfo tile)
    {
        adjacentTile.Clear();
        adjacentTile = gridController.GetAdjacentTiles(tile.X, tile.Y);
    }

    private void FindPath(TileInfo targetTile)
    {
        //if tile is occupide with enemy or obstacles return
        if (!targetTile.IsWalkable) return;
        List<TileInfo> path = FindMovePath(currentPos, new Vector2Int(targetTile.X, targetTile.Y));


        if (path != null && path.Count > 0)
        {
            ClearCurrentPath();
            currentPath = path;
            pathFound = true;
            if (showPathVisually)
            {
                ShowPath(path);
            }
            coordinateText.text = $"Grid Position: ({targetTile.X}, {targetTile.Y})";

        }
        else
        {
            pathFound = false;
            ClearCurrentPath();
            coordinateText.text = "Grid Position: ("+targetTile.X+","+ targetTile.Y+") \n No path found";
        }
    }
    public bool IsMoving() => isMoving;
    public Vector2Int GetCurrentPos() => currentPos;

    public void SetIsPlayersTurn(bool turn)
    {
        isPlayersTurn = turn;
    }
}
