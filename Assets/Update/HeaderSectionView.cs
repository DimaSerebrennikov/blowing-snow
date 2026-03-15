using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
namespace BlowingSnow {
    public class HeaderSectionView {
        public VisualElement Root;
        public Label Label;
        public HeaderSectionView(VisualTreeAsset asset) {
            TemplateContainer button = asset.Instantiate();
            VisualElement background = button.Q<VisualElement>("Background");
            Label = button.Q<Label>("Label");
            background.AddToClassList("header");
            Root = background;
        }
    }
}