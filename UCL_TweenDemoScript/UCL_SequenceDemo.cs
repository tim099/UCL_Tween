using System.Collections;
using System.Collections.Generic;
using UCL.TweenLib.Ease;
using UnityEngine;

namespace UCL.TweenLib.Demo {
#if UNITY_EDITOR
    [Core.ATTR.EnableUCLEditor]
    //[Core.ATTR.RequiresConstantRepaint]
#endif
    public class UCL_SequenceDemo : MonoBehaviour {
        public EaseType m_Ease;
        public EaseType m_CurEase;

        public Transform m_Target;
        public Transform m_Target2;
        public Transform m_Target3;
        public Transform[] m_LookTargets;
        public Transform m_RotTarget;
        public float m_Y = 0;
        public bool m_AutoLoop = true;
        public Core.MathLib.UCL_Curve m_Curve;
        UCL_TweenerCurve m_Cur = null;
#if UNITY_EDITOR
        [Core.ATTR.UCL_DrawTexture2D(128, 128, TextureFormat.ARGB32, typeof(UCL_EaseTexture))]
        public void DrawEaseCurve(Core.TextureLib.UCL_Texture2D texture) {
            if(UnityEditor.EditorApplication.isPlaying && m_Seq != null) {
                UCL_EaseTexture.DrawEase(m_CurEase, texture);
            } else {
                UCL_EaseTexture.DrawEase(m_Ease, texture);
            }

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
#endif
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

            interval = 0.0025f;
            m_Seq.AppendInterval(interval).OnComplete(delegate () {
                //Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer + ",interval:" + interval);
            });
            bool rev = true;
            int look_at = 0;
            {
                int n = at >= Eases.Length ? at = 0 : at++;
                m_Seq.Append(
                    LibTween.Tweener(4)
                    .AddComponent(m_Target.TC_Move(m_Curve).SetReverse(rev ^= true))
                    .AddComponent(m_Target.TC_Rotate(0, 0, 0))//.SetReverse(!rev)
                    .AddComponent(m_Target2.TC_LocalMove(0, 20, 0))//.SetReverse(!rev)
                    .AddComponent(LibTC.Action(delegate (float y) {
                    //Debug.LogWarning("y:" + y);
                        m_Y = y;
                    }))
                    .AddComponent(m_Target3.TC_LookAt(m_LookTargets[look_at++].position , Vector3.up))
                    .SetEase(Eases[n])
                    //.SetReverse(rev ^= true)
                    .OnStart(delegate () {
                        Debug.LogWarning("Start at:" + at + "n:" + n);
                        m_CurEase = Eases[n];
                    })
                    .OnComplete(delegate () {
                        Debug.LogWarning("Curve End!!" + ++times + ",Timer:" + m_Seq.Timer + ",Timer:" + m_Seq.Timer + ",interval:" + interval);
                    })
                    );
            }


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
            {
                int n = at >= Eases.Length ? at = 0 : at++;
                m_Seq.Append(LibTween.Tweener(3)
                    .AddComponent(m_Target.TC_Move(m_Curve).SetReverse(rev ^= true))
                    .AddComponent(m_Target.TC_Rotate(90, 130, -70))//.SetReverse(!rev)
                    .AddComponent(m_Target2.TC_LocalMove(0, -20, 0))//.SetReverse(!rev)
                    .AddComponent(m_Target3.TC_LookAt(m_Target, Vector3.up))
                    .AddComponent(LibTC.Action(delegate (float y) {
                        m_Y = y;
                    }))
                    .AddComponent(m_Target3.TC_LookAt(m_LookTargets[look_at++] , Vector3.up))
                    .SetEase(Eases[n])
                    .OnStart(delegate () {
                        Debug.LogWarning("Start at:" + at + "n:" + n);
                        m_CurEase = Eases[n];
                    })
                    //.SetReverse(rev ^= true)
                    .OnComplete(delegate () {
                        Debug.LogWarning("Curve End!!" + ++times + ",Timer:" + m_Seq.Timer);
                    }));
            }

            m_Seq.AppendInterval(1.0f).OnComplete(delegate () {
                Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer);
            });
            {
                int n = at >= Eases.Length ? at = 0 : at++;
                m_Seq.Append(m_Target.UCL_Move(m_Curve, 2)
                    .AddComponent(
                        m_Target.TC_Rotate(-60, -80, 40).SetReverse(!rev)
                    )
                    .AddComponent(LibTC.Action(delegate (float y) {
                        m_Y = y;
                    }))
                    .AddComponent(m_Target2.TC_LocalMove(Vector3.zero))
                    .AddComponent(m_Target3.TC_LookAt(m_LookTargets[look_at++] , Vector3.up).SetReverse(!rev))
                    .AddComponent(m_Target3.TC_LookAt(m_Target, Vector3.up))
                    .SetEase(Eases[n])
                    .SetReverse(rev ^= true)
                    .OnStart(delegate () {
                        Debug.LogWarning("Start at:" + at + "n:"+n);
                        m_CurEase = Eases[n];
                    })
                    .OnComplete(delegate () {
                        Debug.LogWarning("Curve End!!" + ++times + ",Timer:" + m_Seq.Timer);
                    }));
            }

            {
                int n = at >= Eases.Length ? at = 0 : at++;
                m_Seq.Append(m_Target.UCL_Move(m_Curve, 2)
                    .AddComponent(
                        m_Target.TC_Rotate(m_RotTarget.rotation)
                    )
                    //.AddComponent(m_Target2.TC_LocalMove(Vector3.zero))
                    .AddComponent(LibTC.Action(delegate (float y) {
                    //Debug.LogWarning("y:" + y);
                    m_Y = y;
                    }))
                    .AddComponent(m_Target3.TC_LookAt(m_Target, Vector3.up).SetReverse(!rev))
                    .SetEase(Eases[n])
                    .SetReverse(rev ^= true)
                    .OnStart(delegate () {
                        m_CurEase = Eases[n];
                    })
                    .OnComplete(delegate () {
                        Debug.LogWarning("Curve End!!" + ++times + ",Timer:" + m_Seq.Timer);
                    }));
            }

            m_Seq.AppendInterval(2.0f).OnComplete(delegate () {
                Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer);
            });
            m_Seq.Append(m_Target.UCL_Rotate(45, 90, 150, 2.5f)
                .SetEase(EaseType.OutBounce).
                AddComponent(
                    m_Target.TC_Move(1000, 500, 250).SetReverse(!rev)
                )
                .AddComponent(LibTC.Action(delegate (float y) {
                    //Debug.LogWarning("y:" + y);
                    m_Y = y;
                }))
                );
            m_Seq.Append(m_Target.UCL_Move(m_RotTarget.position, 4)
                .AddComponent(
                    m_Target.TC_Rotate(-30, 190, 70).SetReverse(!rev)
                )
                .AddComponent(LibTC.Action(delegate (float y) {
                    //Debug.LogWarning("y:" + y);
                    m_Y = y;
                }))
                .AddComponent(m_Target3.TC_LookAt(m_Target, Vector3.up).SetReverse(!rev))
                .SetEase(EaseType.InCirc).OnComplete(() => {
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


        [Core.ATTR.UCL_FunctionButton("Kill(complete = true)",true)]
        [Core.ATTR.UCL_FunctionButton("Kill(complete = false)", false)]
        public void Kill(bool complete) {
            if(m_Seq == null) return;
            m_Seq.Kill(complete);
            m_Seq = null;
        }
        [Core.ATTR.UCL_FunctionButton("Time Alter 0.5 sec", 0.5f)]
        [Core.ATTR.UCL_FunctionButton("Time Alter 1 sec", 1f)]
        [Core.ATTR.UCL_FunctionButton("Time Alter 2 sec", 2f)]
        public void TimeAlter(float val) {
            if(m_Seq == null) return;
            m_Seq.TimeAlter(val);
        }


        UCL_Sequence m_Seq = null;
        private void Start() {
            StartDemo();

        }
        private void Update() {
            if(m_AutoLoop && m_Seq == null) {
                StartDemo();
            }
        }
    }
}

