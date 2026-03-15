// SnowCameraPositioning.csC:\Users\DimaS\Blowing snow\Assets\Fluffy snow\SnowCameraPositioning.csSnowCameraPositioning.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace BlowingSnow {
    public class SnowCameraPositioning : MonoBehaviour {
        [SerializeField] RenderTexture _rt;
        [SerializeField] Transform _target;
        [SerializeField] float _yOffset = 20f;
        [SerializeField] Camera _cam;
        public void Setup(RenderTexture rt, Transform target, Camera cam) {
            _rt = rt;
            _target = target;
            _cam = cam;
        }
        void Update() {
            float pixelSize = 2.0f * _cam.orthographicSize / _rt.height;
            Vector3 targetPosition = new(Mathf.Round(_target.position.x / pixelSize) * pixelSize, Mathf.Round((_target.position.y + _yOffset) / pixelSize) * pixelSize, Mathf.Round(_target.position.z / pixelSize) * pixelSize);
            transform.position = targetPosition;
        }
    }
}
