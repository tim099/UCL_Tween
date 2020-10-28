using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TweenerTransform : UCL_Tweener {
        protected Transform m_Target;
        protected bool m_Local = false;

        public UCL_TweenerTransform SetLocal(bool local) {
            m_Local = local;
            return this;
        }
        protected override void CompleteAction() {
            TweenerUpdate(m_Reverse ? 0 : 1);
        }
        override public bool KillOnTransform(Transform t, bool compelete = false) {
            if(base.KillOnTransform(t, compelete))return true;
            Kill(compelete);
            return true;
        }
    }
}