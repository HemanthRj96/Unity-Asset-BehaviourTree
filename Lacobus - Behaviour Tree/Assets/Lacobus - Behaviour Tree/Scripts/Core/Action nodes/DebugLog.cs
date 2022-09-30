using Lacobus.BehaviourTree;
using UnityEngine;


public class DebugLog : ActionNode
{
    public string onStartLogMessage = null;
    public string onUpdateLogMessage = null;
    public string onEndLogMessage = null;

    protected override void OnStart()
    {
        if (!string.IsNullOrEmpty(onStartLogMessage))
            Debug.Log($"On start : {onStartLogMessage}");
    }

    protected override NodeStates OnUpdate()
    {
        if (!string.IsNullOrEmpty(onUpdateLogMessage))
            Debug.Log($"On update : {onUpdateLogMessage}");
        return NodeStates.Success;
    }

    protected override void OnEnd()
    {
        if (!string.IsNullOrEmpty(onEndLogMessage))
            Debug.Log($"On end : {onEndLogMessage}");
    }
}
