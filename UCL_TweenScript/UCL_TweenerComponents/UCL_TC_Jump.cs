using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib
{
    #region Extension
    public static partial class Extension {
        /// <summary>
        /// Create a TC_Jump
        /// </summary>
        /// <param name="iTarget">Move target</param>
        /// <param name="iTargetTransform">Target Transform that target will move to</param>
        /// <param name="iJumpTimes">Jump times</param>
        /// <param name="iUp">Up vector of jump , etc. (0,1,0)</param>
        /// <param name="iHeight">Initial Jump height</param>
        /// <param name="iBounciness">Height decade after each jump, if == 1 then the height stay the same each jump</param>
        /// <returns></returns>
        static public UCL_TC_Jump TC_Jump(this Transform iTarget, Transform iTargetTransform, int iJumpTimes, Vector3 iUp, float iHeight, float iBounciness) {
            return UCL_TC_Jump.Create().Init(iTarget, iTargetTransform, iJumpTimes, iUp, iHeight, iBounciness);
        }
        /// <summary>
        /// Create a TC_Jump
        /// </summary>
        /// <param name="iTarget">Move target</param>
        /// <param name="iTargetPosition">Target position that target will move to</param>
        /// <param name="iJumpTimes">Jump times</param>
        /// <param name="iUp">Up vector of jump , etc. (0,1,0)</param>
        /// <param name="iHeight">Initial Jump height</param>
        /// <param name="iBounciness">Height decade after each jump, if == 1 then the height stay the same each jump</param>
        /// <returns></returns>
        static public UCL_TC_Jump TC_Jump(this Transform iTarget, Vector3 iTargetPosition, int iJumpTimes, Vector3 iUp, float iHeight, float iBounciness) {
            return UCL_TC_Jump.Create().Init(iTarget, iTargetPosition, iJumpTimes, iUp, iHeight, iBounciness);
        }

        /// <summary>
        /// Create a Tweener contains TC_Jump
        /// </summary>
        /// <param name="iDuration">duration of tweener</param>
        /// <param name="iTarget">Move target</param>
        /// <param name="iTargetTransform">Target Transform that target will move to</param>
        /// <param name="iJumpTimes">Jump times</param>
        /// <param name="iUp">Up vector of jump , etc. (0,1,0)</param>
        /// <param name="iHeight">Initial Jump height</param>
        /// <param name="iBounciness">Height decade after each jump, if == 1 then the height stay the same each jump</param>
        /// <returns></returns>
        static public UCL_Tweener UCL_Jump(this Transform iTarget, float iDuration, Transform iTargetTransform, int iJumpTimes,
            Vector3 iUp, float iHeight, float iBounciness) {
            return LibTween.Tweener(iDuration).AddComponent(TC_Jump(iTarget, iTargetTransform, iJumpTimes, iUp, iHeight, iBounciness));
        }

        /// <summary>
        /// Create a Tweener contains TC_Jump
        /// </summary>
        /// <param name="iDuration">duration of tweener</param>
        /// <param name="iTarget">Move target</param>
        /// <param name="iTargetPosition">Target position that target will move to</param>
        /// <param name="iJumpTimes">Jump times</param>
        /// <param name="iUp">Up vector of jump , etc. (0,1,0)</param>
        /// <param name="iHeight">Initial Jump height</param>
        /// <param name="iBounciness">Height decade after each jump, if == 1 then the height stay the same each jump</param>
        /// <returns></returns>
        static public UCL_Tweener UCL_Jump(this Transform iTarget, float iDuration, Vector3 iTargetPosition, int iJumpTimes,
            Vector3 iUp, float iHeight, float iBounciness) {
            return LibTween.Tweener(iDuration).AddComponent(TC_Jump(iTarget, iTargetPosition, iJumpTimes, iUp, iHeight, iBounciness));
        }
    }
    #endregion
    public class UCL_TC_Jump : UCL_TC_Transform
    {

#if UNITY_EDITOR
        public override string OnInspectorGUITips() {
            var tips = base.OnInspectorGUITips();
            tips += "\"TargetVal\" is target position that \"Target\" will move to " +
                "(if \"TargetTransform\" is not null ,\"Target\" will move to \"TargetTransform\" instead)\n" +
                "\"JumpTimes\" is total jump times ,\n\"Up\" is the up vector of jump direction etc.(0,1,0)\n" +
                "\"Bounciness\" is the height decade after each jump, if \"Bounciness\" == 1 then the height stay the same each jump";
            return tips;
        }
#endif
        public static UCL_TC_Jump Create() {
            return new UCL_TC_Jump();
        }
        override public TC_Type GetTC_Type() { return TC_Type.Jump; }

        /// <summary>
        /// Jump times 
        /// </summary>
        [SerializeField] protected int m_JumpTimes = 1;

        /// <summary>
        /// Up vector of jump , etc. (0,1,0)
        /// </summary>
        [SerializeField] protected Vector3 m_Up = Vector3.up;

        /// <summary>
        /// Initial Jump height
        /// </summary>
        [SerializeField] protected float m_Height = 1f;

        /// <summary>
        /// Height decade after each jump, if == 1 then the height stay the same each jump
        /// </summary>
        [SerializeField] protected float m_Bounciness = 1f;
        /// <summary>
        /// Target position that "Target" will move to
        /// </summary>
        [SerializeField] protected Vector3 m_TargetVal;

        /// <summary>
        /// Start position of "Target"
        /// </summary>
        protected Vector3 m_StartVal;

        virtual public UCL_TC_Jump Init(Transform iTarget, Transform iTargetTransform, int iJumpTimes, Vector3 iUp, float iHeight, float iBounciness) {
            m_Target = iTarget;
            m_TargetTransform = iTargetTransform;
            m_JumpTimes = iJumpTimes;
            m_Up = iUp.normalized;
            m_Height = iHeight;
            m_Bounciness = iBounciness;
            return this;
        }
        virtual public UCL_TC_Jump Init(Transform target, Vector3 _TargetVal, int _JumpTimes, Vector3 _Up, float _Height, float _Bounciness) {
            m_Target = target;
            m_TargetVal = _TargetVal;
            m_JumpTimes = _JumpTimes;
            m_Up = _Up.normalized;
            m_Height = _Height;
            m_Bounciness = _Bounciness;
            return this;
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

            Vector3 cur_pos = m_StartVal;

            if(!m_StartVal.Equals(m_TargetVal)) cur_pos = Core.MathLib.Lib.Lerp(m_StartVal, m_TargetVal, pos);
            Vector3 height = m_Up;
            
            if(m_JumpTimes > 0) {
                float r_pos = pos * m_JumpTimes;
                int seg = Mathf.FloorToInt(r_pos);
                float l_pos = r_pos - seg - 0.5f;

                float h = 1f - 4f * l_pos * l_pos;
                if(h != 0 && m_Bounciness != 0) {
                    h *= Mathf.Pow(m_Bounciness, seg);
                }
                
                Vector3 dir = m_TargetVal - m_StartVal;
                if (dir.magnitude > 0.001f)
                {
                    height = m_Up - Vector3.Project(m_Up, dir);
                    if (height.Equals(Vector3.zero))
                    {
                        height = m_Up;
                    }
                }
                else
                {
                    height = m_Up;
                }

                float l = height.magnitude;
                if(l > 0 && l != 1) {
                    height.Normalize();
                }
                //Debug.LogWarning("h:" + h + ",m_Height:" + m_Height + ",height:" + height+ ",dir:"+ dir+ ",m_Up:"+ m_Up);
                height = h * m_Height * height;
                //Debug.LogWarning("h:" + h + ",m_Height:" + m_Height + ",2height:" + height);
            }
            
            if(m_Local) {
                m_Target.transform.localPosition = cur_pos + height;
            } else {
                m_Target.transform.position = cur_pos + height;
            }
            //Debug.LogWarning("ComponentUpdate:" + pos+ ",m_StartVal:"+ m_StartVal+ ",m_TargetVal:"+ m_TargetVal+ ",height:"+ height+ ",cur_pos:"+ cur_pos);
        }
    }
}