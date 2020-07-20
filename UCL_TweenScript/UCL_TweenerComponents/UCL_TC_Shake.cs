using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public static partial class Extension {
        /// <summary>
        /// Create a UCL_TC_Shake
        /// </summary>
        /// <param name="_Range">Max Range of shake</param> 
        /// <param name="_ShakeTimes">shaking times</param> 
        /// <param name="_fade">shaking will fade if true</param> 
        static public UCL_TC_Shake TC_Shake(this Transform target, float _Range, int _ShakeTimes, bool _fade = true) {
            return UCL_TC_Shake.Create().Init(target, _Range, _ShakeTimes).SetFade(_fade);
        }
        /// <summary>
        /// Create a UCL_TC_Shake
        /// </summary>
        /// <param name="_Range">Max Range of shake</param> 
        /// <param name="_ShakeTimes">shaking times</param> 
        /// <param name="_fade">shaking will fade if true</param> 
        /// <param name="_seed">use seed to control the random shake position</param> 
        static public UCL_TC_Shake TC_Shake(this Transform target, float _Range, int _ShakeTimes, int _seed, bool _fade = true) {
            return UCL_TC_Shake.Create().Init(target, _Range, _ShakeTimes, _seed).SetFade(_fade);
        }
        /// <summary>
        /// Create a UCL_TC_Shake
        /// </summary>
        /// <param name="_Range">Max Range of shake</param> 
        /// <param name="_ShakeTimes">shaking times</param> 
        /// <param name="_fade">shaking will fade if true</param> 
        static public UCL_TC_Shake TC_LocalShake(this Transform target, float _Range, int _ShakeTimes, bool _fade = true) {
            var obj = UCL_TC_Shake.Create();
            obj.SetLocal(true);
            return obj.Init(target, _Range, _ShakeTimes).SetFade(_fade);
        }
        /// <summary>
        /// Create a UCL_TC_Shake
        /// </summary>
        /// <param name="_Range">Max Range of shake</param> 
        /// <param name="_ShakeTimes">shaking times</param> 
        /// <param name="_fade">shaking will fade if true</param> 
        /// <param name="_seed">use seed to control the random shake position</param> 
        static public UCL_TC_Shake TC_LocalShake(this Transform target, float _Range, int _ShakeTimes, int _seed, bool _fade = true) {
            var obj = UCL_TC_Shake.Create();
            obj.SetLocal(true);
            return obj.Init(target, _Range, _ShakeTimes, _seed).SetFade(_fade);
        }

        /// <summary>
        /// Create a tweener do TC_Shake on target transform
        /// </summary>
        /// <param name="_Range">Max Range of shake</param> 
        /// <param name="_ShakeTimes">shaking times</param> 
        /// <param name="_fade">shaking will fade if true</param> 
        static public UCL_Tweener UCL_Shake(this Transform target, float duration, float _Range, int _ShakeTimes, bool _fade = true) {
            return LibTween.Tweener(duration).AddComponent(TC_Shake(target, _Range, _ShakeTimes, _fade));
        }

        /// <summary>
        /// Create a tweener do TC_Shake on target transform
        /// </summary>
        /// <param name="_Range">Max Range of shake</param> 
        /// <param name="_ShakeTimes">shaking times</param> 
        /// <param name="_fade">shaking will fade if true</param> 
        /// <param name="_seed">use seed to control the random shake position</param> 
        static public UCL_Tweener UCL_Shake(this Transform target, float duration,
            float _Range, int _ShakeTimes, int _seed, bool _fade = true) {
            return LibTween.Tweener(duration).AddComponent(TC_Shake(target, _Range, _ShakeTimes, _seed, _fade));
        }

        /// <summary>
        /// Create a tweener do TC_LocalShake on target transform
        /// </summary>
        /// <param name="_Range">Max Range of shake</param> 
        /// <param name="_ShakeTimes">shaking times</param> 
        /// <param name="_fade">shaking will fade if true</param> 
        static public UCL_Tweener UCL_LocalShake(this Transform target, float duration, float _Range, int _ShakeTimes, bool _fade = true) {
            return LibTween.Tweener(duration).AddComponent(TC_LocalShake(target, _Range, _ShakeTimes, _fade));
        }

        /// <summary>
        /// Create a tweener do TC_LocalShake on target transform
        /// </summary>
        /// <param name="duration">Duration of the tween</param> 
        /// <param name="_Range">Max Range of shake</param> 
        /// <param name="_ShakeTimes">shaking times</param> 
        /// <param name="_fade">shaking will fade if true</param> 
        /// <param name="_seed">use seed to control the random shake position</param> 
        static public UCL_Tweener UCL_LocalShake(this Transform target, float duration,
            float _Range, int _ShakeTimes, int _seed, bool _fade = true) {
            return LibTween.Tweener(duration).AddComponent(TC_LocalShake(target, _Range, _ShakeTimes, _seed, _fade));
        }
    }
    public class UCL_TC_Shake : UCL_TC_Transform {
        protected bool m_UseRandomSeed = false;
        protected bool m_Fade = true;
        protected int m_RandSeed = 0;
        protected float m_Range = 1f;
        protected int m_ShakeTimes = 10;//Shake times

        //protected Vector3 m_TargetVal;
        //protected Vector3 m_StartVal;

        [HideInInspector] protected Core.MathLib.UCL_Random m_Rnd;
        protected List<Vector3> m_ShakePos;
        protected List<float> m_PosDis;
        //public 
        override public TC_Type GetTC_Type() { return TC_Type.Shake; }
        public static UCL_TC_Shake Create() {
            return new UCL_TC_Shake();
        }
        virtual public UCL_TC_Shake Init(Transform target, float _Range,int _ShakeTimes) {
            Init(target,_Range,_ShakeTimes, 0);
            m_UseRandomSeed = false;
            return this;
        }
        virtual public UCL_TC_Shake Init(Transform target, float _Range, int _ShakeTimes, int _seed) {
            m_Target = target;
            m_Range = _Range;
            m_ShakeTimes = _ShakeTimes;
            m_RandSeed = _seed;
            m_UseRandomSeed = true;
            return this;
        }
        virtual public UCL_TC_Shake SetFade(bool val) {
            m_Fade = val;
            return this;
        }
        virtual public UCL_TC_Shake SetSeed(int seed) {
            m_RandSeed = seed;
            return this;
        }
        public override void OnDrawGizmos() {
            if(m_ShakePos == null) return;
            //base.OnDrawGizmos();
            if(m_Local) {
                if(m_Target != null) {
                    var parent = m_Target.parent;
                    if(parent != null) {
                        var prev = Gizmos.color;
                        Gizmos.color = Color.green;
                        foreach(var pos in m_ShakePos) {
                            Core.UCL_DrawGizmos.DrawConstSizeSphere(parent.TransformPoint(pos), 1f);
                        }
                        Gizmos.color = prev;
                    }
                }

            } else {
                var prev = Gizmos.color;
                Gizmos.color = Color.green;
                foreach(var pos in m_ShakePos) {
                    Core.UCL_DrawGizmos.DrawConstSizeSphere(pos, 1f);
                }
                Gizmos.color = prev;
            }

            
        }
        protected internal override void Start() {
            if(m_UseRandomSeed) {
                m_Rnd = new Core.MathLib.UCL_Random(m_RandSeed);
            } else {
                m_Rnd = new Core.MathLib.UCL_Random();
            }
            
            m_ShakePos = new List<Vector3>();
            m_PosDis = new List<float>();
            if(m_Local) {
                //m_StartVal = m_Target.localPosition;
                m_ShakePos.Add(Vector3.zero);
                float range = m_Range;
                for(int i = 0; i < m_ShakeTimes; i++) {
                    if(m_Fade) range = ((m_ShakeTimes - i) / (float)m_ShakeTimes) * m_Range;
                    m_ShakePos.Add(range * m_Rnd.OnUnitSphere());
                }
                m_ShakePos.Add(Vector3.zero);
            } else {
                //m_StartVal = m_Target.position;
                m_ShakePos.Add(m_Target.position);
                float range = m_Range;
                for(int i = 0; i < m_ShakeTimes; i++) {
                    if(m_Fade) range = ((m_ShakeTimes - i) / (float)m_ShakeTimes) * m_Range;
                    //m_ShakePos.Add(m_Target.TransformPoint(range * m_Rnd.OnUnitCircle()));
                    m_ShakePos.Add(m_Target.TransformPoint(range * m_Rnd.OnUnitSphere()));
                }
                m_ShakePos.Add(m_Target.position);
            }
            {
                int len = m_ShakePos.Count - 1;
                int at = 0;
                m_PosDis = Core.MathLib.LinearMapping.GetLength(m_ShakePos, delegate (Vector3 a, Vector3 b) {
                    float seg_len = (a - b).magnitude;
                    if(m_Fade) seg_len = ((float)len / (len - at++)) * seg_len;
                    return seg_len;
                });
            }

            /*
            float m_TotalLen = 0;
            m_PosDis.Add(0);
            for(int i=0,len = m_ShakePos.Count-1; i < len; i++) {
                float seg_len = (m_ShakePos[i] - m_ShakePos[i + 1]).magnitude;
                if(m_Fade) seg_len = ((float)len / (len - i)) * seg_len;
                m_TotalLen += seg_len;
                m_PosDis.Add(m_TotalLen);
            }
            for(int i = 1, len = m_PosDis.Count; i < len; i++) {
                m_PosDis[i] = m_PosDis[i] / m_TotalLen;
                //Debug.LogWarning("m_PosDis["+i+"]:" + m_PosDis[i]);
            }
            */
        }
        protected override void ComponentUpdate(float pos) {
            //var prev = pos;
            pos = Core.MathLib.LinearMapping.GetX(m_PosDis, pos);
            //Debug.Log("prev:" + prev + ",pos:" + pos);
            if(m_Local) {
                m_Target.transform.localPosition = Core.MathLib.LinearMapping.GetValue(m_ShakePos, pos);
            } else {
                m_Target.transform.position = Core.MathLib.LinearMapping.GetValue(m_ShakePos, pos);
            }
            
            //Debug.LogWarning("ComponentUpdate:" + pos+ ",m_StartVal:"+ m_StartVal+ ",m_TargetVal:"+ m_TargetVal);
        }
    }
}