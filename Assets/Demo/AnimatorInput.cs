// AnimatorInput.csC:\v1\Backup\Halette\Assets\SereDim\Script\Game\v9\AnimatorInput.csAnimatorInput.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Serebrennikov {
    public class AnimatorInput {
        static readonly int InputX = Animator.StringToHash("Input x");
        static readonly int InputY = Animator.StringToHash("Input y");
        Animator _animator;
        public AnimatorInput(Animator animator) {
            _animator = animator;
        }
        public void Update(Vector2 localDirection) {
            _animator.SetFloat(InputX, localDirection.x);
            _animator.SetFloat(InputY, localDirection.y);
        }
    }
}
