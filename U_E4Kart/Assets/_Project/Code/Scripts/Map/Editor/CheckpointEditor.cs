using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Checkpoint))]
public class CheckpointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var parent = (Checkpoint) target;

        if (GUILayout.Button("Fix Position"))
        {
            parent.FixPosition();
            EditorUtility.SetDirty(parent);
        }
    }
}
