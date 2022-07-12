using UnityEngine;


namespace FFG
{
    public abstract class DecoratorNode : BasicNode
    {
        [HideInInspector]
        public BasicNode child;

        public override BasicNode Clone()
        {
            DecoratorNode node = Instantiate(this);
            if (this.child)
                node.child = this.child.Clone();
            else
                node.child = null;
            return node;
        }
    }
}