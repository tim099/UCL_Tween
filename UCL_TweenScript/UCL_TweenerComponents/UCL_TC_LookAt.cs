﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public static partial class Extension {
        static public UCL_TC_LookAt TC_LookAt(this Transform target, Vector3 look_target, Vector3 up) {
            return UCL_TC_LookAt.Create().Init(target, look_target, up);
        }
        static public UCL_TC_LookAt TC_LookAt(this Transform target, Transform look_target, Vector3 up) {
            return UCL_TC_LookAt.Create().Init(target, look_target, up);
        }
        static public UCL_TC_LookAt TC_LocalLookAt(this Transform target, Vector3 look_target, Vector3 up) {
            var obj = UCL_TC_LookAt.Create();
            obj.SetLocal(true);
            return obj.Init(target, look_target, up);
        }
    }
    public class UCL_TC_LookAt : UCL_TC_Transform {
        override public TC_Type GetTC_Type() { return TC_Type.LookAt; }

        public static UCL_TC_LookAt Create() {
            return new UCL_TC_LookAt();
        }
        protected Quaternion m_TargetVal;
        protected Quaternion m_StartVal;

        protected Transform m_LookTargetTransform;
        protected Vector3 m_LookTarget;
        protected Vector3 m_Up;
        virtual public UCL_TC_LookAt Init(Transform target, Vector3 look_target, Vector3 up) {
            m_Target = target;
            m_LookTarget = look_target;
            m_Up = up;
            return this;
        }
        virtual public UCL_TC_LookAt Init(Transform target, Transform look_target, Vector3 up) {
            m_Target = target;
            m_LookTargetTransform = look_target;
            m_LookTarget = m_LookTargetTransform.position;
            m_Up = up;
            return this;
        }
        protected internal override void Start() {
            if(m_LookTargetTransform != null) {
                m_LookTarget = m_LookTargetTransform.position;
            }
            if(m_Local) {
                m_StartVal = m_Target.localRotation;
                var del = m_LookTarget - m_Target.localPosition;
                m_TargetVal = Quaternion.LookRotation(del, m_Up);
            } else {
                m_StartVal = m_Target.rotation;
                var del = m_LookTarget - m_Target.position;
                m_TargetVal = Quaternion.LookRotation(del, m_Up);
            }
        }
        protected override void ComponentUpdate(float pos) {
            if(m_LookTargetTransform != null) {
                m_LookTarget = m_LookTargetTransform.position;
                if(m_Local) {
                    m_TargetVal = Quaternion.LookRotation(m_LookTarget - m_Target.localPosition, m_Up);
                } else {
                    m_TargetVal = Quaternion.LookRotation(m_LookTarget - m_Target.position, m_Up);
                }
            }

            if(m_Local) {
                m_Target.transform.localRotation = Quaternion.Lerp(m_StartVal, m_TargetVal, pos);
            } else {
                m_Target.transform.rotation = Quaternion.Lerp(m_StartVal, m_TargetVal, pos);
            }
        }
    }
}