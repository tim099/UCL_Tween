﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_Tweener : UCL_Tween {
        override public string Name {
            get {
                string name = this.GetType().Name.Replace("UCL_",string.Empty);
                if(m_Components != null && m_Components.Count > 0) {
                    name += "(";
                    for(int i = 0; i < m_Components.Count; i++) {
                        name += m_Components[i].Name;
                        if(i < m_Components.Count - 1) name += ",";
                    }
                    name += ")";
                }

                return name;
            }
        }

        protected Ease.UCL_Ease m_Ease = null;
        protected bool m_Reverse = false;
        protected bool m_CompleteOnException = false;
        protected System.Action<float> m_UpdateAct = null;
        protected List<UCL_TweenerComponent> m_Components = new List<UCL_TweenerComponent>();

        static public UCL_Tweener CreateTweener() {
            return new UCL_Tweener();
        }
        public override void OnDrawGizmos() {
            base.OnDrawGizmos();

            foreach(var tc in m_Components) {
                tc.OnDrawGizmos();
            }
        }
        protected override void InitTween() {
            m_Ease = null;
            foreach(var com in m_Components) com.Init();
        }
        /// <summary>
        /// float value pass to UpdateAct is current y axis value(range 0 ~ 1)
        /// </summary>
        /// <param name="_UpdateAct">y value will pass to _UpdateAct as parameter</param>
        virtual public UCL_Tweener OnUpdate(System.Action<float> _UpdateAct) {
            m_UpdateAct = _UpdateAct;
            return this;
        }
        /// <summary>
        /// float value pass to UpdateAct is current y axis value(range 0 ~ 1)
        /// </summary>
        /// <param name="_UpdateAct"></param>
        /// <returns>y value will pass to _UpdateAct as parameter</returns>
        virtual public UCL_Tweener AddUpdateAction(System.Action<float> _UpdateAct) {
            AddComponent(LibTC.Action(_UpdateAct));
            return this;
        }

        virtual protected void TweenerUpdate(float pos) { }

        virtual protected void TweenerStart() { }
        virtual protected void TweenerCompleteAction() { }

        virtual public UCL_Tweener AddComponent(UCL_TweenerComponent component) {
            //component.p_Tweener = this;
            m_Components.Add(component);
            return this;
        }

        protected internal override void TweenStart() {
            base.TweenStart();
            foreach(var com in m_Components) com.Start();

            TweenerStart();
        }
        protected override long TimeUpdateAction(long time_delta) {
            long remains = 0;
            if(TimerMs > DurationMs) {
                remains = TimerMs - DurationMs;
                m_Timer.SetTime(m_Duration);
            }

            float x = GetX();
            float y = GetY(x);

            foreach(var com in m_Components) {
                try {
                    com.Update(y);
                } catch(System.Exception e) {
                    Debug.LogError("UCL_Tweener.TimeUpdateAction com.Update(y) Exception:" + e);
                    Kill(m_CompleteOnException);
                    return remains;
                }
            }
            TweenerUpdate(y);
            if(m_UpdateAct != null) {
                try {
                    m_UpdateAct.Invoke(y);
                } catch(System.Exception e) {
                    Debug.LogError("UCL_Tweener.TimeUpdateAction m_UpdateAct.Invoke(y) Exception:" + e);
                    Kill(m_CompleteOnException);
                    return remains;
                }
            }

            return remains;
        }
        protected override float TimeUpdateAction(float time_delta) {
            float remains = 0;
            if(Timer > Duration) {
                remains = Timer - Duration;
                m_Timer.SetTime(m_Duration);
            }

            float x = GetX();
            float y = GetY(x);

            foreach(var com in m_Components) {
                try {
                    com.Update(y);
                } catch(System.Exception e) {
                    Debug.LogError("UCL_Tweener.TimeUpdateAction com.Update(y) Exception:" + e);
                    Kill();
                    return remains;
                }
            }
            TweenerUpdate(y);
            if(m_UpdateAct != null) {
                try {
                    m_UpdateAct.Invoke(y);
                } catch(System.Exception e) {
                    Debug.LogError("UCL_Tweener.TimeUpdateAction m_UpdateAct.Invoke(y) Exception:" + e);
                    Kill();
                    return remains;
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
            float x = Timer;
            if(Duration > 0) x /= Duration;

            return new Vector2(x, GetY(x));
        }
        virtual public float GetX() {
            float x = Timer;
            if(Duration > 0) x /= Duration;
            if(x > 1.0f) x = 1.0f;
            return x;
        }
        virtual public float GetY() {
            float x = Timer;
            if(Duration > 0) x /= Duration;
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
            return SetEase(EaseCreator.Get(ease, dir));
        }
        virtual public UCL_Tweener SetEase(EaseType type) {
            return SetEase(EaseCreator.Get(type));
        }
        virtual public UCL_Tweener SetEase(System.Func<float, float> _EaseFunction) {
            SetEase(new Ease.UCL_EaseFunction(_EaseFunction));
            return this;
        }
        virtual public UCL_Tweener SetEase(AnimationCurve _Curve) {
            SetEase(new Ease.UCL_EaseAnimationCurve(_Curve));
            return this;
        }
        virtual public UCL_Tweener SetEase(Ease.UCL_Ease _ease) {
            m_Ease = _ease;
            return this;
        }
        public UCL_Tweener SetCompleteOnException(bool val) {
            m_CompleteOnException = val;
            return this;
        }
        public UCL_Tweener SetReverse(bool val) {
            m_Reverse = val;
            return this;
        }

#if UNITY_EDITOR
        override internal void OnInspectorGUI() {
            base.OnInspectorGUI();
            GUILayout.BeginVertical();
            foreach(var tc in m_Components) {
                tc.OnInspectorGUI();
            }
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Called when being selected
        /// </summary>
        override internal void OnSelected() {
            foreach(var tc in m_Components) {
                tc.OnSelected();
            }
        }
#endif
    }
}