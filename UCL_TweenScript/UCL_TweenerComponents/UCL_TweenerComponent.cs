using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public enum TC_Type {
        TweenerComponent = 0,
        Transform,
        Move,
        Rotate,
        LookAt,
        Curve,
        Action
    }
    /// <summary>
    ///UCL_TweenerComponent extened action on tweener
    /// </summary>
    public class UCL_TweenerComponent {
        #region Create
        virtual protected TC_Type GetTC_Type() { return TC_Type.TweenerComponent; }
        public static UCL_TweenerComponent Create(TC_Type type) {
            UCL_TweenerComponent tc = null;
            switch(type) {
                case TC_Type.Transform: {
                        tc = new UCL_TC_Transform();
                        break;
                    }
                case TC_Type.Move: {
                        tc = new UCL_TC_Move();
                        break;
                    }
                case TC_Type.Rotate: {
                        tc = new UCL_TC_Rotate();
                        break;
                    }
                case TC_Type.LookAt: {
                        tc = new UCL_TC_LookAt();
                        break;
                    }
                case TC_Type.Curve: {
                        tc = new UCL_TC_Curve();
                        break;
                    }
                case TC_Type.Action: {
                        tc = new UCL_TC_Action();
                        break;
                    }
            }
            if(tc == null) tc = new UCL_TweenerComponent();
            return tc;
        }
        #endregion
#if UNITY_EDITOR
        virtual public void OnInspectorGUI(byte[] data) {
            //UnityEditor.SerializedProperty sp = new UnityEditor.SerializedProperty();
            //sp.propertyType = m_Reverse.t
            //UnityEditor.EditorGUILayout.PropertyField(m_Reverse)
            if(data.Length > 0) {
                m_Reverse = (data[0] == 1);
            }
            m_Reverse = GUILayout.Toggle(m_Reverse, "Reverse");
            //Debug.LogWarning("m_Reverse:" + m_Reverse);
            data = new byte[1];
            data[0] = (byte)(m_Reverse? 0:1);
        }
#endif
        protected bool m_Reverse = false;

        virtual protected internal void Init() { }
        virtual protected internal void Start() { }
        virtual protected internal void Complete() { }

        virtual protected void ComponentUpdate(float pos) { }


        internal void Update(float pos) {
            if(m_Reverse) pos = 1 - pos;
            ComponentUpdate(pos);
        }

        public UCL_TweenerComponent SetReverse(bool val) {
            m_Reverse = val;
            return this;
        }
    }
}