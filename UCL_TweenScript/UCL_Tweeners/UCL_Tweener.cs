using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_Tweener : UCL_Tween {
        protected Ease.UCL_Ease m_Ease = null;
        protected bool m_Inverse = false;
        protected System.Action<float> m_UpdateAct = null;
        protected override void InitTween() {
            m_Ease = null;
        }
        /// <summary>
        /// float value pass to UpdateAct is current y axis value(range 0 ~ 1)
        /// </summary>
        /// <param name="_UpdateAct"></param>
        virtual public void OnUpdate(System.Action<float> _UpdateAct) {
            m_UpdateAct = _UpdateAct;
        }
        virtual protected void TweenerUpdate(float pos) { }
        protected override float TimeUpdateAction(float time_delta) {
            float remains = 0;
            if(m_Timer > m_Duration) {
                remains = m_Timer - m_Duration;
                m_Timer = m_Duration;
            }
            
            float x = m_Timer;
            if(m_Duration > 0) x /= m_Duration;
            if(x > 1.0f) x = 1.0f;
            float y = GetY(x);

            
            TweenerUpdate(y);
            m_UpdateAct?.Invoke(y);
            return remains;
        }
        virtual public Vector2 GetPos() {
            float x = m_Timer;
            if(m_Duration > 0) x /= m_Duration;

            return new Vector2(x, GetY(x));
        }
        virtual public float GetY() {
            float x = m_Timer;
            if(m_Duration > 0) x /= m_Duration;
            return GetY(x);
        }
        virtual public float GetY(float x) {
            float y = x;
            if(m_Ease != null) {
                y = m_Ease.GetEase(x);
            }

            if(m_Inverse) {
                y = 1 - y;
            }
            return y;
        }
        virtual public UCL_Tweener SetEase(EaseClass ease,EaseDir dir) {
            m_Ease = EaseCreator.Get(ease, dir);
            return this;
        }
        virtual public UCL_Tweener SetEase(EaseType type) {
            m_Ease = EaseCreator.Get(type);
            return this;
        }
        virtual public UCL_Tweener SetEase(Ease.UCL_Ease _ease) {
            m_Ease = _ease;
            return this;
        }
        public UCL_Tweener SetInverse(bool inv) {
            m_Inverse = inv;
            return this;
        }
    }
}