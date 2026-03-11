// FactoryTicker.csC:\GameDev\Halette\Assets\SereDim\Script\Tool\FactoryTicker.csFactoryTicker.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Serebrennikov {
    public static class LoopBsl {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Run() {
            LoopMb _mono = new GameObject("Loop").AddComponent<LoopMb>();
            Ticker n = new();
            _mono.model = n;
            Object.DontDestroyOnLoad(_mono);
            Loop instance = new(_mono);
        }
    }
}
