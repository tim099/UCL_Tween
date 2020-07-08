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
        Ease.UCL_EaseTexture m_EaseTexture;
        public void Editor_DrawEaseCurve() {
            if(m_EaseTexture == null) {
                m_EaseTexture = new Ease.UCL_EaseTexture(new Vector2Int(128,128), TextureFormat.ARGB32);
            }
            Ease.UCL_EaseTexture.DrawEase(m_Ease, m_EaseTexture);
            GUILayout.Box(m_EaseTexture.texture);
        }
#endif
        #endregion
        
        /// <summary>
        /// m_Tweener.Kill(m_CompleteWhenKill);
        /// </summary>
        public bool m_CompleteWhenKill = false;
        public EaseType m_Ease = EaseType.Linear;
        public float m_Duration = 5f;

        //[HideInInspector]
        public List<UCL_TC_Data> m_TweenerComponents;
        protected UCL_Tweener m_Tweener;

        /// <summary>
        /// override this to implement StartTweener action
        /// </summary>
        virtual public void StartTweener() { CreateTweener().Start(); }
        virtual protected void EndTweener() {
            Kill();
        }
        protected override void StartTweenAction() {
            StartTweener();
        }
        protected override void EndTweenAction() {
            EndTweener();
        }

        virtual protected UCL_Tweener CreateTweener() {
            Kill();
            m_Tweener = LibTween.Tweener(m_Duration).SetEase(m_Ease);
            for(int i = 0; i < m_TweenerComponents.Count; i++) {
                var comp = m_TweenerComponents[i].CreateTweenerComponent();
                m_Tweener.AddComponent(comp);
                //Debug.LogWarning("AddCom:" + comp.GetType().Name);
            }
            m_Tweener.OnComplete(EndTween);
            return m_Tweener;
        }
        virtual public void Kill() {
            if(m_Tweener != null) {
                m_Tweener.Kill(m_CompleteWhenKill);
                m_Tweener = null;
            }
        }
        /*
#if UNITY_EDITOR
        [SerializeField] protected float m_Y = 0;
#endif
        virtual protected void Update() {
#if UNITY_EDITOR
            if(m_Tweener != null) {
                m_Y = m_Tweener.GetY();
            }
#endif
        }
        */
    }
}