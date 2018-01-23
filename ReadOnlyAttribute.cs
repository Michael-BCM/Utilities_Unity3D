using UnityEngine;
using UnityEditor;

//Thanks to this, adding '[ReadOnly]' to a variable declaration will ensure that it will be greyed out in the Inspector, 
//meaning that it cannot be modified outside of the script. 
public class ReadOnlyAttribute : PropertyAttribute
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}
