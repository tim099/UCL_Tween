﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public static partial class Extension {
        static public UCL_TweenerMove UCL_Move(this Transform target, Vector3 target_position, float duration) {
            return UCL_TweenerMove.Create().Init(target, target_position, duration);
        }
    }

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
            m_Duration = duration;
            m_StartVal = m_Target.position;
            //Debug.LogWarning("Init !!m_Target.transform.position:" + m_Target.transform.position +
                //",m_TargetPos:" + m_TargetPos + ",m_StartPos:" + m_StartPos);
            return this;
        }
        protected internal override void TweenStart() {
            base.TweenStart();
            m_StartVal = m_Target.position;
        }
        protected override void TweenerUpdate(float pos) {
            m_Target.transform.position = Vector3.Lerp(m_StartVal, m_TargetVal, pos);
            //Debug.LogWarning("TweenerUpdate:" + pos + ",m_Target.transform.position:"+ m_Target.transform.position + 
                //",m_TargetPos:"+ m_TargetPos+ ",m_StartPos:"+ m_StartPos);

        }
    }
}