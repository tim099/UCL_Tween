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
            for(int i = 0; i < coms.Count; i++) {
                var data = coms[i];
                var sdata = scoms.GetArrayElementAtIndex(i);
                
                var tc = UCL_TweenerComponent.Create(data.m_Type);
                if(tc.OnInspectorGUI(data, sdata)) {//serializedObject
                    EditorUtility.SetDirty(tb);
                }
            }
            serializedObject.ApplyModifiedProperties();
            GUILayout.EndVertical();
        }
    }
}