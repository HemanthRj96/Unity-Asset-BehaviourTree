using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


namespace FFG
{
    [CreateAssetMenu()]
    public class BehaviourTreeAsset : ScriptableObject
    {
        #region Fields

        [HideInInspector] public BasicNode rootNode;
        [HideInInspector] public NodeStates treeState = NodeStates.Running;
        [HideInInspector] public List<BasicNode> nodeList = new List<BasicNode>();
        [HideInInspector][SerializeField] private Blackboard blackboard = new Blackboard();

        #endregion
        #region Public methods

        /// <summary>
        /// Call this method to run this behaviour tree, this method should be invoked inside an update method.
        /// </summary>
        /// <returns>Returns the state of the tree</returns>
        public NodeStates Execute()
        {
            if (rootNode.nodeState == NodeStates.Running)
                treeState = rootNode.UpdateNode();
            return treeState;
        }

        /// <summary>
        /// Returns a list of children linked to a parent node
        /// </summary>
        /// <param name="parent">Node with children</param>
        /// <returns>Returns a list of nodes</returns>
        public static List<BasicNode> GetChildren(BasicNode parent)
        {
            List<BasicNode> children = new List<BasicNode>();

            RootNode root = parent as RootNode;
            if (root != null && root.child != null)
                children.Add(root.child);

            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator != null && decorator.child != null)
                children.Add(decorator.child);

            CompositeNode composite = parent as CompositeNode;
            if (composite != null)
                children.AddRange(composite.children);

            return children;
        }

        /// <summary>
        /// Method to traverse the entire tree to obtain children
        /// </summary>
        /// <param name="parent">Starting node from where traversal should happen</param>
        /// <param name="action">Action method</param>
        public static void Traverse(BasicNode parent, Action<BasicNode> action)
        {
            if (parent)
            {
                action.Invoke(parent);
                var children = GetChildren(parent);
                children.ForEach(c => { Traverse(c, action); });
            }
        }

        /// <summary>
        /// Method to clone this tree, you have to clone this tree inorder to make everything work properly
        /// </summary>
        public BehaviourTreeAsset CreateTreeImage()
        {
            BehaviourTreeAsset tree = Instantiate(this);
            tree.rootNode = rootNode.Clone();
            tree.nodeList = new List<BasicNode>();
            Traverse(tree.rootNode, n =>
            {
                tree.nodeList.Add(n);
            });

            return tree;
        }

        /// <summary>
        /// Method to bind a behvaiour tree handler to this tree
        /// </summary>
        /// <param name="self">The behaviour tree handler</param>
        public void BindMetadataToTree(Metadata metadata)
        {
            Traverse(rootNode, n =>
            {
                n.metadata = metadata;
                n.blackBoard = blackboard;
            });
        }

#if UNITY_EDITOR

        /// <summary>
        /// Editor method to add a child to a parent node
        /// </summary>
        /// <param name="parent">Parent node to which new child node is added</param>
        /// <param name="child">Child node that is being added</param>
        public void AddChild(BasicNode parent, BasicNode child)
        {
            // Recording this object so that undo and redo works

            RootNode root = parent as RootNode;
            if (root != null)
            {
                Undo.RecordObject(root, "Behaviour Tree (AddChild)");
                root.child = child;
                EditorUtility.SetDirty(root);
            }

            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator != null)
            {
                Undo.RecordObject(decorator, "Behaviour Tree (AddChild)");
                decorator.child = child;
                EditorUtility.SetDirty(decorator);
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite != null)
            {
                Undo.RecordObject(composite, "Behaviour Tree (AddChild)");
                composite.children.Add(child);
                EditorUtility.SetDirty(composite);
            }
        }

        /// <summary>
        /// Editor method to remove child from a parent node
        /// </summary>
        /// <param name="parent">Parent node from which the child is removed</param>
        /// <param name="child">Child node that's being removed</param>
        public void RemoveChild(BasicNode parent, BasicNode child)
        {
            RootNode root = parent as RootNode;
            if (root != null)
            {
                Undo.RecordObject(root, "Behaviour Tree (RemoveChild)");
                root.child = null;
                EditorUtility.SetDirty(root);
            }

            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator != null)
            {
                Undo.RecordObject(decorator, "Behaviour Tree (RemoveChild)");
                decorator.child = null;
                EditorUtility.SetDirty(decorator);
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite != null)
            {
                Undo.RecordObject(composite, "Behaviour Tree (RemoveChild)");
                composite.children.Remove(child);
                EditorUtility.SetDirty(composite);
            }
        }

        /// <summary>
        /// Method to create a node from a system type and add its subasset to this tree
        /// </summary>
        /// <param name="nodeType">Type of node to create excluding basic node types</param>
        /// <returns>Return a basic node</returns>
        public BasicNode CreateNode(System.Type nodeType, Vector2 position)
        {
            BasicNode node = CreateInstance(nodeType) as BasicNode;

            if (!node)
                return null;

            node.name = createName(nodeType.Name);
            node.guid = GUID.Generate().ToString();
            node.position = position;

            // Record addition of this object to the nodeList
            Undo.RecordObject(this, "Behaviour Tree (CreateNode)");
            nodeList.Add(node);

            // Record addition of this object as a subasset
            Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();

            return node;
        }

        /// <summary>
        /// Method to delete node and its subasset from this tree
        /// </summary>
        /// <param name="node">Node to be deleted</param>
        public void DeleteNode(BasicNode node)
        {
            // Record removal of this object from the nodeList
            Undo.RecordObject(this, "Behaviour Tree (DeleteNode)");
            nodeList.Remove(node);
            // Record removal of this object from the tree asset
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }

#endif
        #endregion

        private string createName(string targetString)
        {
            string ret = "";

            for (int i = 0; i < targetString.Length; ++i)
            {
                if (Char.IsUpper(targetString[i]) && ret.Length > 0)
                {
                    ret += $" {targetString[i]}";
                    continue;
                }

                ret += targetString[i];
            }
            return ret;
        }
    }
}
