using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public static partial class Extension {
        static public UCL_TC_Curve TC_Move(this Transform target, Core.MathLib.UCL_Curve target_val) {
            return UCL_TC_Curve.Create().Init(target, target_val);
        }
    }
    public class UCL_TC_Curve : UCL_TC_Transform {
        UCL_TC_Curve() {}
        public static UCL_TC_Curve Create() {
            return new UCL_TC_Curve();
        }

        protected Core.MathLib.UCL_Curve m_Curve;


        virtual public UCL_TC_Curve Init(Transform target, Core.MathLib.UCL_Curve curve) {
            m_Curve = curve;
            m_Target = target;
            return this;
        }
        protected override void ComponentUpdate(float pos) {
            m_Target.transform.position = m_Curve.GetPoint(pos);
        }

    }
}