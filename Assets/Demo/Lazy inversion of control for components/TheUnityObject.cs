using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Serebrennikov {
    public static class TheUnityObject {
        static Dictionary<GameObject, GameObject> _dictionarySingleton = new();
        public static T InstanceFromAsset<T>(T a) where T : Component {
            if (a.gameObject.scene.name != null) {
                return a;
            }
            if (TryGet(a, out T instance)) {
                return instance;
            }
            T newComponent = Object.Instantiate(a);
            _dictionarySingleton[a.gameObject] = newComponent.gameObject;
            return newComponent.GetComponent<T>();
        }
        public static T Instance<T>(T a) where T : Component {
            if (a.gameObject.scene.name != null) {
                return a;
            }
            T newComponent = Object.Instantiate(a);
            return newComponent;
        }
        public static void Refresh() {
            _dictionarySingleton = new Dictionary<GameObject, GameObject>();
        }
        public static bool TryGet<T>(T key, out T result) where T : Component {
            if (!_dictionarySingleton.TryGetValue(key.gameObject, out GameObject value)) {
                result = null;
                return false;
            }
            if (value.TryGetComponent<T>(out result)) {
                return true;
            }
            return false;
        }
    }
}
