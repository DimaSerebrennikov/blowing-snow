// DroppingHandler.csC:\Users\DimaS\Blowing snow\Assets\Update\DroppingHandler.csDroppingHandler.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace BlowingSnow {
    public class DroppingHandler : IDisposable {
        readonly VisualElement _target;
        public event Action<UnityEngine.Object[]> Dropped;
        public DroppingHandler(VisualElement target) {
            _target = target;
            _target.RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
            _target.RegisterCallback<DragPerformEvent>(OnDragPerform);
            _target.RegisterCallback<DragLeaveEvent>(OnDragLeave);
        }
        public void Dispose() {
            _target.UnregisterCallback<DragUpdatedEvent>(OnDragUpdated);
            _target.UnregisterCallback<DragPerformEvent>(OnDragPerform);
            _target.UnregisterCallback<DragLeaveEvent>(OnDragLeave);
        }
        void OnDragUpdated(DragUpdatedEvent evt) {
            UnityEngine.Object[] dragged = GetDraggedObjects();
            bool canAccept = dragged.Length > 0;
            DragAndDrop.visualMode = canAccept ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Rejected;
            evt.StopPropagation();
        }
        void OnDragPerform(DragPerformEvent evt) {
            UnityEngine.Object[] dragged = GetDraggedObjects();
            if (dragged.Length == 0) {
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                evt.StopPropagation();
                return;
            }
            DragAndDrop.AcceptDrag();
            Dropped?.Invoke(dragged);
            evt.StopPropagation();
        }
        void OnDragLeave(DragLeaveEvent evt) {
            evt.StopPropagation();
        }
        static UnityEngine.Object[] GetDraggedObjects() {
            return DragAndDrop.objectReferences.OfType<UnityEngine.Object>().ToArray();
        }
    }
}
