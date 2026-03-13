// SnowCameraPositioning.csC:\Users\DimaS\Blowing snow\Assets\Fluffy snow\SnowCameraPositioning.csSnowCameraPositioning.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace BlowingSnow {
    public class SnowCameraPositioning : MonoBehaviour {
        [SerializeField] RenderTexture rt;
        [SerializeField] Transform target;
        [SerializeField] float yOffset = 20f;
        [SerializeField] Camera m_cam;
        void Update() {
            float pixelSize = 2.0f * m_cam.orthographicSize / rt.height;
            Vector3 targetPosition = new(Mathf.Round(target.position.x / pixelSize) * pixelSize, Mathf.Round((target.position.y + yOffset) / pixelSize) * pixelSize, Mathf.Round(target.position.z / pixelSize) * pixelSize);
            transform.position = targetPosition;
        }
    }
}
