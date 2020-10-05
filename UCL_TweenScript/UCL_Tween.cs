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
        virtual public string Name {
            get {
                return this.GetType().Name;
            }
        }
        virtual public float Timer {
            get { return m_Timer.GetTime(); }
        }
        virtual public long TimerMs {
            get { return m_Timer.GetTimeMs(); }
        }
        virtual public float Duration {
            get { return m_Duration.GetTime(); }
            set {
                if(m_Duration == null || !(m_Duration is UCL_TimerFloat)) {
                    m_Duration = new UCL_TimerFloat();
                    var time = m_Timer.GetTime();
                    m_Timer = new UCL_TimerFloat();
                    m_Timer.SetTime(time);
                }
                m_Duration.SetTime(value);
            }
        }
        virtual public long DurationMs {
            get { return m_Duration.GetTimeMs(); }
            set {
                if(m_Duration == null || !(m_Duration is UCL_TimerMs)) {
                    m_Duration = new UCL_TimerMs();
                    var time = m_Timer.GetTimeMs();
                    m_Timer = new UCL_TimerMs();
                    m_Timer.SetTimeMs(time);
                }
                m_Duration.SetTimeMs(value);
            }
        }
        virtual public bool IsPaused {
            get { return m_Paused; }
        }
        protected bool m_Completed = false;
        protected bool m_Started = false;
        protected bool m_End = false;
        protected bool m_Paused = false;
        protected System.Action m_CompleteAct = null;
        protected System.Action m_StartAct = null;

        protected float m_TimeScale = 1f;
        protected UCL_Timer m_Timer = new UCL_TimerFloat();
        protected UCL_Timer m_Duration = new UCL_TimerFloat();

        virtual internal protected void Init() {
            m_Completed = false;
            m_End = false;
            //m_Timer = new UCL_TimerFloat();
            //m_Duration = 0;
            m_CompleteAct = null;
            m_Paused = false;
            InitTween();
        }
        virtual public float GetTime() {
            return m_Timer.GetTime();
        }
        virtual public float GetRemainDuration() {
            return Duration - Timer;
        }
        /// <summary>
        /// Set the timescale of this tweener
        /// </summary>
        /// <param name="scale">timescale of this tweener</param>
        /// <returns></returns>
        virtual public UCL_Tween SetTimeScale(float scale) {
            m_TimeScale = scale;
            return this;
        }
        virtual public UCL_Tween SetDuration(float duration) {
            Duration = duration;
            return this;
        }
        virtual public UCL_Tween SetDurationMs(long duration) {
            Duration = duration;
            return this;
        }
        /// <summary>
        /// TimeAlter ignore pause
        /// </summary>
        /// <param name="time_delta">time_delta in seconds</param>
        /// <returns></returns>
        virtual public float TimeAlter(float time_delta) {
            if(!m_Started) {
                Debug.LogError("UCL_Tween TimeAlter Fail not started yet!!");
                return time_delta;
            }
            if(m_TimeScale != 1f) {
                time_delta *= m_TimeScale;
            }
            if(m_End) return time_delta;

            m_Timer.AlterTime(time_delta);
            
            if(Timer < 0) m_Timer.SetTime(0);

            var remains = TimeUpdateAction(time_delta);

            return remains;
        }
        /// <summary>
        /// TimeAlter ignore pause
        /// </summary>
        /// <param name="time_delta">time delta in Ms</param>
        /// <returns></returns>
        virtual public long TimeAlter(long time_delta) {
            if(m_End) return time_delta;
            if(m_TimeScale != 1f) {
                time_delta = UCL.Core.MathLib.Lib.RoundToLong(time_delta * m_TimeScale);
            }
            m_Timer.AlterTimeMs(time_delta);

            if(TimerMs < 0) m_Timer.SetTimeMs(0);

            var remains = TimeUpdateAction(time_delta);

            return remains;
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

        virtual public void OnDrawGizmos() { }

        /// <summary>
        /// return value is the remain time of update(0 if not complete
        /// </summary>
        /// <param name="time_delta"></param>
        /// <returns></returns>
        virtual internal protected float TimeUpdate(float time_delta) {
            if(m_End) return time_delta;
            if(m_Paused) return 0;
            if(m_TimeScale != 1f) {
                time_delta *= m_TimeScale;
            }
            m_Timer.AlterTime(time_delta);
            float remains = TimeUpdateAction(time_delta);

            return remains;
        }

        /// <summary>
        /// return value is the remain time of update(0 if not complete
        /// </summary>
        /// <param name="time_delta"></param>
        /// <returns></returns>
        virtual internal protected long TimeUpdate(long time_delta) {
            if(m_End) return time_delta;
            if(m_Paused) return 0;
            if(m_TimeScale != 1f) {
                time_delta = UCL.Core.MathLib.Lib.RoundToLong(time_delta * m_TimeScale);
            }
            m_Timer.AlterTimeMs(time_delta);
            long remains = TimeUpdateAction(time_delta);

            return remains;
        }
        virtual protected float TimeUpdateAction(float time_delta) {
            return Timer > Duration ? Timer - Duration : 0 ;
        }
        virtual protected long TimeUpdateAction(long time_delta) {
            return Mathf.RoundToInt(1000f*TimeUpdateAction(0.001f*time_delta));
        }
        virtual internal protected bool CheckComplete() {
            if(m_End) return true;
            if(Duration > 0 && m_Timer >= m_Duration) {
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

#if UNITY_EDITOR
        /// <summary>
        /// Called when being selected in Inspector
        /// </summary>
        virtual internal void OnInspectorGUI() {
            GUILayout.BeginVertical();
            GUILayout.Box(Name);
            GUILayout.Label("time:" + m_Timer.GetTime().ToString("N1")+
                "duration:"+m_Duration.GetTime().ToString("N1"));
            GUILayout.EndVertical();
        }
        /// <summary>
        /// Called when being selected
        /// </summary>
        virtual internal void OnSelected() {

        }
#endif
    }
}

