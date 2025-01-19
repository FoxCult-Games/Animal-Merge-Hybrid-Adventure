#if UNITY_EDITOR
namespace Witchy.Editor
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(SubclassPicker))]
    public class SubclassPickerDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var targetType = fieldInfo.FieldType;
            if (targetType.IsGenericType)
                targetType = targetType.GenericTypeArguments[0];
            
            var typeName = property.managedReferenceValue?.GetType().Name ?? "Not set";

            var dropdownRect = position;
            dropdownRect.x += EditorGUIUtility.labelWidth + 2;
            dropdownRect.width -= EditorGUIUtility.labelWidth + 2;
            dropdownRect.height = EditorGUIUtility.singleLineHeight;
            if (EditorGUI.DropdownButton(dropdownRect, new GUIContent(typeName), FocusType.Keyboard))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("None"), property.managedReferenceValue == null, () =>
                {
                    property.managedReferenceValue = null;
                    property.serializedObject.ApplyModifiedProperties();
                });

                foreach (Type type in GetClasses(targetType))
                {
                    menu.AddItem(new GUIContent(type.Name), typeName == type.Name, () =>
                    {
                        property.managedReferenceValue = type.GetConstructor(Type.EmptyTypes)?.Invoke(null);
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
                menu.ShowAsContext();
            }
            EditorGUI.PropertyField(position, property, label, true);
        }

        private static IEnumerable GetClasses(Type baseType)
        {
            return Assembly.GetAssembly(baseType).GetTypes().Where(t => (InheritsFromInterface(t) || t.IsSubclassOf(baseType)) && t.IsClass && !t.IsAbstract);

            bool InheritsFromInterface(Type type)
            {
                return baseType.IsInterface && type.GetInterfaces().Contains(baseType);
            }
        }
    }
}
#endif