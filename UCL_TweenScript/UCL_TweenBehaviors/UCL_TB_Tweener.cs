using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
    //[Core.ATTR.RequiresConstantRepaint]
#endif
    public class UCL_TB_Tweener : UCL_TweenBehavior {
        #region Editor
#if UNITY_EDITOR
        [Core.ATTR.UCL_DrawTexture2D(128, 128, TextureFormat.ARGB32, typeof(Ease.UCL_EaseTexture))]
        public void DrawEaseCurve(Core.TextureLib.UCL_Texture2D texture) {
            Ease.UCL_EaseTexture.DrawEase(m_Ease, texture);
        }
#endif
        #endregion
        
        /// <summary>
        /// m_Tweener.Kill(m_CompleteWhenKill);
        /// </summary>
        public bool m_CompleteWhenKill = false;
        public EaseType m_Ease = EaseType.Linear;
        public float m_Duration = 5f;

        public List<UCL_TC_Data> m_TweenerComponents;
        protected UCL_Tweener m_Tweener;

        virtual public void StartTweener() { }

        public override void StartTween() {
            base.StartTween();
            StartTweener();
            //CreateTweener();
        }
        public override void EndTween() {
            base.EndTween();
            Kill();
        }
        virtual protected void OnDestroy() {
            Kill();
        }

        virtual protected UCL_Tweener CreateTweener() {
            Kill();
            m_Tweener = Lib.Tweener(m_Duration).SetEase(m_Ease);
            return m_Tweener;
        }
        virtual public void Kill() {
            if(m_Tweener != null) {
                m_Tweener.Kill(m_CompleteWhenKill);
                m_Tweener = null;
            }
        }
    }
}