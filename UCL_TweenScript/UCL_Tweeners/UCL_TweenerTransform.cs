using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TweenerTransform : UCL_Tweener {
        protected Transform m_Target;

        protected override void CompleteAction() {
            TweenerUpdate(m_Inverse ? 0 : 1);
        }
    }
}