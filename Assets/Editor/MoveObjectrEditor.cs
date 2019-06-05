using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MoveObject))]
public sealed class MoveObjectEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
