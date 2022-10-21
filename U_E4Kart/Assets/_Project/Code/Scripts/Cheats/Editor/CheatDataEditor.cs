using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CheatData))]
public class CheatDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var parent = (CheatData) target;

        if (GUILayout.Button("Refresh Checkers"))
        {
            parent.InitializeCheat();
            EditorUtility.SetDirty(parent);
        }
    }
}
