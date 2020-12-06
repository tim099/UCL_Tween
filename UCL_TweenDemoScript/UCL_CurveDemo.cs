using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//========= using UCL.TweenLib; ===========

namespace UCL.TweenLib.Demo
{
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
#endif
    public class UCL_CurveDemo : UCL_TC_Demo
    {
        public UCL.Core.MathLib.UCL_Path m_Path;

        [UCL.Core.ATTR.UCL_FunctionButton]
        override public void StartDemo() {
            //kill previous tweener if not ended
            //假如上個tweener還在執行則進行kill 若Kill參數帶true則會執行OnComplete的Action false則不觸發(可省略此步驟)
            if(m_Tweener != null) m_Tweener.Kill(false);

            //Create UCL_Tweener
            //生成UCL_Tweener
            m_Tweener = LibTween.Tweener(m_Duration).SetEase(m_Ease);

            //Create tweencomponent and add tween component to tweener
            //生成tweencomponent並把tween元件加入tweener
            m_Tweener.AddComponent(m_Target.TC_Move(m_Path));

            //start tweener
            //開始執行tweener
            m_Tweener.Start();
        }
    }
}