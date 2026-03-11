// Mousing2D.csC:\GameDev\Halette\Assets\SereDim\Script\Game\Cmd\Movement\Mousing2D.csMousing2D.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Serebrennikov {
    public class Mousing2D : Mousing {
        Transform _target;
        public Mousing2D(Transform marker, Camera camera, Transform target) : base(marker, camera) {
            _target = target;
        }
        protected override void LookAt(Vector3 target) {
            _target.position = target;
        }
    }
}
