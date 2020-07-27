using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TimerMs : UCL_Timer {
        protected long m_Timer;
        public override long GetTimeMs() {
            return m_Timer;
        }
        public override float GetTime() {
            return m_Timer * 0.001f;
        }

        public override void SetTimeMs(long time) {
            m_Timer = time;
        }
        public override void SetTime(float time) {
            SetTimeMs(ConvertToMs(time));
        }
        public override void SetTime(UCL_Timer timer) {
            m_Timer = timer.GetTimeMs();
        }
        public override void AlterTimeMs(long time) {
            m_Timer += time;
        }
        public override void AlterTime(float time) {
            AlterTimeMs(ConvertToMs(time));
        }
    }
}