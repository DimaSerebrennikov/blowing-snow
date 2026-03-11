// TheInput.csC:\v1\Backup\Halette\Assets\SereDim\Script\Game\v9\TheInput.csTheInput.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Serebrennikov {
    public static class TheInput {
        public static Vector3 Get2Axes() {
            return new Vector2 {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            };
        }
        public static Vector3 Get2RawAxes() {
            return new Vector2 {
                x = Input.GetAxisRaw("Horizontal"),
                y = Input.GetAxisRaw("Vertical")
            };
        }
    }
}
