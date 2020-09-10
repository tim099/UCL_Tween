using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public static partial class Extension {
        static public UCL_TC_Curve TC_Move(this Transform target, Core.MathLib.UCL_Path target_val) {
            return UCL_TC_Curve.Create().Init(target, target_val);
        }
    }
    public class UCL_TC_Curve : UCL_TC_Transform {
#if UNITY_EDITOR
        public override bool OnInspectorGUI(UCL_TC_Data tc_data, UnityEditor.SerializedProperty sdata) {
            {
                var data = sdata.FindPropertyRelative("m_UCL_Path");
                if(data.arraySize == 0) {
                    data.InsertArrayElementAtIndex(0);
                }
                UnityEditor.EditorGUILayout.PropertyField(data.GetArrayElementAtIndex(0), new GUIContent("Path"));
            }
            {
                var data = sdata.FindPropertyRelative("m_Transform");
                if(data.arraySize == 0) {
                    data.InsertArrayElementAtIndex(0);
                }
                UnityEditor.EditorGUILayout.PropertyField(data.GetArrayElementAtIndex(0), new GUIContent("Target"));
            }
            {
                var data = sdata.FindPropertyRelative("m_LookAtFront");
                if(data.arraySize == 0) {
                    data.InsertArrayElementAtIndex(0);
                }
                UnityEditor.EditorGUILayout.PropertyField(data.GetArrayElementAtIndex(0), new GUIContent("LookAtFront"));
            }
            return true;
        }
#endif
        /// <summary>
        /// override to avoid using reflection on IOS
        /// </summary>
        /// <param name="data"></param>
        protected internal override void LoadData(UCL_TC_Data data) {
            if(data.m_UCL_Path.Count > 0) {
                m_Path = data.m_UCL_Path[0];
            }
            if(data.m_Transform.Count > 0) {
                m_Target = data.m_Transform[0];
            }
            if(data.m_LookAtFront.Count > 0) {
                m_LookAtFront = data.m_LookAtFront[0];
            }
        }
        override public TC_Type GetTC_Type() { return TC_Type.Curve; }
        [System.Serializable]
        public class LookAtFront {
            public Vector3 m_Up;

            public Vector3 m_Rot;
            public bool m_DoRot = false;

            [Header("won't change the rotation if not active")] public bool m_Active = true;
        }

        public static UCL_TC_Curve Create() {
            return new UCL_TC_Curve();
        }

        protected Core.MathLib.UCL_Path m_Path;
        protected LookAtFront m_LookAtFront = null;

        virtual public UCL_TC_Curve Init(Transform target, Core.MathLib.UCL_Path _path) {
            m_Path = _path;
            m_Target = target;
            return this;
        }

        virtual public UCL_TC_Curve SetLookAtFront(bool val, Vector3? up = null, Vector3? rot = null) {
            if(val) {
                m_LookAtFront = new LookAtFront();
                if(up.HasValue) m_LookAtFront.m_Up = up.Value;
                else m_LookAtFront.m_Up = Vector3.up;
                if(rot.HasValue) {
                    m_LookAtFront.m_DoRot = true;
                    m_LookAtFront.m_Rot = rot.Value;
                }
            } else {
                m_LookAtFront = null;
            }

            return this;
        }
        protected override void ComponentUpdate(float pos) {
            var cur_pos = m_Path.GetPos(pos);
            m_Target.transform.position = cur_pos;
            if(m_LookAtFront != null && m_LookAtFront.m_Active) {
                const float ndel = 0.0005f;
                var next_pos = m_Path.GetPos(pos + ndel);
                var del = next_pos - cur_pos;
                if(del.magnitude > 0) {
                    var rot = Quaternion.LookRotation(del, m_LookAtFront.m_Up);

                    if(m_LookAtFront.m_DoRot) {
                        rot *= Quaternion.Euler(m_LookAtFront.m_Rot);
                    }
                    m_Target.transform.rotation = rot;
                }
            }
        }

    }
}