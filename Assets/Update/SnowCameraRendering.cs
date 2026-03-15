// SnowCameraRendering.csC:\Users\DimaS\Blowing snow\Assets\Fluffy snow\SnowCameraRendering.csSnowCameraRendering.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace BlowingSnow {
    public class SnowCameraRendering : MonoBehaviour {
        [SerializeField] RenderTexture rt;
        [SerializeField] Camera m_cam;
        public RenderTexture RT {
            get => rt;
            set => rt = value;
        }
        public Camera MCam {
            get => m_cam;
            set => m_cam = value;
        }
        void Awake() {
            Shader.SetGlobalTexture("_GlobalEffectRT", RT);
            Shader.SetGlobalFloat("_OrthographicCamSize", MCam.orthographicSize);
        }
        void LateUpdate() {
            Shader.SetGlobalVector("_Position", transform.position);
        }
    }
}
