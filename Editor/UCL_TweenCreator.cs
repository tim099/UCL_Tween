using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UCL.TweenLib.Editor
{
    public static class UCL_TweenCreator
    {
        [UnityEditor.MenuItem("GameObject/Effects/UCL_Tween/TB_Tweener")]
        private static void CreateTB_Tweener() {
            Object selectedObject = UnityEditor.Selection.activeObject;

            GameObject obj = selectedObject as GameObject;
            Transform p = null;
            if(obj != null) {
                p = obj.transform;
            }
            var tb = Core.GameObjectLib.Create<UCL_TB_Tweener>("TB_Tweener", p);
            UnityEditor.Selection.activeObject = tb;
        }

        [UnityEditor.MenuItem("GameObject/Effects/UCL_Tween/TB_Move")]
        private static void CreateTB_Move() {
            Object selectedObject = UnityEditor.Selection.activeObject;

            GameObject obj = selectedObject as GameObject;
            Transform p = null;
            if(obj != null) {
                p = obj.transform;
            }
            var tb = Core.GameObjectLib.Create<UCL_TB_Move>("TB_Move", p);
            UnityEditor.Selection.activeObject = tb;
        }
        [UnityEditor.MenuItem("GameObject/Effects/UCL_Tween/TB_Timer")]
        private static void CreateTB_Timer() {
            Object selectedObject = UnityEditor.Selection.activeObject;

            GameObject obj = selectedObject as GameObject;
            Transform p = null;
            if(obj != null) {
                p = obj.transform;
            }
            var tb = Core.GameObjectLib.Create<UCL_TB_Timer>("TB_Timer", p);
            UnityEditor.Selection.activeObject = tb;
        }
    }
}

