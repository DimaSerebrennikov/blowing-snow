using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
namespace BlowingSnow {
    public class BlowingSnowExperience {
        VisualElement _root;
        BlowingSnowService _service;
        BlowingSnowState _state;
        VisualTreeAsset _stateView;
        Dictionary<GameObject, VisualElement> _currentStateLabelMap = new();
        public BlowingSnowExperience(VisualElement root, BlowingSnowService service, BlowingSnowState state, VisualTreeAsset stateView) {
            _root = root;
            _service = service;
            _state = state;
            _stateView = stateView;
        }
        public void Start() {
            VisualElement meshDropper = _root.Q<VisualElement>("MeshDropper");
            DroppingHandler droppingHandler = new(meshDropper);
            droppingHandler.Dropped += OnDropMesh;
            VisualElement tracerDropper = _root.Q<VisualElement>("TracerDropper");
            DroppingHandler tracerDropperHandler = new(tracerDropper);
            tracerDropperHandler.Dropped += OnDropTracer;
            VisualElement CamButton = _root.Q<VisualElement>("CamButton");
            VisualElement ProcessorButton = _root.Q<VisualElement>("ProcessorButton");
            VisualElement FollowDropper = _root.Q<VisualElement>("FollowDropper");
            VisualElement _currentStateList = _root.Q<VisualElement>("CurrentStateList");
            DroppingHandler FollowDropperHandler = new(FollowDropper);
            FollowDropperHandler.Dropped += OnDropFollowingTarget;
            CamButton.RegisterCallback<PointerDownEvent>(evt => {
                _service.CreateCam();
                evt.StopPropagation();
            });
            ProcessorButton.RegisterCallback<PointerDownEvent>(evt => {
                _service.CreateProcessor();
                evt.StopPropagation();
            });
            _state.ComponentOnScene.Observe(add => {
                VisualElement view = _stateView.Instantiate().Q<VisualElement>("Root");
                view.RegisterCallback<PointerDownEvent>(evt => {
                    BlowingSnowConfigurationWindow window = ScriptableObject.CreateInstance<BlowingSnowConfigurationWindow>();
                    window.SceneObject = add;
                    window.titleContent = new GUIContent(add.name);
                    Vector2 mouseScreen = GUIUtility.GUIToScreenPoint(evt.originalMousePosition);
                    window.Show();
                    window.position = new Rect(mouseScreen.x, mouseScreen.y, 300, 200);
                });
                Label label = view.Q<Label>("Label");
                label.text = add.name;
                _currentStateLabelMap.Add(add, view);
                _currentStateList.Add(view);
            }, remove => {
                _currentStateLabelMap[remove].RemoveFromHierarchy();
                _currentStateLabelMap.Remove(remove);
            });
        }
        void OnDropFollowingTarget(UnityEngine.Object[] droppedObjects) {
            for (int i = 0; i < droppedObjects.Length; i++) {
                if (droppedObjects[i] is GameObject yes_N_GameObject && yes_N_GameObject.TryGetComponent<Transform>(out Transform transform) && transform) {
                    _service.SetupFollowingTarget(transform);
                } else if (droppedObjects[i] is Transform yes_N_transform) {
                    _service.SetupFollowingTarget(yes_N_transform);
                }
            }
        }
        void OnDropMesh(UnityEngine.Object[] droppedObjects) {
            for (int i = 0; i < droppedObjects.Length; i++) {
                if (droppedObjects[i] is GameObject yes_N_GameObject && yes_N_GameObject.TryGetComponent<MeshFilter>(out MeshFilter meshFilter) && meshFilter.sharedMesh != null) {
                    _service.CreateMeshObjectFromSource(yes_N_GameObject, meshFilter.sharedMesh);
                } else if (droppedObjects[i] is MeshFilter yes_N_MeshFilter && yes_N_MeshFilter.sharedMesh != null) {
                    _service.CreateMeshObjectFromSource(yes_N_MeshFilter.gameObject, yes_N_MeshFilter.sharedMesh);
                }
            }
        }
        void OnDropTracer(UnityEngine.Object[] droppedObjects) {
            for (int i = 0; i < droppedObjects.Length; i++) {
                if (droppedObjects[i] is GameObject yes_N_GameObject && yes_N_GameObject.TryGetComponent<Transform>(out Transform transform) && transform) {
                    _service.CreateTracer(transform);
                } else if (droppedObjects[i] is Transform yes_N_transform) {
                    _service.CreateTracer(yes_N_transform);
                }
            }
        }
    }
}
