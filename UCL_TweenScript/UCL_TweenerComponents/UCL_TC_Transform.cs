using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TC_Transform : UCL_TweenerComponent {

        protected Transform m_Target;
        protected bool m_Local = false;

        public UCL_TC_Transform SetLocal(bool local) {
            m_Local = local;
            return this;
        }
    }
}