using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationCreatorEditor : EditorWindow
{
    [MenuItem("Window/Animation Creator")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(AnimationCreator));
    }

    void OnGUI()
    {
        GUILayout.Label("Inventory Item Editor", EditorStyles.boldLabel);

        if (GUILayout.Button("Create Scriptable Object file"))
        {
            Debug.Log("0");
        }

        if (GUILayout.Button("Create Animation files"))
        {
            Debug.Log("1");
        }
    }
}
