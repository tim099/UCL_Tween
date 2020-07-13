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
        Action,
        Shake
    }
    /// <summary>
    ///UCL_TweenerComponent extened action on tweener
    /// </summary>
    public class UCL_TweenerComponent {
        #region Create
        virtual public TC_Type GetTC_Type() { return TC_Type.TweenerComponent; }
        
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

        #region Editor
#if UNITY_EDITOR

        /// <summary>
        /// return true if data altered
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        virtual public bool OnInspectorGUI(UCL_TC_Data tc_data, UnityEditor.SerializedProperty sdata) {
            var type = this.GetType();
            
            UnityEditor.EditorGUILayout.PropertyField(sdata.FindPropertyRelative("m_Type"));//,new GUIContent("Test")
            //var data = tc_data.m_Data;
            FieldInfo[] fieldInfos1 = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance);
            Dictionary<System.Type, List<string>> m_Names = new Dictionary<System.Type, List<string>>();
            System.Action<FieldInfo> parse_fieldinfo = delegate (FieldInfo info) {
                var value = info.GetValue(this);
                System.Type info_type = info.FieldType;
                if(!m_Names.ContainsKey(info_type)) {
                    m_Names.Add(info_type, new List<string>());
                }
                m_Names[info_type].Add(info.Name);
            };

            for(int i = 0; i < fieldInfos1.Length; i++) {
                parse_fieldinfo(fieldInfos1[i]);
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
            //GUILayout.Box(type.Name);
            //UnityEditor.EditorGUILayout.PropertyField(sdata.FindPropertyRelative("m_Type"));//,new GUIContent("Test")

            foreach(var type_name in m_Names) {
                draw_data(type_name.Key.Name, type_name.Value);
            }
            return false;
        }
#endif
        #endregion

        virtual protected internal void LoadData(UCL_TC_Data data) {
            var type = this.GetType();
            var data_type = data.GetType();
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance);
            //FieldInfo[] datafieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance);
            Dictionary<System.Type, List<FieldInfo>> m_Infos = new Dictionary<System.Type, List<FieldInfo>>();
            System.Action<FieldInfo> parse_fieldinfo = delegate (FieldInfo info) {
                var value = info.GetValue(this);
                System.Type info_type = info.FieldType;
                if(!m_Infos.ContainsKey(info_type)) {
                    m_Infos.Add(info_type, new List<FieldInfo>());
                }
                m_Infos[info_type].Add(info);
            };

            for(int i = 0; i < fieldInfos.Length; i++) {
                parse_fieldinfo(fieldInfos[i]);
            }
            System.Action<string, List<FieldInfo>> load_data = delegate (string type_name, List<FieldInfo> field_infos) {
                var t_datafield = data_type.GetField("m_" + type_name);
                if(t_datafield == null) return;

                IList t_datas = t_datafield.GetValue(data) as IList; //sdata.FindPropertyRelative("m_" + type_name);
                if(t_datas == null) {
                    Debug.LogWarning("LoadData:" + type_name + " not support by UCL_TC_Data yet!!");
                    return;
                }
                for(int i = 0,count = field_infos.Count < t_datas.Count? field_infos.Count : t_datas.Count; i < count; i++) {
                    var f_data = t_datas[i];
                    var f_info = field_infos[i];
                    f_info.SetValue(this, f_data);
                }
            };
            foreach(var type_name in m_Infos) {
                load_data(type_name.Key.Name, type_name.Value);
            }
            
        }

        protected bool m_Reverse = false;
        internal protected UCL_Tweener p_Tweener = null;

        virtual public UCL_TweenerComponent Init() { return this; }
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