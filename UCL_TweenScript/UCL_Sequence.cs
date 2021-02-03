using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UCL.TweenLib {
    public class UCL_Sequence : UCL_Tween {
        UCL_Sequence() {

        }

        protected List<UCL_Tween> m_Tweens;
        protected Dictionary<int, List<UCL_Tween>> m_JoinTweens;
        protected List<UCL_Tween> m_StartedJoinTweens;
        protected int m_CurAt = 0;
        protected internal override void Init() {
            base.Init();
            m_CurAt = 0;
            m_Tweens = new List<UCL_Tween>();
            m_JoinTweens = new Dictionary<int, List<UCL_Tween>>();
            m_StartedJoinTweens = new List<UCL_Tween>();
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
            m_Duration.AlterTime(tween.Duration);
            return this;
        }
        /// <summary>
        /// Append a System.Action in this sequence
        /// </summary>
        /// <param name="iAct"></param>
        /// <returns></returns>
        public UCL_Sequence Append(System.Action iAct)
        {
            var aTweener = TweenLib.LibTween.Tweener(float.Epsilon);
            aTweener.OnStart(iAct);
            Append(aTweener);
            return this;
        }
        /// <summary>
        /// Join a tween to this sequence, tween will start on current appended tween start
        /// </summary>
        /// <param name="tween"></param>
        /// <returns></returns>
        public UCL_Sequence Join(UCL_Tween tween) {
            int at = m_Tweens.Count - 1;
            if(at < 0) at = 0;
            if(!m_JoinTweens.ContainsKey(at)) {
                m_JoinTweens[at] = new List<UCL_Tween>();
            }
            m_JoinTweens[at].Add(tween);
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
        override internal protected long TimeUpdate(long time_delta) {
            if(m_End || m_Paused) return 0;

            var time_remains = TimeUpdateAction(time_delta);

            return time_remains;
        }
        virtual protected void JoinTweenStart(int at) {
            if(m_JoinTweens.ContainsKey(at)) {
                var list = m_JoinTweens[at];
                for(int i = 0; i < list.Count; i++) {
                    var tween = list[i];
                    tween.Start();
                    m_StartedJoinTweens.Add(tween);
                }
            }
        }
        virtual protected void JoinTweenUpdate(float time_delta) {
            for(int i = m_StartedJoinTweens.Count - 1; i >= 0; i--) {
                var tween = m_StartedJoinTweens[i];
                tween.TimeUpdate(time_delta);
                if(tween.CheckComplete()) {
                    m_StartedJoinTweens.Remove(tween);
                }
            }
        }
        override protected float TimeUpdateAction(float time_delta) {
            var cur = GetCurTween();

            int i = 0;
            float tmp_timer = m_Timer.GetTime() + time_delta;
            //Debug.LogWarning("m_Timer:" + m_Timer);
            if(cur != null && !cur.Started) {//First
                cur.TweenStart();
                JoinTweenStart(m_CurAt);
            }

            while(cur != null && time_delta > 0 && i++ < 10000) {
                float del = cur.TimeUpdate(time_delta);
                float time_spent = (time_delta - del);
                JoinTweenUpdate(time_spent);

                m_Timer.AlterTime(time_spent);
                //Debug.LogWarning(i+",m_Timer t:" + Timer + ",del:"+ del+ ",time_spent:"+ time_spent);
                time_delta = del;
                
                if(cur.CheckComplete()) {
                    m_CurAt++;
                    cur = GetCurTween();
                    if(cur != null) cur.TweenStart();

                    JoinTweenStart(m_CurAt);
                }
            }
            if(time_delta > 0) {
                JoinTweenUpdate(time_delta);
            }
            m_Timer.SetTime(tmp_timer - time_delta);
            return time_delta;
        }

        protected UCL_Tween GetCurTween() {
            if(m_CurAt < 0 || m_CurAt >= m_Tweens.Count) return null;

            return m_Tweens[m_CurAt];
        }
        public override void Kill(bool compelete = false) {
            if(compelete) {// && GetCurTween() != null
                TimeUpdateAction(Duration + 1000f);//End all Tween
            }

            base.Kill(compelete);
        }
        override internal protected bool CheckComplete() {
            if(GetCurTween() == null && m_StartedJoinTweens.Count == 0) {
                Complete();
                return true;
            }

            return false;
        }

    }
}