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
        Color,
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
                case TC_Type.Color:
                    {
                        aTC = UCL_TC_Color.Create();
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
        virtual public bool OnInspectorGUI(UCL_TC_Data iTC_Data, UnityEditor.SerializedProperty iSerializedProperty) {
            bool aIsDirty = false;
            Dictionary<Type, List<FieldInfo>> aFieldInfosDic = GetFieldInfosDic();
            if (iTC_Data.UpdateVersion(UpdateVersionAct))
            {
                aIsDirty = true;
            }

            Action<Type, List<FieldInfo>> aDrawData = delegate (Type iType, List<FieldInfo> iFieldInfos) {
                string aTypeName = iType.Name;
                var aDatas = iSerializedProperty.FindPropertyRelative("m_" + aTypeName);
                if(aDatas == null) {
                    //if (typeof(Component).IsAssignableFrom(iType))
                    //{
                    //    aDatas = iSerializedProperty.FindPropertyRelative("m_Component");
                    //}
                    //else
                    {
                        if (GUILayout.Button(new GUIContent(aTypeName + " not supported by UCL_TC_Data",
                            "Click this button to open UCL_TC_Data script.")))
                        {
                            //Assets/UCL/UCL_Modules/UCL_Tween/UCL_TweenScript/UCL_TweenBehaviors/UCL_TweenComponentDatas/UCL_TC_Data.cs
                            string sc_path = Core.FileLib.EditorLib.GetLibFolderPath(Core.FileLib.LibName.UCL_TweenLib)
                             + "/UCL_TweenScript/UCL_TweenBehaviors/UCL_TweenComponentDatas/UCL_TC_Data.cs";
                            //Debug.Log("EaseScript:" + sc_path);
                            var aObj = UCL.Core.EditorLib.AssetDatabaseMapper.LoadMainAssetAtPath(sc_path);
                            if (aObj != null)
                            {
                                UnityEditor.Selection.activeObject = aObj;
                            }
                        }
                        //GUILayout.Box(type_name + " not support by UCL_TC_Data yet!!");
                        return;
                    }
                }
                while(iFieldInfos.Count > aDatas.arraySize) {
                    aDatas.InsertArrayElementAtIndex(aDatas.arraySize);
                    var aFieldInfo = iFieldInfos[aDatas.arraySize - 1];
                    if (aFieldInfo.FieldType.IsBool())
                    {
                        bool aNewVal = (bool)iFieldInfos[aDatas.arraySize - 1].GetValue(this);
                        //Debug.LogError("aNewVal:" + aNewVal+ ",aFieldInfo.Name:" + aFieldInfo.Name);
                        aDatas.GetArrayElementAtIndex(aDatas.arraySize - 1).boolValue = aNewVal;
                    }
                }
                while(iFieldInfos.Count < aDatas.arraySize) {
                    aDatas.DeleteArrayElementAtIndex(aDatas.arraySize - 1);
                }
                for(int i = 0; i < iFieldInfos.Count && i < aDatas.arraySize; i++) {
                    var aInfo = iFieldInfos[i];
                    var aFieldName = aInfo.Name;
                    string aDisplayName = aFieldName.StartsWith("m_") ? aFieldName.Remove(0, 2) : aFieldName;
                    var aHeaderAttr = aInfo.GetCustomAttribute<HeaderAttribute>();
                    if (aHeaderAttr != null)
                    {
                        GUILayout.Box(aHeaderAttr.header);
                    }
                    UnityEditor.EditorGUILayout.PropertyField(aDatas.GetArrayElementAtIndex(i),
                        new GUIContent(aDisplayName), true);
                }
            };

            foreach(var aType in aFieldInfosDic) {
                aDrawData(aType.Key, aType.Value);
            }

            return aIsDirty;
        }
        virtual public string OnInspectorGUITips() {
            return string.Empty;
        }
        virtual public void OnInspectorGUIBasic(UCL_TC_Data tc_data, UnityEditor.SerializedProperty iSerializedProperty, Transform TB_transform) {
            string aTips = OnInspectorGUITips();
            if(!string.IsNullOrEmpty(aTips)) {
                var aTip = aTips.Split('\n');
                foreach(string aStr in aTip) {
                    if(!string.IsNullOrEmpty(aStr)) GUILayout.Box(aStr);
                }
            }
        }
#endif
        #endregion
        protected static Dictionary<Type, FieldInfo[]> s_DataFieldInfos = new Dictionary<Type, FieldInfo[]>();
        virtual protected FieldInfo[] GetDataFieldInfos()
        {
            var aType = this.GetType();
            if (!s_DataFieldInfos.ContainsKey(aType))
            {
                s_DataFieldInfos[aType] = aType.GetAllFieldsUnityVer(typeof(UCL_TweenerComponent)).ToArray();
                    //aType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance);
            }
            return s_DataFieldInfos[aType];
        }
        protected static Dictionary<Type, Dictionary<System.Type, List<FieldInfo>>> s_FieldInfosDic = new Dictionary<Type, Dictionary<Type, List<FieldInfo>>>();
        virtual protected Dictionary<System.Type, List<FieldInfo>> GetFieldInfosDic()
        {
            var aType = this.GetType();
            if (!s_FieldInfosDic.ContainsKey(aType))
            {
                FieldInfo[] aFieldInfos = GetDataFieldInfos();
                Dictionary<System.Type, List<FieldInfo>> aInfos = new Dictionary<System.Type, List<FieldInfo>>();
                System.Action<FieldInfo> aParseFieldinfo = delegate (FieldInfo iInfo) {
                    System.Type aInfoType = iInfo.FieldType;
                    if (!aInfos.ContainsKey(aInfoType))
                    {
                        aInfos.Add(aInfoType, new List<FieldInfo>());
                    }
                    aInfos[aInfoType].Add(iInfo);
                };

                for (int i = 0; i < aFieldInfos.Length; i++)
                {
                    aParseFieldinfo(aFieldInfos[i]);
                }
                s_FieldInfosDic[aType] = aInfos;
            }

            return s_FieldInfosDic[aType];
        }
        /// <summary>
        /// Load Data using reflection
        /// </summary>
        /// <param name="iData"></param>
        virtual protected internal void LoadData(UCL_TC_Data iData) {
            iData.UpdateVersion(UpdateVersionAct);
            var aDataType = iData.GetType();
            Dictionary<System.Type, List<FieldInfo>> aInfos = GetFieldInfosDic();
            System.Action<string, List<FieldInfo>> aLoadData = delegate (string iTypeName, List<FieldInfo> iFieldInfos) {
                FieldInfo aFieldInfo = aDataType.GetField("m_" + iTypeName);
                if(aFieldInfo == null) return;

                IList aDatas = aFieldInfo.GetValue(iData) as IList; //sdata.FindPropertyRelative("m_" + type_name);
                if(aDatas == null) {
                    Debug.LogWarning("LoadData:" + iTypeName + " not support by UCL_TC_Data yet!!");
                    return;
                }
                for(int i = 0, count = iFieldInfos.Count < aDatas.Count ? iFieldInfos.Count : aDatas.Count; i < count; i++) {
                    var aData = aDatas[i];
                    var aInfo = iFieldInfos[i];
                    aInfo.SetValue(this, aData);
                }
            };
            foreach(var aTypeName in aInfos) {
                aLoadData(aTypeName.Key.Name, aTypeName.Value);
            }

        }
        /// <summary>
        /// Update if the data is older version
        /// </summary>
        /// <param name="iCurVersion"></param>
        virtual protected void UpdateVersionAct(UCL_TC_Data.DataVersion iCurVersion, UCL_TC_Data iData)
        {

        }
        virtual public Transform GetTarget() { return null; }
        protected bool m_Reverse = false;
        //internal protected UCL_Tweener p_Tweener = null;

        virtual public UCL_TweenerComponent Init() { return this; }
        virtual protected internal void Start() { }
        virtual protected internal void Complete() { }

        virtual protected void ComponentUpdate(float iPos) { }


        internal void Update(float iPos) {
            if(m_Reverse) iPos = 1 - iPos;
            ComponentUpdate(iPos);
        }

        public UCL_TweenerComponent SetReverse(bool iVal) {
            m_Reverse = iVal;
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