using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace UCL.TweenLib {
    [System.Serializable] public class UCL_TC_Event : UnityEvent<float> { }

    public class UCL_TC_Action : UCL_TweenerComponent {
#if UNITY_EDITOR
        public override bool OnInspectorGUI(UCL_TC_Data tc_data, SerializedProperty sdata) {
            var event_data = sdata.FindPropertyRelative("m_UCL_TC_Event");
            if(event_data.arraySize == 0) {
                event_data.InsertArrayElementAtIndex(0);
            }
            UnityEditor.EditorGUILayout.PropertyField(event_data.GetArrayElementAtIndex(0));
            return true;
        }
#endif
        /// <summary>
        /// override to avoid using reflection on IOS
        /// </summary>
        /// <param name="data"></param>
        protected internal override void LoadData(UCL_TC_Data data) {
            if(data.m_UCL_TC_Event.Count > 0) {
                m_Event = data.m_UCL_TC_Event[0];
            }
        }
        override public TC_Type GetTC_Type() { return TC_Type.Action; }
        public static UCL_TC_Action Create() {
            return new UCL_TC_Action();
        }

        public UCL_TC_Event m_Event;
        protected System.Action<float> m_Act;

        public UCL_TC_Action Init(System.Action<float> act) {
            m_Act = act;
            return this;
        }
        public UCL_TC_Action Add(UnityAction<float> act) {
            if(m_Event == null) {
                m_Event = new UCL_TC_Event();
            }
            m_Event.AddListener(act);
            return this;
        }
        protected override void ComponentUpdate(float pos) {
            if(m_Act != null) {
                m_Act.Invoke(pos);
            }
            if(m_Event != null) {
                m_Event.Invoke(pos);
            }
        }
    }
}