using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TweenBehavior : MonoBehaviour {
        public bool m_StartOnEnable = false;
        public bool m_EndOnDisable = false;
        virtual public void StartTween() { }
        virtual public void EndTween() { }
        virtual protected void OnDisable() {
            if(m_EndOnDisable) EndTween();
        }
        virtual protected void OnEnable() {
            if(m_StartOnEnable) StartTween();
        }
    }
}