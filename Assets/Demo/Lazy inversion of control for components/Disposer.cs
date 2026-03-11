// Disposer.csC:\GameDev\Halette\Assets\SereDim\Script\Game\WhenSmokeIsDone\Disposer.csDisposer.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Serebrennikov {
    public class Disposer : IDisposable {
        Action _onDisposed;
        public Disposer(Action onDisposed) {
            _onDisposed = onDisposed;
        }
        public Disposer() { /*To avoid null checks for disposable objects.*/
            _onDisposed = () => {};
        }
        public void Dispose() {
            _onDisposed();
            _onDisposed = null;
        }
    }
}
