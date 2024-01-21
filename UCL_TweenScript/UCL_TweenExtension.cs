using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib
{
    public static partial class UCL_TweenExtension
    {
        /// <summary>
        /// Kill iTween if not null
        /// </summary>
        /// <param name="iTween"></param>
        public static void SmartKill(this UCL_Tween iTween, bool iCompelete = false)
        {
            if (iTween == null) return;
            iTween.Kill(iCompelete);
        }
        public static Ease.UCL_Ease GetEase(this EaseType iEaseType)
        {
            return EaseCreator.Get(iEaseType);
        }
    }
}

