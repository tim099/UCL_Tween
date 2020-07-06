using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public static class LibTC {
        static public UCL_TC_Action Action(System.Action<float> act) {
            return UCL_TC_Action.Create().Init(act);
        }
    }
    public static class LibTween {
        static public UCL_Sequence Sequence() {
            return UCL_Sequence.Create();
        }
        static public UCL_Tweener Tweener() {
            return UCL_Tweener.CreateTweener();
        }
        static public UCL_Tweener Tweener(float duration) {
            var obj = UCL_Tweener.CreateTweener();
            obj.SetDuration(duration);
            return obj;
        }
    }
}