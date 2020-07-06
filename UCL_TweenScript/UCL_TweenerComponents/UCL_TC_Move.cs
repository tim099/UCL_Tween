using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public static partial class Extension {
        static public UCL_TC_Move TC_Move(this Transform target, Vector3 target_position) {
            return UCL_TC_Move.Create().Init(target, target_position);
        }
        static public UCL_TC_Move TC_Move(this Transform target, float x, float y, float z) {
            return UCL_TC_Move.Create().Init(target, x,y,z);
        }

        static public UCL_TC_Move TC_LocalMove(this Transform target, Vector3 target_position) {
            var obj = UCL_TC_Move.Create();
            obj.SetLocal(true);
            return obj.Init(target, target_position);
        }
        static public UCL_TC_Move TC_LocalMove(this Transform target, float x, float y, float z) {
            var obj = UCL_TC_Move.Create();
            obj.SetLocal(true);
            return obj.Init(target, x, y, z);
        }
    }
    public class UCL_TC_Move : UCL_TC_Transform {
        override public TC_Type GetTC_Type() { return TC_Type.Move; }

        protected Vector3 m_TargetVal;
        protected Vector3 m_StartVal;
        public static UCL_TC_Move Create() {
            return new UCL_TC_Move();
        }
        virtual public UCL_TC_Move Init(Transform target, Vector3 target_position) {
            m_Target = target;
            m_TargetVal = target_position;
            return this;
        }
        virtual public UCL_TC_Move Init(Transform target, float x, float y, float z) {
            return Init(target, new Vector3(x,y,z));
        }
        protected internal override void Start() {
            if(m_Local) {
                m_StartVal = m_Target.localPosition;
            } else {
                m_StartVal = m_Target.position;
            }
        }
        protected override void ComponentUpdate(float pos) {
            if(m_TargetTransform) {
                if(m_Local) {
                    m_TargetVal = m_TargetTransform.localPosition;
                } else {
                    m_TargetVal = m_TargetTransform.position;
                }
            }
            if(m_Local) {
                m_Target.transform.localPosition = Core.MathLib.Lib.Lerp(m_StartVal, m_TargetVal, pos);
            } else {
                m_Target.transform.position = Core.MathLib.Lib.Lerp(m_StartVal, m_TargetVal, pos);
            }
            //Debug.LogWarning("ComponentUpdate:" + pos+ ",m_StartVal:"+ m_StartVal+ ",m_TargetVal:"+ m_TargetVal);
        }
    }
}