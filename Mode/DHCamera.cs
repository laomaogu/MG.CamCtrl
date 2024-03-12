using GxIAPINET;
using MG.CamCtrl.Cameralibs.DHCmera;
using MG.CamCtrl.Common.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace MG.CamCtrl.Mode
{
    internal class DHCamera : BaseCamera
    {
        public DHCamera() : base() {  }


        #region  param

        bool m_bIsOpen = false;                                     ///< 相机打开标识 
        bool m_bIsSnap = false;                                     ///< 相机开始采集标识
        bool m_bColorFilter = false;                                ///< 标识是否支持Bayer格式
        bool m_bAwbLampHouse = false;                               ///< 标示是否支持光源选择
        bool m_bWhiteAutoSelectedIndex = true;                      ///<白平衡列表框转换标志
        IGXFactory m_objIGXFactory = null;                          ///<Factory对像
        IGXDevice m_objIGXDevice = null;                            ///<设备对像
        IGXStream m_objIGXStream = null;                            ///<流对像
        IGXFeatureControl m_objIGXFeatureControl = null;            ///<远端设备属性控制器对像
        IGXFeatureControl m_objIGXStreamFeatureControl = null;      ///<流层属性控制器对象
        IImageProcessConfig m_objCfg = null;                        ///<图像配置参数对象
        GxBitmap m_objGxBitmap = null;                              ///<图像显示类对象
        string m_strPixelColorFilter = null;                        ///<Bayer格式
        string m_strBalanceWhiteAutoValue = "Off";                  ///<自动白平衡当前的值
        bool m_bEnableColorCorrect = false;                         ///<颜色校正使能标志位
        bool m_bEnableGamma = false;                                ///<Gamma使能标志位
        bool m_bEnableSharpness = false;                            ///<锐化使能标志位 
        bool m_bEnableAutoWhite = false;                            ///<自动白平衡使能标志位
        bool m_bEnableAwbLight = false;                             ///<自动白平衡光源使能标志位
        bool m_bEnableDenoise = false;                              ///<图像降噪使能标志位
        bool m_bEnableSaturation = false;                           ///<饱和度使能标志位
        bool m_bEnumDevices = false;                                ///<是否枚举到设备标志位
        List<IGXDeviceInfo> m_listGXDeviceInfo;                     ///<存放枚举到的设备的容器
        public IGXDeviceInfo GXDeviceInfo;
        private List<IGXDeviceInfo> listCameraInfo = new List<IGXDeviceInfo>();
        #endregion 


        #region Operate
        public override List<string> GetListEnum()
        {
            //读取相机列表
            m_objIGXFactory = IGXFactory.GetInstance();
            m_objIGXFactory.Init();
            listCameraInfo.Clear();
            m_objIGXFactory.UpdateDeviceList(200, listCameraInfo);
            if (listCameraInfo.Count < 1) return new List<string>();
            List<string> deviceenum = new List<string>();
            foreach (var item in listCameraInfo)
            {
                deviceenum.Add(item.GetSN());
            }
            return deviceenum;
        }

        public override bool InitDevice(string CamSN)
        {
            GetListEnum();
            if (listCameraInfo.Count < 1 || string.IsNullOrEmpty(CamSN)) return false;

            foreach (var item in listCameraInfo)
            {
                if (item.GetSN().Equals(CamSN))
                {
                    GXDeviceInfo = item;
                    break;
                }
            }

            if (GXDeviceInfo == null) return false;

            _StartInit();
            SN = CamSN;
            return true;
        }

        public override void CloseDevice()
        {
            // 停止采集关闭设备、关闭流
            __CloseAll();
            base.Dispose();
        }

        public override bool SoftTrigger()
        {
            try
            {
                //每次发送触发命令之前清空采集输出队列
                //防止库内部缓存帧，造成本次GXGetImage得到的图像是上次发送触发得到的图
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.FlushQueue();
                }

                //发送软触发命令
                if (null != m_objIGXFeatureControl)
                {
                    m_objIGXFeatureControl.GetCommandFeature("TriggerSoftware").Execute();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion


        #region SettingConfig
        public override bool SetExpouseTime(ushort value)
        {
            try
            {
                if (m_objIGXFeatureControl == null) return false;

                m_objIGXFeatureControl.GetFloatFeature("ExposureTime").SetValue(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool GetExpouseTime(out ushort value)
        {
            value = 0;
            try
            {
                if (m_objIGXFeatureControl == null) return false;
                value = (ushort)(null != m_objIGXFeatureControl ? m_objIGXFeatureControl.GetFloatFeature("ExposureTime").GetValue() : 0);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool SetTriggerMode(TriggerMode mode, TriggerSource triggerEnum = TriggerSource.Line0)
        {
            try
            {
                if (m_objIGXFeatureControl == null) return false;

                switch (mode)
                {

                    case TriggerMode.Off:
                        //m_objIGXFeatureControl.GetEnumFeature("TriggerSelector").SetValue("FrameStart");
                        m_objIGXFeatureControl?.GetEnumFeature("TriggerMode").SetValue("Off");
                        break;
                    case TriggerMode.On:
                        //m_objIGXFeatureControl.GetEnumFeature("TriggerSelector").SetValue("FrameStart");
                        m_objIGXFeatureControl?.GetEnumFeature("TriggerMode").SetValue("On");
                        m_objIGXFeatureControl?.GetEnumFeature("TriggerSource").SetValue(triggerEnum.ToString());
                        break;
                    default:
                        //m_objIGXFeatureControl.GetEnumFeature("TriggerSelector").SetValue("FrameStart");
                        m_objIGXFeatureControl?.GetEnumFeature("TriggerMode").SetValue("On");
                        m_objIGXFeatureControl?.GetEnumFeature("TriggerSource").SetValue(TriggerSource.Line0.ToString());
                        break;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool GetTriggerMode(out TriggerMode mode, out TriggerSource hardTriggerModel)
        {
            mode = TriggerMode.On;
            hardTriggerModel = TriggerSource.Line0;
            try
            {
                if (m_objIGXFeatureControl == null) return false;
                m_objIGXFeatureControl.GetEnumFeature("TriggerSelector").SetValue("FrameStart");
                string modelstr = m_objIGXFeatureControl.GetEnumFeature("TriggerMode").GetValue();
                string hadmodestr = m_objIGXFeatureControl.GetEnumFeature("TriggerSource").GetValue();

                switch (modelstr)
                {
                    case "On":
                        mode = TriggerMode.On;
                        break;
                    case "Off":
                        mode = TriggerMode.Off;
                        break;
                }

                switch (hadmodestr)
                {
                    case "Software":
                        hardTriggerModel = TriggerSource.Software;
                        break;
                    case "Line0":
                        hardTriggerModel = TriggerSource.Line0;
                        break;
                    case "Line1":
                        hardTriggerModel = TriggerSource.Line1;
                        break;
                    case "Line2":
                        hardTriggerModel = TriggerSource.Line2;
                        break;
                    case "Line3":
                        hardTriggerModel = TriggerSource.Line3;
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool SetTriggerPolarity(TriggerPolarity polarity)
        {
            try
            {
                if (null == m_objIGXFeatureControl) return false;
                m_objIGXFeatureControl.GetEnumFeature("TriggerSelector").SetValue("FrameStart");
                m_objIGXFeatureControl.GetEnumFeature("TriggerActivation").SetValue(polarity.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool GetTriggerPolarity(out TriggerPolarity polarity)
        {
            polarity = TriggerPolarity.RisingEdge;
            try
            {
                if (m_objIGXFeatureControl == null) return false;
                string polaritystr = m_objIGXFeatureControl.GetEnumFeature("TriggerActivation").GetValue();
                polarity = (TriggerPolarity)Enum.Parse(typeof(TriggerPolarity), polaritystr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool SetTriggerFliter(ushort flitertime)
        {
            try
            {
                if (m_objIGXFeatureControl == null) return false;
                m_objIGXFeatureControl.GetEnumFeature("RegionSelector").SetValue("Region0");
                m_objIGXFeatureControl.GetFloatFeature("TriggerFilterFallingEdge").SetValue(flitertime);//TriggerFilterRaisingEdge
                m_objIGXFeatureControl.GetFloatFeature("TriggerFilterRaisingEdge").SetValue(flitertime);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool GetTriggerFliter(out ushort flitertime)
        {
            flitertime = 0;
            try
            {
                if (m_objIGXFeatureControl == null) return false;
                m_objIGXFeatureControl.GetEnumFeature("RegionSelector").SetValue("Region0");
                flitertime = (ushort)m_objIGXFeatureControl.GetFloatFeature("TriggerFilterFallingEdge").GetValue();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public override bool SetTriggerDelay(ushort delay)
        {
            try
            {
                if (m_objIGXFeatureControl == null) return false;

                m_objIGXFeatureControl.GetEnumFeature("TriggerSelector").SetValue("FrameStart");
                m_objIGXFeatureControl.GetFloatFeature("TriggerDelay").SetValue(delay);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool GetTriggerDelay(out ushort delay)
        {
            delay = 0;
            try
            {
                if (m_objIGXFeatureControl == null) return false;
                m_objIGXFeatureControl.GetEnumFeature("TriggerSelector").SetValue("FrameStart");
                delay = (ushort)m_objIGXFeatureControl.GetFloatFeature("TriggerDelay").GetValue();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool SetGain(short gain)
        {
            try
            {
                if (m_objIGXFeatureControl == null) return false;

                m_objIGXFeatureControl.GetEnumFeature("GainSelector").SetValue("AnalogAll");
                m_objIGXFeatureControl.GetFloatFeature("Gain").SetValue(gain);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool GetGain(out short gain)
        {
            gain = 0;
            try
            {
                if (m_objIGXFeatureControl == null) return false;

                gain = (short)m_objIGXFeatureControl.GetFloatFeature("Gain").GetValue();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public override bool SetLineMode(IOLines line, LineMode mode)
        {
            throw new NotImplementedException();

        }

        public override bool SetLineStatus(IOLines line, LineStatus linestatus)
        {
            throw new NotImplementedException();
        }

        public override bool GetLineStatus(IOLines line, out LineStatus linestatus)
        {
            throw new NotImplementedException();
        }


        public override bool AutoBalanceWhite()
        {
            try
            {
                if (m_objIGXFeatureControl == null) return false;

                m_objIGXFeatureControl.GetEnumFeature("BalanceWhiteAuto").SetValue("Once");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion


        #region private  

        protected override bool StartGrabbing()
        {
            try
            {
                if (null != m_objIGXStreamFeatureControl)
                {
                    //设置流层Buffer处理模式为OldestFirst
                    m_objIGXStreamFeatureControl.GetEnumFeature("StreamBufferHandlingMode").SetValue("OldestFirst");
                }


                if (null != m_objIGXStream)
                {
                    //RegisterCaptureCallback第一个参数属于用户自定参数(类型必须为引用
                    //类型)，若用户想用这个参数可以在委托函数中进行使用
                    //m_objIGXStream.RegisterCaptureCallback(null, OnFrameCallbackFun);

                    //注册回调

                    m_objIGXStream.RegisterCaptureCallback(this, OnFrameCallbackFun);//  Delegate_Camera += new Action<Bitmap>(DelegateCallBack);
                    //开始采集之前可设置buff个数
                    //开启采集流通道
                    m_objIGXStream.StartGrab();
                }

                //发送开采命令 
                if (null != m_objIGXFeatureControl)
                {
                    m_objIGXFeatureControl.GetCommandFeature("AcquisitionStart").Execute();
                }
                m_bIsSnap = true;
                //m_bIsTrigValid = true;

                // 更新界面UI
                // __UpdateUI();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected override bool StopGrabbing()
        {
            try
            {
                //发送停采命令 ----------------------
                if (null != m_objIGXFeatureControl)
                {
                    m_objIGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                    m_objIGXFeatureControl = null;
                }

                //关闭采集流通道
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.StopGrab();
                    //注销采集回调函数
                    m_objIGXStream.UnregisterCaptureCallback();

                    m_objIGXStream.Close();
                    //m_objIGXStream = null;
                    //m_objIGXStreamFeatureControl = null;
                }

                m_bIsSnap = false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void _StartInit()
        {
            try
            {
                //关闭流
                __CloseStream();
                // 如果设备已经打开则关闭，保证相机在初始化出错情况下能再次打开
                __CloseDevice();


                //打开列表第一个设备 
                m_objIGXDevice = m_objIGXFactory.OpenDeviceBySN(GXDeviceInfo.GetSN(), GX_ACCESS_MODE.GX_ACCESS_EXCLUSIVE);
                m_objIGXFeatureControl = m_objIGXDevice.GetRemoteFeatureControl();


                //打开流
                if (null != m_objIGXDevice)
                {
                    m_objIGXStream = m_objIGXDevice.OpenStream(0);
                    m_objIGXStreamFeatureControl = m_objIGXStream.GetFeatureControl();
                }

                // 建议用户在打开网络相机之后，根据当前网络环境设置相机的流通道包长值，
                // 以提高网络相机的采集性能,设置方法参考以下代码。
                GX_DEVICE_CLASS_LIST objDeviceClass = m_objIGXDevice.GetDeviceInfo().GetDeviceClass();
                if (GX_DEVICE_CLASS_LIST.GX_DEVICE_CLASS_GEV == objDeviceClass)
                {
                    // 判断设备是否支持流通道数据包功能
                    if (true == m_objIGXFeatureControl.IsImplemented("GevSCPSPacketSize"))
                    {
                        // 获取当前网络环境的最优包长值
                        uint nPacketSize = m_objIGXStream.GetOptimalPacketSize();
                        // 将最优包长值设置为当前设备的流通道包长值
                        m_objIGXFeatureControl.GetIntFeature("GevSCPSPacketSize").SetValue(nPacketSize);
                    }
                }

                if (null != m_objIGXFeatureControl)
                {
                    //设置采集模式连续采集
                    m_objIGXFeatureControl.GetEnumFeature("AcquisitionMode").SetValue("Continuous"); 
                    if (GXDeviceInfo.GetDeviceClass() == GX_DEVICE_CLASS_LIST.GX_DEVICE_CLASS_GEV)
                    {

                        //设置心跳超时时间为1s
                        //针对千兆网相机，程序在Debug模式下调试运行时，相机的心跳超时时间自动设置为5min，
                        //这样做是为了不让相机的心跳超时影响程序的调试和单步执行，同时这也意味着相机在这5min内无法断开，除非使相机断电再上电
                        //为了解决掉线重连问题，将相机的心跳超时时间设置为1s，方便程序掉线后可以重新连接
                        m_objIGXFeatureControl.GetIntFeature("GevHeartbeatTimeout").SetValue(1000); 
                    }
                }


                m_objCfg = m_objIGXDevice.CreateImageProcessConfig();



                m_objGxBitmap = new GxBitmap(m_objIGXDevice);
                //Utilbitmap = new Util_Bitmap(m_objIGXDevice);

                // 更新设备打开标识
                m_bIsOpen = true;

            }
            catch (Exception e)
            { MessageBox.Show(e.ToString()); }

        }

        /// <summary>
        ///  采集事件的委托函数
        /// </summary>
        /// <param name="objUserParam">用户私有参数</param>
        /// <param name="objIFrameData">图像信息对象</param>
        private void OnFrameCallbackFun(object objUserParam, IFrameData objIFrameData)
        {

            //用户私有参数 obj，用户在注册回调函数的时候传入了设备对象，在回调函数内部可以将此
            //参数还原为用户私有参数
            //IGXDevice objIGXDevice = objUserParam as IGXDevice;
            //if (objIFrameData.GetStatus() == GX_FRAME_STATUS_LIST.GX_FRAME_STATUS_SUCCESS)  //完整帧 
            //{
            // m_objGxBitmap = new GxBitmap(m_objIGXDevice);
            //Bitmap bmp = m_objGxBitmap.GetBmp(objIFrameData);
            GxBitmap bitmap = new GxBitmap(m_objIGXDevice);
            Bitmap bmp = bitmap.GetBmp(objIFrameData);
            //数据更新 
            ActionGetImage?.Invoke(bmp);
            //GC.Collect();
            //}
        }

        /// <summary>
        /// 关闭流
        /// </summary>
        private void __CloseStream()
        {
            try
            {
                //关闭流
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.Close();
                    m_objIGXStream = null;
                    m_objIGXStreamFeatureControl = null;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        private void __CloseDevice()
        {
            try
            {
                //关闭设备
                if (null != m_objIGXDevice)
                {
                    m_objIGXDevice.Close();
                    m_objIGXDevice = null;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 停止采集关闭设备、关闭流
        /// </summary>
        private void __CloseAll()
        {
            try
            {
                // 如果未停采则先停止采集
                if (m_bIsSnap)
                {
                    if (null != m_objIGXFeatureControl)
                    {
                        m_objIGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                        m_objIGXFeatureControl = null;
                    }
                }
            }
            catch (Exception)
            {
            }
            m_bIsSnap = false;
            try
            {
                //停止流通道和关闭流
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.StopGrab();
                    //注销采集回调函数
                    m_objIGXStream.UnregisterCaptureCallback();
                    m_objIGXStream?.Close();
                    m_objIGXStream = null;
                    m_objIGXStreamFeatureControl = null;
                }
            }
            catch (Exception ee)
            {
                Trace.WriteLine("#######  " + ee);
            }


            //关闭设备
            __CloseDevice();
            m_bIsOpen = false;
        }
        #endregion private
    }

}


