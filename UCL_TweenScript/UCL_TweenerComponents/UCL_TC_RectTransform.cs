using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib
{
    public static partial class TC_Extension
    {
        /// <summary>
        /// Create a TweeneComponent that move iTarget toward iTargetRectTransform,
        /// also scale and rotate to fit iTargetRectTransform
        /// </summary>
        /// <param name="iTarget"></param>
        /// <param name="iTargetRectTransform"></param>
        /// <returns></returns>
        static public UCL_TC_RectTransform TC_RectTransform(this RectTransform iTarget, RectTransform iTargetRectTransform)
        {
            return UCL_TC_RectTransform.Create().Init(iTarget, iTargetRectTransform);
        }
        /// <summary>
        /// Create a Tweener that move iTarget toward iTargetRectTransform,
        /// also scale and rotate to fit iTargetRectTransform
        /// </summary>
        /// <param name="iTarget"></param>
        /// <param name="iTargetRectTransform"></param>
        /// <returns></returns>
        static public UCL_Tweener UCL_RectTransform(this RectTransform iTarget, float iDuration, RectTransform iTargetRectTransform)
        {
            return LibTween.Tweener(iDuration).AddComponent(iTarget.TC_RectTransform(iTargetRectTransform));
        }
    }
    public class UCL_TC_RectTransform : UCL_TweenerComponent
    {
        /// <summary>
        /// Create a UCL_TC_RectTransform
        /// </summary>
        /// <returns></returns>
        public static UCL_TC_RectTransform Create()
        {
            return new UCL_TC_RectTransform();
        }
        override public string Name
        {
            get
            {
                string name = this.GetType().Name.Replace("UCL_TC_", string.Empty);
                if (m_Target != null)
                {
                    name += "[" + m_Target.name + "]";
                }
                return name;
            }
        }
        override public Transform GetTarget() { return m_Target; }

        //[Tooltip("Target that tween component move")]
        [SerializeField] protected RectTransform m_Target;

        /// <summary>
        /// Target Transform that target will move to
        /// </summary>
        [SerializeField] protected RectTransform m_TargetTransform;


        [HideInInspector] protected Vector3 m_StartPos;
        [HideInInspector] protected Vector3 m_TargetPos;

        [HideInInspector] protected Vector2 m_StartSize;
        [HideInInspector] protected Vector2 m_TargetSize;

        [HideInInspector] protected Quaternion m_StartRot;
        [HideInInspector] protected Quaternion m_TargetRot;
        #region EDITOR
#if UNITY_EDITOR
        public override string OnInspectorGUITips()
        {
            var tips = base.OnInspectorGUITips();
            tips += "\"Target\" is the move target of TweenerComponent\n";
            tips += "\"TargetTransform\" is target position that \"Target\" will move to\n";
            return tips;
        }
        override public void OnInspectorGUIBasic(UCL_TC_Data iTcData, UnityEditor.SerializedProperty iSerializedProperty, Transform iTransform)
        {
            base.OnInspectorGUIBasic(iTcData, iSerializedProperty, iTransform);
            UnityEditor.SerializedProperty aTransformDatas = iSerializedProperty.FindPropertyRelative("m_Transform");
            if (iTransform != null && aTransformDatas != null)
            {
                if (aTransformDatas.arraySize == 0)
                {//Init
                    aTransformDatas.InsertArrayElementAtIndex(0);
                    aTransformDatas.InsertArrayElementAtIndex(1);
                    aTransformDatas.GetArrayElementAtIndex(0).objectReferenceValue = iTransform;
                }
            }
        }
#endif
        #endregion
        virtual public UCL_TC_RectTransform Init(RectTransform iTarget, RectTransform iTargetTransform)
        {
            m_Target = iTarget;
            m_TargetTransform = iTargetTransform;
            return this;
        }
        protected internal override void Start()
        {
            var aTarget = m_Target;
            var aTargetTransform = m_TargetTransform;

            //Debug.LogError("aTarget.sizeDelta:" + aTarget.sizeDelta);
            aTarget.pivot = new Vector2(0.5f, 0.5f);//iTarget.pivot;
            aTarget.anchorMin = 0.5f * Vector2.one;
            aTarget.anchorMax = 0.5f * Vector2.one;

            //Transform aTargetCanvasTransform = aTarget.GetComponentInParent<Canvas>().transform;//iRect.parent;
            Canvas aCanvasB = aTargetTransform.GetComponentInParent<Canvas>();
            Vector3[] aCorners = new Vector3[4];
            aTargetTransform.GetWorldCorners(aCorners);
            for (int i = 0; i < 4; i++)
            {
                aCorners[i] = aCanvasB.transform.InverseTransformPoint(aCorners[i]);
            }
            Vector2 aHorVec = aCorners[0] - aCorners[3];
            Vector2 aVerVec = aCorners[0] - aCorners[1];
            //Vector2 aMidPoint = 0.5f * (aCorners[0] + aCorners[2]);

            m_StartSize = aTarget.sizeDelta;
            //Debug.LogError("m_StartSize:"+ m_StartSize);
            m_TargetSize = new Vector2(aHorVec.magnitude, aVerVec.magnitude);


            m_StartPos = m_Target.position;
            
            aTarget.SetAnchorPositionScreenSpace(aTargetTransform.GetScreenSpaceRect().center);


            m_TargetPos = m_Target.position;//aTargetCanvasTransform.TransformPoint(aMidPoint);

            float aX = -aHorVec.x;
            float aY = -aHorVec.y;
            float aRot = Mathf.Atan2(aY, aX) * Mathf.Rad2Deg;

            m_StartRot = aTarget.transform.rotation;
            m_TargetRot = Quaternion.Euler(0, 0, aRot);

        }
        protected override void ComponentUpdate(float iPos)
        {
            if(m_Target == null)
            {
                return;
            }
            m_Target.position = Core.MathLib.Lib.Lerp(m_StartPos, m_TargetPos, iPos);
            m_Target.sizeDelta = Core.MathLib.Lib.Lerp(m_StartSize, m_TargetSize, iPos);
            m_Target.rotation = Core.MathLib.Lib.Lerp(m_StartRot, m_TargetRot, iPos);
            //Debug.LogWarning("ComponentUpdate:" + pos+ ",m_StartVal:"+ m_StartVal+ ",m_TargetVal:"+ m_TargetVal);
        }
    }

}