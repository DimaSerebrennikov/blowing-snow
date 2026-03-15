using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace BlowingSnow {
    public class ReactiveList<T> : IList {
        public T this[int index] {
            get => _items[index];
            set {
                T old = _items[index];
                if (EqualityComparer<T>.Default.Equals(old, value)) {
                    return;
                }
                _items[index] = value;
                _onRemove(old);
                _onAdd(value);
            }
        }
        object IList.this[int index] { get => this[index]!; set => this[index] = Cast(value); }
        /*data*/
        readonly List<T> _items = new();
        Action<T> _onAdd = _ => {};
        Action<T> _onRemove = _ => {};
        /*logic*/
        public int Count => _items.Count;
        public bool IsReadOnly => false;
        public bool IsFixedSize => false;
        public bool IsSynchronized => false;
        public object SyncRoot => ((ICollection)_items).SyncRoot;
        public IDisposable Observe(Action<T> add, Action<T> remove) {
            State(add);
            return Listen(add, remove);
        }
        public void State(Action<T> add) {
            for (int i = 0; i < _items.Count; i++) {
                add(_items[i]);
            }
        }
        public IDisposable Listen(Action<T> add, Action<T> remove) {
            _onAdd += add;
            _onRemove += remove;
            Disposer d = new(() => {
                _onAdd -= add;
                _onRemove -= remove;
            });
            return d;
        }
        public void Add(T value) {
            _items.Add(value);
            _onAdd(value);
        }
        public bool Remove(T value) {
            int idx = _items.IndexOf(value);
            if (idx < 0) return false;
            _items.RemoveAt(idx);
            _onRemove(value);
            return true;
        }
        int IList.Add(object value) {
            T item = Cast(value);
            _items.Add(item);
            _onAdd(item);
            return _items.Count - 1;
        }
        public void Clear() {
            if (_items.Count == 0) return;
            foreach (T item in _items) {
                _onRemove(item);
            }
            _items.Clear();
        }
        bool IList.Contains(object value) {
            if (!IsCompatible(value)) return false;
            return _items.Contains((T)value!);
        }
        public bool Contains(T item) {
            return _items.Contains(item);
        }
        int IList.IndexOf(object value) {
            if (!IsCompatible(value)) return -1;
            return _items.IndexOf((T)value!);
        }
        void IList.Insert(int index, object value) {
            T item = Cast(value);
            _items.Insert(index, item);
            _onAdd(item);
        }
        void IList.Remove(object value) {
            if (!IsCompatible(value)) return;
            Remove((T)value!);
        }
        public void RemoveAt(int index) {
            T item = _items[index];
            _items.RemoveAt(index);
            _onRemove(item);
        }
        public void CopyTo(Array array, int index) {
            ((ICollection)_items).CopyTo(array, index);
        }
        public IEnumerator GetEnumerator() {
            return _items.GetEnumerator();
        }
        static bool IsCompatible(object value) {
            if (value is T) return true;
            if (value is null) {
                Type t = typeof(T);
                return !t.IsValueType || Nullable.GetUnderlyingType(t) != null;
            }
            return false;
        }
        static T Cast(object value) {
            if (!IsCompatible(value)) {
                throw new ArgumentException($"Value must be of type {typeof(T)}", nameof(value));
            }
            return (T)value!;
        }
    }
}