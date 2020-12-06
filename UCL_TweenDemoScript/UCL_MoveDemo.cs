using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========= using UCL.TweenLib; ===========

namespace UCL.TweenLib.Demo
{
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
#endif
    public class UCL_MoveDemo : UCL_TC_Demo
    {
        public Transform m_Start;//起點
        public Transform m_Goal;//終點

        /// <summary>
        /// 使用TweenComponent方式
        /// </summary>
        [UCL.Core.ATTR.UCL_FunctionButton]
        override public void StartDemo() {
            //kill previous tweener if not ended
            //假如上個tweener還在執行則進行kill 若Kill參數帶true則會執行OnComplete的Action false則不觸發(可省略此步驟)
            if(m_Tweener != null) {
                m_Tweener.Kill(false);
            }
            //Reset position to start
            //重設目標位置道起點
            m_Target.position = m_Start.position;

            //Create UCL_Tweener
            //生成UCL_Tweener
            m_Tweener = LibTween.Tweener(m_Duration);
            if(m_UseEaseCurve) {
                //Use AnimationCurve as Ease
                //用AnimationCurve作為緩動函式
                m_Tweener.SetEase(m_EaseCurve);
            } else {
                //Set Ease function
                //設定緩動函式
                m_Tweener.SetEase(m_Ease);
            }
            //Create tweencomponent
            //生成tweencomponent
            var tween_component = m_Target.TC_Move(m_Goal);
            //var tween_component = m_Target.TC_Move(m_Goal.position);範例 使用Vector3座標作為目標

            //Add tween component to tweener
            //把tween元件加入tweener
            m_Tweener.AddComponent(tween_component);

            //Set OnComplete action
            //設定完成後執行的action(這項非必要 action會在完成時被呼叫)
            m_Tweener.OnComplete(() => {
                m_Tweener = null;
            });
            //start tweener
            //開始執行tweener
            m_Tweener.Start();
        }

        /// <summary>
        /// 直接使用UCL_Move的方式
        /// </summary>
        [UCL.Core.ATTR.UCL_FunctionButton]
        public void StartTweenerDemo() {
            //kill previous tweener if not ended
            //假如上個tweener還在執行則進行kill 若Kill參數帶true則會執行OnComplete的Action false則不觸發(可省略此步驟)
            if(m_Tweener != null) {
                m_Tweener.Kill(false);
            }
            //Reset position to start
            //重設目標位置到起點
            m_Target.position = m_Start.position;

            //Create UCL_Tweener
            //生成UCL_Tweener
            m_Tweener = m_Target.UCL_Move(m_Duration, m_Goal);
            if(m_UseEaseCurve) {
                //Use AnimationCurve as Ease
                //用AnimationCurve作為緩動函式
                m_Tweener.SetEase(m_EaseCurve);
            } else {
                //Set Ease function
                //設定緩動函式
                m_Tweener.SetEase(m_Ease);
            }
            //start tweener
            //開始執行tweener
            m_Tweener.Start();
        }
    }
}