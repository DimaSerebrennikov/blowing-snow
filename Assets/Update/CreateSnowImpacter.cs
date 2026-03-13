#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace BlowingSnow {
    [ExecuteAlways]
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(SnowImpacter))]
    [AddComponentMenu("Snow impacter")]
    public class CreateSnowImpacter : MonoBehaviour {
        void OnEnable() {
            gameObject.layer = LayerMask.NameToLayer("Snow");
            SnowImpacter snowImpacter = GetComponent<SnowImpacter>();
            ParticleSystem trying = GetComponent<ParticleSystem>();
            snowImpacter.ParticleSystem = trying;
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Update/Predefiened particle system for impacter.prefab");
            ParticleSystem prefabPS = prefab.GetComponent<ParticleSystem>();
            if (prefabPS != null && trying != null) {
                EditorUtility.CopySerialized(prefabPS, trying);
                ParticleSystemRenderer src = prefabPS.GetComponent<ParticleSystemRenderer>();
                ParticleSystemRenderer dst = trying.GetComponent<ParticleSystemRenderer>();
                if (src && dst) {
                    EditorUtility.CopySerialized(src, dst);
                }
            }
        }
        void Update() {
            DestroyImmediate(this);
        }
    }
}
#endif
