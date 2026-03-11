using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Serebrennikov {
    public class Loop {
        static LoopMb _mono;
        public Loop(LoopMb mono) {
            _mono = mono;
        }
        public static IDisposable Tick(ITick onTick) {
            return _mono.RegisterTick(onTick);
        }
        public static IDisposable FTick(IFixedTick onTick) {
            return _mono.RegisterFixedTick(onTick);
        }
        public static IDisposable Tick(Action on) {
            return _mono.RegisterTick(new Action_as_Tick(on));
        }
        public static IDisposable FTick(Action on) {
            return _mono.RegisterFixedTick(new Action_as_FixedTick(on));
        }
        public static void Refresh() {
            _mono.Refresh();
        }
    }
    public class Action_as_FixedTick : IFixedTick {
        readonly Action _callback;
        public Action_as_FixedTick(Action callback) {
            _callback = callback;
        }
        public void FTick() {
            _callback();
        }
    }
    public class Action_as_Tick : ITick {
        public Action callback { get; }
        public Action_as_Tick(Action callback) {
            this.callback = callback;
        }
        public void Tick() {
            callback();
        }
    }
    public readonly struct OneTick<T> : IDisposable {
        readonly List<T> _list;
        readonly T _target;
        public OneTick(List<T> list, T target) {
            _list = list;
            _target = target;
        }
        public void Dispose() {
            _list.Remove(_target);
        }
    }
}
