using UnityEngine;


namespace FFG
{
    public class Wait : ActionNode
    {
        public float duration = 0;

        private float _startTime = 0;


        protected override void OnStart()
        {
            _startTime = Time.time;
            duration = Mathf.Max(0.002f, duration);
        }

        protected override NodeStates OnUpdate()
        {
            if (Time.time - _startTime > duration)
                return NodeStates.Success;
            return NodeStates.Running;
        }

        protected override void OnEnd() { }
    }
}
