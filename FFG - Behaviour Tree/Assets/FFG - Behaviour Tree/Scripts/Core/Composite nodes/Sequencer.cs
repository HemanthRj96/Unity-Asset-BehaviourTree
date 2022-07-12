namespace FFG
{
    public class Sequencer : CompositeNode
    {
        private int _childIndex = 0;

        protected override void OnStart()
        {
            _childIndex = 0;
        }

        protected override NodeStates OnUpdate()
        {
            if (children.Count == 0)
                return NodeStates.Failure;

            var child = children[_childIndex];

            switch (child.UpdateNode())
            {
                case NodeStates.Failure:
                    return NodeStates.Failure;
                case NodeStates.Running:
                    return NodeStates.Running;
                case NodeStates.Success:
                    ++_childIndex;
                    break;
            }
            return _childIndex == children.Count ? NodeStates.Success : NodeStates.Running;
        }

        protected override void OnEnd() { }
    }
}
