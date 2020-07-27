using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public static partial class Extension {
        static public UCL_TweenerCurve UCL_Move(this Transform target, Core.MathLib.UCL_Curve curve, float duration) {
            return UCL_TweenerCurve.Create().Init(target, curve, duration);
        }
    }

    public class UCL_TweenerCurve : UCL_TweenerTransform {
        protected Core.MathLib.UCL_Curve m_Curve;
        UCL_TweenerCurve() { }

        public static UCL_TweenerCurve Create() {
            return new UCL_TweenerCurve();
        }
        virtual public UCL_TweenerCurve Init(Transform target, Core.MathLib.UCL_Curve curve, float duration) {
            Duration = duration;
            m_Curve = curve;
            m_Target = target;
            return this;
        }

        protected override void TweenerUpdate(float pos) {
            m_Target.transform.position = m_Curve.GetPos(pos);
            //Debug.LogWarning("TweenerUpdate:" + pos+ ",m_Target.transform.position:"+ m_Target.transform.position);
        }
    }
}