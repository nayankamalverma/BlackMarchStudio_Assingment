using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public abstract class IAI : MonoBehaviour
    {
        [SerializeField] protected GridController gridController;
        [SerializeField] protected float moveSpeed = 2f;
        [SerializeField] protected bool showPathVisually = true;
        
        protected bool isMoving = false;
        protected List<TileInfo> currentPath = new List<TileInfo>();
        protected Vector2Int currentPos;

        protected List<TileInfo> FindMovePath(Vector2Int start, Vector2Int goal)
        {
            // Priority queue for open set (using List for simplicity, could use proper priority queue for better performance)
            List<PathNode> openSet = new List<PathNode>();
            HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

            PathNode startNode = new PathNode
            {
                position = start,
                gCost = 0,
                hCost = CalculateHeuristic(start, goal),
                parent = null
            };
            startNode.fCost = startNode.gCost + startNode.hCost;

            openSet.Add(startNode);

            // Directions for 4-way movement (up, down, left, right)
            Vector2Int[] directions = {
            new Vector2Int(0, 1),   // Up
            new Vector2Int(0, -1),  // Down
            new Vector2Int(1, 0),   // Right
            new Vector2Int(-1, 0)   // Left
        };

            while (openSet.Count > 0)
            {
                // Find node with lowest f cost
                PathNode currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost ||
                        (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode.position);

                // Check if we reached the goal
                if (currentNode.position == goal)
                {
                    return ReconstructPath(currentNode);
                }

                // Check all neighbors
                foreach (Vector2Int direction in directions)
                {
                    Vector2Int neighborPos = currentNode.position + direction;

                    // Skip if out of bounds
                    if (!gridController.IsValidCoordinate(neighborPos.x, neighborPos.y))
                        continue;

                    // Skip if already in closed set
                    if (closedSet.Contains(neighborPos))
                        continue;

                    TileInfo neighborTile = gridController.GeTileInfo(neighborPos.x, neighborPos.y);

                    // Skip if not walkable (but allow goal tile even if occupied by player)
                    if (!neighborTile.IsWalkable && neighborPos != goal)
                        continue;

                    float tentativeGCost = currentNode.gCost + 1f; // Assuming uniform cost of 1 for each step

                    // Check if this neighbor is already in open set
                    PathNode existingNode = openSet.Find(node => node.position == neighborPos);

                    if (existingNode == null)
                    {
                        // Add new node to open set
                        PathNode newNode = new PathNode
                        {
                            position = neighborPos,
                            gCost = tentativeGCost,
                            hCost = CalculateHeuristic(neighborPos, goal),
                            parent = currentNode
                        };
                        newNode.fCost = newNode.gCost + newNode.hCost;
                        openSet.Add(newNode);
                    }
                    else if (tentativeGCost < existingNode.gCost)
                    {
                        // Update existing node with better path
                        existingNode.gCost = tentativeGCost;
                        existingNode.fCost = existingNode.gCost + existingNode.hCost;
                        existingNode.parent = currentNode;
                    }
                }
            }
            // No path found
            return null;
        }

        protected float CalculateHeuristic(Vector2Int a, Vector2Int b)
        {
            // Manhattan distance heuristic
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        private List<TileInfo> ReconstructPath(PathNode goalNode)
        {
            List<TileInfo> path = new List<TileInfo>();
            PathNode currentNode = goalNode;

            while (currentNode != null)
            {
                TileInfo tile = gridController.GeTileInfo(currentNode.position.x, currentNode.position.y);
                path.Insert(0, tile); // Insert at beginning to reverse the path
                currentNode = currentNode.parent;
            }

            // Remove the starting tile from the path since we're already there
            if (path.Count > 0)
                path.RemoveAt(0);

            return path;
        }

        protected void ShowPath(List<TileInfo> path)
        {
            foreach (TileInfo tile in path)
            {
                // Change tile color to indicate path (you'll need to implement this in TileInfo)
                tile.GetComponent<Renderer>().material.color = Color.blue;
            }
        }

        protected void ClearCurrentPath()
        {
            foreach (TileInfo tile in currentPath)
            {
                // Reset tile color (you'll need to implement this in TileInfo)
                tile.GetComponent<Renderer>().material.color = Color.white;
            }
            currentPath.Clear();
        }

        protected IEnumerator MoveAlongPath(List<TileInfo> path)
        {
            isMoving = true;

            // Clear occupation of current tile
            TileInfo currentTile = gridController.GeTileInfo(currentPos.x, currentPos.y);
            currentTile.SetIsOccupied(false);

            foreach (TileInfo tile in path)
            {
                Vector3 startPos = transform.position;
                Vector3 targetPos = tile.transform.position;
                float elapsedTime = 0f;
                float journeyTime = 1f / moveSpeed;

                while (elapsedTime < journeyTime)
                {
                    elapsedTime += Time.deltaTime;
                    float fractionOfJourney = elapsedTime / journeyTime;
                    transform.position = Vector3.Lerp(startPos, targetPos, fractionOfJourney);
                    yield return null;
                }

                transform.position = targetPos;
                currentPos.x = tile.X;
                currentPos.y = tile.Y;
            }

            // Set occupation of final tile
            TileInfo finalTile = gridController.GeTileInfo(currentPos.x, currentPos.y);
            finalTile.SetIsOccupied(true);

            // Clear path visualization
            if (showPathVisually)
            {
                ClearCurrentPath();
            }

            isMoving = false;
        }

        // Helper class for A* pathfinding
        private class PathNode
        {
            public Vector2Int position;
            public float gCost; // Distance from start
            public float hCost; // Heuristic distance to goal
            public float fCost; // Total cost (g + h)
            public PathNode parent;
        }
    }
}