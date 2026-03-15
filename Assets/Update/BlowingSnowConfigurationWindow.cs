// BlowingSnowConfigurationWindow.csC:\Users\DimaS\Blowing snow\Assets\Update\BlowingSnowConfigurationWindow.csBlowingSnowConfigurationWindow.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace BlowingSnow {
    public class BlowingSnowConfigurationWindow : EditorWindow {
        [SerializeField] VisualTreeAsset _view;
        UnityEngine.Object _sceneObject;
        public UnityEngine.Object SceneObject { get => _sceneObject; set => _sceneObject = value; }
        void CreateGUI() {
            _view.CloneTree(rootVisualElement);
            VisualElement open = rootVisualElement.Q<VisualElement>("Open");
            VisualElement remove = rootVisualElement.Q<VisualElement>("Remove");
            VisualElement ping = rootVisualElement.Q<VisualElement>("Ping");
            open.RegisterCallback<PointerDownEvent>(evt => {
                evt.StopPropagation();
                Selection.activeObject = _sceneObject;
                EditorUtility.OpenPropertyEditor(_sceneObject);
            });
            remove.RegisterCallback<PointerDownEvent>(evt => {
                DestroyImmediate(_sceneObject);
            });
            ping.RegisterCallback<PointerDownEvent>(evt => {
                EditorGUIUtility.PingObject(_sceneObject);
            });
        }
    }
}
