using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TB_Move : UCL_TB_Transform {
        public Transform m_TargetTransform;
        protected override void StartTweener() {
            CreateTweener().AddComponent(m_Target.TC_Move(m_TargetTransform.position));
            m_Tweener.Start(m_TimeManager);
        }
    }
}