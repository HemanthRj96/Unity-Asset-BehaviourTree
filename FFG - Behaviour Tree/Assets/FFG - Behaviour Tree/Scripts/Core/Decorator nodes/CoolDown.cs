using UnityEngine;


namespace FFG
{
    public class CoolDown : DecoratorNode
    {
        public bool beginCoolDownOnSuccess;
        public bool beginCoolDownOnFailure;
        public float coolDownDuration;

        private float _startTime = -1;

        protected override void OnStart() 
        {
            _startTime = -1;
        }

        protected override void OnEnd() { }

        protected override NodeStates OnUpdate()
        {
            if (_startTime != -1)
            {
                if (Time.time - _startTime > coolDownDuration)
                    _startTime = -1;
                return NodeStates.Running;
            }

            switch (child.UpdateNode())
            {
                case NodeStates.Running:
                    return NodeStates.Running;
                case NodeStates.Failure:
                    {
                        if (beginCoolDownOnFailure)
                            _startTime = Time.time;

                        return NodeStates.Failure;
                    }
                case NodeStates.Success:
                    {
                        if (beginCoolDownOnSuccess)
                            _startTime = Time.time;

                        return NodeStates.Success;
                    }
            }
            return NodeStates.Success;
        }
    }
}