using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//========= using UCL.TweenLib; ===========

namespace UCL.TweenLib.Demo
{
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
#endif
    public class UCL_RotateDemo : UCL_TC_Demo
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
            //Reset target
            //重設目標
            m_Target.rotation = m_Start.rotation;

            //Create UCL_Rotate
            //生成UCL_Rotate
            m_Tweener = m_Target.UCL_Rotate(m_Duration, m_Goal);
            if(m_UseEaseCurve) {
                //Use AnimationCurve as Ease
                //用AnimationCurve作為緩動函式
                m_Tweener.SetEase(m_EaseCurve);
            } else {
                //Set Ease function
                //設定緩動函式
                m_Tweener.SetEase(m_Ease);
            }

            //Set OnComplete action
            //設定完成後執行的action(這項非必要 action會在完成時被呼叫)
            m_Tweener.OnComplete(() => {
                m_Tweener = null;
            });
            //start tweener
            //開始執行tweener
            m_Tweener.Start();
        }
    }
}
