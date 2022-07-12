using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace FFG
{
    [System.Serializable]
    public class Blackboard
    {
        [System.Serializable]
        public class BlackboardData
        {
            public enum BlackboardDataType
            {
                boolData,
                intData,
                stringData,
                floatData,
                doubleData,
                vector2Data,
                vector3Data,
                vector4Data,
                quaternionData,
                rectData,
                objectData,
                colorData
            }

            public string key;
            public BlackboardDataType dataType;
            public bool isReadOnly = false;

            [SerializeField] private bool _boolData;
            [SerializeField] private int _intData;
            [SerializeField] private string _stringData;
            [SerializeField] private float _floatData;
            [SerializeField] private double _doubleData;
            [SerializeField] private Vector2 _vector2Data;
            [SerializeField] private Vector3 _vector3Data;
            [SerializeField] private Vector4 _vector4Data;
            [SerializeField] private Quaternion _quatData;
            [SerializeField] private Rect _rectData;
            [SerializeField] private Object _objectData;
            [SerializeField] private Color _colorData;

            public bool boolData
            {
                get
                {
                    if (dataType == BlackboardDataType.boolData)
                        return _boolData;
                    else
                        return default;
                }
                set
                {
                    if (!isReadOnly)
                        _boolData = value;
                }
            }
            public int intData
            {
                get
                {
                    if (dataType == BlackboardDataType.intData)
                        return _intData;
                    else
                        return default;
                }
                set
                {
                    if (!isReadOnly)
                        _intData = value;
                }
            }
            public string stringData
            {
                get
                {
                    if (dataType == BlackboardDataType.stringData)
                        return _stringData;
                    else
                        return default;
                }
                set
                {
                    if (!isReadOnly)
                        _stringData = value;
                }
            }
            public float floatData
            {
                get
                {
                    if (dataType == BlackboardDataType.floatData)
                        return _floatData;
                    else
                        return default;
                }
                set
                {
                    if (!isReadOnly)
                        _floatData = value;
                }
            }
            public double doubleData
            {
                get
                {
                    if (dataType == BlackboardDataType.doubleData)
                        return _doubleData;
                    else
                        return default;
                }
                set
                {
                    if (!isReadOnly)
                        _doubleData = value;
                }
            }
            public Vector2 vector2Data
            {
                get
                {
                    if (dataType == BlackboardDataType.vector2Data)
                        return _vector2Data;
                    else
                        return default;
                }
                set
                {
                    if (!isReadOnly)
                        _vector2Data = value;
                }
            }
            public Vector3 vector3Data
            {
                get
                {
                    if (dataType == BlackboardDataType.vector3Data)
                        return _vector3Data;
                    else
                        return default;
                }
                set
                {
                    if (!isReadOnly)
                        _vector3Data = value;
                }
            }
            public Vector4 vector4Data
            {
                get
                {
                    if (dataType == BlackboardDataType.vector4Data)
                        return _vector4Data;
                    else
                        return default;
                }
                set
                {
                    if (!isReadOnly)
                        _vector4Data = value;
                }
            }
            public Quaternion quatData
            {
                get
                {
                    if (dataType == BlackboardDataType.quaternionData)
                        return _quatData;
                    else
                        return default;
                }
                set
                {
                    if (!isReadOnly)
                        _quatData = value;
                }
            }
            public Rect rectData
            {
                get
                {
                    if (dataType == BlackboardDataType.rectData)
                        return _rectData;
                    else
                        return default;
                }
                set
                {
                    if (!isReadOnly)
                        _rectData = value;
                }
            }
            public Object objectData
            {
                get
                {
                    if (dataType == BlackboardDataType.objectData)
                        return _objectData;
                    else
                        return default;
                }
                set
                {
                    if (!isReadOnly)
                        _objectData = value;
                }
            }
            public Color colorData
            {
                get
                {
                    if (dataType == BlackboardDataType.colorData)
                        return _colorData;
                    else
                        return default;
                }
                set
                {
                    if (!isReadOnly)
                        _colorData = value;
                }
            }
        }


        [SerializeField]
        private BlackboardData[] container;

        public bool GetData(string key, out BlackboardData data)
        {
            data = null;
            data = container.ToList().Find((c) => c.key == key);
            return data != null;
        }
    }
}