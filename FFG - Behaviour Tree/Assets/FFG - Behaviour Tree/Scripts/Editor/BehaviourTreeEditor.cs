using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using System.Collections.Generic;


namespace FFG._Editor
{
    public class BehaviourTreeEditor : EditorWindow
    {
        static Vector2 pos = new Vector2();
        BehaviourTreeGraphView treeView;
        InspectorView inspectorView;
        IMGUIContainer blackboardView;
        ToolbarMenu toolbarMenu;

        SerializedObject treeObject;
        SerializedProperty blackboardProperty;

        [MenuItem("FFG/Behaviour Tree/Open graph editor...")]
        public static void OpenWindow()
        {
            // For opening window
            BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
            wnd.titleContent = new GUIContent("Graph view");
            wnd.minSize = new Vector2(1400, 900);
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int lineNumber)
        {
            // Open editor window when you double click the behaviour tree asset
            BehaviourTreeAsset tree = Selection.activeObject as BehaviourTreeAsset;
            if (tree)
            {
                OpenWindow();
                return true;
            }
            return false;
        }

        public void CreateGUI()
        {
            // Get the root visual element
            VisualElement root = rootVisualElement;

            // Import Uxml
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/FFG - Behaviour Tree/Scripts/UIBuilder/BehaviourTreeEditor.uxml");
            visualTree.CloneTree(root);
            
            // Style sheet for all the children
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/FFG - Behaviour Tree/Scripts/UIBuilder/BehaviourTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            // Main tree view
            treeView = root.Q<BehaviourTreeGraphView>();
            treeView.onNodeSelected = onNodeSelectionChanged;

            // Inspector view
            inspectorView = root.Q<InspectorView>();

            // Blackboard view
            blackboardView = root.Q<IMGUIContainer>();
            blackboardView.onGUIHandler = () =>
            {
                if (treeObject != null && treeObject.targetObject)
                {
                    treeObject.Update();
                    blackboardCustomInspector(blackboardProperty);
                    treeObject.ApplyModifiedProperties();
                }
            };

            // Toolbar asset menu
            toolbarMenu = root.Q<ToolbarMenu>();
            var behaviourTrees = loadAssets<BehaviourTreeAsset>();
            behaviourTrees.ForEach(tree => {
                toolbarMenu.menu.AppendAction($"{tree.name}", (a) => {
                    Selection.activeObject = tree;
                });
            });

            OnSelectionChange();
        }

        private void OnInspectorUpdate()
        {
            // To update the node views while in playmode to see runtime outputs
            treeView?.UpdateNodeStates();
        }

        private void OnSelectionChange()
        {
            BehaviourTreeAsset tree = Selection.activeObject as BehaviourTreeAsset;

            // Check if the actively selected game object has tree asset
            if (!tree && Selection.activeGameObject)
            {
                BehaviourTreeHandler handler = Selection.activeGameObject.GetComponent<BehaviourTreeHandler>();
                if (handler)
                    tree = handler.tree;
            }

            // Populate tree view in runtime
            if (tree && Application.isPlaying)
                treeView?.PopulateView(tree);

            // Populate tree view in editor mode
            else if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
                treeView?.PopulateView(tree);

            // Initialize serialized object and serilized property for blackboard
            if (tree)
            {
                treeObject = new SerializedObject(tree);
                blackboardProperty = treeObject.FindProperty("blackboard");
            }
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= onPlayModeStateChanged;
            EditorApplication.playModeStateChanged += onPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= onPlayModeStateChanged;
        }

        private void onPlayModeStateChanged(PlayModeStateChange ch)
        {
            // Tree view update
            switch (ch)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }

        private void onNodeSelectionChanged(NodeView nodeView)
        {
            // Update inspector view when a node is selected
            inspectorView.UpdateSelection(nodeView);
        }

        private void blackboardCustomInspector(SerializedProperty property)
        {
            // Custom inspector for blackboard
            SerializedProperty containerArray = property.FindPropertyRelative("container");
            SerializedProperty container(int index) => containerArray.GetArrayElementAtIndex(index);
            SerializedProperty key(int index) => container(index).FindPropertyRelative("key");
            SerializedProperty dataType(int index) => container(index).FindPropertyRelative("dataType");
            SerializedProperty isReadOnly(int index) => container(index).FindPropertyRelative("isReadOnly");

            SerializedProperty boolData(int index) => container(index).FindPropertyRelative("_boolData");
            SerializedProperty intData(int index) => container(index).FindPropertyRelative("_intData");
            SerializedProperty stringData(int index) => container(index).FindPropertyRelative("_stringData");
            SerializedProperty floatData(int index) => container(index).FindPropertyRelative("_floatData");
            SerializedProperty doubleData(int index) => container(index).FindPropertyRelative("_doubleData");
            SerializedProperty vector2Data(int index) => container(index).FindPropertyRelative("_vector2Data");
            SerializedProperty vector3Data(int index) => container(index).FindPropertyRelative("_vector3Data");
            SerializedProperty vector4Data(int index) => container(index).FindPropertyRelative("_vector4Data");
            SerializedProperty quatData(int index) => container(index).FindPropertyRelative("_quatData");
            SerializedProperty rectData(int index) => container(index).FindPropertyRelative("_rectData");
            SerializedProperty objectData(int index) => container(index).FindPropertyRelative("_objectData");
            SerializedProperty colorData(int index) => container(index).FindPropertyRelative("_colorData");


            space(3);
            heading("Blackboard Keys");
            space(3);

            if (EditorApplication.isPlaying == false)
            {
                beginHorizontal();
                label("Add or remove keys");
                if (button("+", 17.5f, 17.5f))
                {
                    containerArray.InsertArrayElementAtIndex(containerArray.arraySize);
                }
                if (button("-", 17.5f, 17.5f))
                {
                    if (containerArray.arraySize > 0)
                        containerArray.GetArrayElementAtIndex(containerArray.arraySize - 1).DeleteCommand();
                }
                endHorizontal();

                beginHorizontal();
                label("Clear all keys");
                if (button("x", 17.5f, 17.5f))
                {
                    containerArray.ClearArray();
                    return;
                }
                endHorizontal();
                space(8);
            }

            pos = EditorGUILayout.BeginScrollView(pos, false, false);

            for (int i = 0; i < containerArray.arraySize; ++i)
            {
                container(i).isExpanded = EditorGUILayout.Foldout(container(i).isExpanded, $"{key(i).stringValue}");

                if (container(i).isExpanded)
                {
                    EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
                    propertyField(key(i), "Key : ", "This will be the key used to get a specific data");

                    string k = key(i).stringValue;
                    if (!string.IsNullOrEmpty(k))
                        for (int j = 0; j < containerArray.arraySize; ++j)
                            if (j != i && key(j).stringValue == k)
                            {
                                Debug.LogWarning($"Duplicate keys found! {k}");
                                key(i).stringValue = $"{k + "_duplicate"}";
                            }

                    propertyField(dataType(i), "Data type : ", "The type of data this key should store");
                    propertyField(isReadOnly(i), "Is editor readonly : ", "Set this as true if this key shouldn't be modified by other nodes");

                    switch ((Blackboard.BlackboardData.BlackboardDataType)dataType(i).enumValueIndex)
                    {
                        case Blackboard.BlackboardData.BlackboardDataType.boolData:
                            propertyField(boolData(i), "Bool data : ", "");
                            break;
                        case Blackboard.BlackboardData.BlackboardDataType.intData:
                            propertyField(intData(i), "Int data : ", "");
                            break;
                        case Blackboard.BlackboardData.BlackboardDataType.stringData:
                            propertyField(stringData(i), "String data : ", "");
                            break;
                        case Blackboard.BlackboardData.BlackboardDataType.floatData:
                            propertyField(floatData(i), "Float data : ", "");
                            break;
                        case Blackboard.BlackboardData.BlackboardDataType.doubleData:
                            propertyField(doubleData(i), "Double data : ", "");
                            break;
                        case Blackboard.BlackboardData.BlackboardDataType.vector2Data:
                            propertyField(vector2Data(i), "Vector2 data : ", "");
                            break;
                        case Blackboard.BlackboardData.BlackboardDataType.vector3Data:
                            propertyField(vector3Data(i), "Vector3 data : ", "");
                            break;
                        case Blackboard.BlackboardData.BlackboardDataType.vector4Data:
                            propertyField(vector4Data(i), "Vector4 data : ", "");
                            break;
                        case Blackboard.BlackboardData.BlackboardDataType.quaternionData:
                            propertyField(quatData(i), "Quaternion data : ", "");
                            break;
                        case Blackboard.BlackboardData.BlackboardDataType.rectData:
                            propertyField(rectData(i), "Rect data : ", "");
                            break;
                        case Blackboard.BlackboardData.BlackboardDataType.objectData:
                            propertyField(objectData(i), "Object data : ", "");
                            break;
                        case Blackboard.BlackboardData.BlackboardDataType.colorData:
                            propertyField(colorData(i), "Color data : ", "");
                            break;
                    }
                    EditorGUI.EndDisabledGroup();
                }

                if (EditorApplication.isPlaying == false)
                {
                    beginHorizontal();
                    if (!Application.isPlaying && button("+", 17.5f, 17.5f))
                    {
                        containerArray.InsertArrayElementAtIndex(i + 1);
                        break;
                    }
                    if (!Application.isPlaying && button("-", 17.5f, 17.5f))
                    {
                        containerArray.GetArrayElementAtIndex(i).DeleteCommand();
                        break;
                    }
                    endHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void propertyField(SerializedProperty property, string propertyName, string tooltip)
            => EditorGUILayout.PropertyField(property, new GUIContent(propertyName, tooltip));

        private void space(float val)
            => GUILayout.Space(val);

        private void heading(string label)
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            EditorGUILayout.LabelField(label, style, GUILayout.ExpandWidth(true));
        }

        private bool button(string content, float height, float width)
            => GUILayout.Button(content, GUILayout.Height(height), GUILayout.Width(width));

        private void beginHorizontal()
            => EditorGUILayout.BeginHorizontal();

        private void endHorizontal()
            => EditorGUILayout.EndHorizontal();

        private void label(string labelContent)
            => EditorGUILayout.LabelField(labelContent);

        private List<T> loadAssets<T>() where T : UnityEngine.Object
        {
            string[] assetIds = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            List<T> assets = new List<T>();
            foreach (var assetId in assetIds)
            {
                string path = AssetDatabase.GUIDToAssetPath(assetId);
                T asset = AssetDatabase.LoadAssetAtPath<T>(path);
                assets.Add(asset);
            }
            return assets;
        }
    }
}