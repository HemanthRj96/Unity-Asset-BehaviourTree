using System;
using UnityEngine.UIElements;
using UnityEditor;


namespace FFG._Editor
{
    public class InspectorView : VisualElement
    {
        private Editor _editor;

        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }
        public InspectorView() { }

        public void UpdateSelection(NodeView nodeView)
        {
            Clear();
            UnityEngine.Object.DestroyImmediate(_editor);
            _editor = Editor.CreateEditor(nodeView.node);
            IMGUIContainer container = new IMGUIContainer(() =>
            {
                if (_editor.target)
                    _editor.OnInspectorGUI();
            });
            Add(container);
        }
    }
}
