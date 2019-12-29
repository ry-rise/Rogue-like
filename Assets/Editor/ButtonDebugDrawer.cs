using System.Reflection;
using UnityEngine;
using UnityEditor;

/// 上記ボタンアトリビュートをボタン化／および実行するための関数
[CustomPropertyDrawer(typeof(ButtonDebugAttribute))]
public class ButtonDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var buttonAttribute = attribute as ButtonDebugAttribute;

        if (GUI.Button(position, buttonAttribute.buttonName))
        {
            var objectReferenceValue = property.serializedObject.targetObject;
            var type = objectReferenceValue.GetType();
            var bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var method = type.GetMethod(buttonAttribute.buttonFunction, bindingAttr);

            method.Invoke(objectReferenceValue, null);
        }
    }
}
