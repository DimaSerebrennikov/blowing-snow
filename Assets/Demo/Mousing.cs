// Mousing.csC:\GameDev\Halette\Assets\SereDim\Script\Game\Cmd\Movement\Mousing.csMousing.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Serebrennikov {
    public abstract class Mousing {
        Transform marker;
        Camera camera;
        protected Mousing(Transform marker, Camera camera) {
            this.marker = marker;
            this.camera = camera;
        }
        protected abstract void LookAt(Vector3 targetVector);
        public IDisposable Start() {
            float targetY = marker.position.y;
            return Loop.Tick(_TickDirectToMouse);
            void _TickDirectToMouse() {
                Tick_DirectToMouse(targetY, out Vector3 targetVector);
                LookAt(targetVector);
            }
        }
        void Tick_DirectToMouse(float targetY, out Vector3 targetVector) {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            float x = ray.origin.x + (targetY - ray.origin.y) / ray.direction.y * ray.direction.x;
            float z = ray.origin.z + (targetY - ray.origin.y) / ray.direction.y * ray.direction.z;
            targetVector = new Vector3(x, targetY, z);
        }
    }
}
