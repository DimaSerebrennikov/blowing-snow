using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace BlowingSnow {
    public class BlowingSnowState {
        ReactiveList<GameObject> _componentOnScene = new();
        public ReactiveList<GameObject> ComponentOnScene {
            get => _componentOnScene;
            set => _componentOnScene = value;
        }
        public void Update() {
            GameObject[] current = FindAllComponents();
            for (int i = 0; i < current.Length; i++) {
                GameObject go = current[i];
                if (!ComponentOnScene.Contains(go)) {
                    ComponentOnScene.Add(go);
                }
            }
            for (int i = ComponentOnScene.Count - 1; i >= 0; i--) {
                GameObject go = ComponentOnScene[i];
                bool exists = false;
                for (int j = 0; j < current.Length; j++) {
                    if (current[j] == go) {
                        exists = true;
                        break;
                    }
                }
                if (!exists) {
                    ComponentOnScene.Remove(go);
                }
            }
        }
        public GameObject[] FindAllComponents() {
            EditorMark[] marks = UnityEngine.Object.FindObjectsByType<EditorMark>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            HashSet<GameObject> result = new();
            for (int i = 0; i < marks.Length; i++) {
                Transform parent = marks[i].transform.parent;
                if (parent != null) {
                    result.Add(parent.gameObject);
                }
            }
            GameObject[] array = new GameObject[result.Count];
            result.CopyTo(array);
            return array;
        }
    }
}