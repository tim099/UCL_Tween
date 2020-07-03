using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TweenBehavior : MonoBehaviour {
        public bool m_StartOnEnable = false;
        public bool m_EndOnDisable = false;
        public UnityEngine.Events.UnityEvent m_StartEvent;
        public UnityEngine.Events.UnityEvent m_EndEvent;
        virtual public void StartTween() {
            m_StartEvent?.Invoke();
        }
        virtual public void EndTween() {
            m_EndEvent?.Invoke();
        }
        virtual protected void OnDisable() {
            if(m_EndOnDisable) EndTween();
        }
        virtual protected void OnEnable() {
            if(m_StartOnEnable) StartTween();
        }
    }
}