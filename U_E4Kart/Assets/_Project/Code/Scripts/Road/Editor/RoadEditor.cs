using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Road))]
public class RoadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var parent = (Road) target;

        if (GUILayout.Button("Refresh Collisions"))
        {
            parent.RefreshCollision();
            EditorUtility.SetDirty(parent);
        }
    }
}
