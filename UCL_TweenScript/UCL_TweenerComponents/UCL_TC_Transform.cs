using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TC_Transform : UCL_TweenerComponent {
        //[Tooltip("Target that tween component move")]
        //[Header("Move target")]
        protected Transform m_Target;

        //[Header("Target position")]
        protected Transform m_TargetTransform;
        protected bool m_Local = false;
        #region EDITOR
#if UNITY_EDITOR
        public override string OnInspectorGUITips() {
            var tips = base.OnInspectorGUITips();
            tips += "\"Target\" is the move target of TweenerComponent\n";
            tips += "\"TargetTransform\" is target position that \"Target\" will move to\n";
            return tips;
        }
        override public void OnInspectorGUIBasic(UCL_TC_Data tc_data, UnityEditor.SerializedProperty sdata, Transform TB_transform) {
            base.OnInspectorGUIBasic(tc_data, sdata, TB_transform);
            //UnityEditor.EditorGUILayout.PropertyField(sdata.FindPropertyRelative("m_Type"));//,new GUIContent("Test")
            UnityEditor.SerializedProperty t_datas = sdata.FindPropertyRelative("m_Transform");
            if(TB_transform != null && t_datas != null) {
                if(t_datas.arraySize == 0) {
                    t_datas.InsertArrayElementAtIndex(0);
                    t_datas.InsertArrayElementAtIndex(1);
                    t_datas.GetArrayElementAtIndex(0).objectReferenceValue = TB_transform;
                }
            }
            /*
            if(tc_data.m_Transform == null) {
                tc_data.m_Transform = new List<Transform>();
            }
            if(tc_data.m_Transform.Count == 0) {
                tc_data.m_Transform.Add(TB_transform);
            }
            */
        }
#endif
        #endregion
        public UCL_TC_Transform SetTarget(Transform _Target) {
            m_Target = _Target;
            return this;
        }
        public UCL_TC_Transform SetTargetTransform(Transform _TargetTransform) {
            m_TargetTransform = _TargetTransform;
            return this;
        }
        public UCL_TC_Transform SetLocal(bool local) {
            m_Local = local;
            return this;
        }
    }
}