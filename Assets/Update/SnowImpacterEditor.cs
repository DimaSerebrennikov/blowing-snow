using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
namespace BlowingSnow.Editor {
    [CustomEditor(typeof(SnowImpacter))]
    public class SnowImpacterEditor : UnityEditor.Editor {
        const float DiscRadiusFactor = 0.35355339059f;
        const float DefaultSignificantSizeRatio = 1f;
        public override VisualElement CreateInspectorGUI() {
            SnowImpacter snowImpacter = (SnowImpacter)target;
            VisualElement root = new();
            serializedObject.Update();
            PropertyField particleSystemField = new(serializedObject.FindProperty(nameof(SnowImpacter.ParticleSystem)), "Particle System");
            root.Add(particleSystemField);
            FloatField particleSizeField = new("Particle Size");
            particleSizeField.isDelayed = true;
            UpdateParticleSizeFieldValue(snowImpacter, particleSizeField);
            particleSizeField.RegisterValueChangedCallback(evt => {
                if (snowImpacter.ParticleSystem == null) {
                    return;
                }
                Undo.RecordObject(snowImpacter.ParticleSystem, "Change Particle Size");
                ParticleSystem.MainModule main = snowImpacter.ParticleSystem.main;
                main.startSize = evt.newValue;
                EditorUtility.SetDirty(snowImpacter.ParticleSystem);
                SceneView.RepaintAll();
            });
            root.Add(particleSizeField);
            FloatField significantSizeRatioField = new("Significant Size Ratio (%)");
            significantSizeRatioField.isDelayed = true;
            significantSizeRatioField.tooltip = "100 = actual particle size, 150 = 1.5x, 50 = 0.5x";
            significantSizeRatioField.SetValueWithoutNotify(GetSignificantSizeRatioPercent(snowImpacter));
            significantSizeRatioField.RegisterValueChangedCallback(evt => {
                float clampedValue = Mathf.Max(0f, evt.newValue);
                significantSizeRatioField.SetValueWithoutNotify(clampedValue);
                SetSignificantSizeRatioPercent(snowImpacter, clampedValue);
                SceneView.RepaintAll();
            });
            root.Add(significantSizeRatioField);
            particleSystemField.RegisterValueChangeCallback(_ => {
                serializedObject.ApplyModifiedProperties();
                UpdateParticleSizeFieldValue(snowImpacter, particleSizeField);
                SceneView.RepaintAll();
            });
            root.Bind(serializedObject);
            return root;
        }
        static void UpdateParticleSizeFieldValue(SnowImpacter snowImpacter, FloatField particleSizeField) {
            if (snowImpacter.ParticleSystem == null) {
                particleSizeField.SetValueWithoutNotify(0f);
                particleSizeField.SetEnabled(false);
                return;
            }
            ParticleSystem.MainModule main = snowImpacter.ParticleSystem.main;
            particleSizeField.SetValueWithoutNotify(main.startSize.constant);
            particleSizeField.SetEnabled(true);
        }
        void OnSceneGUI() {
            SnowImpacter snowImpacter = (SnowImpacter)target;
            if (snowImpacter == null || snowImpacter.ParticleSystem == null) {
                return;
            }
            Transform tr = snowImpacter.transform;
            ParticleSystem.MainModule main = snowImpacter.ParticleSystem.main;
            float actualSize = main.startSize.constant;
            float actualRadius = actualSize * DiscRadiusFactor;
            float significantRatio = GetSignificantSizeRatioPercent(snowImpacter) / 100f;
            float significantSize = actualSize * significantRatio;
            float significantRadius = significantSize * DiscRadiusFactor;
            Vector3 position = tr.position;
            Vector3 labelPosition = position + Vector3.up * 1.0f;
            Handles.color = new Color(1f, 0.68f, 0f);
            Handles.DrawWireDisc(position, Vector3.up, actualRadius);
            Handles.color = new Color(0.83f, 1f, 0f);
            Handles.DrawWireDisc(position, Vector3.up, significantRadius);
            GUIStyle labelStyle = LabelStyle();
            Handles.Label(labelPosition, $"Particle Size: {actualSize:0.###}\n" + $"Significant Size: {significantSize:0.###} ({significantRatio * 100f:0.#}%)", labelStyle);
        }
        static GUIStyle LabelStyle() {
            Texture2D labelBackground = new(1, 1);
            labelBackground.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.6f));
            labelBackground.Apply();
            GUIStyle labelStyle = new(EditorStyles.boldLabel);
            labelStyle.fontSize = 16;
            labelStyle.normal.textColor = Color.white;
            labelStyle.normal.background = labelBackground;
            labelStyle.padding = new RectOffset(6, 6, 3, 3);
            return labelStyle;
        }
        static float GetSignificantSizeRatioPercent(SnowImpacter snowImpacter) {
            string key = GetSignificantSizeRatioKey(snowImpacter);
            return EditorPrefs.GetFloat(key, DefaultSignificantSizeRatio * 100f);
        }
        static void SetSignificantSizeRatioPercent(SnowImpacter snowImpacter, float value) {
            string key = GetSignificantSizeRatioKey(snowImpacter);
            EditorPrefs.SetFloat(key, value);
        }
        static string GetSignificantSizeRatioKey(SnowImpacter snowImpacter) {
            GlobalObjectId globalObjectId = GlobalObjectId.GetGlobalObjectIdSlow(snowImpacter);
            return $"BlowingSnow.SnowImpacter.SignificantSizeRatio.{globalObjectId}";
        }
    }
}
