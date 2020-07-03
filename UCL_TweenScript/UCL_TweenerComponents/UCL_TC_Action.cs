using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TC_Action : UCL_TC_Transform {
        override protected TC_Type GetTC_Type() { return TC_Type.Action; }
        public static UCL_TC_Action Create() {
            return new UCL_TC_Action();
        }

        protected System.Action<float> m_Act;

        public UCL_TC_Action Init(System.Action<float> act) {
            m_Act = act;
            return this;
        }
        protected override void ComponentUpdate(float pos) {
            if(m_Act != null) {
                m_Act.Invoke(pos);
            }
        }
    }
}