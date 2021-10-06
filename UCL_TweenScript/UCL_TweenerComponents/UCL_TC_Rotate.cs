using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public static partial class Extension {
        static public UCL_TC_Rotate TC_Rotate(this Transform target, Quaternion target_rotation) {
            return UCL_TC_Rotate.Create().Init(target, target_rotation);
        }
        static public UCL_TC_Rotate TC_Rotate(this Transform target, Vector3 target_rotation) {
            return UCL_TC_Rotate.Create().Init(target, target_rotation.x, target_rotation.y, target_rotation.z);
        }
        static public UCL_TC_Rotate TC_Rotate(this Transform target, float x, float y, float z) {
            return UCL_TC_Rotate.Create().Init(target, Quaternion.Euler(x, y, z));
        }

        static public UCL_TC_Rotate TC_LocalRotate(this Transform target, Quaternion target_rotation) {
            var obj = UCL_TC_Rotate.Create();
            obj.SetLocal(true);
            return obj.Init(target, target_rotation);
        }
        static public UCL_TC_Rotate TC_LocalRotate(this Transform target, Vector3 target_rotation) {
            var obj = UCL_TC_Rotate.Create();
            obj.SetLocal(true);
            return obj.Init(target, target_rotation.x, target_rotation.y, target_rotation.z);
        }
        static public UCL_TC_Rotate TC_LocalRotate(this Transform target, float x, float y, float z) {
            var obj = UCL_TC_Rotate.Create();
            obj.SetLocal(true);
            return obj.Init(target, Quaternion.Euler(x, y, z));
        }

        static public UCL_Tweener UCL_LocalRotate(this Transform target, float duration, Quaternion target_rotation) {
            return LibTween.Tweener(duration).AddComponent(TC_LocalRotate(target, target_rotation));
        }
        static public UCL_Tweener UCL_LocalRotate(this Transform target, float duration, Vector3 target_rotation) {
            return LibTween.Tweener(duration).AddComponent(TC_LocalRotate(target, target_rotation));
        }
        static public UCL_Tweener UCL_LocalRotate(this Transform target, float duration, float x, float y, float z) {
            return LibTween.Tweener(duration).AddComponent(TC_LocalRotate(target, x, y, z));
        }
    }
    public class UCL_TC_Rotate : UCL_TC_Transform {
        override public TC_Type GetTC_Type() { return TC_Type.Rotate; }

        [HideInInspector] protected Quaternion m_TargetVal;
        [HideInInspector] protected Quaternion m_StartVal;
        public static UCL_TC_Rotate Create() {
            return new UCL_TC_Rotate();
        }
        public override UCL_TweenerComponent Init() {
            return this;
        }
        virtual public UCL_TC_Rotate Init(Transform target, Quaternion target_rotation) {
            m_Target = target;
            m_TargetVal = target_rotation;

            return this;
        }
        virtual public UCL_TC_Rotate Init(Transform target, Vector3 target_rotation) {
            Init(target, Quaternion.Euler(target_rotation.x, target_rotation.y, target_rotation.z));
            return this;
        }
        virtual public UCL_TC_Rotate Init(Transform target, float x, float y, float z) {
            Init(target, Quaternion.Euler(x, y, z));
            return this;
        }
        protected internal override void Start() {
            if(m_Local) {
                m_StartVal = m_Target.localRotation;
            } else {
                m_StartVal = m_Target.rotation;
            }
        }
        protected override void ComponentUpdate(float pos) {
            if(m_TargetTransform) {
                if(m_Local) {
                    m_TargetVal = m_TargetTransform.localRotation;
                } else {
                    m_TargetVal = m_TargetTransform.rotation;
                }
            }
            if(m_Local) {
                m_Target.transform.localRotation = Core.MathLib.Lib.Lerp(m_StartVal, m_TargetVal, pos);
            } else {
                m_Target.transform.rotation = Core.MathLib.Lib.Lerp(m_StartVal, m_TargetVal, pos);
            }
            //Debug.LogWarning("ComponentUpdate:" + pos + ",m_StartVal:" + m_StartVal + ",m_TargetVal:" + m_TargetVal+
                //",m_Target.transform.rotation:"+ m_Target.transform.rotation);
        }
    }
}