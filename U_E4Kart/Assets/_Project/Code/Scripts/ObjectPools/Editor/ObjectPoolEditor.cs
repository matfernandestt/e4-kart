using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectPoolBase), true)]
public class ObjectPoolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var parent = (ObjectPoolBase) target;

        if (GUILayout.Button("Initialize pool"))
        {
            parent.SpawnObjects();
        }
    }
}
