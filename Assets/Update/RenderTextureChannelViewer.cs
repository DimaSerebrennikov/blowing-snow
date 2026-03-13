// RenderTextureChannelViewer.csC:\Users\DimaS\Blowing snow\Assets\Update\RenderTextureChannelViewer.csRenderTextureChannelViewer.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
namespace BlowingSnow {
    [RequireComponent(typeof(RawImage))]
    public class RenderTextureChannelViewer : MonoBehaviour {
        [SerializeField] RenderTexture source;
        [SerializeField] Material debugMaterial;
        [SerializeField] int channel = 1; // 0=R, 1=G, 2=B, 3=A
        [SerializeField] float intensity = 1f;
        [SerializeField] bool invert;
        RawImage rawImage;
        void Awake() {
            rawImage = GetComponent<RawImage>();
            Apply();
        }
        void OnEnable() {
            Apply();
        }
        [ContextMenu("Apply")]
        public void Apply() {
            if (rawImage == null) {
                rawImage = GetComponent<RawImage>();
            }
            rawImage.texture = source;
            rawImage.color = Color.white;
            if (debugMaterial != null) {
                Material runtimeMat = new(debugMaterial);
                runtimeMat.SetFloat("_Channel", channel);
                runtimeMat.SetFloat("_Intensity", intensity);
                runtimeMat.SetFloat("_Invert", invert ? 1f : 0f);
                rawImage.material = runtimeMat;
            }
        }
    }
}
