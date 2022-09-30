using Lacobus.BehaviourTree;


public class RepeatUntil : DecoratorNode
{
    public bool repeatUntilChildFails = false;
    public bool repeatUntilChildSucceeds = false;

    protected override void OnStart() { }

    protected override void OnEnd() { }

    protected override NodeStates OnUpdate()
    {
        switch (child.UpdateNode())
        {
            case NodeStates.Running:
                return NodeStates.Running;
            case NodeStates.Failure:
                if (repeatUntilChildFails)
                    return NodeStates.Success;
                return NodeStates.Running;
            case NodeStates.Success:
                if (repeatUntilChildSucceeds)
                    return NodeStates.Success;
                return NodeStates.Running;
        }
        return NodeStates.Success;
    }
}
