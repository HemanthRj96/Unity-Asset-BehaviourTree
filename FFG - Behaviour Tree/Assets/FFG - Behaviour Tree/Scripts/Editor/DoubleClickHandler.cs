using UnityEngine.UIElements;
using UnityEditor;


namespace FFG._Editor
{
    public class DoubleClickHandler : MouseManipulator
    {
        double time;
        double doubleClickDuration = 0.3;

        public DoubleClickHandler()
        {
            time = EditorApplication.timeSinceStartup;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            var graphView = target as BehaviourTreeGraphView;
            if (graphView == null)
                return;

            double duration = EditorApplication.timeSinceStartup - time;
            if (duration < doubleClickDuration)
            {
                SelectChildren(evt);
            }

            time = EditorApplication.timeSinceStartup;
        }

        void SelectChildren(MouseDownEvent evt)
        {

            var graphView = target as BehaviourTreeGraphView;
            if (graphView == null)
                return;

            if (!CanStopManipulation(evt))
                return;

            NodeView clickedElement = evt.target as NodeView;
            if (clickedElement == null)
            {
                var ve = evt.target as VisualElement;
                clickedElement = ve.GetFirstAncestorOfType<NodeView>();
                if (clickedElement == null)
                    return;
            }

            // Add children to selection so the root element can be moved
            BehaviourTreeAsset.Traverse(clickedElement.node, node => {
                var view = graphView.FindNodeView(node);
                graphView.AddToSelection(view);
            });
        }
    }
}