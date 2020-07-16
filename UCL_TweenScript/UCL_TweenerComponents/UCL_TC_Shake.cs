using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TC_Shake : UCL_TC_Transform {
        protected int m_RandSeed = 0;
        protected float m_Range;
        protected float m_StartVelocity;//Velocity of Shake

        protected Vector3 m_TargetVal;
        protected Vector3 m_StartVal;

        Core.MathLib.UCL_Random m_Rnd;
        //public 
        override public TC_Type GetTC_Type() { return TC_Type.Shake; }
        public static UCL_TC_Shake Create() {
            return new UCL_TC_Shake();
        }
        virtual public UCL_TC_Shake Init(Transform target, float _Range,float _StartVel) {
            m_Target = target;
            m_Range = _Range;
            m_StartVelocity = _StartVel;
            return this;
        }
        virtual public UCL_TC_Shake SetSeed(int seed) {
            m_RandSeed = seed;
            return this;
        }
        protected internal override void Start() {
            m_Rnd = new Core.MathLib.UCL_Random(m_RandSeed);
            if(m_Local) {
                m_StartVal = m_Target.localPosition;
                m_TargetVal = m_StartVal + m_Range * m_Rnd.InUnitSphere();
            } else {
                m_StartVal = m_Target.position;
                m_TargetVal = m_Target.TransformPoint(m_Range * m_Rnd.InUnitSphere());
            }
            


        }
    }
}