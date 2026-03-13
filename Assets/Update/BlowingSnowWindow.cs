using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace BlowingSnow {
    public class ButtonView {
        public VisualElement Root;
        public Label Label;
        public ButtonView(VisualTreeAsset asset) {
            TemplateContainer button = asset.Instantiate();
            VisualElement background = button.Q<VisualElement>("Background");
            Label = button.Q<Label>("Label");
            background.AddToClassList("function");
            background.AddToClassList("white");
            Label.AddToClassList("label");
            Root = background;
        }
    }
    public class BlowingSnowWindow : EditorWindow {
        [SerializeField] VisualTreeAsset _button;
        [SerializeField] StyleSheet _styleSheet;
        [MenuItem("Tools/Blowing snow")]
        static void Open() {
            GetWindow<BlowingSnowWindow>();
        }
        void CreateGUI() {
            rootVisualElement.styleSheets.Add(_styleSheet);
            Add_Button("Execute the provided configuration for the current scene");
            Add_Button("[ Target which is followed by rendering ]");
            Add_Button("[ Transforms which make trace on snow ]");
            Add_Button("[ Mesh which duplicated to draw a snow into ]");
        }
        void Add_Button(string text) {
            ButtonView ExecuteTheProvidedConfigurationForTheCurrentScene = new(_button);
            ExecuteTheProvidedConfigurationForTheCurrentScene.Label.text = text;
            ExecuteTheProvidedConfigurationForTheCurrentScene.Root.RegisterCallback<PointerDownEvent>(evt => {
                evt.StopPropagation();
            });
            rootVisualElement.Add(ExecuteTheProvidedConfigurationForTheCurrentScene.Root);
        }
    }
}
