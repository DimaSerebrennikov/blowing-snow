// SnowCameraRendering.csC:\Users\DimaS\Blowing snow\Assets\Fluffy snow\SnowCameraRendering.csSnowCameraRendering.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace BlowingSnow {
    public class SnowCameraRendering : MonoBehaviour {
        [SerializeField] RenderTexture rt;
        [SerializeField] Camera m_cam;
        void Awake() {
            Shader.SetGlobalTexture("_GlobalEffectRT", rt);
            Shader.SetGlobalFloat("_OrthographicCamSize", m_cam.orthographicSize);
        }
        void LateUpdate() {
            Shader.SetGlobalVector("_Position", transform.position);
        }
    }
}
