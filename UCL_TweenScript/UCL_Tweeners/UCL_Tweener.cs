﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_Tweener : UCL_Tween {

        protected Ease.UCL_Ease m_Ease = null;
        protected System.Action<float> m_UpdateAct = null;
        protected override void InitTween() {
            m_Ease = null;

        }
        virtual public void OnUpdate(System.Action<float> _UpdateAct) {
            m_UpdateAct = _UpdateAct;
        }
        virtual protected void TweenerUpdate(float pos) { }
        protected override void TimeUpdateAction() {
            float at = m_Timer;
            if(m_Duration > 0) at /= m_Duration;
            if(m_Ease != null) {
                at = m_Ease.GetEase(at);
            }
            
            TweenerUpdate(at);
            m_UpdateAct?.Invoke(at);
        }
        virtual public Vector2 GetPos() {
            float x = m_Timer;
            float y = x;
            if(m_Duration > 0) x /= m_Duration;
            if(m_Ease != null) {
                y = m_Ease.GetEase(x);
            }
            return new Vector2(x, y);
        }
        virtual public UCL_Tweener SetEase(EaseClass ease,EaseDir dir) {
            m_Ease = EaseCreator.Get(ease, dir);
            return this;
        }
        virtual public UCL_Tweener SetEase(EaseType type) {
            m_Ease = EaseCreator.Get(type);
            return this;
        }


    }
}