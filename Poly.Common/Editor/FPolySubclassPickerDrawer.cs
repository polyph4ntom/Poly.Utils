using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Poly.Common.Editor
{
    [CustomPropertyDrawer(typeof(FPolySubclassPickerAttribute))]
    public class FPolySubclassPickerDrawer : PropertyDrawer
    {
        private readonly Dictionary<Type, List<Type>> typeCache = new();
        private readonly Dictionary<Type, string[]> typeNamesCache = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Must be TypeReference
            if (fieldInfo == null || fieldInfo.FieldType != typeof(FPolyTypeReference))
            {
                EditorGUI.LabelField(position, label.text, "Use with FPolyTypeReference only");
                return;
            }

            SerializedProperty typeNameProp = property.FindPropertyRelative("typeName");

            if (typeNameProp == null || typeNameProp.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, label.text, "Invalid TypeReference format.");
                return;
            }

            string currentTypeName = typeNameProp.stringValue;
            Type currentType = !string.IsNullOrEmpty(currentTypeName) ? Type.GetType(currentTypeName) : null;

            var picker = (FPolySubclassPickerAttribute)attribute;
            var baseType = picker.BaseType;

            // Cache available types
            if (!typeCache.TryGetValue(baseType, out var availableTypes))
            {
                availableTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a =>
                    {
                        try { return a.GetTypes(); }
                        catch { return Array.Empty<Type>(); }
                    })
                    .Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
                    .OrderBy(t => t.Name)
                    .ToList();

                typeCache[baseType] = availableTypes;
                typeNamesCache[baseType] = availableTypes.Select(t => t.Name).ToArray();
            }

            var names = typeNamesCache[baseType];

            // If currentType is null → index = -1 → Unity will show nothing selected
            int currentIndex = availableTypes.FindIndex(t => t == currentType);

            // Draw dropdown
            int newIndex = EditorGUI.Popup(position, label.text, currentIndex, names);

            // Assign selected type
            if (newIndex >= 0 && newIndex < availableTypes.Count)
            {
                typeNameProp.stringValue = availableTypes[newIndex].AssemblyQualifiedName;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            EditorGUIUtility.singleLineHeight;
    }
}
