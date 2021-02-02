using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib
{
    public class UCL_TC_EulerRotation : UCL_TC_Transform {

        override public TC_Type GetTC_Type() { return TC_Type.Rotate; }
        protected Vector3 m_RotateVal;
        [HideInInspector] protected Quaternion m_StartVal;

        public static UCL_TC_EulerRotation Create()
        {
            return new UCL_TC_EulerRotation();
        }
        public override UCL_TweenerComponent Init()
        {
            return this;
        }
        virtual public UCL_TC_EulerRotation Init(Transform iTarget, Vector3 iTargetRotation)
        {
            m_Target = iTarget;
            m_RotateVal = iTargetRotation;
            return this;
        }
        virtual public UCL_TC_EulerRotation Init(Transform iTarget, float x, float y, float z)
        {
            Init(iTarget, new Vector3(x, y, z));
            return this;
        }
        protected internal override void Start()
        {
            if (m_Local)
            {
                m_StartVal = m_Target.localRotation;
            }
            else
            {
                m_StartVal = m_Target.rotation;
            }
        }
        protected override void ComponentUpdate(float pos)
        {
            if (m_TargetTransform)
            {
                if (m_Local)
                {
                    m_RotateVal = m_TargetTransform.localRotation.eulerAngles;
                }
                else
                {
                    m_RotateVal = m_TargetTransform.rotation.eulerAngles;
                }
            }
            if (m_Local)
            {
                m_Target.transform.localRotation = m_StartVal * Quaternion.Euler(pos * m_RotateVal);
            }
            else
            {
                m_Target.transform.rotation = m_StartVal * Quaternion.Euler(pos * m_RotateVal);
            }
            //Debug.LogWarning("ComponentUpdate:" + pos + ",m_StartVal:" + m_StartVal + ",m_TargetVal:" + m_TargetVal+
            //",m_Target.transform.rotation:"+ m_Target.transform.rotation);
        }
    }
}