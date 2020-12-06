using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TweenerMove : UCL_TweenerTransform {
        protected Vector3 m_TargetVal;
        protected Vector3 m_StartVal;
        public static UCL_TweenerMove Create() {
            var obj = new UCL_TweenerMove();
            return obj;
        }

        virtual public UCL_TweenerMove Init(Transform target,Vector3 target_position,float duration) {
            m_Target = target;
            m_TargetVal = target_position;
            Duration = duration;
            return this;
        }
        protected override void TweenerStart() {
            if(m_Local) {
                m_StartVal = m_Target.localPosition;
            } else {
                m_StartVal = m_Target.position;
            }
        }
        protected override void TweenerUpdate(float pos) {
            if(m_Local) {
                m_Target.transform.localPosition = Core.MathLib.Lib.Lerp(m_StartVal, m_TargetVal, pos);
            } else {
                m_Target.transform.position = Core.MathLib.Lib.Lerp(m_StartVal, m_TargetVal, pos);
            }
        }
    }
}