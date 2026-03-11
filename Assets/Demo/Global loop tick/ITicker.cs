// ITicker.csC:\v1\Backup\Halette\Assets\Serebrennikov\ITicker.csITicker.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Serebrennikov {
    public interface ITicker : IFixedTick, ITick {
        public IDisposable RegisterTick(ITick onTick);
        public IDisposable RegisterFixedTick(IFixedTick onTick);
        public void Refresh();
    }
}
