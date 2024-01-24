using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(SFX_List))]
public class SFX_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space(40);

        SFX_List sFX_List = (SFX_List)target;


        if (GUILayout.Button("Save", GUILayout.Height(40)))
        {
            sFX_List.ConvertToString();
        }
        base.OnInspectorGUI();
    }
}
#endif