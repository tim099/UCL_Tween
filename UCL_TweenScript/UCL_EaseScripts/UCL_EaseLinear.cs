﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib.Ease {
    [System.Serializable]
    public class Linear : UCL_Ease {
        override public EaseClass GetClass() {
            return EaseClass.Linear;
        }
        public override float GetEase(float start, float end, float value) {
            return Mathf.Lerp(start, end, value);//Linear
        }
        public override float GetEase(float value) {
            /*
            switch(m_Dir) {
                case EaseDir.In: return value;
                case EaseDir.Out: return 1.0f - value;
            }
            */
            return value;
        }
    }
}