using System;
using System.Diagnostics;

namespace MG.CamCtrl.Cameralibs.DHCmera
{

    public class CStopWatch
    {
        double m_dStartTime = 0.0;    ///< 开始时间
        double m_dStopTime = 0.0;     ///< 停止时间 


        public CStopWatch()
        {
            Start();
        }

        /// <summary>
        /// 开始计数
        /// </summary>
        public void Start()
        {
            m_dStartTime = Stopwatch.GetTimestamp();
        }

        /// <summary>
        /// 停止计数
        /// </summary>
        /// <returns>时间差单位ms</returns>
        public double Stop()
        {
            m_dStopTime = Stopwatch.GetTimestamp();
            double theElapsedTime = ElapsedTime();

            m_dStartTime = m_dStopTime;
            return theElapsedTime;
        }


        /// <summary>
        /// 获取时间差
        /// </summary>
        /// <returns>时间差单位ms</returns>
        public double ElapsedTime()
        {
            m_dStopTime = Stopwatch.GetTimestamp();
            double dTimeElapsed = (m_dStopTime - m_dStartTime) * 1000.0;
          
            return dTimeElapsed / Stopwatch.Frequency;
        }
    }
}


