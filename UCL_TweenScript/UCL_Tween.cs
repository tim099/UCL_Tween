using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UCL.TweenLib {
    public class UCL_Tween {
        public bool Completed {
            get {
                return m_Completed;
            }
        }
        public bool IsTweener {
            get {
                return (this is UCL_Tweener);
            }
        }
        public bool IsSequence {
            get {
                return (this is UCL_Sequence);
            }
        }
        /// <summary>
        /// true if Tween is Complete or Killed
        /// </summary>
        public bool End {
            get {
                return m_End;
            }
        }
        public bool Started {
            get {
                return m_Started;
            }
        }
        public float Timer {
            get { return m_Timer; }
        }
        public float Duration {
            get { return m_Duration; }
        }
        protected bool m_Completed = false;
        protected bool m_Started = false;
        protected bool m_End = false;
        protected bool m_Paused = false;
        protected System.Action m_CompleteAct = null;
        protected System.Action m_StartAct = null;
        protected float m_Timer = 0;
        protected float m_Duration = 0;
        
        virtual internal protected void Init() {
            m_Completed = false;
            m_End = false;
            m_Timer = 0;
            m_Duration = 0;
            m_CompleteAct = null;
            m_Paused = false;
            InitTween();
        }
        virtual public float GetTime() {
            return m_Timer;
        }
        virtual public float GetRemainDuration() {
            return m_Duration - m_Timer;
        }
        virtual public UCL_Tween SetDuration(float duration) {
            m_Duration = duration;
            return this;
        }
        /// <summary>
        /// TimeAlter ignore pause
        /// </summary>
        /// <param name="time_delta"></param>
        /// <returns></returns>
        virtual public float TimeAlter(float time_delta) {
            if(m_End) return time_delta;

            m_Timer += time_delta;
            if(m_Timer < 0) m_Timer = 0;

            float remains = TimeUpdateAction(time_delta);

            return remains;
            //return TimeUpdate(delta_time);//this;
        }
        virtual protected void InitTween() { }
        /// <summary>
        /// Start the Tween on manager
        /// </summary>
        /// <returns></returns>
        virtual public UCL_Tween Start(UCL_TweenTimeManager manager = null) {
            if(manager == null) {
                if(UCL_TweenManager.Instance != null) {
                    manager = UCL_TweenManager.Instance.TimeManager;
                }
            }
            if(manager != null) manager.Add(this);
            return this;
        }
        /// <summary>
        /// Called once when tween start
        /// </summary>
        virtual internal protected void TweenStart() {
            if(m_Started) return;
            m_Started = true;
            if(m_StartAct != null) {
                try {
                    m_StartAct.Invoke();
                } catch(System.Exception e) {
                    Debug.LogError("UCL_Tween m_StartAct.Invoke() Exception:" + e);
                }
                
            }
        }
        /// <summary>
        /// return value is the remain time of update(0 if not complete
        /// </summary>
        /// <param name="time_delta"></param>
        /// <returns></returns>
        virtual internal protected float TimeUpdate(float time_delta) {
            if(m_End) return time_delta;
            if(m_Paused) return 0;

            m_Timer += time_delta;
            //if(m_Timer >= m_Duration) m_Timer = m_Duration;
            float remains = TimeUpdateAction(time_delta);
            //CheckComplete();

            return remains;
        }
        virtual protected float TimeUpdateAction(float time_delta) {
            return m_Timer > m_Duration ? m_Timer - m_Duration : 0 ;
        }
        virtual internal protected bool CheckComplete() {
            if(m_End) return true;
            if(m_Duration > 0 && m_Timer >= m_Duration) {
                Complete();
                return true;
            }
            return false;
        }
        virtual public void SetPause(bool _pause) {
            if(_pause) {
                Pause();
            } else {
                Resume();
            }
        }
        virtual public void Pause() {
            m_Paused = true;
        }
        virtual public void Resume() {
            m_Paused = false;
        }
        virtual public void Kill(bool compelete = false) {
            if(compelete) Complete();
            m_End = true;
        }
        virtual internal protected void Complete() {
            if(m_Completed) return;

            CompleteAction();

            m_Completed = true;
            m_End = true;

            if(m_CompleteAct != null) {
                try {
                    m_CompleteAct.Invoke();
                } catch(System.Exception e) {
                    Debug.LogError("UCL_Tween m_CompleteAct.Invoke() Exception:" + e);
                }
            }
        }
        virtual protected void CompleteAction() {

        }
        virtual public UCL_Tween OnStart(System.Action _Act) {
            m_StartAct = _Act;
            return this;
        }
        virtual public UCL_Tween OnComplete(System.Action _CompleteAct) {
            m_CompleteAct = _CompleteAct;
            return this;
        }
    }
}

