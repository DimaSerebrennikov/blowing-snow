#if UNITY_EDITOR
// FluffySnowContextInspector.csC:\Users\DimaS\Desktop\FeebleSnowOriginal-master\Assets\Dima Serebrennikov\Fluffy snow\FluffySnowContextInspector.csFluffySnowContextInspector.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
namespace Serebrennikov {
    [CustomEditor(typeof(FluffySnowContext))]
    public sealed class FluffySnowContextInspector : Editor {
        TextField nameField;
        public override VisualElement CreateInspectorGUI() {
            VisualElement root = new();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            Button button = new(AddCurrentlySettedTextures);
            button.text = "Add currently setted textures";
            root.Add(button);
            Button buttonZero = new(SetOnZero);
            buttonZero.text = "Set current material with zero element's data";
            root.Add(buttonZero);
            nameField = new TextField("Texture group name");
            nameField.value = "Default";
            root.Add(nameField);
            return root;
        }
        void AddCurrentlySettedTextures() {
            FluffySnowContext context = (FluffySnowContext)target;
            Undo.RecordObject(context, "Add currently setted textures");
            context.AddCurrentlySettedTextures(nameField.value);
            EditorUtility.SetDirty(context);
        }
        void SetOnZero() {
            FluffySnowContext context = (FluffySnowContext)target;
            Undo.RecordObject(context, "Set current material with zero element's data");
            context.SetOnZero();
            EditorUtility.SetDirty(context);
        }
    }
}
    #endif
