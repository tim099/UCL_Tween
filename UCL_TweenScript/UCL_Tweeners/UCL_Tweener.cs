using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_Tweener : UCL_Tween {
        protected Ease.UCL_Ease m_Ease = null;
        protected bool m_Reverse = false;
        protected System.Action<float> m_UpdateAct = null;
        protected List<UCL_TweenerComponent> m_Components = new List<UCL_TweenerComponent>();

        static public UCL_Tweener CreateTweener() {
            return new UCL_Tweener();
        }
        protected override void InitTween() {
            m_Ease = null;
            foreach(var com in m_Components) com.Init();
        }
        /// <summary>
        /// float value pass to UpdateAct is current y axis value(range 0 ~ 1)
        /// </summary>
        /// <param name="_UpdateAct"></param>
        virtual public void OnUpdate(System.Action<float> _UpdateAct) {
            m_UpdateAct = _UpdateAct;
        }
        virtual protected void TweenerUpdate(float pos) { }

        virtual protected void TweenerStart() { }
        virtual protected void TweenerCompleteAction() { }

        virtual public UCL_Tweener AddComponent(UCL_TweenerComponent component) {
            m_Components.Add(component);
            return this;
        }
        protected internal override void TweenStart() {
            base.TweenStart();
            foreach(var com in m_Components) com.Start();

            TweenerStart();
        }
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

            foreach(var com in m_Components) {
                try {
                    com.Update(y);
                } catch(System.Exception e) {
                    Debug.LogWarning("UCL_Tweener.TimeUpdateAction com.Update(y) Exception:" + e);
                }
            }
            TweenerUpdate(y);
            if(m_UpdateAct != null) {
                try {
                    m_UpdateAct.Invoke(y);
                } catch(System.Exception e) {
                    Debug.LogWarning("UCL_Tweener.TimeUpdateAction m_UpdateAct.Invoke(y) Exception:" + e);
                }
            }

            return remains;
        }
        protected override void CompleteAction() {
            base.CompleteAction();
            foreach(var com in m_Components) com.Complete();
            TweenerCompleteAction();
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

            if(m_Reverse) {
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
        public UCL_Tweener SetReverse(bool val) {
            m_Reverse = val;
            return this;
        }
    }
}