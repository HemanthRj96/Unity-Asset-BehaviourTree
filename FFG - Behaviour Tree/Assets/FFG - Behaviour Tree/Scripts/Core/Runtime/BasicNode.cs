using UnityEngine;


namespace FFG
{
    public abstract class BasicNode : ScriptableObject
    {
        #region Fields

        [HideInInspector] public string guid = null;
        [HideInInspector] public Vector2 position;
        [HideInInspector] public NodeStates nodeState = NodeStates.Running;
        [HideInInspector] public bool hasStarted = false;
        [HideInInspector] public Metadata metadata = null;
        [HideInInspector] public Blackboard blackBoard = null;
        [TextArea] public string description = null;

        #endregion
        #region Public methods

        /// <summary>
        /// Call this method to update this node
        /// </summary>
        /// <returns>The state of this node</returns>
        public NodeStates UpdateNode()
        {
            if (hasStarted == false)
            {
                hasStarted = true;
                OnStart();
            }

            nodeState = OnUpdate();

            if (nodeState == NodeStates.Failure || nodeState == NodeStates.Success)
            {
                OnEnd();
                hasStarted = false;
            }

            return nodeState;
        }

        /// <summary>
        /// Method to clone this node
        /// </summary>
        /// <returns>Clone of this node</returns>
        public virtual BasicNode Clone()
        {
            return Instantiate(this);
        }

        /// <summary>
        /// Method to stop this node and it's child nodes
        /// </summary>
        public void Abort()
        {
            BehaviourTreeAsset.Traverse(this, (node) => {
                node.hasStarted = false;
                node.nodeState = NodeStates.Running;
                node.OnEnd();
            });
        }

        #endregion
        #region Protected methods

        protected abstract void OnStart();
        protected abstract void OnEnd();
        protected abstract NodeStates OnUpdate();

        #endregion
    }
}
