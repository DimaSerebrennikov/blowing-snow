#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(RenderTextureSimpleDrawer))]
public class RenderTextureSimpleDrawerEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        RenderTextureSimpleDrawer drawer = (RenderTextureSimpleDrawer)target;
        if (GUILayout.Button("Draw")) {
            drawer.Draw();
        }
    }
}
#endif
