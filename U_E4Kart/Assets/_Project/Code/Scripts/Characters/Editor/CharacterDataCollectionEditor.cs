using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterDataCollection))]
public class CharacterDataCollectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var parent = (CharacterDataCollection) target;
        
        if (GUILayout.Button("Refresh Character List"))
        {
            var agents = new List<CharacterData>();
            var assetsGuid = AssetDatabase.FindAssets("", new[] {"Assets/_Project/Data/Characters"});
            foreach (var guid in assetsGuid)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<CharacterData>(path);
                
                agents.Add(asset);
            }
            parent.gameAgents = agents.ToArray();
            EditorUtility.SetDirty(parent);
        }
    }
}
