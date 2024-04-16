using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MG.CamCtrl.Cameralibs.DHCmera
{
    public class CStatistics
    {
        double m_dSumTime = 0.0;      ///< 总时间
        double m_dMax     = 0.0;      ///< 最大值
        double m_dMin     = 0.0;      ///< 最小值
        Int64  m_nNum     = 0;        ///< 统计次数计数

        /// 构造函数
        public CStatistics()
        {
            m_dSumTime = 0.0;
            m_dMax = 0.0;
            m_dMin = 0.0;
            m_nNum = 0;
        }

        /// <summary>
        /// 增加统计计数并计算相关数据
        /// </summary>
        /// <param name="dData"></param>
        public void AddStatisticalData(double dData)
        {
            __IncSumTimeData(dData);
            __UpdateData(dData);
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns></returns>
        public double GetMax()
        {
            return m_dMax;
        }

        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <returns></returns>
        public double GetMin()
        {
            return m_dMin;
        }

        /// <summary>
        /// 获取平均值
        /// </summary>
        /// <returns></returns>
        public double GetAverage()
        {
            if (m_nNum == 0)
            {
                return 0;
            }

            return m_dSumTime / m_nNum;
        }

        /// <summary>
        /// 数据重置函数
        /// </summary>
        public void Reset()
        {
            m_dSumTime = 0.0;
            m_dMax = 0.0;
            m_dMin = 0.0;
            m_nNum = 0;
        }

        /// <summary>
        /// 增加统计总时间计数
        /// </summary>
        /// <param name="dData"></param>
        private void __IncSumTimeData(double dData)
        {
            m_dSumTime = m_dSumTime + dData;
            m_nNum++;
        }

        /// <summary>
        /// 数据比较更新最大值和最小值
        /// </summary>
        /// <param name="dData"></param>
        private void __UpdateData(double dData)
        {
            if (m_nNum == 1)
            {
                // 统计次数为1时
                m_dMax = dData;
                m_dMin = dData;
                return;
            }

            if (dData > m_dMax)
            {
                m_dMax = dData;
            }

            if (dData < m_dMin)
            {
                m_dMin = dData;
            }
        }
    }
}


