using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FinishLine))]
public class FinishLineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var parent = (FinishLine) target;

        if (GUILayout.Button("Fix Position"))
        {
            parent.FixPosition();
            EditorUtility.SetDirty(parent);
        }
    }
}
