using UnityEngine.Events;


namespace FFG
{
    public class ChildMessageBroadcast : DecoratorNode
    {
        public UnityEvent onNodeStartEvent;
        public UnityEvent onNodeEndEvent;
        public UnityEvent onChildSuccessEvent;
        public UnityEvent onChildFailureEvent;
        public bool bypassChildState;

        protected override void OnStart()
        {
            onNodeEndEvent?.Invoke();
        }

        protected override void OnEnd()
        {
            onNodeEndEvent?.Invoke();
        }

        protected override NodeStates OnUpdate()
        {
            switch (child.UpdateNode())
            {
                case NodeStates.Running:
                    return NodeStates.Running;
                case NodeStates.Failure:
                    {
                        onChildFailureEvent?.Invoke();

                        if (bypassChildState)
                            return NodeStates.Success;
                        else
                            return NodeStates.Failure;
                    }
                case NodeStates.Success:
                    {
                        onChildSuccessEvent?.Invoke();
                        return NodeStates.Success;
                    }
            }
            return NodeStates.Success;
        }
    }
}
