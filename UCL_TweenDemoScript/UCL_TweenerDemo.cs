using System.Collections;
using System.Collections.Generic;
using UCL.TweenLib.Ease;
using UnityEngine;

namespace UCL.TweenLib.Demo {
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
    [Core.ATTR.RequiresConstantRepaint]
#endif
    public class UCL_TweenerDemo : MonoBehaviour {
        public EaseType m_Ease;
        public float m_Duration = 5.0f;
        public Transform m_Target;
        public Core.MathLib.UCL_Curve m_Curve;
        UCL_TweenerCurve m_Cur = null;
#if UNITY_EDITOR
        private void Start() {
            StartTweener();
        }

        [Core.ATTR.UCL_DrawTexture2D(128 , 128, TextureFormat.ARGB32, typeof(UCL_EaseTexture))]
        public void DrawEaseCurve(Core.TextureLib.UCL_Texture2D texture) {
            UCL_EaseTexture.DrawEase(m_Ease, texture);
            if(m_Cur != null) {
                Vector2 pos = m_Cur.GetPos();
                var tex = texture as UCL_EaseTexture;
                if(tex != null) {
                    pos.y -= tex.m_Min;
                    pos.y /= tex.m_Range;
                    //pos.y *= 0.99f;
                }
                texture.DrawDot(pos.x, pos.y, Color.red, 2);
            }
        }
        int count = 0;
        [Core.ATTR.UCL_FunctionButton]
        public void StartTweener() {
            if(UnityEditor.EditorApplication.isPlaying) {
                var obj = Instantiate(m_Target.gameObject, m_Target.parent);
                obj.name = "target " + ++count;
                m_Cur = UCL_TweenerCurve.Create();
                var cur = m_Cur;
                m_Cur.Init(m_Duration, m_Curve, obj.transform)
                    .SetEase(m_Ease)
                    .OnComplete(delegate() {
                        if(m_Cur == cur) {
                            m_Cur = null;
                        }
                        Destroy(obj);
                    }).Start();
            } else {
                Debug.LogWarning("StartTweener() !UnityEditor.EditorApplication.isPlaying");
            }
        }
#endif

    }
}