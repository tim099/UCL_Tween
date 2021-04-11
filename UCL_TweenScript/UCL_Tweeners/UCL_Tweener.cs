using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_Tweener : UCL_Tween {
        override public string Name {
            get {
                string name = this.GetType().Name.Replace("UCL_", string.Empty);
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
        /// <summary>
        /// Ease of the tweener
        /// </summary>
        protected Ease.UCL_Ease m_Ease = null;
        /// <summary>
        /// Reverse the tweener
        /// </summary>
        protected bool m_Reverse = false;
        /// <summary>
        /// Do backfolding(x from 0 to 1 then go from 1 to 0)
        /// 折返 x值從0到達1之後折返回0
        /// </summary>
        protected bool m_Backfolding = false;
        /// <summary>
        /// Trigger complete action on exception heppened
        /// </summary>
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
        override public bool KillOnTransform(Transform t, bool compelete = false) {
            foreach(var tc in m_Components) {
                if(tc.GetTarget().Equals(t)) {
                    Kill(compelete);
                    return true;
                }
            }
            return false;
        }
        protected internal override void TweenStart() {
            base.TweenStart();
            foreach(var com in m_Components) com.Start();

            TweenerStart();
        }
        virtual protected void TweenTimeUpdate(float iX)
        {
            float aY = GetY(iX);

            foreach (var com in m_Components)
            {
                try
                {
                    com.Update(aY);
                }
                catch (System.Exception iE)
                {
                    Debug.LogException(iE);
                    Kill(m_CompleteOnException);
                    return;
                }
            }
            TweenerUpdate(aY);
            if (m_UpdateAct != null)
            {
                try
                {
                    m_UpdateAct.Invoke(aY);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("UCL_Tweener.SetTweenTime m_UpdateAct.Invoke(y) Exception:" + e);
                    Kill(m_CompleteOnException);
                    return;
                }
            }
        }
        protected override long TimeUpdateAction(long time_delta) {
            long remains = 0;
            if(TimerMs > DurationMs) {
                remains = TimerMs - DurationMs;
                m_Timer.SetTime(m_Duration);
            }

            TweenTimeUpdate(GetX());

            return remains;
        }
        protected override float TimeUpdateAction(float time_delta) {
            float remains = 0;
            if(Timer > Duration) {
                remains = Timer - Duration;
                m_Timer.SetTime(m_Duration);
            }

            TweenTimeUpdate(GetX());

            return remains;
        }
        protected override void CompleteAction() {
            base.CompleteAction();
            if (m_Backfolding)
            {
                TweenTimeUpdate(0f);
            }
            else
            {
                TweenTimeUpdate(1f);
            }

            foreach(var com in m_Components) com.Complete();
            TweenerCompleteAction();
        }
        virtual public Vector2 GetPos() {
            float x = Timer;
            if(Duration > 0) x /= Duration;

            return new Vector2(x, GetY(x));
        }
        /// <summary>
        /// Get progress of tween (range from 0 ~ 1)
        /// </summary>
        /// <returns></returns>
        virtual public float GetProgress()
        {
            float aProgress = Timer;
            if (Duration > 0) aProgress /= Duration;
            if (aProgress > 1.0f) aProgress = 1.0f;
            return aProgress;
        }
        /// <summary>
        /// Convert tween time to x(range from 0 ~ 1) ,and Ignore Backfolding
        /// </summary>
        /// <returns></returns>
        virtual public float GetOriginX()
        {
            float aX = Timer;
            if (Duration > 0) aX /= Duration;
            if (aX > 1.0f) aX = 1.0f;
            return aX;
        }
        /// <summary>
        /// Get tween position, f(X) = Y where f is the EaseFunction, and Ignore Backfolding
        /// </summary>
        /// <returns></returns>
        virtual public float GetOriginY()
        {
            float aX = Timer;
            if (Duration > 0) aX /= Duration;
            return GetY(aX);
        }
        /// <summary>
        /// Convert tween time to x(range from 0 ~ 1)
        /// </summary>
        /// <returns></returns>
        virtual public float GetX() {
            float x = Timer;
            if(Duration > 0) x /= Duration;
            if(x > 1.0f) x = 1.0f;
            if (m_Backfolding)
            {
                x = 2 * x;
                if (x > 1f)
                {
                    x = 2 - x;
                }
            }
            return x;
        }
        /// <summary>
        /// Get tween position, f(X) = Y where f is the EaseFunction
        /// </summary>
        /// <returns></returns>
        virtual public float GetY() {
            return GetY(GetX());
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
        virtual public UCL_Tweener SetEase(EaseClass ease, EaseDir dir) {
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
        /// <summary>
        /// Reverse the movement of tweener
        /// </summary>
        /// <param name="iDoReverse"></param>
        /// <returns></returns>
        public UCL_Tweener SetReverse(bool iDoReverse) {
            m_Reverse = iDoReverse;
            return this;
        }
        public UCL_Tweener SetBackfolding(bool iDoBackfolding)
        {
            m_Backfolding = iDoBackfolding;
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