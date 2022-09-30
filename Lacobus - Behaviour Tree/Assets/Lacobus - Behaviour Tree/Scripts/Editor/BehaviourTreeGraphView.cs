using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Lacobus.BehaviourTree;


namespace Lacobus.BehaviourTreeEditor
{
    public class BehaviourTreeGraphView : GraphView
    {
        public Action<NodeView> onNodeSelected;
        private BehaviourTreeAsset tree;


        public new class UxmlFactory : UxmlFactory<BehaviourTreeGraphView, GraphView.UxmlTraits> { }

        public BehaviourTreeGraphView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new DoubleClickHandler());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Lacobus - Behaviour Tree/Scripts/UIBuilder/BehaviourTreeEditor.uss");
            styleSheets.Add(styleSheet);

            Undo.undoRedoPerformed += onUndoRedo;
        }


        public void PopulateView(BehaviourTreeAsset tree)
        {
            this.tree = tree;

            graphViewChanged -= onGraphViewChange;
            DeleteElements(graphElements);
            graphViewChanged += onGraphViewChange;

            if (tree.rootNode == null)
            {
                tree.rootNode = tree.CreateNode(typeof(RootNode), Vector2.zero) as RootNode;
                EditorUtility.SetDirty(tree);
                AssetDatabase.SaveAssets();
            }

            // Create the node views
            tree.nodeList.ForEach(n => createNodeView(n));

            // Create edges
            tree.nodeList.ForEach(n =>
            {
                var children = BehaviourTreeAsset.GetChildren(n);
                children.ForEach(c =>
                {
                    NodeView parent = FindNodeView(n);
                    NodeView child = FindNodeView(c);

                    Edge edge = parent.output.ConnectTo(child.input);
                    AddElement(edge);
                });
            });
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            VisualElement vElem = ElementAt(1);
            Vector3 localMousePos = evt.localMousePosition;
            Vector2 worldMousePosition = localMousePos - vElem.transform.position;
            worldMousePosition *= 1 / vElem.transform.scale.x;

            {
                var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
                foreach (var type in types)
                    evt.menu.AppendAction($"{type.BaseType.Name}/{type.Name}", (a) => createNodeAndNodeView(type, worldMousePosition));
            }
            {
                var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
                foreach (var type in types)
                    evt.menu.AppendAction($"{type.BaseType.Name}/{type.Name}", (a) => createNodeAndNodeView(type, worldMousePosition));
            }
            {
                var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
                foreach (var type in types)
                    evt.menu.AppendAction($"{type.BaseType.Name}/{type.Name}", (a) => createNodeAndNodeView(type, worldMousePosition));
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }

        public void UpdateNodeStates()
        {
            nodes.ForEach(n =>
            {
                NodeView view = n as NodeView;
                view.UpdateNodeStatesClass();
            });
        }

        public NodeView FindNodeView(BasicNode node)
        {
            return GetNodeByGuid(node.guid) as NodeView;
        }

        private void onUndoRedo()
        {
            PopulateView(tree);
            AssetDatabase.SaveAssets();
        }

        private GraphViewChange onGraphViewChange(GraphViewChange graphViewChange)
        {
            // Removing nodes and edges
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    NodeView nodeView = elem as NodeView;
                    if (nodeView != null)
                        tree.DeleteNode(nodeView.node);

                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        NodeView parent = edge.output.node as NodeView;
                        NodeView child = edge.input.node as NodeView;
                        tree.RemoveChild(parent.node, child.node);
                    }
                });
            }

            // Adding edges to create
            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parent = edge.output.node as NodeView;
                    NodeView child = edge.input.node as NodeView;
                    tree.AddChild(parent.node, child.node);
                });
            }

            // Updating node positions
            if (graphViewChange.movedElements != null)
            {
                nodes.ForEach(n =>
                {
                    NodeView nodeView = n as NodeView;
                    nodeView.SortChildren();
                });
            }

            return graphViewChange;
        }

        private void createNodeAndNodeView(System.Type type, Vector2 position)
        {
            if (tree == null)
                return;
            createNodeView(tree.CreateNode(type, position));
        }

        private void createNodeView(BasicNode node)
        {
            //todo: put the node where the mouse cursor is
            NodeView nodeView = new NodeView(node);
            nodeView.onNodeSelected = onNodeSelected;
            AddElement(nodeView);
        }
    }
}
