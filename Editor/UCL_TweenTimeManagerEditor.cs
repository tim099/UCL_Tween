using UnityEditor;
using UnityEngine;

namespace UCL.TweenLib {
    [CustomEditor(typeof(UCL_TweenTimeManager))]
    public class UCL_TweenTimeManagerEditor : Core.EditorLib.UCL_MonobehaviorEditor {
        Vector2 m_ScrollPos = Vector2.zero;
        public override bool RequiresConstantRepaint() {
            return true;
        }
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            UCL_TweenTimeManager manager = target as UCL_TweenTimeManager;

            m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos);
            manager.OnInspectorGUI();
            GUILayout.EndScrollView();
        }
    }
}