using UnityEditor;
using UnityEngine;


namespace FFG._Editor
{

    [CustomEditor(typeof(BlackboardWrite))]
    public class BlackboardWriteEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            SerializedProperty key = serializedObject.FindProperty("key");
            SerializedProperty dataType = serializedObject.FindProperty("dataType");

            SerializedProperty boolData = serializedObject.FindProperty("boolData");
            SerializedProperty intData = serializedObject.FindProperty("intData");
            SerializedProperty stringData = serializedObject.FindProperty("stringData");
            SerializedProperty floatData = serializedObject.FindProperty("floatData");
            SerializedProperty doubleData = serializedObject.FindProperty("doubleData");
            SerializedProperty vector2Data = serializedObject.FindProperty("vector2Data");
            SerializedProperty vector3Data = serializedObject.FindProperty("vector3Data");
            SerializedProperty vector4Data = serializedObject.FindProperty("vector4Data");
            SerializedProperty quatData = serializedObject.FindProperty("quatData");
            SerializedProperty rectData = serializedObject.FindProperty("rectData");
            SerializedProperty objectData = serializedObject.FindProperty("objectData");
            SerializedProperty colorData = serializedObject.FindProperty("colorData");

            EditorGUILayout.PropertyField(key, new GUIContent("Target key : "));
            EditorGUILayout.PropertyField(dataType, new GUIContent("Data type : "));

            switch ((Blackboard.BlackboardData.BlackboardDataType)dataType.enumValueIndex)
            {
                case Blackboard.BlackboardData.BlackboardDataType.boolData:
                    EditorGUILayout.PropertyField(boolData, new GUIContent("Bool data : "));
                    break;
                case Blackboard.BlackboardData.BlackboardDataType.intData:
                    EditorGUILayout.PropertyField(intData, new GUIContent("Int data : "));
                    break;
                case Blackboard.BlackboardData.BlackboardDataType.stringData:
                    EditorGUILayout.PropertyField(stringData, new GUIContent("String data : "));
                    break;
                case Blackboard.BlackboardData.BlackboardDataType.floatData:
                    EditorGUILayout.PropertyField(floatData, new GUIContent("Float data : "));
                    break;
                case Blackboard.BlackboardData.BlackboardDataType.doubleData:
                    EditorGUILayout.PropertyField(doubleData, new GUIContent("Double data : "));
                    break;
                case Blackboard.BlackboardData.BlackboardDataType.vector2Data:
                    EditorGUILayout.PropertyField(vector2Data, new GUIContent("Vector 2 data : "));
                    break;
                case Blackboard.BlackboardData.BlackboardDataType.vector3Data:
                    EditorGUILayout.PropertyField(vector3Data, new GUIContent("Vector 3 data : "));
                    break;
                case Blackboard.BlackboardData.BlackboardDataType.vector4Data:
                    EditorGUILayout.PropertyField(vector4Data, new GUIContent("Vector 4 data : "));
                    break;
                case Blackboard.BlackboardData.BlackboardDataType.quaternionData:
                    EditorGUILayout.PropertyField(quatData, new GUIContent("Quaternion data : "));
                    break;
                case Blackboard.BlackboardData.BlackboardDataType.rectData:
                    EditorGUILayout.PropertyField(rectData, new GUIContent("Rect data : "));
                    break;
                case Blackboard.BlackboardData.BlackboardDataType.objectData:
                    EditorGUILayout.PropertyField(objectData, new GUIContent("Object data : "));
                    break;
                case Blackboard.BlackboardData.BlackboardDataType.colorData:
                    EditorGUILayout.PropertyField(colorData, new GUIContent("Color data : "));
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
