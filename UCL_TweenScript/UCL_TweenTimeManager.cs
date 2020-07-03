using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public class UCL_TweenTimeManager : MonoBehaviour{
        #region Get
        public int TweenCount {
            get {
                return m_Tweens.Count;
            }
        }
        public int TweenerCount {
            get {
                int c = 0;
                for(int i = 0; i < m_Tweens.Count; i++) {
                    if(m_Tweens[i].IsTweener) {
                        c++;
                    }
                }
                return c;
            }
        }
        public int SequenceCount {
            get {
                int c = 0;
                for(int i = 0; i < m_Tweens.Count; i++) {
                    if(m_Tweens[i].IsSequence) {
                        c++;
                    }
                }
                return c;
            }
        }
        #endregion
        public float TimeScale = 1f;

        List<UCL_Tween> m_Tweens = new List<UCL_Tween>();
        List<UCL_Tween> m_EndTweens = new List<UCL_Tween>();
        //UCL_TweenTimeManager() {}
        public static UCL_TweenTimeManager Create(GameObject obj) {
            return obj.AddComponent<UCL_TweenTimeManager>();
            //return new UCL_TweenTimeManager();
        }
        public void Init() {

        }

        internal void Add(UCL_Tween tween) {
            tween.TweenStart();
            m_Tweens.Add(tween);
        }
        public void KillAllTweens(bool complete = false) {
            for(int i = 0; i < m_Tweens.Count; i++) {
                m_Tweens[i].Kill(complete);
            }
        }
        internal void TimeUpdate(float delta_time) {
            if(TimeScale != 1) {
                delta_time *= TimeScale;
            }

            for(int i = 0; i < m_Tweens.Count; i++) {
                var tween = m_Tweens[i];
                if(tween.End) {
                    m_EndTweens.Add(tween);
                } else {
                    tween.TimeUpdate(delta_time);
                    if(tween.CheckComplete()) {
                        m_EndTweens.Add(tween);
                    }
                }
            }
            for(int i = 0; i < m_EndTweens.Count; i++) {
                m_Tweens.Remove(m_EndTweens[i]);
                //Debug.LogWarning("remove:" + i + ",TweenCount:" + TweenCount);
            }
            m_EndTweens.Clear();
        }
    }
}