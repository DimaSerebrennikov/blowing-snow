// SnowStepSoundManager.csC:\Users\DimaS\Desktop\FeebleSnowOriginal-master\Assets\Dima Serebrennikov\Fluffy snow\SnowStepSoundManager.csSnowStepSoundManager.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Serebrennikov {
    public class SnowStepSoundManager : MonoBehaviour {
        [SerializeField] AudioSource _audioSource;
        [SerializeField] ParticleSystem _snowStepEmitter;
        [SerializeField] AudioClip[] _clips;
        [SerializeField] float _soundThrottleSeconds = 0.15f;
        int _lastAliveCount;
        float _nextAllowedTime;
        void Awake() {
            _lastAliveCount = 0;
            _nextAllowedTime = 0f;
        }
        void LateUpdate() {
            int aliveCount = _snowStepEmitter.particleCount;
            if (aliveCount > _lastAliveCount) {
                TryHandleEmission();
            }
            _lastAliveCount = aliveCount;
        }
        void TryHandleEmission() {
            if (Time.time < _nextAllowedTime) {
                return;
            }
            _nextAllowedTime = Time.time + _soundThrottleSeconds;
            OnParticlesEmitted();
        }
        void OnParticlesEmitted() {
            AudioClip clip = _clips[Random.Range(0, _clips.Length)];
            _audioSource.PlayOneShot(clip);
        }
    }
}
