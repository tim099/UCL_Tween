using System.Collections;
using System.Collections.Generic;
using UCL.TweenLib.Ease;
using UnityEngine;

namespace UCL.TweenLib.Demo {
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
    [Core.ATTR.RequiresConstantRepaint]
#endif
    public class UCL_SequenceDemo : MonoBehaviour {
        public EaseType m_Ease;
        //public float m_Duration = 5.0f;
        public Transform m_Target;
        public Transform m_RotTarget;
        public Core.MathLib.UCL_Curve m_Curve;
        UCL_TweenerCurve m_Cur = null;

        [Core.ATTR.UCL_DrawTexture2D(128, 128, TextureFormat.ARGB32, typeof(UCL_EaseTexture))]
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
        [Core.ATTR.UCL_FunctionButton("Kill(complete = true)",true)]
        [Core.ATTR.UCL_FunctionButton("Kill(complete = false)", false)]
        public void Kill(bool complete) {
            if(m_Seq == null) return;
            m_Seq.Kill(complete);
            m_Seq = null;
        }
        [Core.ATTR.UCL_FunctionButton]
        public void StartDemo() {
            Kill(false);

            m_Seq = UCL_Sequence.Create();
            EaseType[] Eases = (EaseType[])System.Enum.GetValues(m_Ease.GetType());
            int at = 0;
            for(; at < Eases.Length;) {
                if(m_Ease == Eases[at]) {
                    break;
                }
                at++;
            }

            int times = 0;
            float interval = 0.001f;
            m_Seq.AppendInterval(interval).OnComplete(delegate () {
                //Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer + ",interval:" + interval);
            });

            m_Seq.AppendInterval(interval).OnComplete(delegate () {
                //Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer + ",interval:" + interval);
            });
            interval = 0.0025f;
            m_Seq.AppendInterval(interval).OnComplete(delegate () {
                //Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer + ",interval:" + interval);
            });
            bool inv = false;
            m_Seq.Append(m_Target.UCL_Move(m_Curve,2)
                .SetEase(Eases[at >= Eases.Length ? at = 0 : at++])
                .SetInverse(inv ^= true)
                .OnComplete(delegate () {
                    Debug.LogWarning("Curve End!!" + ++times+ ",Timer:" + m_Seq.Timer + ",Timer:" + m_Seq.Timer + ",interval:" + interval);
                }));

            interval = 0.005f;
            m_Seq.AppendInterval(interval).OnComplete(delegate () {
                //Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer + ",interval:" + interval);
            });
            interval = 0.01f;
            m_Seq.AppendInterval(interval).OnComplete(delegate () {
                //Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer + ",interval:" + interval);
            });

            interval = 0.1f;
            m_Seq.AppendInterval(interval).OnComplete(delegate () {
                //Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer + ",interval:" + interval);
            });
            interval = 0.5f;
            m_Seq.AppendInterval(interval).OnComplete(delegate () {
                //Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer + ",interval:" + interval);
            });
            m_Seq.Append(m_Target.UCL_Move(m_Curve, 2)
                .SetEase(Eases[at >= Eases.Length ? at = 0 : at++])
                .SetInverse(inv ^= true)
                .OnComplete(delegate () {
                    Debug.LogWarning("Curve End!!" + ++times+ ",Timer:" + m_Seq.Timer);
                }));
            m_Seq.AppendInterval(1.0f).OnComplete(delegate () {
                Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer);
            });
            m_Seq.Append(m_Target.UCL_Move(m_Curve, 2)
                .SetEase(Eases[at >= Eases.Length ? at = 0 : at++])
                .SetInverse(inv ^= true)
                .OnComplete(delegate () {
                    Debug.LogWarning("Curve End!!" + ++times+ ",Timer:" + m_Seq.Timer);
                }));
            m_Seq.AppendInterval(0.001f).OnComplete(delegate () {
                Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer);
            });
            m_Seq.AppendInterval(0.021f).OnComplete(delegate () {
                Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer);
            });
            m_Seq.Append(m_Target.UCL_Move(m_Curve, 2)
                .SetEase(Eases[at >= Eases.Length ? at = 0 : at++])
                .SetInverse(inv ^= true)
                .OnComplete(delegate () {
                    Debug.LogWarning("Curve End!!" + ++times+ ",Timer:" + m_Seq.Timer);
                }));
            m_Seq.AppendInterval(2.0f).OnComplete(delegate () {
                Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer);
            });
            m_Seq.Append(m_Target.UCL_Rotate(45, 90, 150, 2.5f).SetEase(EaseType.OutBounce));
            m_Seq.Append(m_Target.UCL_Move(m_RotTarget.position, 4).SetEase(EaseType.InCirc).OnComplete(()=> {
                Debug.LogWarning("Move End!!" + ++times + ",Timer:" + m_Seq.Timer);
            }));
            m_Seq.Append(m_Target.UCL_Rotate(m_RotTarget.rotation, 2f).SetEase(EaseType.OutBounce));
            //m_RotTarget
            m_Seq.OnComplete(delegate () {
                Debug.LogWarning("End!! " + ",Timer:" + m_Seq.Timer);
                m_Seq = null;
            });
            
            m_Seq.Start();
        }

        UCL_Sequence m_Seq = null;
        private void Start() {
            StartDemo();

        }
        private void Update() {

        }
    }
}

