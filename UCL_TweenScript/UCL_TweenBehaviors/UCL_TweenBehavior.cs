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

        public bool m_Looping = false;
        
        public UnityEngine.Events.UnityEvent m_StartEvent;
        public UnityEngine.Events.UnityEvent m_EndEvent;
        protected bool m_Started = false;
        protected bool m_End = false;

        virtual protected void Start() {
            if(m_StartOption == StartOption.OnStart) StartTween();
        }
        virtual protected void OnDestroy() {
            m_End = true;
            if(m_EndOnDestroy) EndTween();
        }
        virtual protected void StartTweenAction() { }
        virtual protected void EndTweenAction() { }
#if UNITY_EDITOR
        [Core.ATTR.UCL_RuntimeOnly]
        [Core.ATTR.UCL_FunctionButton("StartTween(Editor)")]
        public void Editor_StartTween() {
            if(!Application.isPlaying) {
                Debug.LogError("UCL_TweenBehavior Editor_StartTween() Fail!! Please press in play mode!!");
                return;
            }
            StartTween();
        }
        [Core.ATTR.UCL_RuntimeOnly]
        [Core.ATTR.UCL_FunctionButton("EndTween(Editor)")]
        public void Editor_EndTween() {
            EndTween();
        }
        [Core.ATTR.UCL_RuntimeOnly]
        [Core.ATTR.UCL_FunctionButton("PauseTween(Editor)")]
        public void Editor_PauseTween() {
            PauseTween();
        }
        [Core.ATTR.UCL_RuntimeOnly]
        [Core.ATTR.UCL_FunctionButton("ResumeTween(Editor)")]
        public void Editor_ResumeTween() {
            ResumeTween();
        }

#endif
        virtual public void StartTween() {
            if(m_Started) EndTween();
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
        virtual public void EndTween() {
            if(!m_Started) return;
            //Debug.LogWarning("EndTween()!!");
            if(m_EndEvent != null) {
                try {
                    m_EndEvent.Invoke();
                } catch(System.Exception e) {
                    Debug.LogWarning("UCL_TweenBehavior m_EndEvent.Invoke() Exception:" + e);
                }
            }
            //m_EndEvent?.Invoke();
            EndTweenAction();
            m_Started = false;
            if(m_Looping && !m_End) StartTween();
        }
        virtual public void PauseTween() { }
        virtual public void ResumeTween() { }
        virtual protected void OnDisable() {
            if(m_EndOnDisable) EndTween();
        }
        virtual protected void OnEnable() {
            if(m_StartOption == StartOption.OnEnable) StartTween();
        }
    }
}