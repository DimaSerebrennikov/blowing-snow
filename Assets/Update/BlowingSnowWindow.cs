using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace BlowingSnow {
    public class BlowingSnowWindow : EditorWindow {
        [SerializeField] VisualTreeAsset _view;
        [SerializeField] VisualTreeAsset _stateView;
        [SerializeField] GameObject _mesh;
        [SerializeField] GameObject _tracer;
        [SerializeField] GameObject _cam;
        [SerializeField] GameObject _processor;
        BlowingSnowState _state;
        [MenuItem("Tools/Blowing snow")]
        static void Open() {
            GetWindow<BlowingSnowWindow>("Blowing snow");
        }
        void CreateGUI() {
            _view.CloneTree(rootVisualElement);
            BlowingSnowService service = new(_mesh, _tracer, _cam, _processor);
            _state = new BlowingSnowState();
            BlowingSnowExperience exp = new(rootVisualElement, service, _state, _stateView);
            exp.Start();
        }
        void Update() {
            _state.Update();
        }
    }
}
