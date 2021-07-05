using System;
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
        Shake,
        Scale,
        Jump,
        EulerRotation,
        /// <summary>
        /// Move and scale RectTransform toward target
        /// </summary>
        RectTransform,
    }
    /// <summary>
    ///UCL_TweenerComponent extened action on tweener
    /// </summary>
    public class UCL_TweenerComponent {
        virtual public string Name {
            get {
                return this.GetType().Name.Replace("UCL_TC_", string.Empty);
            }
        }

        #region Create
        virtual public TC_Type GetTC_Type() { return TC_Type.TweenerComponent; }
        virtual public void OnDrawGizmos() { }
        public static UCL_TweenerComponent Create(TC_Type iType) {
            UCL_TweenerComponent aTC = null;
            switch(iType) {
                case TC_Type.Move: {
                        aTC = UCL_TC_Move.Create();
                        break;
                    }
                case TC_Type.Rotate: {
                        aTC = UCL_TC_Rotate.Create();
                        break;
                    }
                case TC_Type.LookAt: {
                        aTC = UCL_TC_LookAt.Create();
                        break;
                    }
                case TC_Type.Curve: {
                        aTC = UCL_TC_Curve.Create();
                        break;
                    }
                case TC_Type.Action: {
                        aTC = UCL_TC_Action.Create();
                        break;
                    }
                case TC_Type.Shake: {
                        aTC = UCL_TC_Shake.Create();
                        break;
                    }
                case TC_Type.Scale: {
                        aTC = UCL_TC_Scale.Create();
                        break;
                    }
                case TC_Type.Jump: {
                        aTC = UCL_TC_Jump.Create();
                        break;
                    }
                case TC_Type.EulerRotation:
                    {
                        aTC = UCL_TC_EulerRotation.Create();
                        break;
                    }
                case TC_Type.TweenerComponent: {
                        aTC = new UCL_TweenerComponent();
                        break;
                    }
                case TC_Type.RectTransform:
                    {
                        aTC = UCL_TC_RectTransform.Create();
                        break;
                    }
                default: {
                        string type_name = "UCL.TweenLib.UCL_TC_" + iType.ToString();
                        Type tc_type = Type.GetType(type_name);
                        if(tc_type != null) {
                            aTC = Activator.CreateInstance(tc_type) as UCL_TweenerComponent;
                        } else {
                            Debug.LogError("type_name:" + type_name + ", not exist!!");
                        }
                        break;
                    }
            }
            if(aTC == null) {
                aTC = new UCL_TweenerComponent();
            }
            return aTC;
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

            FieldInfo[] fieldInfos1 = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance);
            Dictionary<System.Type, List<string>> m_Names = new Dictionary<System.Type, List<string>>();
            System.Action<FieldInfo> parse_fieldinfo = delegate (FieldInfo info) {
                if(info.GetCustomAttribute<HideInInspector>() != null) return;
                var value = info.GetValue(this);
                System.Type info_type = info.FieldType;
                if(info_type.IsGenericType) return;

                if(!m_Names.ContainsKey(info_type)) {
                    m_Names.Add(info_type, new List<string>());
                }
                m_Names[info_type].Add(info.Name);
            };

            for(int i = 0; i < fieldInfos1.Length; i++) {
                parse_fieldinfo(fieldInfos1[i]);
            }
            System.Action<string, List<string>> draw_data = delegate (string type_name, List<string> type_names) {
                var t_datas = sdata.FindPropertyRelative("m_" + type_name);
                if(t_datas == null) {
                    if(GUILayout.Button(new GUIContent(type_name + " not supported by UCL_TC_Data yet!!",
                        "Click this button to open UCL_TC_Data script."))) {
                        //Assets/UCL/UCL_Modules/UCL_Tween/UCL_TweenScript/UCL_TweenBehaviors/UCL_TweenComponentDatas/UCL_TC_Data.cs
                        string sc_path = Core.FileLib.EditorLib.GetLibFolderPath(Core.FileLib.LibName.UCL_TweenLib)
                         + "/UCL_TweenScript/UCL_TweenBehaviors/UCL_TweenComponentDatas/UCL_TC_Data.cs";
                        //Debug.Log("EaseScript:" + sc_path);
                        var aObj = UCL.Core.EditorLib.AssetDatabaseMapper.LoadMainAssetAtPath(sc_path);
                        if(aObj != null) {
                            UnityEditor.Selection.activeObject = aObj;
                        }
                    }
                    //GUILayout.Box(type_name + " not support by UCL_TC_Data yet!!");
                    return;
                }
                while(type_names.Count > t_datas.arraySize) {
                    t_datas.InsertArrayElementAtIndex(t_datas.arraySize);
                }
                while(type_names.Count < t_datas.arraySize) {
                    t_datas.DeleteArrayElementAtIndex(t_datas.arraySize - 1);
                }
                for(int i = 0; i < type_names.Count && i < t_datas.arraySize; i++) {
                    var t_name = type_names[i];
                    UnityEditor.EditorGUILayout.PropertyField(t_datas.GetArrayElementAtIndex(i),
                        new GUIContent(t_name.StartsWith("m_") ? t_name.Remove(0, 2) : t_name), true);
                }
            };
            //GUILayout.Box(type.Name);
            //UnityEditor.EditorGUILayout.PropertyField(sdata.FindPropertyRelative("m_Type"));//,new GUIContent("Test")

            foreach(var type_name in m_Names) {
                draw_data(type_name.Key.Name, type_name.Value);
            }

            return false;
        }
        virtual public string OnInspectorGUITips() {
            return string.Empty;
        }
        virtual public void OnInspectorGUIBasic(UCL_TC_Data tc_data, UnityEditor.SerializedProperty sdata,
            Transform TB_transform) {
            UnityEditor.EditorGUILayout.PropertyField(sdata.FindPropertyRelative("m_Type"));//,new GUIContent("Test")

            string tips = OnInspectorGUITips();
            if(!string.IsNullOrEmpty(tips)) {
                var strs = tips.Split('\n');
                foreach(string str in strs) {
                    if(!string.IsNullOrEmpty(str)) GUILayout.Box(str);
                }
            }
        }
#endif
        #endregion

        /// <summary>
        /// Load Data using reflection
        /// </summary>
        /// <param name="iData"></param>
        virtual protected internal void LoadData(UCL_TC_Data iData) {
            var aType = this.GetType();
            var aDataType = iData.GetType();
            FieldInfo[] aFieldInfos = aType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance);
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

            for(int i = 0; i < aFieldInfos.Length; i++) {
                parse_fieldinfo(aFieldInfos[i]);
            }
            System.Action<string, List<FieldInfo>> load_data = delegate (string type_name, List<FieldInfo> field_infos) {
                var t_datafield = aDataType.GetField("m_" + type_name);
                if(t_datafield == null) return;

                IList t_datas = t_datafield.GetValue(iData) as IList; //sdata.FindPropertyRelative("m_" + type_name);
                if(t_datas == null) {
                    Debug.LogWarning("LoadData:" + type_name + " not support by UCL_TC_Data yet!!");
                    return;
                }
                for(int i = 0, count = field_infos.Count < t_datas.Count ? field_infos.Count : t_datas.Count; i < count; i++) {
                    var f_data = t_datas[i];
                    var f_info = field_infos[i];
                    f_info.SetValue(this, f_data);
                }
            };
            foreach(var type_name in m_Infos) {
                load_data(type_name.Key.Name, type_name.Value);
            }

        }
        virtual public Transform GetTarget() { return null; }
        protected bool m_Reverse = false;
        //internal protected UCL_Tweener p_Tweener = null;

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
#if UNITY_EDITOR
        virtual internal void OnInspectorGUI() {

        }
        /// <summary>
        /// Called when being selected
        /// </summary>
        virtual internal void OnSelected() {

        }
#endif
    }
}