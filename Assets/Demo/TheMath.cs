// Cmd.csC:\GameDev\Halette\Assets\SereDim\Script\Math\Cmd.csCmd.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Serebrennikov {
    public static class TheMath {
        public static Vector2 SquareNormalize(Vector2 v) {
            float m = Mathf.Max(Mathf.Abs(v.x), Mathf.Abs(v.y));
            return m > 1e-6f ? v / m : Vector2.zero;
        }
        public static float ExponentDecay(float a, float b, float decay, float dt) {
            return (a - b) * Mathf.Exp(-decay * dt) + b;
        }
        public static Quaternion ExponentDecay(Quaternion a, Quaternion b, float decay, float dt) {
            return Quaternion.Slerp(a, b, 1 - Mathf.Exp(-decay * dt));
        }
        public static int AsWeightedSelection(IEnumerable<float> input) { /*has no IEnumerable time issues*/
            float sum = input.Sum();
            float randomNumber = Random.value * sum;
            float segment = 0f;
            int i = 0;
            foreach (float n in input) {
                segment += n;
                if (randomNumber < segment) return i;
                i++;
            }
            return i; /*not reachable*/
        }
        public static float AsWeightedSelection(List<(float value, float priority)> input) { /*will have time issues with IEnumerable*/
            IEnumerable<float> priorities = input.Select(item => item.priority);
            int index = AsWeightedSelection(priorities);
            return input[index].value;
        }
        public static float Slow0Boost1(float x, float a) {
            if (x > 1) {
                x = 1f;
            }
            return 1 - Mathf.Pow(1 - x, 1f / a);
        }
        public static float Decay0Fast1(float x, float a) {
            if (x > 1) {
                x = 1f;
            }
            return Mathf.Pow(x, a);
        }
        public static float Boost0Slow1(float x, float a) {
            if (x > 1) {
                x = 1f;
            }
            return Mathf.Pow(x, 1f / a);
        }
        public static float Fast0Decay1(float x, float a) {
            if (x > 1) {
                x = 1f;
            }
            return 1 - Mathf.Pow(1 - x, a);
        }
        public static float From1To0(float coef) {
            if (coef >= 0 && coef <= 0.5f) {
                return 1f - 2f * Mathf.Pow(coef, 2f);
            }
            return 2f - 4f * coef + 2f * Mathf.Pow(coef, 2f);
        }
        public static float From0To1(float coef) {
            if (coef >= 0 && coef <= 0.5f) {
                return 2f * Mathf.Pow(coef, 2f);
            }
            return 2f - 4f * coef + 2f * Mathf.Pow(coef, 2f);
        }
        public static Color ExponentDecay(Color a, Color b, float decay, float dt) {
            return (a - b) * Mathf.Exp(-decay * dt) + b;
        }
        public static Vector3 ExponentDecay(Vector3 a, Vector3 b, float decay, float dt) {
            return (a - b) * Mathf.Exp(-decay * dt) + b;
        }
        public static float AsExponentialDistribution(float expectedValue) {
            float zeta;
            do {
                zeta = Random.value;
            } while (zeta <= Mathf.Epsilon);
            return -expectedValue * Mathf.Log(zeta);
        }
        public static float NormalDistribution_BoxMuller(float expectedValue, float dispersion) {
            if (dispersion <= 0.0f) {
                dispersion = Mathf.Epsilon;
            }
            float deviation = Mathf.Sqrt(dispersion);
            float n1 = Random.value;
            float n2 = Random.value;
            /*--BOX MULLER--*/
            float x = -2f * Mathf.Log(n1);
            float x1 = 2f * Mathf.PI * n2;
            float mullerX = Mathf.Sqrt(x);
            float mullerX1 = Mathf.Cos(x1);
            float mullerResult = mullerX * mullerX1;
            return expectedValue + deviation * mullerResult;
        }
    }
}
