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
        public const DataVersion CurVersion = DataVersion.Ver2;
        public enum DataVersion
        {
            InitVer = 0,
            Ver1,
            Ver2,
        }

        static public UCL_TC_Data Create() {
            var data = new UCL_TC_Data();
            data.m_Foldout = true;
            return data;
        } 
        public UCL_TweenerComponent CreateTweenerComponent() {
            var aTC = UCL_TweenerComponent.Create(m_Type);
            aTC.LoadData(this);
            return aTC;
        }
        /// <summary>
        /// Update data version
        /// </summary>
        /// <param name="iUpdateAct"></param>
        /// <returns>return true if Version updated</returns>
        public bool UpdateVersion(System.Action<DataVersion, UCL_TC_Data> iUpdateAct)
        {
            if (m_Version == DataVersion.InitVer)//Init first
            {
                
                if(m_Transform != null && m_Transform.Count > 0)
                {//Ver1 is the first version and don't have m_Version record, so check the m_Transform to detect
                    m_Version = DataVersion.Ver1;
                }
                else
                {
                    m_Version = CurVersion;
                }
            }

            if (m_Version == CurVersion)//Newest Version
            {
                return false;
            }

            switch (m_Version)
            {
                case DataVersion.Ver1:
                    {//Update to Ver2
                        iUpdateAct(m_Version, this);
                        m_Version = DataVersion.Ver2;
                        UpdateVersion(iUpdateAct);//Check if newer version exist
                        return true;
                    }
            }
            return false;
        }
        /// <summary>
        /// Init data
        /// </summary>
        public void Init()
        {
            m_Version = DataVersion.InitVer;
            m_Transform.Clear();
            m_RectTransform.Clear();
            m_Quaternion.Clear();
            m_UCL_Path.Clear();
            m_LookAtFront.Clear();
            m_UCL_TC_Event.Clear();

            m_Vector3.Clear();
            m_Vector2.Clear();
            m_Boolean.Clear();
            m_Byte.Clear();
            m_Int32.Clear();
            m_Single.Clear();
        }
        public TC_Type m_Type = TC_Type.Move;
        public List<Transform> m_Transform;
        public List<RectTransform> m_RectTransform;
        public List<Quaternion> m_Quaternion;
        public List<UCL_Path> m_UCL_Path;
        public List<UCL_TC_Curve.LookAtFront> m_LookAtFront;
        public List<UCL_TC_Event> m_UCL_TC_Event;


        public List<Vector3> m_Vector3;
        public List<Vector2> m_Vector2;
        public List<System.Boolean> m_Boolean;
        public List<System.Byte> m_Byte;
        public List<System.Int32> m_Int32;
        public List<System.Single> m_Single;
        public DataVersion m_Version = DataVersion.InitVer;

        [HideInInspector] public bool m_Foldout = false;

        //public List<Vector4> m_Vector4;
        //public List<Vector3Int> m_Vector3Int;
        //public List<Vector2Int> m_Vector2Int;


        //public List<System.Int16> m_Int16;

        //public List<System.Int64> m_Int64;


        //public List<System.Double> m_Double;


        //public List<string> m_FieldNames = new List<string>();


        //public byte[] m_Data;

    }
}