// DropView.csC:\Users\DimaS\Blowing snow\Assets\Update\DropView.csDropView.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
namespace BlowingSnow {
    public class DropView {
        public VisualElement Root;
        public Label Label;
        public DropView(VisualTreeAsset asset) {
            TemplateContainer button = asset.Instantiate();
            VisualElement background = button.Q<VisualElement>("Background");
            Label = button.Q<Label>("Label");
            background.AddToClassList("drop");
            Root = background;
        }
    }
}
