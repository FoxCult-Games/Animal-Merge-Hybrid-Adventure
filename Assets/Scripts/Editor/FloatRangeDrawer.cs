namespace FoxCultGames.Editor
{
    using UnityEditor;
    using UnityEngine;
    using Utilities;

    [CustomPropertyDrawer(typeof(FloatRange))]
    public class FloatRangeDrawer : PropertyDrawer
    {
        private const float FieldWidthMultiplier = 0.2f;
        private const float SliderSpacing = 10f;
        private const float MinMaxValue = 200f;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var minProperty = property.FindPropertyRelative("min");
            var maxProperty = property.FindPropertyRelative("max");

            EditorGUI.BeginProperty(position, label, property);
        
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Set the layout for the two float fields (min and max) side by side
            var fieldWidth = position.width * FieldWidthMultiplier;
            var sliderWidth = position.width - 2 * fieldWidth;

            var minRect = new Rect(position.x, position.y, fieldWidth, position.height);
            var sliderRect = new Rect(position.x + fieldWidth + SliderSpacing, position.y, sliderWidth, position.height);
            var maxRect = new Rect(position.x + fieldWidth + sliderWidth + SliderSpacing, position.y, fieldWidth, position.height);

            var minValue = minProperty.floatValue;
            var maxValue = maxProperty.floatValue;
            
            EditorGUI.BeginChangeCheck();
            minValue = EditorGUI.FloatField(minRect, minValue);
            maxValue = EditorGUI.FloatField(maxRect, maxValue);
            
            if (EditorGUI.EndChangeCheck())
            {
                // Ensure the values are correctly clamped
                minValue = Mathf.Clamp(minValue, -MinMaxValue, maxValue);
                maxValue = Mathf.Clamp(maxValue, minValue, MinMaxValue);
            }
            
            EditorGUI.BeginChangeCheck();
            EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, -MinMaxValue, MinMaxValue);
            if (EditorGUI.EndChangeCheck())
            {
                // Update the properties based on slider values
                minProperty.floatValue = minValue;
                maxProperty.floatValue = maxValue;
            }

            minProperty.floatValue = minValue;
            maxProperty.floatValue = maxValue;

            EditorGUI.EndProperty();
        }
    }
}