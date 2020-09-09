using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UCL.TweenLib {
    public class UCL_TB_Timer : UCL_TB_Tweener {
        public Text m_TimerText;

        /// <summary>
        /// Call once on time update
        /// </summary>
        public UnityEngine.Events.UnityEvent m_UpdateEvent;

        /// <summary>
        /// Call once on time update if Time <= m_Threshold
        /// </summary>
        public UnityEngine.Events.UnityEvent m_UpdateUnderThresholdEvent;
        public int m_Threshold = 0;

        protected int m_Time;
        public override void StartTweener() {
            TimerUpdate(Mathf.CeilToInt(m_Duration));
            CreateTweener().OnUpdate((y)=> {
                int time = Mathf.CeilToInt(m_Duration - m_Tweener.Timer);
                while(m_Time > time) {
                    TimerUpdate(m_Time - 1);
                }
            });
            m_Tweener.Start(m_TimeManager);
        }
        virtual protected void TimerUpdate(int time) {
            //Debug.LogWarning("Tick:" + time);
            m_Time = time;
            if(m_TimerText != null) {
                m_TimerText.text = m_Time.ToString();
            }
            if(m_UpdateEvent != null) m_UpdateEvent.Invoke();
            if(m_Time <= m_Threshold) {
                //Debug.LogWarning("m_Time <= m_Threshold m_Time:" + m_Time+ ",m_Threshold:"+ m_Threshold);
                if(m_UpdateUnderThresholdEvent != null) m_UpdateUnderThresholdEvent.Invoke();
            }
        }
    }
}