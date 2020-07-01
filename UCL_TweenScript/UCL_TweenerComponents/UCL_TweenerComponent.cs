using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    /// <summary>
    ///UCL_TweenerComponent extened action on tweener
    /// </summary>
    public class UCL_TweenerComponent {
        protected bool m_Reverse = false;
        virtual protected internal void Init() {

        }
        virtual protected internal void Start() {

        }
        virtual protected void ComponentUpdate(float pos) {

        }
        virtual protected internal void Complete() {

        }

        internal void Update(float pos) {
            if(m_Reverse) pos = 1 - pos;
            ComponentUpdate(pos);
        }

        public UCL_TweenerComponent SetReverse(bool val) {
            m_Reverse = val;
            return this;
        }
    }
}