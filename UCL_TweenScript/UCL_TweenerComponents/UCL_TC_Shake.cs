using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TC_Shake : UCL_TC_Transform {
        protected int m_RandSeed = 0;
        protected float m_Range = 1f;
        protected int m_ShakeTimes = 10;//Shake times

        protected Vector3 m_TargetVal;
        protected Vector3 m_StartVal;

        protected Core.MathLib.UCL_Random m_Rnd;
        protected List<Vector3> m_ShakePos;
        //public 
        override public TC_Type GetTC_Type() { return TC_Type.Shake; }
        public static UCL_TC_Shake Create() {
            return new UCL_TC_Shake();
        }
        virtual public UCL_TC_Shake Init(Transform target, float _Range,int _ShakeTimes) {
            Init(target,_Range,_ShakeTimes, Core.MathLib.UCL_Random.Instance.Next());
            return this;
        }
        virtual public UCL_TC_Shake Init(Transform target, float _Range, int _ShakeTimes,int _seed) {
            m_Target = target;
            m_Range = _Range;
            m_ShakeTimes = _ShakeTimes;
            m_RandSeed = _seed;
            return this;
        }
        virtual public UCL_TC_Shake SetSeed(int seed) {
            m_RandSeed = seed;
            return this;
        }
        public override void OnDrawGizmos() {
            if(m_ShakePos == null) return;
            //base.OnDrawGizmos();
            foreach(var pos in m_ShakePos) {
                Core.UCL_DrawGizmos.DrawConstSizeSphere(pos, 1f);
            }
            
        }
        protected internal override void Start() {
            m_Rnd = new Core.MathLib.UCL_Random(m_RandSeed);
            m_ShakePos = new List<Vector3>();
            if(m_Local) {
                m_StartVal = m_Target.localPosition;
                m_ShakePos.Add(Vector3.zero);
            } else {
                m_StartVal = m_Target.position;
                m_ShakePos.Add(m_Target.position);
            }
            
            for(int i = 0; i < m_ShakeTimes; i++) {
                if(m_Local) {
                    m_ShakePos.Add(m_Range * m_Rnd.OnUnitSphere());
                } else {
                    m_ShakePos.Add(m_Target.TransformPoint(m_Range * m_Rnd.OnUnitSphere()));
                }
            }

            if(m_Local) {
                m_ShakePos.Add(Vector3.zero);
            } else {
                m_ShakePos.Add(m_Target.position);
            }
        }
        protected override void ComponentUpdate(float pos) {
            if(m_Local) {
                m_Target.transform.localPosition = Core.MathLib.LinearMapping.GetValue(m_ShakePos, pos);
            } else {
                m_Target.transform.position = Core.MathLib.LinearMapping.GetValue(m_ShakePos, pos);
            }
            
            //Debug.LogWarning("ComponentUpdate:" + pos+ ",m_StartVal:"+ m_StartVal+ ",m_TargetVal:"+ m_TargetVal);
        }
    }
}