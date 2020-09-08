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
        [Core.ATTR.UCL_DrawTexture2D]
        public Core.TextureLib.UCL_Texture2D Editor_DrawEaseCurve() {
            if(m_EaseTexture == null) {
                m_EaseTexture = new Ease.UCL_EaseTexture(new Vector2Int(128,128), TextureFormat.ARGB32);
            }
            Ease.UCL_EaseTexture.DrawEase(m_Ease, m_EaseTexture);
            return m_EaseTexture;
            //GUILayout.Box(m_EaseTexture.texture);
        }
        [Core.ATTR.UCL_RuntimeOnly]
        [Core.ATTR.UCL_FunctionButton("Time Alter -0.5 sec", -0.5f)]
        [Core.ATTR.UCL_FunctionButton("Time Alter 0.5 sec", 0.5f)]
        public void TimeAlter(float val) {
            if(m_Tweener == null) return;
            m_Tweener.TimeAlter(val);
        }
#endif
        #endregion

        /// <summary>
        /// m_Tweener.Kill(m_CompleteWhenKill);
        /// </summary>
        public bool m_CompleteWhenKill = false;
        public EaseType m_Ease = EaseType.Linear;
        public float m_Duration = 5f;

        [Header("Debug Setting")]
        public bool m_DrawGizmos = true;
        [Header("Tweener use the default TimeManager when m_TimeManager is null")]
        public UCL_TweenTimeManager m_TimeManager;
        //[HideInInspector]
        public List<UCL_TC_Data> m_TweenerComponents;
        protected UCL_Tweener m_Tweener;

        /// <summary>
        /// override this to implement StartTweener action
        /// </summary>
        virtual public void StartTweener() { CreateTweener().Start(m_TimeManager); }
        virtual protected void EndTweener() { Kill(); }

        protected override void StartTweenAction() {
            StartTweener();
        }
        protected override void EndTweenAction(bool complete) {
            EndTweener();
        }
        public override void PauseTween() {
            if(!m_Started) return;
            base.PauseTween();
            if(m_Tweener != null) {
                m_Tweener.Pause();
            }
        }
        public override void ResumeTween() {
            if(!m_Started) return;
            base.ResumeTween();
            if(m_Tweener != null) {
                m_Tweener.Resume();
            }
        }
        virtual protected void OnDrawGizmos() {
#if UNITY_EDITOR
            if(!m_DrawGizmos) return;
            if(m_Tweener != null) {
                m_Tweener.OnDrawGizmos();
            }
#endif
        }
#if UNITY_EDITOR
        [Core.ATTR.UCL_DrawString]
        string GetY() {
            if(m_Tweener == null) return "Y:0";// return null;
            return "Y:" + m_Tweener.GetY().ToString("N2");
        }
#endif
        virtual protected UCL_Tweener CreateTweener() {
            Kill();
            m_Tweener = LibTween.Tweener(m_Duration).SetEase(m_Ease);
            for(int i = 0; i < m_TweenerComponents.Count; i++) {
                var comp = m_TweenerComponents[i].CreateTweenerComponent();
                m_Tweener.AddComponent(comp);
                //Debug.LogWarning("AddCom:" + comp.GetType().Name);
            }
            m_Tweener.OnComplete(()=> { EndTween(true); });
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