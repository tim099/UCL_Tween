using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UCL.TweenLib {
    public enum TC_Type {
        TweenerComponent = 0,
        //Transform,
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
                /*
                case TC_Type.Transform: {
                        tc = new UCL_TC_Transform();
                        break;
                    }
                    */
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
        /// <summary>
        /// return true if data altered
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        virtual public bool OnInspectorGUI(UCL_TC_Data tc_data, UnityEditor.SerializedProperty sdata) {
            var type = this.GetType();
            var data = tc_data.m_Data;
            List<string> TransformNames = new List<string>();
            FieldInfo[] fieldInfos1 = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance);
            Dictionary<System.Type, List<string>> m_Names = new Dictionary<System.Type, List<string>>();
            System.Action<FieldInfo> draw_fieldinfo = delegate (FieldInfo info) {
                var value = info.GetValue(this);
                System.Type info_type = info.FieldType;
                if(!m_Names.ContainsKey(info_type)) {
                    m_Names.Add(info_type, new List<string>());
                }
                m_Names[info_type].Add(info.Name);
            };

            for(int i = 0; i < fieldInfos1.Length; i++) {
                draw_fieldinfo(fieldInfos1[i]);
            }
            System.Action<string, List<string>> draw_data = delegate (string type_name,List<string> type_names) {
                var t_datas = sdata.FindPropertyRelative("m_"+ type_name);
                if(t_datas == null) {
                    GUILayout.Box(type_name + " not support by UCL_TC_Data yet!!");
                    return;
                }
                while(type_names.Count > t_datas.arraySize) {
                    t_datas.InsertArrayElementAtIndex(t_datas.arraySize);
                }
                while(type_names.Count < t_datas.arraySize) {
                    t_datas.DeleteArrayElementAtIndex(t_datas.arraySize-1);
                }
                for(int i = 0; i < type_names.Count && i < t_datas.arraySize; i++) {
                    var t_name = type_names[i];
                    UnityEditor.EditorGUILayout.PropertyField(t_datas.GetArrayElementAtIndex(i), 
                        new GUIContent(t_name.StartsWith("m_") ? t_name.Remove(0,2) : t_name), true);
                }
            };
            GUILayout.Box(type.Name);

            //draw_data(typeof(Transform).Name, TransformNames);
            /*
            var tdatas = sdata.FindPropertyRelative("m_Transforms");
            while(TransformNames.Count > tdatas.arraySize) {//tc_data.m_Transforms.Count
                //tc_data.m_Transforms.Add(null);
                tdatas.InsertArrayElementAtIndex(tdatas.arraySize);
            }
            for(int i=0;i < TransformNames.Count && i < tdatas.arraySize; i++) {
                UnityEditor.EditorGUILayout.PropertyField(tdatas.GetArrayElementAtIndex(i), new GUIContent(TransformNames[i]), false);
            }
            */
            //GUILayout.Box("TransformCount:" + TransformCount);

            UnityEditor.EditorGUILayout.PropertyField(sdata.FindPropertyRelative("m_Type"));//,new GUIContent("Test")

            foreach(var type_name in m_Names) {
                draw_data(type_name.Key.Name, type_name.Value);
            }
            //sp.propertyType = m_Reverse.t
            //UnityEditor.EditorGUILayout.PropertyField(sdata.FindPropertyRelative("m_Data"));

            //UnityEditor.SerializedObject o = new UnityEditor.SerializedObject(this);
            /*
            if(data.Length > 0) {
                m_Reverse = (data[0] == 1);
            }
            
            var val = GUILayout.Toggle(m_Reverse, "Reverse");
            //Debug.LogWarning("m_Reverse:" + m_Reverse);
            if(m_Reverse != val) {
                data = new byte[1];
                data[0] = (byte)(m_Reverse ? 0 : 1);
                tc_data.m_Data = data;
                return true;
            }
            */
            return false;
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