using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UCL.TweenLib {
    public class UCL_Sequence : UCL_Tween {
        UCL_Sequence() {

        }

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
        virtual public UCL_Tween AppendInterval(float interval) {
            UCL_Tween tween = UCL_TweenTimer.Create();

            tween.SetDuration(interval);
            Append(tween);
            return tween;
        }
        /// <summary>
        /// Append a tween to this sequence, tween in sequence will start in order
        /// </summary>
        /// <param name="tween">tween to append to this sequence</param>
        /// <returns></returns>
        public UCL_Sequence Append(UCL_Tween tween) {
            m_Tweens.Add(tween);
            m_Duration += tween.Duration;
            return this;
        }
        /// <summary>
        /// Not done yet!!
        /// </summary>
        /// <param name="tween"></param>
        /// <returns></returns>
        public UCL_Sequence Join(UCL_Tween tween) {

            return this;
        }
        protected internal override void TweenStart() {
            base.TweenStart();
        }
        override internal protected float TimeUpdate(float time_delta) {
            if(m_End || m_Paused) return 0;
            
            var time_remains = TimeUpdateAction(time_delta);

            return time_remains;
        }
        override protected float TimeUpdateAction(float time_delta) {
            //base.TimeUpdateAction();
            
            var cur = GetCurTween();


            int i = 0;
            var tmp_timer = m_Timer + time_delta;
            //Debug.LogWarning("m_Timer:" + m_Timer);
            if(cur != null && !cur.Started) {
                cur.Start();
            }

            while(cur != null && time_delta > 0 && i++ < 10000) {
                var del = cur.TimeUpdate(time_delta);
                m_Timer += (time_delta - del);
                //Debug.LogWarning(i+",m_Timer t:" + m_Timer);
                time_delta = del;
                
                if(cur.CheckComplete()) {
                    m_CurAt++;
                    cur = GetCurTween();
                    if(cur != null) cur.TweenStart();
                }
            }
            m_Timer = tmp_timer - time_delta;
            return time_delta;
        }
        protected UCL_Tween GetCurTween() {
            if(m_CurAt < 0 || m_CurAt >= m_Tweens.Count) return null;

            return m_Tweens[m_CurAt];
        }
        public override void Kill(bool compelete = false) {
            if(compelete) {// && GetCurTween() != null
                TimeUpdateAction(m_Duration + 1000f);//End all Tween
            }

            base.Kill(compelete);
        }
        override internal protected bool CheckComplete() {
            if(GetCurTween() == null) {
                Complete();
                return true;
            }

            return false;
        }

    }
}