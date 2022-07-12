using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFG
{
    public class Selector : CompositeNode
    {
        protected int _currentIndex = 0;

        protected override void OnStart()
        {
            _currentIndex = 0;
        }

        protected override void OnEnd() { }

        protected override NodeStates OnUpdate()
        {
            for (int i = _currentIndex; i < children.Count; ++i)
            {
                _currentIndex = i;
                var child = children[i];

                switch (child.UpdateNode())
                {
                    case NodeStates.Running:
                        return NodeStates.Running;
                    case NodeStates.Success:
                        return NodeStates.Success;
                    case NodeStates.Failure:
                        continue;
                }
            }
            return NodeStates.Failure;
        }
    }
}