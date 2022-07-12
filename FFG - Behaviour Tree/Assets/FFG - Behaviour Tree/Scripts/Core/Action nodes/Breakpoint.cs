using UnityEngine;


namespace FFG
{
    public class Breakpoint : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Log("Triggering breakpoint");
            Debug.Break();
        }

        protected override NodeStates OnUpdate()
        {
            return NodeStates.Success;
        }

        protected override void OnEnd() { }
    }
}
