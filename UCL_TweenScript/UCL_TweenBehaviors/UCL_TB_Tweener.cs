using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TB_Tweener : UCL_TweenBehavior {
        /// <summary>
        /// m_Tweener.Kill(m_CompleteWhenKill);
        /// </summary>
        public bool m_CompleteWhenKill = false;
        public EaseType m_Ease = EaseType.Linear;
        public float m_Duration = 5f;
        protected UCL_Tweener m_Tweener;

        virtual public void StartTweener() { }

        public override void StartTween() {
            base.StartTween();
            StartTweener();
            //CreateTweener();
        }
        public override void EndTween() {
            base.EndTween();
            Kill();
        }
        virtual protected void OnDestroy() {
            Kill();
        }

        virtual protected UCL_Tweener CreateTweener() {
            Kill();
            m_Tweener = Lib.Tweener(m_Duration).SetEase(m_Ease);
            return m_Tweener;
        }
        virtual public void Kill() {
            if(m_Tweener != null) {
                m_Tweener.Kill(m_CompleteWhenKill);
                m_Tweener = null;
            }
        }
    }
}