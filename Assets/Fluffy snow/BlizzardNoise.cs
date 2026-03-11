// BlizzardNoise.csC:\Users\DimaS\Desktop\FeebleSnowOriginal-master\Assets\Dima Serebrennikov\Fluffy snow\BlizzardNoise.csBlizzardNoise.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Serebrennikov {
    public class BlizzardNoise : MonoBehaviour {
        [SerializeField] AudioSource _audioSource;
        [SerializeField] ParticleSystem _blizzard;
        [SerializeField] float _max;
        float _min;
        void Awake() {
            _min = _blizzard.emission.rateOverTime.constant;
        }
        void Update() {
            _audioSource.volume = (_blizzard.emission.rateOverTime.constant - _min) / (_max - _min);
        }
    }
}
