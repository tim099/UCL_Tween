using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCL.TweenLib {
    public interface UCL_ITimer {
        float GetTime();
        double GetTimeDouble();

        /// <summary>
        /// Get time in milisecond
        /// </summary>
        /// <returns>time in milisecond</returns>
        long GetTimeMs();

        void SetTime(float time);
        void SetTime(double time);
        /// <summary>
        /// Set time in milisecond
        /// </summary>
        /// <param name="time">time in milisecond</param>
        void SetTimeMs(long time);


        void AlterTime(float time);
        void AlterTime(double time);
        /// <summary>
        /// Alter time in milisecond
        /// </summary>
        /// <param name="time">time in milisecond</param>
        void AlterTimeMs(long time);
    }
    public class UCL_Timer : UCL_ITimer {
        /*
        public static implicit operator float(UCL_Timer timer) {
            return timer.GetTime();
        }
        public static implicit operator double(UCL_Timer timer) {
            return timer.GetTimeDouble();
        }
        public static implicit operator long(UCL_Timer timer) {
            return timer.GetTimeMs();
        }
        */
        public static long ConvertToMs(float time) {
            long l = Mathf.FloorToInt(time);
            return l * 1000 + Mathf.RoundToInt((time - l) * 1000f);
        }
        public static float ConvertToSec(long time) {
            return 0.001f * time;
        }
        public static bool operator >(UCL_Timer a, UCL_Timer b) {
            if(a is UCL_TimerMs) {
                return a.GetTimeMs() > b.GetTimeMs();
            }
            return a.GetTime() > b.GetTime();
        }
        public static bool operator >=(UCL_Timer a, UCL_Timer b) {
            if(a is UCL_TimerMs) {
                return a.GetTimeMs() >= b.GetTimeMs();
            }
            return a.GetTime() >= b.GetTime();
            //return !(a < b);
        }
        public static bool operator <=(UCL_Timer a, UCL_Timer b) {
            if(a is UCL_TimerMs) {
                return a.GetTimeMs() <= b.GetTimeMs();
            }
            return a.GetTime() <= b.GetTime();
        }
        public static bool operator <(UCL_Timer a, UCL_Timer b) {
            if(a is UCL_TimerMs) {
                return a.GetTimeMs() < b.GetTimeMs();
            }
            return a.GetTime() < b.GetTime();
        }
        virtual public float GetTime() { return 0; }
        virtual public double GetTimeDouble() { return GetTime(); }

        /// <summary>
        /// Get time in milisecond
        /// </summary>
        /// <returns>time in milisecond</returns>
        virtual public long GetTimeMs() { return ConvertToMs(GetTime()); }

        virtual public void SetTime(float time) { }
        virtual public void SetTime(double time) { SetTime((float)time); }
        /// <summary>
        /// Set time in milisecond
        /// </summary>
        /// <param name="time">time in milisecond</param>
        virtual public void SetTimeMs(long time) { SetTime((time * 0.001f)); }
        virtual public void SetTime(UCL_Timer timer) { }

        virtual public void AlterTime(float time) { }
        virtual public void AlterTime(double time) { AlterTime((float)time); }
        /// <summary>
        /// Alter time in milisecond
        /// </summary>
        /// <param name="time">time in milisecond</param>
        virtual public void AlterTimeMs(long time) { AlterTime((time * 0.001f)); }
    }
}