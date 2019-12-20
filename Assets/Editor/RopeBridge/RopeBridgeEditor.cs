using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(RopeBridge))]
public class RopeBridgeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RopeBridge ropeBridge = (RopeBridge)target;

        if (GUILayout.Button("Build Bridge"))
        {
            ropeBridge.BuildBridge();
        }

        if (GUILayout.Button("Destroy Bridge"))
        {
            ropeBridge.DestroyBridge();
        }
    }
}
