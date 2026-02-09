
using UnityEditor;
using UnityEngine;

namespace Poly.Common.Editor
{
    public class FPolySceneReferencePostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
        {
            // Skip if no scene files are involved
            if (!ContainsSceneAssets(importedAssets) && !ContainsSceneAssets(movedAssets))
                return;

            UpdateSceneReferencesInScriptableObjects();
        }

        private static bool ContainsSceneAssets(string[] paths)
        {
            foreach (var path in paths)
            {
                if (path.EndsWith(".unity"))
                    return true;
            }
            return false;
        }

        private static void UpdateSceneReferencesInScriptableObjects()
        {
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject");

            int updatedCount = 0;

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (asset == null) continue;

                var so = new SerializedObject(asset);
                bool changed = false;

                var iterator = so.GetIterator();

                while (iterator.NextVisible(true))
                {
                    if (iterator.propertyType == SerializedPropertyType.Generic && iterator.type == nameof(FPolySceneReference))
                    {
                        var sceneAssetProp = iterator.FindPropertyRelative("sceneAsset");
                        var scenePathProp = iterator.FindPropertyRelative("scenePath");

                        if (sceneAssetProp != null && sceneAssetProp.objectReferenceValue != null)
                        {
                            string newPath = AssetDatabase.GetAssetPath(sceneAssetProp.objectReferenceValue);
                            if (scenePathProp.stringValue != newPath)
                            {
                                scenePathProp.stringValue = newPath;
                                changed = true;
                            }
                        }
                    }
                }

                if (changed)
                {
                    so.ApplyModifiedProperties();
                    EditorUtility.SetDirty(asset);
                    updatedCount++;
                }
            }

            if (updatedCount > 0)
            {
                AssetDatabase.SaveAssets();
                Debug.Log($"[PolySceneSerializer] Updated {updatedCount} ScriptableObject(s).");
            }
        }
    }
}