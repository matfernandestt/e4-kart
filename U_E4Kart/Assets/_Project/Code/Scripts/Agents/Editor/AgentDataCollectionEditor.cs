using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AgentDataCollection))]
public class AgentDataCollectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var parent = (AgentDataCollection) target;
        
        if (GUILayout.Button("Refresh Agent List"))
        {
            var agents = new List<AgentData>();
            var assetsGuid = AssetDatabase.FindAssets("", new[] {"Assets/_Project/Data/Agents"});
            foreach (var guid in assetsGuid)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<AgentData>(path);
                
                agents.Add(asset);
            }
            parent.gameAgents = agents.ToArray();
            EditorUtility.SetDirty(parent);
        }
    }
}
