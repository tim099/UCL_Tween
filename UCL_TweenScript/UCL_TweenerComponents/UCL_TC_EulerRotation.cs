using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib
{
    #region Extension
    public static partial class Extension
    {
        /// <summary>
        /// Create a  TC_EulerRotation
        /// </summary>
        /// <param name="iTarget">Rotaion target</param>
        /// <param name="iRotation">rotaion euler angle</param>
        /// <returns></returns>
        static public UCL_TC_EulerRotation TC_EulerRotation(this Transform iTarget, Vector3 iRotation)
        {
            return UCL_TC_EulerRotation.Create().Init(iTarget, iRotation);
        }
        /// <summary>
        ///  Create a  TC_EulerRotation
        /// </summary>
        /// <param name="iTarget">Rotaion target</param>
        /// <param name="iX"></param>
        /// <param name="iY"></param>
        /// <param name="iZ"></param>
        /// <returns></returns>
        static public UCL_TC_EulerRotation TC_EulerRotation(this Transform iTarget, float iX, float iY, float iZ)
        {
            return UCL_TC_EulerRotation.Create().Init(iTarget, iX, iY, iZ);
        }

        /// <summary>
        /// Create a  TC_EulerRotation
        /// </summary>
        /// <param name="iTarget"></param>
        /// <param name="iX"></param>
        /// <returns></returns>
        static public UCL_TC_EulerRotation TC_RotateByX(this Transform iTarget, float iX)
        {
            return UCL_TC_EulerRotation.Create().Init(iTarget, iX, 0, 0);
        }
        /// <summary>
        /// Create a  TC_EulerRotation
        /// </summary>
        /// <param name="iTarget"></param>
        /// <param name="iY"></param>
        /// <returns></returns>
        static public UCL_TC_EulerRotation TC_RotateByY(this Transform iTarget, float iY)
        {
            return UCL_TC_EulerRotation.Create().Init(iTarget, 0, iY, 0);
        }
        /// <summary>
        /// Create a  TC_EulerRotation
        /// </summary>
        /// <param name="iTarget"></param>
        /// <param name="iZ"></param>
        /// <returns></returns>
        static public UCL_TC_EulerRotation TC_RotateByZ(this Transform iTarget, float iZ)
        {
            return UCL_TC_EulerRotation.Create().Init(iTarget, 0, 0, iZ);
        }
        /// <summary>
        /// Create a Tweener contains TC_EulerRotation
        /// </summary>
        /// <param name="target"></param>
        /// <param name="iDuration"></param>
        /// <param name="iRotation"></param>
        /// <returns></returns>
        static public UCL_Tweener UCL_EulerRotation(this Transform target, float iDuration, Vector3 iRotation)
        {
            return LibTween.Tweener(iDuration).AddComponent(TC_EulerRotation(target, iRotation));
        }

        /// <summary>
        /// Create a Tweener contains TC_EulerRotation
        /// </summary>
        /// <param name="target"></param>
        /// <param name="iDuration"></param>
        /// <param name="iX"></param>
        /// <param name="iY"></param>
        /// <param name="iZ"></param>
        /// <returns></returns>
        static public UCL_Tweener UCL_EulerRotation(this Transform target, float iDuration, float iX, float iY, float iZ)
        {
            return LibTween.Tweener(iDuration).AddComponent(TC_EulerRotation(target, iX, iY, iZ));
        }
        /// <summary>
        /// Create a Tweener contains TC_EulerRotation
        /// </summary>
        /// <param name="target"></param>
        /// <param name="iDuration"></param>
        /// <param name="iX"></param>
        /// <returns></returns>
        static public UCL_Tweener UCL_RotateByX(this Transform target, float iDuration, float iX)
        {
            return LibTween.Tweener(iDuration).AddComponent(TC_RotateByX(target, iX));
        }
        /// <summary>
        /// Create a Tweener contains TC_EulerRotation
        /// </summary>
        /// <param name="target"></param>
        /// <param name="iDuration"></param>
        /// <param name="iY"></param>
        /// <returns></returns>
        static public UCL_Tweener UCL_RotateByY(this Transform target, float iDuration, float iY)
        {
            return LibTween.Tweener(iDuration).AddComponent(TC_RotateByY(target, iY));
        }
        /// <summary>
        /// Create a Tweener contains TC_EulerRotation
        /// </summary>
        /// <param name="target"></param>
        /// <param name="iDuration"></param>
        /// <param name="iZ"></param>
        /// <returns></returns>
        static public UCL_Tweener UCL_RotateByZ(this Transform target, float iDuration, float iZ)
        {
            return LibTween.Tweener(iDuration).AddComponent(TC_RotateByZ(target, iZ));
        }
    }
    #endregion
    public class UCL_TC_EulerRotation : UCL_TC_Transform {
        override public TC_Type GetTC_Type() { return TC_Type.EulerRotation; }
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
        virtual public UCL_TC_EulerRotation Init(Transform iTarget, float iX, float iY, float iZ)
        {
            Init(iTarget, new Vector3(iX, iY, iZ));
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