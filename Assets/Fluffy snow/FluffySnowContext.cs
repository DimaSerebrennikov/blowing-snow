// FluffySnowContext.csC:\Users\DimaS\Desktop\FeebleSnowOriginal-master\Assets\Dima Serebrennikov\Fluffy snow\FluffySnowContext.csFluffySnowContext.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Serebrennikov {
    public class FluffySnowContext : MonoBehaviour {
        static readonly int SnowHeight = Shader.PropertyToID("_SnowHeight");
        static readonly int MainTex = Shader.PropertyToID("_MainTex");
        static readonly int Noise = Shader.PropertyToID("_Noise");
        static readonly int NoiseScale = Shader.PropertyToID("_NoiseScale");
        static readonly int NoiseWeight = Shader.PropertyToID("_NoiseWeight");
        static readonly int ColorInShader = Shader.PropertyToID("_Color");
        [SerializeField] float _incrementMaxHeight = 1f;
        [SerializeField] float _incrementDeltaSnow = 1f;
        [SerializeField] float _incrementBlizzard = 1f;
        [SerializeField] ParticleSystem _blizzard;
        [SerializeField] Material _snowMaterial;
        [SerializeField] List<Material> _texturePacks = new();
        int _textureCounter;
        [SerializeField] AudioClip _titDown;
        [SerializeField] AudioClip _titUp;
        [SerializeField] AudioClip _clickDown;
        [SerializeField] AudioClip _clickUp;
        [SerializeField] AudioClip _clackDown;
        [SerializeField] AudioClip _clackUp;
        [SerializeField] AudioSource _audioSource;
        [SerializeField] [Min(0f)] float _heldKeySoundInterval = 0.12f;
        float _nextDownTime;
        public void AddCurrentlySettedTextures(string assetName) {
    #if UNITY_EDITOR
            string folderPath = "Assets/SnowSnapshots";
            string assetPath = $"{folderPath}/{assetName}.mat";
            if (!AssetDatabase.IsValidFolder(folderPath)) {
                AssetDatabase.CreateFolder("Assets", "SnowSnapshots");
            }
            if (AssetDatabase.LoadAssetAtPath<Material>(assetPath) != null) {
                return;
            }
            Material snapshot = new(_snowMaterial);
            snapshot.name = assetName;
            AssetDatabase.CreateAsset(snapshot, assetPath);
            EditorUtility.SetDirty(snapshot);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            _texturePacks.Insert(0, snapshot);
    #endif
        }
        public void SetOnZero() {
            if (_texturePacks.Count == 0) {
                return;
            }
            _snowMaterial.CopyPropertiesFromMaterial(_texturePacks[0]);
        }
        void TryPlayHeld(AudioClip clip) {
            if (clip == null || _audioSource == null) {
                return;
            }
            float currentTime = Time.unscaledTime;
            if (currentTime < _nextDownTime) {
                return;
            }
            _audioSource.PlayOneShot(clip);
            _nextDownTime = currentTime + _heldKeySoundInterval;
        }
        void Update() {
            if (Input.GetKey(KeyCode.Z)) {
                TryPlayHeld(_clickDown);
                AddRateOverTime(-_incrementBlizzard * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.X)) {
                TryPlayHeld(_clickUp);
                AddRateOverTime(+_incrementBlizzard * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftShift)) {
                TryPlayHeld(_clackDown);
                float currentValue = _snowMaterial.GetFloat(SnowHeight);
                _snowMaterial.SetFloat(SnowHeight, currentValue + _incrementMaxHeight * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftControl)) {
                TryPlayHeld(_clickUp);
                float currentValue = _snowMaterial.GetFloat(SnowHeight);
                _snowMaterial.SetFloat(SnowHeight, currentValue - _incrementMaxHeight * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.C)) {
                _audioSource.PlayOneShot(_titDown);
                _textureCounter = (_textureCounter + 1) % _texturePacks.Count;
                SetTextureInCurrentIndex();
            }
            if (Input.GetKeyDown(KeyCode.V)) {
                _audioSource.PlayOneShot(_titUp);
                _textureCounter = (_textureCounter - 1 + _texturePacks.Count) % _texturePacks.Count;
                SetTextureInCurrentIndex();
            }
        }
        void SetTextureInCurrentIndex() {
            if (_texturePacks.Count == 0) {
                return;
            }
            _snowMaterial.CopyPropertiesFromMaterial(_texturePacks[_textureCounter]);
        }
        void AddRateOverTime(float delta) {
            ParticleSystem.EmissionModule emission = _blizzard.emission;
            ParticleSystem.MinMaxCurve curve = emission.rateOverTime;
            float current = curve.constant;
            float next = Mathf.Max(0f, current + delta);
            curve.constant = next;
            emission.rateOverTime = curve;
        }
    }
}
