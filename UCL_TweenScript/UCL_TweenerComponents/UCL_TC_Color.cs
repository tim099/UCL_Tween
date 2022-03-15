using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UCL.TweenLib
{
    public class UCL_TC_Color : UCL_TweenerComponent
    {
        public static UCL_TC_Color Create()
        {
            return new UCL_TC_Color();
        }
        override public string Name => "Color";

        public Image m_TargetImage = null;
        public RawImage m_RawImage = null;
        public Color m_StartColor = Color.clear;
        public Color m_EndColor = Color.clear;
        protected override void ComponentUpdate(float iPos) {
            Color aCol = Color.Lerp(m_StartColor, m_EndColor, iPos);
            if (m_TargetImage != null)
            {
                m_TargetImage.color = aCol;
            }
            if(m_RawImage != null)
            {
                m_RawImage.color = aCol;
            }
        }
    }
}