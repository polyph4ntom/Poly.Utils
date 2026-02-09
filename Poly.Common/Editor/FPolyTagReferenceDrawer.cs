 using UnityEditor;
using UnityEngine;

namespace Poly.Common.Editor
{
    [CustomPropertyDrawer(typeof(FPolyTagReference))]
    public class FPolyTagReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty tagProp = property.FindPropertyRelative("tag");

            // Draw foldout label properly
            EditorGUI.BeginProperty(position, label, property);

            // Optional: draw label manually if needed
            position = EditorGUI.PrefixLabel(position, label);

            // Avoid indent on children
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if (tagProp != null)
            {
                tagProp.stringValue = EditorGUI.TagField(position, GUIContent.none, tagProp.stringValue);
            }
            else
            {
                EditorGUI.LabelField(position, "Tag property missing");
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
