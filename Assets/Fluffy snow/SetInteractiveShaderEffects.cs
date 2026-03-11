using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class SetInteractiveShaderEffects : MonoBehaviour {
    [SerializeField] RenderTexture rt;
    [SerializeField] Transform target;
    [SerializeField] float yOffset = 20f;
    [SerializeField] Camera m_cam;
    void Awake() {
        Shader.SetGlobalTexture("_GlobalEffectRT", rt);
        Shader.SetGlobalFloat("_OrthographicCamSize", m_cam.orthographicSize);
    }
    void Update() {
        float pixelSize = 2.0f * m_cam.orthographicSize / rt.height;
        Vector3 targetPosition = new(Mathf.Round(target.position.x / pixelSize) * pixelSize, Mathf.Round((target.position.y + yOffset) / pixelSize) * pixelSize, Mathf.Round(target.position.z / pixelSize) * pixelSize);
        transform.position = targetPosition;
        Shader.SetGlobalVector("_Position", transform.position);
    }
}
