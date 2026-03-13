using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
namespace BlowingSnow {
    [CustomEditor(typeof(SnowSystem))]
    public class SnowSystemEditor : UnityEditor.Editor {
        public override VisualElement CreateInspectorGUI() {
            VisualElement root = new();
            ObjectField sourceField = new("Source MeshFilter") {
                objectType = typeof(MeshFilter),
                allowSceneObjects = true
            };
            Button createButton = new(() => {
                SnowSystem snowSystem = (SnowSystem)target;
                MeshFilter sourceMeshFilter = sourceField.value as MeshFilter;
                if (snowSystem == null || sourceMeshFilter == null) {
                    return;
                }
                CreateRendererObject(snowSystem, sourceMeshFilter);
                sourceField.value = null;
            }) {
                text = "Create Snow Mesh Object"
            };
            root.Add(sourceField);
            root.Add(createButton);
            return root;
        }
        static void CreateRendererObject(SnowSystem snowSystem, MeshFilter sourceMeshFilter) {
            if (sourceMeshFilter.sharedMesh == null) {
                EditorUtility.DisplayDialog("MeshFilter has no mesh", "The selected MeshFilter does not contain a shared mesh.", "OK");
                return;
            }
            GameObject go = new(sourceMeshFilter.name + "_SnowMesh");
            Undo.RegisterCreatedObjectUndo(go, "Create Snow Mesh Object");
            go.transform.SetParent(snowSystem.transform, false);
            go.transform.position = sourceMeshFilter.transform.position;
            go.transform.rotation = sourceMeshFilter.transform.rotation;
            go.transform.localScale = sourceMeshFilter.transform.lossyScale;
            MeshFilter newMeshFilter = Undo.AddComponent<MeshFilter>(go);
            MeshRenderer newMeshRenderer = Undo.AddComponent<MeshRenderer>(go);
            newMeshFilter.sharedMesh = sourceMeshFilter.sharedMesh;

            // Material can be assigned later.
            newMeshRenderer.sharedMaterial = null;
            EditorUtility.SetDirty(go);
            EditorUtility.SetDirty(snowSystem);
        }
    }
}
