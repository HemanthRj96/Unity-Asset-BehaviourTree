using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

namespace FFG._Editor
{
    public class NodeView : Node
    {
        public Action<NodeView> onNodeSelected;
        public BasicNode node;
        public Port input;
        public Port output;

        public NodeView(BasicNode node) : base("Assets/FFG - Behaviour Tree/Scripts/UIBuilder/NodeView.uxml")
        {
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            setupClasses();
            createInputPorts();
            createOutputPorts();

            Label labelDescription = this.Q<Label>("description");
            labelDescription.bindingPath = "description";
            labelDescription.Bind(new SerializedObject(node));
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(node, "Behaviour Tree (SetPosition)");
            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
            EditorUtility.SetDirty(node);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            onNodeSelected?.Invoke(this);
        }

        public void SortChildren()
        {
            CompositeNode node = this.node as CompositeNode;
            if (node)
                node.children.Sort(sortByHorizontalPosition);
        }

        public void UpdateNodeStatesClass()
        {
            RemoveFromClassList("running");
            RemoveFromClassList("success");
            RemoveFromClassList("failure");

            if (Application.isPlaying)
            {
                switch (node.nodeState)
                {
                    case NodeStates.Running:
                        if (node.hasStarted)
                            AddToClassList("running");
                        break;
                    case NodeStates.Failure:
                        AddToClassList("failure");
                        break;
                    case NodeStates.Success:
                        AddToClassList("success");
                        break;
                }
            }
        }

        private int sortByHorizontalPosition(BasicNode left, BasicNode right)
        {
            return left.position.x < right.position.x ? -1 : 1;
        }

        private void setupClasses()
        {
            if (node is ActionNode)
            {
                AddToClassList("action");

                if (node is DebugLog)
                    AddToClassList("action-debuglog");
            }
            else if (node is CompositeNode)
            {
                AddToClassList("composite");

                if (node is Sequencer)
                    AddToClassList("composite-sequencer");
            }
            else if (node is DecoratorNode)
            {
                AddToClassList("decorator");

                if (node is Wait)
                    AddToClassList("decorator-wait");
                if (node is RepeatFor)
                    AddToClassList("decorator-repeat");
            }
            else if (node is RootNode)
            {
                AddToClassList("root");
            }
        }

        private void createInputPorts()
        {
            if (node is ActionNode)
                input = new NodePort(Direction.Input, Port.Capacity.Single);
            else if (node is CompositeNode)
                input = new NodePort(Direction.Input, Port.Capacity.Single);
            else if (node is DecoratorNode)
                input = new NodePort(Direction.Input, Port.Capacity.Single);
            else if (node is RootNode) { }

            if (input != null)
            {
                input.portName = "";
                input.style.flexDirection = FlexDirection.Column;
                inputContainer.Add(input);
            }
        }

        private void createOutputPorts()
        {
            if (node is ActionNode) { }
            else if (node is CompositeNode)
                output = new NodePort(Direction.Output, Port.Capacity.Multi);
            else if (node is DecoratorNode)
                output = new NodePort(Direction.Output, Port.Capacity.Single);
            else if (node is RootNode)
                output = new NodePort(Direction.Output, Port.Capacity.Single);

            if (output != null)
            {
                output.portName = "";
                output.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(output);
            }
        }
    }
}
