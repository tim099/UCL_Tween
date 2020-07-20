using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib.Ease {
    public class UCL_EaseFunction : UCL_Ease {
        public UCL_EaseFunction(System.Func<float, float> _EaseFunction) {
            m_EaseFunction = _EaseFunction;
        }
        protected System.Func<float, float> m_EaseFunction;
        public override float GetEase(float x) {
            if(m_EaseFunction == null) {
                return x;
            }
            return m_EaseFunction(x); 
        }
    }
}