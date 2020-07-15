using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TC_Shake : UCL_TC_Transform {
        int m_RandSeed = 0;
        Core.MathLib.UCL_Random m_Rnd;
        //public 
        override public TC_Type GetTC_Type() { return TC_Type.Shake; }
        public static UCL_TC_Shake Create() {
            return new UCL_TC_Shake();
        }
        protected internal override void Start() {
            m_Rnd = new Core.MathLib.UCL_Random(m_RandSeed);
            if(m_Local) {
                //m_StartVal = m_Target.localPosition;
            } else {
                //m_StartVal = m_Target.position;
            }
        }
    }
}