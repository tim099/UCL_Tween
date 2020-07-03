﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public static partial class Extension {
        static public UCL_TC_Curve TC_Move(this Transform target, Core.MathLib.UCL_Curve target_val) {
            return UCL_TC_Curve.Create().Init(target, target_val);
        }
    }
    public class UCL_TC_Curve : UCL_TC_Transform {
        override protected TC_Type GetTC_Type() { return TC_Type.Curve; }

        protected class LookAtFront {
            public Vector3 m_Up;

            public Vector3 m_Rot;
            public bool m_DoRot = false;
        }

        public static UCL_TC_Curve Create() {
            return new UCL_TC_Curve();
        }

        protected Core.MathLib.UCL_Curve m_Curve;
        protected LookAtFront m_LookAtFront = null;

        virtual public UCL_TC_Curve Init(Transform target, Core.MathLib.UCL_Curve curve) {
            m_Curve = curve;
            m_Target = target;
            return this;
        }

        virtual public UCL_TC_Curve SetLookAtFront(bool val, Vector3? up = null, Vector3? rot = null) {
            if(val) {
                m_LookAtFront = new LookAtFront();
                if(up.HasValue) m_LookAtFront.m_Up = up.Value;
                else m_LookAtFront.m_Up = Vector3.up;
                if(rot.HasValue) {
                    m_LookAtFront.m_DoRot = true;
                    m_LookAtFront.m_Rot = rot.Value;
                }
            } else {
                m_LookAtFront = null;
            }

            return this;
        }
        protected override void ComponentUpdate(float pos) {
            var cur_pos = m_Curve.GetPoint(pos);
            m_Target.transform.position = cur_pos;
            if(m_LookAtFront != null) {
                const float ndel = 0.0001f;
                if(pos <= 1 - ndel) {
                    var next_pos = m_Curve.GetPoint(pos + ndel);
                    var rot = Quaternion.LookRotation(next_pos - cur_pos, m_LookAtFront.m_Up);
                    
                    if(m_LookAtFront.m_DoRot) {
                        rot *= Quaternion.Euler(m_LookAtFront.m_Rot);
                    }
                    m_Target.transform.rotation = rot;
                }
            }
        }

    }
}