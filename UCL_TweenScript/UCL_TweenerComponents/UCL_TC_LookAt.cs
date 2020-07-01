using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TC_LookAt : UCL_TC_Transform {
        protected Quaternion m_TargetVal;
        protected Quaternion m_StartVal;
        protected Vector3 m_LookTarget;
        protected Vector3 m_Up;
        virtual public UCL_TC_LookAt Init(Transform target, Vector3 look_target,Vector3 up) {
            m_Target = target;
            m_LookTarget = look_target;
            m_Up = up;
            return this;
        }
        protected internal override void Start() {
            m_StartVal = m_Target.rotation;
            //m_TargetVal = 
        }

    }
}