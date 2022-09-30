using Lacobus.BehaviourTree;
using UnityEngine.Events;


public class MessageBroadcast : ActionNode
{
    public UnityEvent targetEvent;

    protected override void OnStart() { }

    protected override void OnEnd() { }

    protected override NodeStates OnUpdate()
    {
        if (targetEvent.GetPersistentEventCount() == 0)
            return NodeStates.Failure;

        targetEvent.Invoke();
        return NodeStates.Success;
    }
}
