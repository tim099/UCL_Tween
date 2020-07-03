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
        /*
        static public UCL_TC_Rotate TC_LookAt(this Transform target, Vector3 target_position, Vector3 up) {
            //target_position
            return UCL_TC_Rotate.Create().Init(target, target_position.x, target_position.y, target_position.z);
        }
        */
    }
    public class UCL_TC_Rotate : UCL_TC_Transform {
        override protected TC_Type GetTC_Type() { return TC_Type.Rotate; }

        protected Quaternion m_TargetVal;
        protected Quaternion m_StartVal;
        public static UCL_TC_Rotate Create() {
            return new UCL_TC_Rotate();
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
            if(m_Local) {
                m_Target.transform.localRotation = Quaternion.Lerp(m_StartVal, m_TargetVal, pos);
            } else {
                m_Target.transform.rotation = Quaternion.Lerp(m_StartVal, m_TargetVal, pos);
            }
        }
    }
}