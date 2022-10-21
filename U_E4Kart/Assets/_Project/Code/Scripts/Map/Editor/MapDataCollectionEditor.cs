using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapDataCollection))]
public class MapDataCollectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var parent = (MapDataCollection) target;
        
        if (GUILayout.Button("Refresh Map List"))
        {
            var maps = new List<MapData>();
            maps.Add(null);
            var assetsGuid = AssetDatabase.FindAssets("", new[] {"Assets/_Project/Data/Maps"});
            foreach (var guid in assetsGuid)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<MapData>(path);
                
                maps.Add(asset);
            }
            parent.maps = maps.ToArray();
            EditorUtility.SetDirty(parent);
        }
    }
}
