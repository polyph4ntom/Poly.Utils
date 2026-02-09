using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Poly.Common.Editor
{
    [CustomPropertyDrawer(typeof(FPolySerializableDictionary<,>), true)]
    public class FPolySerializableDictionaryDrawer : PropertyDrawer
    {
        private Dictionary<string, ReorderableList> lists = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string key = property.propertyPath;

            if (!lists.TryGetValue(key, out var list))
            {
                list = CreateList(property, label);
                lists[key] = list;
            }

            SerializedProperty entriesProp = property.FindPropertyRelative("entries");

            // Draw foldout label
            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            entriesProp.isExpanded = EditorGUI.Foldout(foldoutRect, entriesProp.isExpanded, label, true);

            // If folded, skip drawing the list
            if (!entriesProp.isExpanded)
                return;

            // Draw list below label
            Rect listRect = new Rect(
                position.x,
                position.y + EditorGUIUtility.singleLineHeight + 2,
                position.width,
                list.GetHeight()
            );

            list.DoList(listRect);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            string key = property.propertyPath;

            if (!lists.TryGetValue(key, out var list))
            {
                list = CreateList(property, label);
                lists[key] = list;
            }

            SerializedProperty entriesProp = property.FindPropertyRelative("entries");

            if (!entriesProp.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            return EditorGUIUtility.singleLineHeight + 2 + list.GetHeight();
        }

        private ReorderableList CreateList(SerializedProperty property, GUIContent label)
        {
            var entriesProp = property.FindPropertyRelative("entries");

            var list = new ReorderableList(property.serializedObject, entriesProp, true, true, true, true);

            list.drawHeaderCallback = (Rect rect) =>
            {
                float keyWidth = rect.width * 0.35f;
                float valueWidth = rect.width - keyWidth;
                EditorGUI.LabelField(new Rect(rect.x, rect.y, keyWidth, EditorGUIUtility.singleLineHeight), "Key");
                EditorGUI.LabelField(new Rect(rect.x + keyWidth, rect.y, valueWidth, EditorGUIUtility.singleLineHeight), "Value");
            };

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = entriesProp.GetArrayElementAtIndex(index);
                var keyProp = element.FindPropertyRelative("Key");
                var valueProp = element.FindPropertyRelative("Value");

                bool isDuplicate = IsDuplicate(entriesProp, keyProp, index);
                float half = rect.width / 2f;

                // Optional: vertical padding
                rect.y += 2;
                
                float keyWidth = rect.width * 0.35f;
                float valueWidth = rect.width - keyWidth;

                Color oldColor = GUI.color;

                if (isDuplicate)
                    GUI.color = new Color(1f, 0.4f, 0.4f); // soft red

                
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, keyWidth - 5f, EditorGUIUtility.singleLineHeight),
                    keyProp, GUIContent.none);

                GUI.color = oldColor;
                EditorGUI.PropertyField(
                    new Rect(rect.x + keyWidth + 5f, rect.y, valueWidth - 5f, EditorGUIUtility.singleLineHeight),
                    valueProp, GUIContent.none, true);
            };

            list.elementHeightCallback = (int index) =>
            {
                var element = entriesProp.GetArrayElementAtIndex(index);
                var valueProp = element.FindPropertyRelative("Value");

                float keyHeight = EditorGUIUtility.singleLineHeight;
                float valueHeight = EditorGUI.GetPropertyHeight(valueProp, true);

                return Mathf.Max(keyHeight, valueHeight) + 6;
            };

            return list;
        }

        private bool IsDuplicate(SerializedProperty list, SerializedProperty keyProp, int currentIndex)
        {
            for (int i = 0; i < list.arraySize; i++)
            {
                if (i == currentIndex) continue;
                var otherKey = list.GetArrayElementAtIndex(i).FindPropertyRelative("Key");
                if (SerializedProperty.DataEquals(keyProp, otherKey))
                    return true;
            }
            return false;
        }
    }
}
