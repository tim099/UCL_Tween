using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TimerFloat : UCL_Timer {
        protected float m_Timer;
        public override float GetTime() {
            return m_Timer;
        }
        public override void SetTime(float time) {
            m_Timer = time;
        }
        public override void AlterTime(float time) {
            m_Timer += time;
        }
        public override void SetTime(UCL_Timer timer) {
            m_Timer = timer.GetTime();
        }
    }
}