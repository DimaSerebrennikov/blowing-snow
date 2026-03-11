// Translator.csC:\GameDev\Halette\Assets\SereDim\Script\Game\Character\Translator.csTranslator.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Serebrennikov {
    /// The class takes a Transform (usually a camera) and converts a 2D input vector (normalized in local input space, e.g., WASD/joystick) into a world-space movement direction relative to that transform’s forward/right axes, ignoring the Y axis. The result is a Vector2 in the XZ plane, preserving the original input magnitude.
    public class Translator {
        Transform transform;
        public Translator(Transform transform) {
            this.transform = transform;
        }
        /// <param name="input">is normalized</param>
        public Vector2 Get(Vector2 input) {
            float savedMag = input.magnitude;
            Vector3 noY = new(1f, 0f, 1f);
            Vector3 cameraForwardNoY = Vector3.Scale(transform.forward, noY);
            Vector3 cameraRightNoY = Vector3.Scale(transform.right, noY);
            Vector3 result = cameraForwardNoY * input.y + cameraRightNoY * input.x;
            Vector3 direction = result.normalized * savedMag;
            return new Vector2(direction.x, direction.z);
        }
    }
}
