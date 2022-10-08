using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameEditor : EditorWindow
{
    [MenuItem("Roller Ball/Data")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GameEditor));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Delete all saved data"))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}

