// PlayerController.csC:\Users\DimaS\Desktop\FeebleSnowOriginal-master\Assets\Dima Serebrennikov\Map generation tool\PlayerController.csPlayerController.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Serebrennikov {
    public class PlayerController : MonoBehaviour, ISpeed {
        [SerializeField] Animator _animator;
        [SerializeField] Transform _bodyPt;
        [SerializeField] Camera _camera;
        [SerializeField] Transform _target;
        Movement _movement;
        Mousing2D _targetToMouse;
        [SerializeField] float _speed;
        void Awake() {
            _target = TheUnityObject.InstanceFromAsset(_target);
            _movement = new Movement(_animator, _bodyPt, _camera.transform, this);
            _targetToMouse = new Mousing2D(_bodyPt, _camera, _target);
        }
        void Start() {
            _targetToMouse.Start();
        }
        void Update() {
            _movement.Update();
            LookAtTarget();
        }
        void LookAtTarget() {
            Vector3 targetPosition = _target.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
        }
        public float Speed => _speed;
    }
}
