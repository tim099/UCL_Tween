using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//========= using UCL.TweenLib; ===========

namespace UCL.TweenLib.Demo
{
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
#endif
    public class UCL_JumpDemo : UCL_TC_Demo
    {
        public Transform m_Start;
        public Transform m_Goal;
        public int m_JumpTimes = 6;
        public Vector3 m_Up = Vector3.up;
        public float m_Height = 10f;
        public float m_Bounciness = 0.7f;
        [UCL.Core.ATTR.UCL_FunctionButton]
        override public void StartDemo() {
            //kill previous tweener if not ended
            //假如上個tweener還在執行則進行kill 若Kill參數帶true則會執行OnComplete的Action false則不觸發(可省略此步驟)
            if(m_Tweener != null) {
                m_Tweener.Kill(false);
            }
            //Reset position to start
            //重設目標位置到起點
            m_Target.position = m_Start.position;

            //Create tweencomponent
            //生成tweencomponent
            var tween_component = m_Target.TC_Jump(m_Goal, m_JumpTimes, m_Up, m_Height, m_Bounciness);

            //Create UCL_Tweener
            //生成UCL_Tweener
            m_Tweener = CreateTweener();
            //Add tween component to tweener
            //把tween元件加入tweener
            m_Tweener.AddComponent(tween_component);

            //Set OnComplete action
            //設定完成後執行的action(這項非必要)
            m_Tweener.OnComplete(() => {
                m_Tweener = null;
            });
            //start tweener
            //開始執行tweener
            m_Tweener.Start();
        }
    }
}