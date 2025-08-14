using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }

    private bool isWalkable = true;
    private bool isOccupied = false;
    private Color originalColor;
    private Renderer tileRenderer;

    private void Awake()
    {
        tileRenderer = GetComponent<Renderer>();
        if (tileRenderer != null)
        {
            originalColor = tileRenderer.material.color;
        }
    }

    public void SetCoordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool IsWalkable => isWalkable && !isOccupied;
    public bool IsOccupied => isOccupied;

    public void SetIsWalkable(bool walkable)
    {
        isWalkable = walkable;
        UpdateVisual();
    }

    public void SetIsOccupied(bool occupied)
    {
        isOccupied = occupied;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (tileRenderer == null) return;

        if (!isWalkable)
        {
            tileRenderer.material.color = Color.red; // Unwalkable tiles are red
        }
        else if (isOccupied)
        {
            tileRenderer.material.color = Color.green; // Occupied tiles are green
        }
        else
        {
            tileRenderer.material.color = originalColor; // Default color
        }
    }

    public void SetPathColor()
    {
        if (tileRenderer != null)
        {
            tileRenderer.material.color = Color.blue;
        }
    }

    public void ResetColor()
    {
        if (tileRenderer != null)
        {
            UpdateVisual();
        }
    }
}