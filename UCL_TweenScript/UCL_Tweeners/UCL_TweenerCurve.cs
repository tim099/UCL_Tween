using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public static partial class Extension {
        static public UCL_TweenerCurve UCL_Move(this Transform target, float duration, Core.MathLib.UCL_Path path) {
            return UCL_TweenerCurve.Create().Init(target, path, duration);
        }
    }

    public class UCL_TweenerCurve : UCL_TweenerTransform {
        protected Core.MathLib.UCL_Path m_Path;
        UCL_TweenerCurve() { }

        public static UCL_TweenerCurve Create() {
            return new UCL_TweenerCurve();
        }
        virtual public UCL_TweenerCurve Init(Transform _target, Core.MathLib.UCL_Path _path, float _duration) {
            Duration = _duration;
            m_Path = _path;
            m_Target = _target;
            return this;
        }

        protected override void TweenerUpdate(float pos) {
            m_Target.transform.position = m_Path.GetPos(pos);
            //Debug.LogWarning("TweenerUpdate:" + pos+ ",m_Target.transform.position:"+ m_Target.transform.position);
        }
    }
}