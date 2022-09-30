using UnityEditor;
using UnityEngine;


namespace Lacobus.BehaviourTreeEditor
{
    [CustomEditor(typeof(RepeatFor))]
    public class RepeatForEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty repeatCount = serializedObject.FindProperty("repeatCount");
            SerializedProperty shouldLoopOnFail = serializedObject.FindProperty("shouldLoopOnFail");
            SerializedProperty infiniteLoop = serializedObject.FindProperty("infiniteLoop");

            EditorGUILayout.PropertyField(infiniteLoop, new GUIContent("Should loop endlessly? ", "Set this as true if you want this node to loop endlessly"));

            bool infLoop = infiniteLoop.boolValue;

            if (!infLoop)
                EditorGUILayout.PropertyField(repeatCount, new GUIContent("Repeat count : ", "Total number of loops this node should do"));

            shouldLoopOnFail.boolValue = EditorGUILayout.Toggle("Should loop on failure", shouldLoopOnFail.boolValue);
        }
    }
}
