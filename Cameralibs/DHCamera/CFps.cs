using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MG.CamCtrl.Cameralibs.DHCmera
{
   public class CFps
    {
        UInt64                m_nFrameCount            = 0;                           ///< 从上次计算完毕开始累积的帧数
        double                m_dBeginTime             = 0.0;                         ///< 第一帧之前的一帧的时间（初始为0）
        double                m_dEndTime               = 0.0;                         ///< 最后一帧的时间
        double                m_dFps                   = 0.0;                         ///< 通过帧数与时间间隔之比得出的帧率(帧/秒)
        double                m_dCurrentFps            = 0.0;                         ///< 当前的帧率，可能是预测得到的（帧/秒）
        UInt64                m_nTotalFrameCount       = 0;                           ///< 累积的帧数
        CStopWatch            m_objTime                = new CStopWatch();            ///< 计时器
        object                m_objLock                = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public CFps()
        {
            //重置所有参数
            Reset();
        }


        /// <summary>
        /// 获取最近一次的帧率
        /// </summary>
        /// <returns>当前帧图像</returns>
        public double GetFps()
        {
            lock (m_objLock)
            {
                //返回当前的帧率
                return m_dCurrentFps;
            }
        }

        /// <summary>
        /// 获取累积的总帧数
        /// </summary>
        /// <returns>当前帧图像</returns>
        public UInt64 GetTotalFrameCount()
        {
            lock (m_objLock)
            {
                return m_nTotalFrameCount;
            }
        }

        /// <summary>
        /// 增加帧数
        /// </summary>
        public void IncreaseFrameNum()
        {
            lock (m_objLock)
            {
                //累积帧数
                m_nTotalFrameCount++;

                //增加帧数
                m_nFrameCount++;

                //更新时间间隔
                m_dEndTime = m_objTime.ElapsedTime();
            }
        }

        /// <summary>
        /// 更新帧率
        /// 如果该函数被调用的频率超过了帧频率，则帧率会降为零
        /// </summary>
        public void UpdateFps()
        {
            lock (m_objLock)
            {
                //计算时间间隔
                double dInterval = m_dEndTime - m_dBeginTime;

                //时间间隔大于零（有新帧）
                if (dInterval > 0)
                {
                    m_dFps = 1000.0 * m_nFrameCount / dInterval;
                    m_nFrameCount = 0;              //累积帧数清零
                    m_dBeginTime = m_dEndTime;      //更新起始时间

                    m_dCurrentFps = m_dFps;
                }
                else if (dInterval == 0) //时间间隔等于零（无新帧）
                {
                    //如果上次的帧率非零，则调整帧率
                    if (m_dCurrentFps != 0)
                    {
                        //从上一帧到现在的经历的时间（毫秒）
                        double dCurrentInterval = m_objTime.ElapsedTime() - m_dBeginTime;

                        //根据当前帧率计算更新帧率的时间阈值
                        double dPeriod = 1000.0 / m_dCurrentFps;   //上次的帧周期(毫秒)
                        const double RATIO = 1.5;                      //超过帧周期的多少倍，帧率才更新
                        double dThresh = RATIO * dPeriod;          //多长时间没有来帧，帧率就更新

                        //如果超过2秒没有来帧，则帧率降为零。
                        const double ZERO_FPS_INTERVAL = 2000;
                        if (dCurrentInterval > ZERO_FPS_INTERVAL)
                        {
                            m_dCurrentFps = 0;
                        }
                        //如果在2秒之内已经超过1.5倍的帧周期没有来帧，则降低帧率
                        else if (dCurrentInterval > dThresh)
                        {
                            m_dCurrentFps = m_dFps / (dCurrentInterval / (1000.0 / m_dFps));
                        }
                        else { }
                    }
                    else { }
                }
                else { }
            }

        }

        /// <summary>
        /// 将计时器恢复为初始状态
        /// </summary>
        public void Reset()
        {
            m_nFrameCount = 0;
            m_dBeginTime = 0.0;
            m_dEndTime = 0.0;
            m_nTotalFrameCount = 0;
            m_dFps = 0.0;
            m_dCurrentFps = 0.0;
            m_objTime.Start();          //重启计时器
        }
    }
}


