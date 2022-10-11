using UnityEngine;
using UnityEditor;

namespace EpopeiaGames.Utilities
{
    [InitializeOnLoad]
    public static class HierarchyWindowGroupHeader
    {
        static HierarchyWindowGroupHeader()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        static void HierarchyWindowItemOnGUI(int instanceId, Rect selectionRect)
        {
            var gameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;

            if (gameObject != null && gameObject.name.StartsWith("---", System.StringComparison.Ordinal))
            {
                EditorGUI.DrawRect(selectionRect, Color.gray);
                EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("---", "").ToUpperInvariant());
            }
        }
    }
}