using Lacobus.BehaviourTree;
using System.Collections.Generic;


public class Parallel : CompositeNode
{
    List<NodeStates> childrenLeftToExecute = new List<NodeStates>();

    protected override void OnStart()
    {
        childrenLeftToExecute.Clear();
        children.ForEach(a =>
        {
            childrenLeftToExecute.Add(NodeStates.Running);
        });
    }

    protected override void OnEnd() { }

    protected override NodeStates OnUpdate()
    {
        bool stillRunning = false;
        for (int i = 0; i < childrenLeftToExecute.Count; ++i)
        {
            if (childrenLeftToExecute[i] == NodeStates.Running)
            {
                var status = children[i].UpdateNode();

                if (status == NodeStates.Failure)
                {
                    AbortRunningChildren();
                    return NodeStates.Failure;
                }

                if (status == NodeStates.Running)
                {
                    stillRunning = true;
                }

                childrenLeftToExecute[i] = status;
            }
        }

        return stillRunning ? NodeStates.Running : NodeStates.Success;
    }

    void AbortRunningChildren()
    {
        for (int i = 0; i < childrenLeftToExecute.Count; ++i)
        {
            if (childrenLeftToExecute[i] == NodeStates.Running)
            {
                children[i].Abort();
            }
        }
    }
}
