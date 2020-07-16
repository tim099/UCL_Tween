using System.Collections;
using System.Collections.Generic;
using UCL.Core.MathLib;
using UnityEngine;

namespace UCL.TweenLib {
    /// <summary>
    /// you can add new supported type by add new List to UCL_TC_Data
    /// Example
    /// to support type "System.Int64"
    /// just add a List of System.Int64 name m_Int64 (public List<System.Int64> m_Int64;)
    /// </summary>
    [System.Serializable]
    public class UCL_TC_Data {
        public UCL_TweenerComponent CreateTweenerComponent() {
            var tc = UCL_TweenerComponent.Create(m_Type);
            tc.LoadData(this);
            return tc;
        }

        public TC_Type m_Type = TC_Type.Move;
        public List<Transform> m_Transform;
        public List<Quaternion> m_Quaternion;
        public List<UCL_Curve> m_UCL_Curve;
        public List<UCL_TC_Curve.LookAtFront> m_LookAtFront;
        public List<UCL_TC_Event> m_UCL_TC_Event;

        //public List<Vector4> m_Vector4;
        public List<Vector3> m_Vector3;
        public List<Vector2> m_Vector2;
        //public List<Vector3Int> m_Vector3Int;
        //public List<Vector2Int> m_Vector2Int;

        public List<System.Boolean> m_Boolean;
        public List<System.Byte> m_Byte;
        //public List<System.Int16> m_Int16;
        public List<System.Int32> m_Int32;
        //public List<System.Int64> m_Int64;

        public List<System.Single> m_Single;
        //public List<System.Double> m_Double;

        [HideInInspector] public bool m_Foldout = false;
        //public byte[] m_Data;

    }
}