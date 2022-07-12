using UnityEngine;


namespace FFG
{
    public class RepeatFor : DecoratorNode
    {
        public int repeatCount = 1;
        public bool shouldLoopOnFail = false;
        public bool infiniteLoop = false;

        private int currentRepeatCount = 0;

        protected override void OnStart()
        {
            if (infiniteLoop == false)
            {
                repeatCount = Mathf.Max(1, repeatCount);
                currentRepeatCount = 1;
            }
        }

        protected override NodeStates OnUpdate()
        {
            if (!child)
                return NodeStates.Failure;

            if (infiniteLoop == false)
            {
                switch (child.UpdateNode())
                {
                    case NodeStates.Running:
                        return NodeStates.Running;
                    case NodeStates.Failure:
                        if (shouldLoopOnFail)
                            return NodeStates.Running;
                        return NodeStates.Failure;
                    case NodeStates.Success:
                        {
                            if (currentRepeatCount < repeatCount)
                            {
                                ++currentRepeatCount;
                                return NodeStates.Running;
                            }
                            break;
                        }
                }
                return NodeStates.Success;
            }
            else
            {
                child.UpdateNode();
                return NodeStates.Running;
            }
        }

        protected override void OnEnd() { }
    }
}
