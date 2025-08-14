using UnityEngine;
using UnityEditor;

public class GridObstacleEditor : EditorWindow
{
    private ObstacleData obstacleData;

    private const int gridSize = 10;

    [MenuItem("Tools/Grid Obstacle Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridObstacleEditor>("Grid Obstacle Editor");
    }

    private void OnGUI()
    {
        // Select or drag ObstacleData ScriptableObject
        obstacleData = EditorGUILayout.ObjectField("Obstacle Data", obstacleData, typeof(ObstacleData), false) as ObstacleData;

        if (obstacleData == null)
        {
            EditorGUILayout.HelpBox("Please assign an ObstacleData ScriptableObject.", MessageType.Warning);
            return;
        }

        EditorGUILayout.LabelField("Toggle obstacles on the grid:");

        // Display 10x10 toggles representing blocked tiles
        for (int x = 0; x < gridSize; x++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int y = 0; y < gridSize; y++)
            {
                int index = x * gridSize + y;
                bool currentValue = obstacleData.blockedTiles[index];

                // Toggle button for each tile
                bool newValue = GUILayout.Toggle(currentValue, $"{x}-{y}");

                // Update bool array if toggled changed
                if (newValue != currentValue)
                {
                    Undo.RecordObject(obstacleData, "Toggle blocked tile");
                    obstacleData.blockedTiles[index] = newValue;
                    EditorUtility.SetDirty(obstacleData);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}