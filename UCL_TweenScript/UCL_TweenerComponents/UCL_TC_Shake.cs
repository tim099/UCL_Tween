using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TC_Shake : UCL_TC_Transform {
        override public TC_Type GetTC_Type() { return TC_Type.Shake; }
        protected internal override void Start() {
            if(m_Local) {
                //m_StartVal = m_Target.localPosition;
            } else {
                //m_StartVal = m_Target.position;
            }
        }
    }
}