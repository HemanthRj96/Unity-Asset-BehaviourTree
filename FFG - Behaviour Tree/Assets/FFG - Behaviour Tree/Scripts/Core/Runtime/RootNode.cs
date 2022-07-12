using UnityEngine;


namespace FFG
{
    public class RootNode : BasicNode
    {
        #region Fields

        [HideInInspector]
        public BasicNode child;

        #endregion
        #region Public methods

        /// <summary>
        /// Method to clone this node
        /// </summary>
        /// <returns>Returns the clone of this node</returns>
        public override BasicNode Clone()
        {
            RootNode node = Instantiate(this);
            if (this.child)
                node.child = this.child.Clone();
            else
                node.child = null;
            return node;
        }

        #endregion
        #region Protected methods

        protected override void OnEnd() 
        { 
        }

        protected override void OnStart() 
        { 
        }

        protected override NodeStates OnUpdate()
        {
            return child.UpdateNode();
        }

        #endregion
    }
}
