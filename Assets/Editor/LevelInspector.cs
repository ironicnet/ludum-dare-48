using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level))]
public class LevelInspector : Editor
{
    private void OnSceneGUI()
    {
        Level level = target as Level;

        Handles.color = new Color(239, 127, 26);
        if (level.StartPositions!=null) {
          for (int i = 0; i < level.StartPositions.Length; i++)
          {
              UnityEditor.Handles.DrawWireCube(level.StartPositions[i].position, i == 0 ? new Vector3(1,1.5f,1) : Vector3.one);
          }
        }

        Handles.color = Color.red;
        if (level.Boundaries!=null) {
          for (int i = 0; i < level.Boundaries.Length; i++)
          {
              UnityEditor.Handles.DrawWireCube(level.transform.position + level.Boundaries[i].Center, level.Boundaries[i].Size);

          }
        }
    }
}