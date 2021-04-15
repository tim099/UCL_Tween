using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
#endif
    public class UCL_TweenTimeManager : MonoBehaviour {
        public enum UpdateMode {
            FloatSecond,
            LongMs
        }
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
        public UpdateMode m_UpdateMode = UpdateMode.FloatSecond;

        /// <summary>
        /// if TimeUpdate delta_time is longer then MaxTimeInterval
        /// delta_time will be slice into segment smaller then MaxTimeInterval
        /// </summary>
        public float m_MaxTimeInterval = 0.05f;
        public long m_MaxTimeIntervalMs = 50;//50 Ms
        List<UCL_Tween> m_Tweens = new List<UCL_Tween>();
        Queue<UCL_Tween> m_NewTweenQue = new Queue<UCL_Tween>();
        List<UCL_Tween> m_EndTweens = new List<UCL_Tween>();
        bool m_Updating = false;
        //UCL_TweenTimeManager() {}
        public static UCL_TweenTimeManager Create(GameObject obj) {
            return obj.AddComponent<UCL_TweenTimeManager>();
            //return new UCL_TweenTimeManager();
        }
        public void Init() {

        }
        internal void Add(UCL_Tween tween) {
            if(m_Updating) {
                m_NewTweenQue.Enqueue(tween);
                return;
            }
            tween.TweenStart();
            m_Tweens.Add(tween);
        }
#if UNITY_EDITOR
        [UCL.Core.ATTR.UCL_RuntimeOnly]
        [Core.ATTR.UCL_FunctionButton("KillAllTweens(complete = false)", false)]
        [Core.ATTR.UCL_FunctionButton("KillAllTweens(complete = true)", true)]
#endif
        public void KillAllTweens(bool complete = false) {
            m_Updating = true;
            for(int i = 0; i < m_Tweens.Count; i++) {
                m_Tweens[i].Kill(complete);
            }
            m_Updating = false;
            while(m_NewTweenQue.Count > 0) {
                Add(m_NewTweenQue.Dequeue());
            }
        }

        public void KillAllOnTransform(Transform t, bool complete = false) {
            m_Updating = true;
            for(int i = 0; i < m_Tweens.Count; i++) {
                m_Tweens[i].KillOnTransform(t, complete);
            }
            m_Updating = false;
            while(m_NewTweenQue.Count > 0) {
                Add(m_NewTweenQue.Dequeue());
            }
        }


#if UNITY_EDITOR
        [UCL.Core.ATTR.UCL_RuntimeOnly]
        [Core.ATTR.UCL_FunctionButton]
#endif
        public void PauseAllTweens() {
            for(int i = 0; i < m_Tweens.Count; i++) {
                m_Tweens[i].Pause();
            }
        }
#if UNITY_EDITOR
        [UCL.Core.ATTR.UCL_RuntimeOnly]
        [Core.ATTR.UCL_FunctionButton]
#endif
        public void ResumeAllTweens() {
            for(int i = 0; i < m_Tweens.Count; i++) {
                m_Tweens[i].Resume();
            }
        }

        public float TimeUpdate(float delta_time) {
            if(TimeScale != 1) {
                delta_time *= TimeScale;
            }
            if(delta_time <= m_MaxTimeInterval) {
                TimeUpdateAction((tween) => { tween.TimeUpdate(delta_time); });
            } else {
                int seg = Mathf.CeilToInt(delta_time / m_MaxTimeInterval);
                float seg_time = delta_time / seg;
                //Debug.LogWarning("Seg:" + seg + ",seg_time:" + seg_time);
                for(int i = 0; i < seg; i++) {
                    TimeUpdateAction((tween) => { tween.TimeUpdate(seg_time); });
                }
            }
            return delta_time;
        }
        public long TimeUpdate(long delta_time) {
            if(TimeScale != 1) {
                delta_time = Mathf.RoundToInt(delta_time * TimeScale);
            }
            if(delta_time <= m_MaxTimeIntervalMs) {
                TimeUpdateAction((tween) => { tween.TimeUpdate(delta_time); });
            } else {
                int seg = Mathf.CeilToInt(delta_time / m_MaxTimeIntervalMs);
                long seg_time = Mathf.RoundToInt(delta_time / seg);
                //Debug.LogWarning("Seg:" + seg + ",seg_time:" + seg_time);
                for(int i = 0; i < seg - 1; i++) {
                    TimeUpdateAction((tween)=> { tween.TimeUpdate(seg_time); });
                }
                var final_time = delta_time - seg_time * (seg - 1);
                if(final_time > 0) {
                    TimeUpdateAction((tween) => { tween.TimeUpdate(final_time); });
                }
            }
            return delta_time;
        }
        void TimeUpdateAction(System.Action<UCL_Tween> iUpdateAct) {
            m_Updating = true;
            for(int i = 0; i < m_Tweens.Count; i++) {
                var tween = m_Tweens[i];
                if(tween.End) {
                    m_EndTweens.Add(tween);
                } else {
                    try {
                        iUpdateAct.Invoke(tween);
                        //tween.TimeUpdate(delta_time);
                    } catch(System.Exception e) {
                        tween.Kill();
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

            m_Updating = false;
            while(m_NewTweenQue.Count > 0) {
                Add(m_NewTweenQue.Dequeue());
            }
        }
        private void Update() {
            if(m_AutoUpdate) {
                switch(m_UpdateMode) {
                    case UpdateMode.FloatSecond: {
                            TimeUpdate(Time.deltaTime);
                            break;
                        }
                    case UpdateMode.LongMs: {
                            TimeUpdate(Mathf.RoundToInt(1000f*Time.deltaTime));
                            break;
                        }
                }
                
            }
        }


#if UNITY_EDITOR
        UCL_Tween m_EditorSelectedTween;
        public void OnInspectorGUI() {
            GUILayout.BeginVertical();
            
            if(m_Tweens != null) {
                GUILayout.Box("TweenCount:" + m_Tweens.Count);
                for(int i = 0; i < m_Tweens.Count; i++) {
                    var tween = m_Tweens[i];

                    GUILayout.Label(tween.Name);
                    GUILayout.BeginHorizontal();
                    if(GUILayout.Button(tween.IsPaused ? "Resume" : "Pause", GUILayout.Width(100))) {
                        tween.SetPause(!tween.IsPaused);
                    }
                    if(GUILayout.Button("Kill", UCL.Core.UI.UCL_GUIStyle.TextRed, GUILayout.Width(80))) {
                        tween.Kill(false);
                    }
                    if(GUILayout.Button("Kill(Complete)", UCL.Core.UI.UCL_GUIStyle.TextRed)) {
                        tween.Kill(true);
                    }
                    if(GUILayout.Button("Select")) {
                        m_EditorSelectedTween = tween;
                        m_EditorSelectedTween.OnSelected();
                    }
                    GUILayout.EndHorizontal();
                }
            } else {
                GUILayout.Box("TweenCount: 0");
            }
            if(m_EditorSelectedTween != null) {
                if(m_EditorSelectedTween.End) {
                    m_EditorSelectedTween = null;
                } else {
                    m_EditorSelectedTween.OnInspectorGUI();
                }
            }


            GUILayout.EndVertical();
        }
#endif
    }
}