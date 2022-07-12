using System.Collections.Generic;
using UnityEngine;


namespace FFG
{
    public abstract class CompositeNode : BasicNode
    {
        [HideInInspector]
        public List<BasicNode> children = new List<BasicNode>();

        public override BasicNode Clone()
        {
            CompositeNode node = Instantiate(this);
            if (this.children.Count != 0)
                node.children = this.children.ConvertAll<BasicNode>(child => child.Clone());
            else
                node.children = new List<BasicNode>();
            return node;
        }
    }
}
