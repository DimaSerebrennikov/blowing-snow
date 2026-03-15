using System;
using System.Collections.Generic;
using System.IO;
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
            background.AddToClassList("button");
            Root = background;
        }
    }
}
