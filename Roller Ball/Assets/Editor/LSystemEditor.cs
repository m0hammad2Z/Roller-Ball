using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LSystem))]
public class LSystemEditor : Editor
{
    int numberOfLevels;
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();
        
        numberOfLevels = EditorGUILayout.IntSlider(numberOfLevels, 1, 100);
        LSystem myScript = (LSystem)target;
        if (GUILayout.Button("Asign the Values To The Array"))
        {
            myScript.ArrayDecleare();
            Debug.Log("Array has been decleared!");
        }

        if (GUILayout.Button("Add new " + numberOfLevels + " Levels"))
        {
            myScript.AddLevels(numberOfLevels);
        }

        if (GUILayout.Button("Clear Levels List"))
        {
            if (EditorUtility.DisplayDialog("Clear the list?", "Are you sure you want to clear all 'Levels' list data", "Yes, delete all data!", "No"))
            {
                myScript.levels.Clear();
                EditorUtility.DisplayDialog("Deleted completed", "'Levels' list has been cleard", "Ok");
            }
        }
    }
}
