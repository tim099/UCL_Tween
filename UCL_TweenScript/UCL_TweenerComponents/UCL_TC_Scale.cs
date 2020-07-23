using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public static partial class Extension {
        static public UCL_TC_Scale TC_Scale(this Transform target, Vector3 target_scale) {
            return UCL_TC_Scale.Create().Init(target, target_scale);
        }
        static public UCL_TC_Scale TC_Scale(this Transform target, float x, float y, float z) {
            return UCL_TC_Scale.Create().Init(target, x, y, z);
        }
        static public UCL_TC_Scale TC_Scale(this Transform target, float size) {
            return UCL_TC_Scale.Create().Init(target, size, size, size);
        }

        static public UCL_TC_Scale TC_ScaleX(this Transform target, float val) {
            return UCL_TC_Scale.Create().Init(target, val, target.localScale.y, target.localScale.z);
        }
        static public UCL_TC_Scale TC_ScaleY(this Transform target, float val) {
            return UCL_TC_Scale.Create().Init(target, target.localScale.x, val, target.localScale.z);
        }
        static public UCL_TC_Scale TC_ScaleZ(this Transform target, float val) {
            return UCL_TC_Scale.Create().Init(target, target.localScale.x, target.localScale.y, val);
        }

        static public UCL_Tweener UCL_Scale(this Transform target, float duration, Vector3 target_scale) {
            return LibTween.Tweener(duration).AddComponent(TC_Scale(target, target_scale));
        }
        static public UCL_Tweener UCL_Scale(this Transform target, float duration, float x, float y, float z) {
            return LibTween.Tweener(duration).AddComponent(TC_Scale(target, x, y, z));
        }
        static public UCL_Tweener UCL_Scale(this Transform target, float duration, float size) {
            return LibTween.Tweener(duration).AddComponent(TC_Scale(target, size));
        }
        static public UCL_Tweener UCL_ScaleX(this Transform target, float duration, float val) {
            return LibTween.Tweener(duration).AddComponent(TC_ScaleX(target, val));
        }
        static public UCL_Tweener UCL_ScaleY(this Transform target, float duration, float val) {
            return LibTween.Tweener(duration).AddComponent(TC_ScaleY(target, val));
        }
        static public UCL_Tweener UCL_ScaleZ(this Transform target, float duration, float val) {
            return LibTween.Tweener(duration).AddComponent(TC_ScaleZ(target, val));
        }
    }
    public class UCL_TC_Scale : UCL_TC_Transform {
        override public TC_Type GetTC_Type() { return TC_Type.Scale; }

        protected Vector3 m_TargetVal;
        protected Vector3 m_StartVal;

        public static UCL_TC_Scale Create() {
            return new UCL_TC_Scale();
        }
        virtual public UCL_TC_Scale Init(Transform target, Vector3 target_scale) {
            m_Target = target;
            m_TargetVal = target_scale;
            return this;
        }
        virtual public UCL_TC_Scale Init(Transform target, float x, float y, float z) {
            return Init(target, new Vector3(x, y, z));
        }
        protected internal override void Start() {
            m_StartVal = m_Target.localScale;
        }
        protected override void ComponentUpdate(float pos) {
            if(m_TargetTransform) {
                m_TargetVal = m_TargetTransform.localScale;
            }
            m_Target.transform.localScale = Core.MathLib.Lib.Lerp(m_StartVal, m_TargetVal, pos);
            //Debug.LogWarning("ComponentUpdate:" + pos+ ",m_StartVal:"+ m_StartVal+ ",m_TargetVal:"+ m_TargetVal);
        }
    }
}