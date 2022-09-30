using Lacobus.BehaviourTree;


public class Inverter : DecoratorNode
{
    protected override void OnStart() { }

    protected override void OnEnd() { }

    protected override NodeStates OnUpdate()
    {
        switch (child.UpdateNode())
        {
            case NodeStates.Running:
                return NodeStates.Running;
            case NodeStates.Failure:
                return NodeStates.Success;
            case NodeStates.Success:
                return NodeStates.Failure;
        }
        return NodeStates.Failure;
    }
}
