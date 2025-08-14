using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleDataSO", menuName = "ScriptableObjects/ObstacleData")]
public class ObstacleData : ScriptableObject
{
    public bool[] blockedTiles = new bool[100]; // 10x10 gridSize
}
