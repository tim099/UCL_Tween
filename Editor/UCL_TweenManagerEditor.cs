using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UCL.TweenLib {
    [CustomEditor(typeof(UCL_TweenManager))]
    public class UCL_TweenManagerEditor : Core.EditorLib.UCL_MonobehaviorEditor {
        public override bool RequiresConstantRepaint() {
            return true;
        }
        public override void OnInspectorGUI() {
            var manager = target as UCL_TweenManager;
            int tweenerc = manager.TweenerCount;
            int seq_c = manager.SequenceCount;
            GUILayout.BeginVertical();
            //Debug.LogWarning("TweenCount:" + manager.TweenCount);
            GUILayout.Box("TweenCount:" + manager.TweenCount+"("+ tweenerc + "+" + seq_c + ")");
            GUILayout.Box("TweenerCount:" + tweenerc);
            GUILayout.Box("SequenceCount:" + seq_c);
            GUILayout.EndVertical();

            base.OnInspectorGUI();
            //this.Repaint();
            //this.RequiresConstantRepaint
        }
    }
}

