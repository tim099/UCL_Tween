using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
#endif
    public class UCL_TweenBehavior : MonoBehaviour {
        //[SerializeField] protected float m_Timer;//For Inspector
        public bool m_StartOnEnable = false;
        public bool m_EndOnDisable = false;
        public UnityEngine.Events.UnityEvent m_StartEvent;
        public UnityEngine.Events.UnityEvent m_EndEvent;
        protected bool m_Started = false;


        virtual protected void StartTweenAction() { }
        virtual protected void EndTweenAction() { }
#if UNITY_EDITOR
        [Core.ATTR.UCL_RuntimeOnly]
        [Core.ATTR.UCL_FunctionButton]
        public void Editor_StartTween() {
            if(!Application.isPlaying) {
                Debug.LogError("Editor_StartTween() Fail!! Please press in play mode!!");
                return;
            }
            StartTween();
        }
        [Core.ATTR.UCL_RuntimeOnly]
        [Core.ATTR.UCL_FunctionButton]
        public void Editor_EndTween() {
            if(!Application.isPlaying) {
                Debug.LogError("Editor_EndTween() Fail!! Please press in play mode!!");
                return;
            }
            EndTween();
        }
#endif
        virtual public void StartTween() {
            if(m_Started) EndTween();
            Debug.LogWarning("StartTween()!!");
            m_Started = true;
            m_StartEvent?.Invoke();
            StartTweenAction();
            
        }
        virtual public void EndTween() {
            if(!m_Started) return;
            Debug.LogWarning("EndTween()!!");
            m_EndEvent?.Invoke();
            EndTweenAction();
            m_Started = false;
        }
        virtual protected void OnDisable() {
            if(m_EndOnDisable) EndTween();
        }
        virtual protected void OnEnable() {
            if(m_StartOnEnable) StartTween();
        }
    }
}