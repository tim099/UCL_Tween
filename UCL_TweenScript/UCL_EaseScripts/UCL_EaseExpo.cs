﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib.Ease {
    public class Expo : UCL_Ease {
        override public EaseClass GetClass() {
            return EaseClass.Expo;
        }
        public override float GetEase(float x) {
            switch(m_Dir) {
                case EaseDir.In:  return x == 0 ? 0 : Mathf.Pow(2, 10 * (x - 1));
                case EaseDir.Out: return x == 1 ? 1 : 1 - Mathf.Pow(2, - 10 * x);
            }
            return x == 0 ? 0
                 : x == 1 ? 1
                 : x < 0.5 ? 0.5f * Mathf.Pow(2, 20 * x - 10)
                 : 0.5f * (2 - Mathf.Pow(2, -20 * x + 10));//InOut
        }
    }
}