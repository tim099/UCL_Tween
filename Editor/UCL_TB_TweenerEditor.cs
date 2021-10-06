using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UCL.TweenLib {
    [CustomEditor(typeof(UCL_TB_Tweener), true)]
    public class UCL_TB_TweenerEditor : Core.EditorLib.UCL_MonobehaviorEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            UCL_TB_Tweener aTB = target as UCL_TB_Tweener;
            GUILayout.BeginVertical();

            var aSerializedComponents = serializedObject.FindProperty("m_TweenerComponents");
            var aComponents = aTB.m_TweenerComponents;
            int add_at = -1;
            int delete_at = -1;
            bool aIsModified = false;
            int aChangeTypeAt = -1;
            EditorGUI.BeginChangeCheck();
            if(aComponents != null) {
                for(int i = 0; i < aComponents.Count; i++) {
                    var aTC_Data = aComponents[i];
                    var aSerializedData = aSerializedComponents.GetArrayElementAtIndex(i);
                    var aTweenComponent = UCL_TweenerComponent.Create(aTC_Data.m_Type);
                    var aType = aTweenComponent.GetType();
                    /*
                    if(GUILayout.Button("Insert TweenerComponent")) {
                        add_at = i;
                        break;
                    }
                    */
                    UnityEditor.EditorGUILayout.BeginHorizontal();
                    aTC_Data.m_Foldout = UnityEditor.EditorGUILayout.Foldout(aTC_Data.m_Foldout, aType.Name, true);

                    if(GUILayout.Button("Delete", UCL.Core.UI.UCL_GUIStyle.TextRed)) {
                        delete_at = i;
                        break;
                    }
                    
                    UnityEditor.EditorGUILayout.EndHorizontal();
                    if(aTC_Data.m_Foldout) {
                        aIsModified = true;
                        var aTypeData = aSerializedData.FindPropertyRelative("m_Type");
                        var aTypeEnum = aTypeData.enumValueIndex;
                        UnityEditor.EditorGUILayout.PropertyField(aTypeData);
                        if (aTypeEnum != aTypeData.enumValueIndex)//Change Type
                        {
                            aChangeTypeAt = i;
                        }
                        else
                        {
                            aTweenComponent.OnInspectorGUIBasic(aTC_Data, aSerializedData, aTB.transform);
                            if (aTweenComponent.OnInspectorGUI(aTC_Data, aSerializedData))
                            {//serializedObject
                                UCL.Core.EditorLib.EditorUtilityMapper.SetDirty(aTB);
                            }
                        }
                    }

                }
            }
            if(aIsModified) {
                if(EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(target, "serializedObject.ApplyModifiedProperties()");
                    serializedObject.ApplyModifiedProperties();
                }
                //Undo.RecordObject(target, "serializedObject.ApplyModifiedProperties()");

                //if(EditorGUI.EndChangeCheck()) Undo.RecordObject(target, "serializedObject.ApplyModifiedProperties()");
            }
            if (aChangeTypeAt >= 0)
            {
                Undo.RecordObject(aTB, "m_TweenerComponents.Init()" + aChangeTypeAt);
                aComponents[aChangeTypeAt].Init();
                UCL.Core.EditorLib.EditorUtilityMapper.SetDirty(aTB);
                //serializedObject.Update();
            }
            if(aTB.m_TweenerComponents == null) {
                aTB.m_TweenerComponents = new List<UCL_TC_Data>();
            }
            if(GUILayout.Button("Add TweenerComponent")) {
                add_at = aTB.m_TweenerComponents.Count;
            }
            if(add_at >= 0) {
                //EditorGUI.BeginChangeCheck();
                Undo.RecordObject(aTB, "m_TweenerComponents.Insert_" + add_at);
                aTB.m_TweenerComponents.Insert(add_at, UCL_TC_Data.Create());
                
            } else if(delete_at >= 0) {
                //EditorGUI.BeginChangeCheck();
                Undo.RecordObject(target, "m_TweenerComponents.RemoveAt_" + delete_at);
                aTB.m_TweenerComponents.RemoveAt(delete_at);
                //if(EditorGUI.EndChangeCheck()) Undo.RecordObject(target, "m_TweenerComponents.RemoveAt_"+delete_at); 
            }

            GUILayout.EndVertical();
        }
    }
}