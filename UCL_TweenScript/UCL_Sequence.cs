using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UCL.TweenLib {
    public class UCL_Sequence : UCL_Tween {
        protected List<UCL_Tween> m_Tweens;
        protected int m_CurAt = 0;
        protected internal override void Init() {
            base.Init();
            m_CurAt = 0;
            m_Tweens = new List<UCL_Tween>();
        }
        static public UCL_Sequence Create() {
            var seq = new UCL_Sequence();
            seq.Init();
            return seq;
        }
        virtual public void AppendInterval(float interval) {
            
        }
        public void Append(UCL_Tween tween) {
            m_Tweens.Add(tween);
        }
        protected override void TimeUpdateAction(float time_delta) {
            //base.TimeUpdateAction();
            var cur = GetCurTween();
            int i = 0;
            while(cur != null && time_delta > 0 && i++ < 10000) {
                time_delta = cur.TimeUpdate(time_delta);
                if(cur.End) {
                    m_CurAt++;
                    cur = GetCurTween();
                }
            }
        }
        protected UCL_Tween GetCurTween() {
            if(m_CurAt < 0 || m_CurAt >= m_Tweens.Count) return null;

            return m_Tweens[m_CurAt];
        }
        override protected float CheckComplete() {
            if(GetCurTween() == null) {
                if(m_Duration > 0 && m_Timer >= m_Duration) {
                    Complete();
                    return m_Timer - m_Duration;
                }
            }

            return 0;
        }
        UCL_Sequence() {

        }
    }
}