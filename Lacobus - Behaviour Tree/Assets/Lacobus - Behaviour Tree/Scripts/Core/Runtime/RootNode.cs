using UnityEngine;


namespace Lacobus.BehaviourTree
{
    public class RootNode : BasicNode
    {
        // Fields

        [HideInInspector]
        public BasicNode child;


        // Public methods

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


        // Private methods

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
    }
}
