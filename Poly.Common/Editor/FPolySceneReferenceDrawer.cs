using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Poly.Common.Editor
{
    [CustomPropertyDrawer(typeof(FPolySceneReference))]
    public class FPolySceneReferenceDrawer : PropertyDrawer
    {
        private const float ButtonWidth = 22f;
        private const float Padding = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty sceneAssetProp = property.FindPropertyRelative("sceneAsset");
            bool showWarning = ShouldShowWarning(sceneAssetProp);

            float baseHeight = EditorGUIUtility.singleLineHeight;
            return baseHeight + (showWarning ? Padding : 0f);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty sceneAssetProp = property.FindPropertyRelative("sceneAsset");
            SerializedProperty pathProp = property.FindPropertyRelative("scenePath");

            EditorGUI.BeginProperty(position, label, property);

            bool showWarning = ShouldShowWarning(sceneAssetProp);
            
            Rect fieldRect = new Rect(
                position.x,
                position.y + (showWarning ? Padding : 0),
                position.width - ButtonWidth - Padding,
                EditorGUIUtility.singleLineHeight
            );
            Rect buttonRect = new Rect(
                fieldRect.xMax + Padding,
                fieldRect.y,
                ButtonWidth,
                EditorGUIUtility.singleLineHeight
            );

            Color oldColor = GUI.color;
            if (showWarning)
            {
                GUI.color = new Color(1f, 1f, 0.5f); // light yellow
            }

            // Draw field and detect changes
            EditorGUI.BeginChangeCheck();
            SceneAsset newSceneAsset = EditorGUI.ObjectField(
                fieldRect,
                label,
                sceneAssetProp.objectReferenceValue,
                typeof(SceneAsset),
                false
            ) as SceneAsset;
            if (EditorGUI.EndChangeCheck())
            {
                sceneAssetProp.objectReferenceValue = newSceneAsset;

                // Auto-update path when scene asset is assigned or changed
                string newPath = newSceneAsset != null ? AssetDatabase.GetAssetPath(newSceneAsset) : string.Empty;
                pathProp.stringValue = newPath;
            }
            GUI.color = oldColor;

            // Add to build settings button
            if (showWarning && sceneAssetProp.objectReferenceValue != null)
            {
                if (GUI.Button(buttonRect, "+", EditorStyles.miniButton))
                {
                    string path = AssetDatabase.GetAssetPath(sceneAssetProp.objectReferenceValue);
                    AddSceneToBuildSettings(path);
                }
            }

            EditorGUI.EndProperty();
        }

        private bool ShouldShowWarning(SerializedProperty sceneAssetProp)
        {
            if (sceneAssetProp == null || sceneAssetProp.objectReferenceValue == null)
                return false;

            string scenePath = AssetDatabase.GetAssetPath(sceneAssetProp.objectReferenceValue);
            return EditorBuildSettings.scenes.All(s => s.path != scenePath);
        }

        private void AddSceneToBuildSettings(string scenePath)
        {
            if (string.IsNullOrEmpty(scenePath))
                return;

            var scenes = EditorBuildSettings.scenes.ToList();

            if (scenes.Any(s => s.path == scenePath))
                return;

            scenes.Add(new EditorBuildSettingsScene(scenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();

            Debug.Log($"[SceneReference] Scene added to Build Settings: {scenePath}");
        }
    }
}
