using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UCL.TweenLib {
    [CustomEditor(typeof(UCL_TB_Tweener))]
    public class UCL_TB_TweenerEditor : Core.EditorLib.UCL_MonobehaviorEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            UCL_TB_Tweener tb = target as UCL_TB_Tweener;
            GUILayout.BeginVertical();
            var coms = tb.m_TweenerComponents;
            for(int i = 0; i < coms.Count; i++) {
                var data = coms[i];
                var tc = UCL_TweenerComponent.Create(data.m_Type);
                tc.OnInspectorGUI(data.m_Data);
            }

            GUILayout.EndVertical();
        }
    }
}