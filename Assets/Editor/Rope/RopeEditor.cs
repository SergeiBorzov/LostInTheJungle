using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(Rope))]
public class RopeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Rope rope = (Rope)target;

        if (GUILayout.Button("Create rope"))
        {
            rope.CreateRope();
        }

        if (GUILayout.Button("Destroy Rope"))
        {
            rope.DestroyRope();
        }
    }
}
