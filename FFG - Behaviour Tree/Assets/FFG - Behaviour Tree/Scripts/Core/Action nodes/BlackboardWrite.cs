using UnityEngine;


namespace FFG
{
    public class BlackboardWrite : ActionNode
    {
        [HideInInspector][SerializeField] public string key;
        [HideInInspector][SerializeField] public Blackboard.BlackboardData.BlackboardDataType dataType;
        [HideInInspector][SerializeField] public bool boolData;
        [HideInInspector][SerializeField] public int intData;
        [HideInInspector][SerializeField] public string stringData;
        [HideInInspector][SerializeField] public float floatData;
        [HideInInspector][SerializeField] public double doubleData;
        [HideInInspector][SerializeField] public Vector2 vector2Data;
        [HideInInspector][SerializeField] public Vector3 vector3Data;
        [HideInInspector][SerializeField] public Vector4 vector4Data;
        [HideInInspector][SerializeField] public Quaternion quatData;
        [HideInInspector][SerializeField] public Rect rectData;
        [HideInInspector][SerializeField] public Object objectData;
        [HideInInspector][SerializeField] public Color colorData;


        protected override void OnStart() { }

        protected override void OnEnd() { }

        protected override NodeStates OnUpdate()
        {
            if (blackBoard.GetData(key, out Blackboard.BlackboardData data) == false || data.dataType != dataType)
                return NodeStates.Failure;

            switch (dataType)
            {
                case Blackboard.BlackboardData.BlackboardDataType.boolData:
                    data.boolData = boolData;
                    break;
                case Blackboard.BlackboardData.BlackboardDataType.intData:
                    data.intData = intData;
                    break;
                case Blackboard.BlackboardData.BlackboardDataType.stringData:

                case Blackboard.BlackboardData.BlackboardDataType.floatData:

                case Blackboard.BlackboardData.BlackboardDataType.doubleData:

                case Blackboard.BlackboardData.BlackboardDataType.vector2Data:

                case Blackboard.BlackboardData.BlackboardDataType.vector3Data:

                case Blackboard.BlackboardData.BlackboardDataType.vector4Data:

                case Blackboard.BlackboardData.BlackboardDataType.quaternionData:

                case Blackboard.BlackboardData.BlackboardDataType.rectData:

                case Blackboard.BlackboardData.BlackboardDataType.objectData:

                case Blackboard.BlackboardData.BlackboardDataType.colorData:
                    break;
            }
            return NodeStates.Success;
        }
    }
}
