using UnityEngine;


namespace FFG
{
    public class TimeOut : DecoratorNode
    {
        public float timeOutDuration;

        private float _startTime;


        protected override void OnStart()
        {
            _startTime = Time.time;
        }

        protected override void OnEnd() { }

        protected override NodeStates OnUpdate()
        {
            if (Time.time - _startTime > timeOutDuration)
                return NodeStates.Failure;

            return child.UpdateNode();
        }
    }
}
