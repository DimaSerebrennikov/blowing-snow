using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Animations;
namespace BlowingSnow {
    public class BlowingSnowService {
        GameObject _mesh;
        GameObject _tracer;
        GameObject _cam;
        GameObject _processor;
        public BlowingSnowService(GameObject mesh, GameObject tracer, GameObject cam, GameObject processor) {
            _mesh = mesh;
            _tracer = tracer;
            _cam = cam;
            _processor = processor;
        }
        public void CreateCam() {
            GameObject newTracer = UnityEngine.Object.Instantiate(_cam);
            newTracer.name = "Snow cam";
        }
        public void CreateProcessor() {
            GameObject newTracer = UnityEngine.Object.Instantiate(_processor);
            newTracer.name = "Snow post processor";
        }
        public void SetupFollowingTarget(Transform target) {
            SnowCameraRendering cameraRendering = UnityEngine.Object.FindFirstObjectByType<SnowCameraRendering>();
            if (cameraRendering == null) return;
            SnowCameraPositioning following = cameraRendering.GetComponent<SnowCameraPositioning>() ?? cameraRendering.gameObject.AddComponent<SnowCameraPositioning>();
            following.Setup(cameraRendering.RT, target, cameraRendering.MCam);
        }
        public void CreateTracer(Transform target) {
            GameObject newTracer = UnityEngine.Object.Instantiate(_tracer);
            newTracer.name = $"Snow tracer for {target.name}";
            newTracer.transform.position = target.position;
            PositionConstraint constraint = newTracer.AddComponent<PositionConstraint>();
            ConstraintSource source = new() {
                sourceTransform = target,
                weight = 1f
            };
            constraint.AddSource(source);
            constraint.translationAxis = Axis.X | Axis.Z;
            constraint.constraintActive = true;
            constraint.locked = true;
        }
        public void CreateMeshObjectFromSource(GameObject source, Mesh mesh) {
            GameObject n = UnityEngine.Object.Instantiate(_mesh, source.transform.position, source.transform.rotation);
            n.name = $"Snow area with the form of {source.name}";
            MeshFilter filter = n.GetComponent<MeshFilter>();
            if (filter != null) {
                filter.sharedMesh = mesh;
            }
        }
    }
}
