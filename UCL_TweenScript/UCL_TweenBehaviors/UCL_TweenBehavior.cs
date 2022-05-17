using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
#endif
    
    public class UCL_TweenBehavior : MonoBehaviour {
        public enum StartOption {
            None = 0,//No Auto Invoke StartTween()
            OnStart,//Invoke StartTween() On Start()
            OnEnable,//Invoke StartTween() On OnEnable()
            OnAwake,//Invoke StartTween() On Awake()
        }
        public bool IsStarted { get { return m_Started; } }

        //[SerializeField] protected float m_Timer;//For Inspector

        /*
        /// <summary>
        /// Invoke StartTween(); when Start()
        /// </summary>
        public bool m_AutoStart = true;
        public bool m_StartOnEnable = false;
        */

        public StartOption m_StartOption = StartOption.OnStart;


        public bool m_EndOnDestroy = true;
        public bool m_EndOnDisable = false;
        /// <summary>
        /// Do backfolding(x from 0 to 1 then go from 1 to 0)
        /// 折返 x值從0到達1之後折返回0
        /// </summary>
        public bool m_Backfolding = false;
        public bool m_Looping = false;
        
        public UnityEngine.Events.UnityEvent m_StartEvent;
        public UnityEngine.Events.UnityEvent m_EndEvent;
        protected bool m_Started = false;
        protected bool m_End = false;
        protected System.Action m_EndAct;
        virtual protected void Start() {
            if(m_StartOption == StartOption.OnStart) StartTween();
        }
        virtual protected void OnDestroy() {
            m_End = true;
            if(m_EndOnDestroy) EndTween();
        }

        /// <summary>
        /// Call this function to start TweenBehavior
        /// </summary>
        [Core.ATTR.UCL_RuntimeOnly]
        [Core.ATTR.UCL_FunctionButton]
        public void StartTween() {
            StartTween(null);
        }
        /// <summary>
        /// Call this function to start TweenBehavior
        /// </summary>
        virtual public void StartTween(System.Action iEndAct) {
            m_End = false;
            if(m_Started) EndTween();

            m_EndAct = iEndAct;
            //Debug.LogWarning("StartTween()!!");
            m_Started = true;

            if(m_StartEvent != null) {
                try {
                    m_StartEvent.Invoke();
                } catch(System.Exception e) {
                    Debug.LogWarning("UCL_TweenBehavior m_StartEvent.Invoke() Exception:" + e);
                }
            }

            StartTweenAction();
        }
        /// <summary>
        /// Call this function to stop TweenBehavior
        /// </summary>
        [Core.ATTR.UCL_RuntimeOnly]
        [Core.ATTR.UCL_FunctionButton]
        public void EndTween() {
            EndTween(false);
        }
        /// <summary>
        /// Call this function to stop TweenBehavior
        /// </summary>
        virtual public void EndTween(bool iComplete) {
            if(!m_Started) return;
            //Debug.LogWarning("EndTween()!!");
            if(iComplete) {
                if(m_EndAct != null) {
                    m_EndAct.Invoke();
                }
                if(m_EndEvent != null) {
                    try {
                        m_EndEvent.Invoke();
                    } catch(System.Exception e) {
                        Debug.LogWarning("UCL_TweenBehavior m_EndEvent.Invoke() Exception:" + e);
                    }
                }
            }
            m_EndAct = null;
            //m_EndEvent?.Invoke();
            EndTweenAction(iComplete);
            m_Started = false;
            if(m_Looping && !m_End) StartTween();
        }
        virtual protected void StartTweenAction() { }
        virtual protected void EndTweenAction(bool complete) { }
#if UNITY_EDITOR


#endif
        virtual public void PauseTween() { }
        virtual public void ResumeTween() { }

        virtual protected void OnDisable() {
            if (m_EndOnDisable)
            {
                m_End = true;
                EndTween();
            }
        }
        virtual protected void OnEnable() {
            if(m_StartOption == StartOption.OnEnable) StartTween();
        }
    }
}