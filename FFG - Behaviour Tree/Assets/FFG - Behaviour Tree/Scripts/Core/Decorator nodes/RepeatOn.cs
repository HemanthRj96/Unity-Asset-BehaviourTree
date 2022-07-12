namespace FFG
{
    public class RepeatOn : DecoratorNode
    {
        public bool restartOnSuccess;
        public bool restartOnFailure;
        public bool bypassChildState;


        protected override void OnStart() { }

        protected override void OnEnd() { }

        protected override NodeStates OnUpdate()
        {
            switch (child.UpdateNode())
            {
                case NodeStates.Running:
                    return NodeStates.Running;
                case NodeStates.Failure:
                    if (restartOnFailure)
                        return NodeStates.Running;
                    else
                    {
                        if (bypassChildState)
                            return NodeStates.Success;
                        else
                            return NodeStates.Failure;
                    }
                case NodeStates.Success:
                    if (restartOnSuccess)
                        return NodeStates.Running;
                    return NodeStates.Success;
            }
            return NodeStates.Success;
        }
    }
}
