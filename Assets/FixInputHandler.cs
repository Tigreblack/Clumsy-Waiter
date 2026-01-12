using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class FixInputHandler
{
    [MenuItem("Tools/Fix Input Handler to Both")]
    public static void SetInputHandlerToBoth()
    {
        var playerSettings = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0];
        SerializedObject serializedObject = new SerializedObject(playerSettings);
        SerializedProperty activeInputHandler = serializedObject.FindProperty("activeInputHandler");
        
        if (activeInputHandler != null)
        {
            activeInputHandler.intValue = 1;
            serializedObject.ApplyModifiedProperties();
            Debug.Log("Input Handler set to Both (1). Please restart Unity for changes to take effect.");
        }
        else
        {
            Debug.LogError("Could not find activeInputHandler property");
        }
    }
}
#endif
