using System.ComponentModel;

namespace MG.CamCtrl
{
    public enum CameraBrand
    {
        /// <summary>
        /// 海康相机
        /// </summary>
        [Description("海康相机")]
        HIK,
        /// <summary>
        /// 大恒相机
        /// </summary>
        [Description("大恒相机")]
        DaHeng,
        /// <summary>
        /// 大华相机 未添加
        /// </summary>
        [Description("大华相机")]
        DaHua,
        /// <summary>
        /// 索尼相机
        /// </summary>
        [Description("索尼相机")]
        SONY,
        /// <summary>
        /// 巴斯勒相机 未添加
        /// </summary>
        [Description("巴斯勒相机")]
        Basler
    }

    /// <summary>
    /// 触发源
    /// </summary>
    public enum TriggerSource
    {
        /// <summary>
        /// 软触发
        /// </summary>
        [Description("软触发")]
        Software,
        /// <summary>
        /// 线路0
        /// </summary>
        [Description("线路0")]
        Line0,
        /// <summary>
        /// 线路1
        /// </summary>
        [Description("线路1")]
        Line1,
        /// <summary>
        /// 线路2
        /// </summary>
        [Description("线路2")]
        Line2,
        /// <summary>
        /// 线路3
        /// </summary>
        [Description("线路3")]
        Line3,

    }

    public enum IOLines
    {
        Line0,
        Line1,
        Line2,
        Line3,
    }

    public enum LineMode
    {
        Input,
        Output
    }

    public enum LineStatus
    {
        Hight,
        Low
    }

    public enum TriggerMode
    {
        Off,
        On
    }

    public enum TriggerPolarity
    {
        RisingEdge,
        FallingEdge
    }
}
