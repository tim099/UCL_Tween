using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public static partial class Extension {
        /// <summary>
        /// Move target to target_position
        /// </summary>
        /// <param name="target">move target</param>
        /// <param name="target_position">target position</param>
        /// <returns></returns>
        static public UCL_TC_Move TC_Move(this Transform target, Vector3 target_position) {
            return UCL_TC_Move.Create().Init(target, target_position);
        }
        /// <summary>
        /// Move target to Vector3(x,y,z)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        static public UCL_TC_Move TC_Move(this Transform target, float x, float y, float z) {
            return UCL_TC_Move.Create().Init(target, x,y,z);
        }
        /// <summary>
        /// Move target to target_transform
        /// </summary>
        /// <param name="target"></param>
        /// <param name="target_transform"></param>
        /// <returns></returns>
        static public UCL_TC_Move TC_Move(this Transform target, Transform target_transform) {
            return UCL_TC_Move.Create().Init(target, target_transform);
        }

        static public UCL_TC_Move TC_LocalMove(this Transform target, Transform target_transform) {
            var obj = UCL_TC_Move.Create();
            obj.SetLocal(true);
            return obj.Init(target, target_transform);
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

        /// <summary>
        /// Move target to target_transform in duration(Second)
        /// </summary>
        /// <param name="target">move target</param>
        /// <param name="duration">duration in second</param>
        /// <param name="target_transform">target position</param>
        /// <returns></returns>
        static public UCL_Tweener UCL_Move(this Transform target, float duration, Transform target_transform) {
            return LibTween.Tweener(duration).AddComponent(TC_Move(target, target_transform));
        }
        static public UCL_Tweener UCL_Move(this Transform target, float duration, Vector3 target_position) {
            return LibTween.Tweener(duration).AddComponent(TC_Move(target, target_position));
        }

        static public UCL_Tweener UCL_LocalMove(this Transform target, float duration, Transform target_transform) {
            return LibTween.Tweener(duration).AddComponent(TC_LocalMove(target, target_transform));
        }
        static public UCL_Tweener UCL_LocalMove(this Transform target, float duration, Vector3 target_position) {
            return LibTween.Tweener(duration).AddComponent(TC_LocalMove(target, target_position));
        }
        static public UCL_Tweener UCL_LocalMove(this Transform target, float duration, float x, float y, float z) {
            return LibTween.Tweener(duration).AddComponent(TC_LocalMove(target, x, y, z));
        }
    }
    public class UCL_TC_Move : UCL_TC_Transform {
        #region EDITOR
#if UNITY_EDITOR
        public override string OnInspectorGUITips() {
            var tips = base.OnInspectorGUITips();
            tips += "\"TargetVal\" is target position that \"Target\" will move to " +
                "(if \"TargetTransform\" is not null ,\"Target\" will move to \"TargetTransform\" instead)";
            return tips;
        }
#endif
        #endregion
        override public TC_Type GetTC_Type() { return TC_Type.Move; }

        /// <summary>
        /// Target position that "Target" will move to
        /// </summary>
        protected Vector3 m_TargetVal;
        [HideInInspector] protected Vector3 m_StartVal;
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
        virtual public UCL_TC_Move Init(Transform target, Transform _TargetTransform) {
            m_Target = target;
            m_TargetTransform = _TargetTransform;
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
            if(m_Local) {
                m_Target.transform.localPosition = Core.MathLib.Lib.Lerp(m_StartVal, m_TargetVal, pos);
            } else {
                m_Target.transform.position = Core.MathLib.Lib.Lerp(m_StartVal, m_TargetVal, pos);
            }
            //Debug.LogWarning("ComponentUpdate:" + pos+ ",m_StartVal:"+ m_StartVal+ ",m_TargetVal:"+ m_TargetVal);
        }
    }
}