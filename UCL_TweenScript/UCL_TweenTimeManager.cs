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
        /// <summary>
        /// if m_AutoUpdate == true then UCL_TweenTimeManager will call TimeUpdate on Update
        /// </summary>
        public bool m_AutoUpdate = true;
        /// <summary>
        /// if TimeUpdate delta_time is longer then MaxTimeInterval
        /// delta_time will be slice into segment smaller then MaxTimeInterval
        /// </summary>
        public float m_MaxTimeInterval = 0.05f;
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
        public void TimeUpdate(float delta_time) {
            if(TimeScale != 1) {
                delta_time *= TimeScale;
            }
            if(delta_time <= m_MaxTimeInterval) {
                TimeUpdateAction(delta_time);
            } else {
                int seg = Mathf.CeilToInt(delta_time / m_MaxTimeInterval);
                
                float seg_time = delta_time / seg;
                //Debug.LogWarning("Seg:" + seg + ",seg_time:" + seg_time);
                for(int i = 0; i < seg; i++) {
                    TimeUpdateAction(seg_time);
                }
            }

        }
        void TimeUpdateAction(float delta_time) {
            for(int i = 0; i < m_Tweens.Count; i++) {
                var tween = m_Tweens[i];
                if(tween.End) {
                    m_EndTweens.Add(tween);
                } else {
                    try {
                        tween.TimeUpdate(delta_time);
                    } catch(System.Exception e) {
                        Debug.LogWarning("UCL_TweenTimeManager tween.TimeUpdate Exception:" + e);
                    }
                    try {
                        if(tween.CheckComplete()) {
                            m_EndTweens.Add(tween);
                        }
                    } catch(System.Exception e) {
                        Debug.LogWarning("UCL_TweenTimeManager tween.CheckComplete() Exception:" + e);
                    }

                }
            }
            for(int i = 0; i < m_EndTweens.Count; i++) {
                m_Tweens.Remove(m_EndTweens[i]);
                //Debug.LogWarning("remove:" + i + ",TweenCount:" + TweenCount);
            }
            m_EndTweens.Clear();
        }

        private void Update() {
            if(m_AutoUpdate) {
                TimeUpdate(Time.deltaTime);
            }
        }
    }
}