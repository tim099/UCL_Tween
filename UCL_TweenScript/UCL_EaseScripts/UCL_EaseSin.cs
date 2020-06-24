﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib.Ease {
    [System.Serializable]
    public class Sin : UCL_Ease {
        override public EaseClass GetClass() {
            return EaseClass.Sin;
        }
        public override float GetEase(float x) {
            switch(m_Dir) {
                case EaseDir.In: return (1.0f - Mathf.Cos(0.5f * Mathf.PI * x));
                case EaseDir.Out: return Mathf.Sin(0.5f * Mathf.PI * x);
            }
            return -0.5f * (Mathf.Cos(Mathf.PI * x) - 1);//InOut
        }
    }
}