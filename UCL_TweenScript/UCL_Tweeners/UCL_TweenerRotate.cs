﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public static partial class Extension {
        static public UCL_TweenerRotate UCL_Rotate(this Transform target, float duration, Quaternion target_rotation) {
            return UCL_TweenerRotate.Create().Init(target, target_rotation, duration);
        }
        static public UCL_TweenerRotate UCL_Rotate(this Transform target, float duration, Vector3 target_rotation) {
            return UCL_TweenerRotate.Create().Init(target, target_rotation.x, target_rotation.y, target_rotation.z, duration);
        }
        static public UCL_TweenerRotate UCL_Rotate(this Transform target, float duration, float x, float y, float z) {
            return UCL_TweenerRotate.Create().Init(target, Quaternion.Euler(x, y, z), duration);
        }
        static public UCL_TweenerRotate UCL_Rotate(this Transform target, float duration, Transform target_rotation) {
            return UCL_TweenerRotate.Create().Init(target, target_rotation.rotation, duration);
        }
    }
    public class UCL_TweenerRotate : UCL_TweenerTransform {
        protected Quaternion m_TargetVal;
        protected Quaternion m_StartVal;

        public static UCL_TweenerRotate Create() {
            var obj = new UCL_TweenerRotate();
            return obj;
        }
        virtual public UCL_TweenerRotate Init(Transform target, Vector3 target_rotation, float duration) {
            Init(target, Quaternion.Euler(target_rotation.x, target_rotation.y, target_rotation.z), duration);
            return this;
        }
        virtual public UCL_TweenerRotate Init(Transform target, float x, float y, float z, float duration) {
            Init(target, Quaternion.Euler(x, y, z), duration);
            return this;
        }
        virtual public UCL_TweenerRotate Init(Transform target, Quaternion target_rotation, float duration) {
            m_Target = target;
            m_TargetVal = target_rotation;
            Duration = duration;

            return this;
        }
        protected override void TweenerStart() {
            m_StartVal = m_Target.rotation;
        }
        protected override void TweenerUpdate(float pos) {
            m_Target.transform.rotation = Core.MathLib.Lib.Lerp(m_StartVal, m_TargetVal, pos);


        }
    }
}