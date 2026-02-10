using UnityEditor;
using UnityEngine;

namespace Poly.Name.Editor
{
    [CustomPropertyDrawer(typeof(FPolyName))]
    public class FPolyNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            var idProp = property.FindPropertyRelative("id");
            var id = idProp.hash128Value;

            string current = FPolyNameRegistryEditor.TryGetText(id, out var s) ? s : string.Empty;
            
            var rect = EditorGUI.PrefixLabel(position, label);
            string newText = EditorGUI.DelayedTextField(rect, current);

            if (newText != current)
            {
                newText = (newText ?? string.Empty).Trim();

                var newId = string.IsNullOrEmpty(newText)
                    ? default
                    : FPolyDeterministicNameId.FromString(newText);
                
                idProp.hash128Value = newId;
                
                if (newId.isValid)
                    FPolyNameRegistryEditor.EnsureEntry(newId, newText);

                property.serializedObject.ApplyModifiedProperties();
            }
            
            EditorGUI.EndProperty();
        }
    }
}
