using UnityEngine;
using UnityEngine.AI;


namespace FFG
{
    public class MoveToRandomLocation : ActionNode
    {
        public Vector2 minRange;
        public Vector2 maxRange;
        public float speed = 5;
        public float stoppingDistance = 0.1f;
        public bool updateRotation = true;
        public float acceleration = 40.0f;
        public float tolerance = 1.0f;

        protected override void OnStart()
        {
            Vector2 targetPos = new Vector2(Random.Range(minRange.x, maxRange.x), Random.Range(minRange.y, maxRange.y));
            if (metadata.agent)
            {
                metadata.agent.speed = speed;
                metadata.agent.stoppingDistance = stoppingDistance;
                metadata.agent.acceleration = acceleration;
                metadata.agent.updateRotation = updateRotation;
                metadata.agent.destination = targetPos;
            }
        }

        protected override void OnEnd() { }

        protected override NodeStates OnUpdate()
        {
            if (!metadata.agent)
                return NodeStates.Failure;
            if (metadata.agent.pathPending)
                return NodeStates.Running;
            if (metadata.agent.remainingDistance < tolerance)
                return NodeStates.Success;
            if (metadata.agent.pathStatus == NavMeshPathStatus.PathInvalid)
                return NodeStates.Failure;
            return NodeStates.Running;
        }
    }
}
