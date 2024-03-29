﻿using System.Collections;
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
        public Transform m_TargetChild;
        public Transform m_Target3;

        //public Transform m_TargetChild;
        public AnimationCurve m_AnimCurve;
        public Transform[] m_LookTargets;
        public Transform m_RotTarget;
        public float m_Y = 0;
        public bool m_AutoLoop = true;
        public Core.MathLib.UCL_Curve m_Curve;
#pragma warning disable 0414 
        UCL_TweenerCurve m_Cur = null;
#pragma warning restore 0414

#if UNITY_EDITOR
        Core.TextureLib.UCL_Texture2D m_Texture;
        [Core.ATTR.UCL_DrawTexture2D]//(128, 128, TextureFormat.ARGB32, typeof(UCL_EaseTexture))
        Core.TextureLib.UCL_Texture2D DrawEaseCurve() {
            if(m_Texture == null) {
                m_Texture = new UCL_EaseTexture(128, 128, TextureFormat.ARGB32);
            }
            if(UCL.Core.EditorLib.EditorApplicationMapper.isPlaying && m_Seq != null) {
                UCL_EaseTexture.DrawEase(m_CurEase, m_Texture);
            } else {
                UCL_EaseTexture.DrawEase(m_Ease, m_Texture);
            }

            if(m_Cur != null) {
                Vector2 pos = m_Cur.GetPos();
                var tex = m_Texture as UCL_EaseTexture;
                if(tex != null) {
                    pos.y -= tex.m_Min;
                    pos.y /= tex.m_Range;
                    //pos.y *= 0.99f;
                }
                m_Texture.DrawDot(pos.x, pos.y, Color.red, 2);
            }
            return m_Texture;
        }
#endif
        [Core.ATTR.UCL_FunctionButton]
        public void StartDemo() {
            Debug.LogWarning("StartDemo()");
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
            float interval = 0.101f;
            m_Seq.AppendInterval(interval).OnComplete(delegate () {
                //Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer + ",interval:" + interval);
            });
            
            interval = 0.2025f;
            m_Seq.AppendInterval(interval).OnComplete(delegate () {
                //Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer + ",interval:" + interval);
            });
            m_Seq.Join(m_TargetChild.UCL_LocalShake(1f, 30f, 30, true));

            bool rev = true;
            int look_at = 0;
            {
                int n = at >= Eases.Length ? at = 0 : at++;
                m_Seq.Append(
                    LibTween.Tweener(4f)
                    .AddComponent(m_Target.TC_Move(m_Curve).SetReverse(rev ^= true))
                    .AddComponent(m_Target.TC_Rotate(0, 0, 0))//.SetReverse(!rev)
                    .AddComponent(m_TargetChild.TC_LocalMove(0, 20, 0))//.SetReverse(!rev)
                    .AddComponent(LibTC.Action(delegate (float y) {
                    //Debug.LogWarning("y:" + y);
                        m_Y = y;
                    }))
                    .AddComponent(m_Target3.TC_LookAt(m_LookTargets[look_at++].position , Vector3.up))
                    .SetEase(m_AnimCurve)
                    //.SetEase(Eases[n])
                    //.SetReverse(rev ^= true)
                    .OnStart(delegate () {
                        //Debug.LogWarning("Start at:" + at + "n:" + n);
                        m_CurEase = Eases[n];
                    })
                    .OnComplete(delegate () {
                        //Debug.LogWarning("Curve End!!" + ++times + ",Timer:" + m_Seq.Timer + ",Timer:" + m_Seq.Timer + ",interval:" + interval);
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
                m_Seq.Append(LibTween.Tweener(3f)
                    .AddComponent(m_Target.TC_Move(m_Curve).SetReverse(rev ^= true))
                    .AddComponent(m_Target.TC_Rotate(90, 130, -70))//.SetReverse(!rev)
                    //.AddComponent(m_TargetChild.TC_LocalMove(0, -20, 0))//.SetReverse(!rev)
                    .AddComponent(m_Target3.TC_LookAt(m_Target, Vector3.up))
                    .AddComponent(LibTC.Action(delegate (float y) {
                        m_Y = y;
                    }))
                    .AddComponent(m_Target3.TC_LookAt(m_LookTargets[look_at++] , Vector3.up))
                    .SetEase(Eases[n])
                    .OnStart(delegate () {
                        //Debug.LogWarning("Start at:" + at + "n:" + n);
                        m_CurEase = Eases[n];
                    })
                    //.SetReverse(rev ^= true)
                    .OnComplete(delegate () {
                        //Debug.LogWarning("Curve End!!" + ++times + ",Timer:" + m_Seq.Timer);
                    }));
            }

            m_Seq.AppendInterval(1.0f).OnComplete(delegate () {
                Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer);
            });
            {
                int n = at >= Eases.Length ? at = 0 : at++;
                m_Seq.Append(m_Target.UCL_Move(2, m_Curve)
                    .AddComponent(
                        m_Target.TC_Rotate(-60, -80, 40).SetReverse(!rev)
                    )
                    .AddComponent(m_TargetChild.TC_LocalShake(20, 60))
                    .AddUpdateAction(delegate (float y) {
                        m_Y = y;
                    })
                    //.AddComponent(m_TargetChild.TC_LocalMove(Vector3.zero))
                    .AddComponent(m_Target3.TC_LookAt(m_LookTargets[look_at++] , Vector3.up).SetReverse(!rev))
                    .AddComponent(m_Target3.TC_LookAt(m_Target, Vector3.up))
                    .SetEase(delegate(float y) {
                        float x = 4 * y;
                        x -= Mathf.FloorToInt(x);
                        return x;
                    })//Eases[n]
                    .SetReverse(rev ^= true)
                    .OnStart(delegate () {
                        //Debug.LogWarning("Start at:" + at + "n:"+n);
                        m_CurEase = Eases[n];
                    })
                    .OnComplete(delegate () {
                        //Debug.LogWarning("Curve End!!" + ++times + ",Timer:" + m_Seq.Timer);
                    }));
            }

            {
                int n = at >= Eases.Length ? at = 0 : at++;
                m_Seq.Append(m_Target.UCL_Move(2, m_Curve)
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
                        //Debug.LogWarning("Curve End!!" + ++times + ",Timer:" + m_Seq.Timer);
                    }));
            }
            m_Seq.AppendInterval(2.0f).OnComplete(delegate () {
                //Debug.LogWarning("Test " + ++times + ",Timer:" + m_Seq.Timer);
            });
            m_Seq.Append(m_Target.UCL_Rotate(2.5f, 45, 90, 150)
                .SetEase(EaseType.OutBounce).
                AddComponent(
                    m_Target.TC_Move(1000, 500, 250).SetReverse(!rev)
                )
                .AddComponent(LibTC.Action(delegate (float y) {
                    //Debug.LogWarning("y:" + y);
                    m_Y = y;
                }))
                );
            m_Seq.Append(m_Target.UCL_Move(4, m_RotTarget.position)
                .AddComponent(
                    m_Target.TC_Rotate(-30, 190, 70).SetReverse(!rev)
                )
                .AddComponent(LibTC.Action(delegate (float y) {
                    //Debug.LogWarning("y:" + y);
                    m_Y = y;
                }))
                .AddComponent(m_Target3.TC_LookAt(m_Target, Vector3.up).SetReverse(!rev))
                .SetEase(EaseType.InCirc).OnComplete(() => {
                    //Debug.LogWarning("Move End!!" + ++times + ",Timer:" + m_Seq.Timer);
                }));
            m_Seq.Append(m_Target.UCL_Shake(2f, 20f, 30).SetEase(EaseType.OutQuart));
            m_Seq.Append(m_Target.UCL_Rotate(2f, m_RotTarget.rotation).SetEase(EaseType.OutBounce));
            
            //m_RotTarget
            m_Seq.OnComplete(delegate () {
                Debug.LogWarning("End!! " + ",Timer:" + m_Seq.Timer);
                m_Seq = null;
            });

            m_Seq.Start();
        }

        [Core.ATTR.UCL_FunctionButton("Pause", true)]
        [Core.ATTR.UCL_FunctionButton("Resume", false)]
        public void Pause(bool val) {
            if(m_Seq == null) return;
            m_Seq.SetPause(val);
        }

        [Core.ATTR.UCL_FunctionButton("Kill(complete = true)",true)]
        [Core.ATTR.UCL_FunctionButton("Kill(complete = false)", false)]
        public void Kill(bool complete) {
            if(m_Seq == null) return;
            m_Seq.Kill(complete);
            m_Seq = null;
        }
        [Core.ATTR.UCL_FunctionButton("Time Alter -0.5 sec", -0.5f)]
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

