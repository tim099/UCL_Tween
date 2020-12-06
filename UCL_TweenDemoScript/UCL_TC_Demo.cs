using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib.Demo
{
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
#endif
    public class UCL_TC_Demo : MonoBehaviour
    {
        //目標移動對象
        //Move target
        public Transform m_Target;

        //Ease function
        //緩動函式
        public EaseType m_Ease;

        //5 sec
        //預設5秒
        public float m_Duration = 5f;

        [Header("Use EaseCurve instead of Ease")]
        public bool m_UseEaseCurve = false;

        public AnimationCurve m_EaseCurve;

        protected UCL_Tweener m_Tweener = null;

        [UCL.Core.ATTR.UCL_FunctionButton]
        virtual public void StartDemo() {
            var tc = CreateTC();
            if(tc != null) {
                if(m_Tweener != null) {
                    m_Tweener.Kill(false);
                }
                m_Tweener = CreateTweener();

                //Add tween component to tweener
                //把tween元件加入tweener
                m_Tweener.AddComponent(tc);

                //start tweener
                //開始執行tweener
                m_Tweener.Start();
            }
        }
        virtual public UCL_Tweener CreateTweener() {
            //Create UCL_Tweener
            //生成UCL_Tweener
            UCL_Tweener tweener = LibTween.Tweener(m_Duration);
            if(m_UseEaseCurve) {
                //Use AnimationCurve as Ease
                //用AnimationCurve作為緩動函式
                tweener.SetEase(m_EaseCurve);
            } else {
                //Set Ease function
                //設定緩動函式
                tweener.SetEase(m_Ease);
            }

            return tweener;
        }
        virtual public UCL_TweenerComponent CreateTC() {
            return null;
        }
    }
}