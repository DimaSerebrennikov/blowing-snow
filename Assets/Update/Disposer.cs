using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace BlowingSnow {
    public class Disposer : IDisposable {
        Action _onDisposed;
        public Disposer(Action onDisposed) {
            _onDisposed = onDisposed;
        }
        public void Dispose() {
            _onDisposed();
            _onDisposed = null;
        }
    }
}