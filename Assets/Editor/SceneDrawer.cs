using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneAttribute))]
public class SceneDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "Use Scene with string.");
            return;
        }

        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        string[] sceneNames = new string[scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
        {
            string path = scenes[i].path;
            sceneNames[i] = System.IO.Path.GetFileNameWithoutExtension(path);
        }

        int selectedIndex = System.Array.IndexOf(sceneNames, property.stringValue);

        if (selectedIndex < 0)
            selectedIndex = 0;

        int newIndex = EditorGUI.Popup(position, label.text, selectedIndex, sceneNames);

        if (scenes.Length > 0)
            property.stringValue = sceneNames[newIndex];
    }
}