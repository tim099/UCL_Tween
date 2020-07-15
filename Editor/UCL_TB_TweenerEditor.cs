using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UCL.TweenLib {
    [CustomEditor(typeof(UCL_TB_Tweener), true)]
    public class UCL_TB_TweenerEditor : Core.EditorLib.UCL_MonobehaviorEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            UCL_TB_Tweener tb = target as UCL_TB_Tweener;
            GUILayout.BeginVertical();

            var scoms = serializedObject.FindProperty("m_TweenerComponents");
            var coms = tb.m_TweenerComponents;
            int add_at = -1;
            int delete_at = -1;
            bool modified = false;
            EditorGUI.BeginChangeCheck();
            for(int i = 0; i < coms.Count; i++) {
                var data = coms[i];
                var sdata = scoms.GetArrayElementAtIndex(i);
                var tc = UCL_TweenerComponent.Create(data.m_Type);
                var type = tc.GetType();
                if(GUILayout.Button("Insert TweenerComponent")) {
                    add_at = i;
                    break;
                }
                UnityEditor.EditorGUILayout.BeginHorizontal();
                data.m_Foldout = UnityEditor.EditorGUILayout.Foldout(data.m_Foldout, type.Name, true);

                if(GUILayout.Button("Delete", UCL.Core.UI.UCL_GUIStyle.TextRed)) {
                    delete_at = i;
                    break;
                }

                UnityEditor.EditorGUILayout.EndHorizontal();
                if(data.m_Foldout) {
                    modified = true;
                    if(tc.OnInspectorGUI(data, sdata)) {//serializedObject
                        EditorUtility.SetDirty(tb);
                    }
                }

            }
            if(GUILayout.Button("Add TweenerComponent")) {
                add_at = tb.m_TweenerComponents.Count;
            }
            if(add_at >= 0) {
                //EditorGUI.BeginChangeCheck();
                Undo.RecordObject(tb, "m_TweenerComponents.Insert_" + add_at);
                tb.m_TweenerComponents.Insert(add_at, new UCL_TC_Data());
                
            } else if(delete_at >= 0) {
                //EditorGUI.BeginChangeCheck();
                Undo.RecordObject(target, "m_TweenerComponents.RemoveAt_" + delete_at);
                tb.m_TweenerComponents.RemoveAt(delete_at);
                //if(EditorGUI.EndChangeCheck()) Undo.RecordObject(target, "m_TweenerComponents.RemoveAt_"+delete_at); 
            }
            if(modified) {
                if(EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(target, "serializedObject.ApplyModifiedProperties()");
                    serializedObject.ApplyModifiedProperties();
                }
                //Undo.RecordObject(target, "serializedObject.ApplyModifiedProperties()");
                
                //if(EditorGUI.EndChangeCheck()) Undo.RecordObject(target, "serializedObject.ApplyModifiedProperties()");
            }
            GUILayout.EndVertical();
        }
    }
}