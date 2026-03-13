using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace BlowingSnow.Editor {
//    public static class SnowImpacterGizmos {
//        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
//        static void DrawSnowImpacterGizmo(SnowImpacter snowImpacter, GizmoType gizmoType) {
//            if (snowImpacter == null || snowImpacter.ParticleSystem == null) {
//                return;
//            }
//            Transform tr = snowImpacter.transform;
//            ParticleSystem.MainModule main = snowImpacter.ParticleSystem.main;
//            ParticleSystem.MinMaxCurve startSize = main.startSize;
//            Vector3 position = tr.position;
//            Vector3 labelPosition = position + tr.up * 1.0f;
//            Handles.color = Color.cyan;
//            switch (startSize.mode) {
//                case ParticleSystemCurveMode.Constant:
//                    {
//                        float size = startSize.constant;
//                        Handles.DrawWireDisc(position, tr.up, size * 0.5f);
//                        Handles.Label(labelPosition, $"Particle Size: {size:0.###}");
//                        break;
//                    }
//                case ParticleSystemCurveMode.TwoConstants:
//                    {
//                        float min = startSize.constantMin;
//                        float max = startSize.constantMax;
//                        Handles.DrawWireDisc(position, tr.up, max * 0.5f);
//                        Handles.Label(labelPosition, $"Particle Size: {min:0.###} .. {max:0.###}");
//                        break;
//                    }
//                case ParticleSystemCurveMode.Curve:
//                    Handles.Label(labelPosition, "Particle Size: Curve");
//                    break;
//                case ParticleSystemCurveMode.TwoCurves:
//                    Handles.Label(labelPosition, "Particle Size: Two Curves");
//                    break;
//                default:
//                    Handles.Label(labelPosition, "Particle Size: Unknown");
//                    break;
//            }
//        }
//    }
}
