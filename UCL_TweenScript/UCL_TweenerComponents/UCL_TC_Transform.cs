using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TC_Transform : UCL_TweenerComponent {
        override public string Name {
            get {
                string name = this.GetType().Name.Replace("UCL_TC_", string.Empty);
                if(m_Target != null) {
                    name += "[" + m_Target.name + "]";
                }
                return name;
            }
        }
        override public Transform GetTarget() { return m_Target; }
        //[Tooltip("Target that tween component move")]
        //[Header("Move target")]
        protected Transform m_Target;

        //[Header("Target position")]
        /// <summary>
        /// Target Transform that target will move to
        /// </summary>
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
        override public void OnInspectorGUIBasic(UCL_TC_Data iTcData, UnityEditor.SerializedProperty iSerializedProperty, Transform iTransform) {
            base.OnInspectorGUIBasic(iTcData, iSerializedProperty, iTransform);
            UnityEditor.SerializedProperty aTransformDatas = iSerializedProperty.FindPropertyRelative("m_Transform");
            if(iTransform != null && aTransformDatas != null) {
                if(aTransformDatas.arraySize == 0) {//Init
                    aTransformDatas.InsertArrayElementAtIndex(0);
                    aTransformDatas.InsertArrayElementAtIndex(1);
                    aTransformDatas.GetArrayElementAtIndex(0).objectReferenceValue = iTransform;
                }
            }
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
#if UNITY_EDITOR
        //UnityEditor.SerializedProperty m_TargetProperty = null;
        override internal void OnInspectorGUI() {
            //if(m_TargetProperty == null) {
            //    UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
            //    m_TargetProperty = serializedObject.FindProperty("m_Target");//new UnityEditor.SerializedProperty(m_Target);
            //}

            GUILayout.BeginVertical();
            m_Target = UnityEditor.EditorGUILayout.ObjectField("Target", m_Target, m_Target.GetType(), true) as Transform;
            //UnityEditor.EditorGUILayout.PropertyField(m_TargetProperty);
            GUILayout.EndVertical();
        }
        /// <summary>
        /// Called when being selected
        /// </summary>
        override internal void OnSelected() {
            //if(m_Target != null) UnityEditor.Selection.activeObject = m_Target;
        }
#endif
    }
}