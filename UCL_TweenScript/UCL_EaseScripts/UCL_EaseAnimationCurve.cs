using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib.Ease {
    public class UCL_EaseAnimationCurve : UCL_Ease {
        protected AnimationCurve m_AnimationCurve;
        public UCL_EaseAnimationCurve(AnimationCurve _AnimationCurve) {
            m_AnimationCurve = _AnimationCurve;
        }
        public override float GetEase(float x) {
            if(m_AnimationCurve == null) {
                return x;
            }
            return m_AnimationCurve.Evaluate(x);
        }
    }
}