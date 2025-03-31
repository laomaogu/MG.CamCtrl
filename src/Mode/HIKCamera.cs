using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using MG.CamCtrl.Cameralibs.HKCamera;


namespace MG.CamCtrl.Mode
{

    internal class HIKCamera : BaseCamera
    {
        public HIKCamera() : base() { }

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        #region param

        private MVCameraCtrl _myCamera = new MVCameraCtrl();
        private MVCameraCtrl.cbOutputExdelegate _imageCallbackDelegate = null;



        private static Object BufForDriverLock = new Object();
        UInt32 m_nBufSizeForDriver = 0;
        Bitmap m_bitmap = null;
        PixelFormat m_bitmapPixelFormat = PixelFormat.DontCare;
        IntPtr m_BufForDriver = IntPtr.Zero;
        IntPtr m_ConvertDstBuf = IntPtr.Zero;
        UInt32 m_nConvertDstBufLen = 0;
        #endregion


        #region operate

        public override List<string> GetListEnum()
        {
            GC.Collect();
            List<string> listsn = new List<string>();
            var m_stDeviceList = new MVCameraCtrl.MV_CC_DEVICE_INFO_LIST();
            List<MVCameraCtrl.MV_CC_DEVICE_INFO> deviceList = new List<MVCameraCtrl.MV_CC_DEVICE_INFO>();
            m_stDeviceList.nDeviceNum = 0;
            MVCameraCtrl.MV_CC_EnumDevices_NET(MVCameraCtrl.MV_GIGE_DEVICE | MVCameraCtrl.MV_USB_DEVICE, ref m_stDeviceList);
            for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
            {
                MVCameraCtrl.MV_CC_DEVICE_INFO device = (MVCameraCtrl.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i], typeof(MVCameraCtrl.MV_CC_DEVICE_INFO));

                if (device.nTLayerType == MVCameraCtrl.MV_GIGE_DEVICE)
                {
                    MVCameraCtrl.MV_GIGE_DEVICE_INFO gigeInfo = (MVCameraCtrl.MV_GIGE_DEVICE_INFO)MVCameraCtrl.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MVCameraCtrl.MV_GIGE_DEVICE_INFO));
                    listsn.Add(gigeInfo.chSerialNumber);

                }
                else if (device.nTLayerType == MVCameraCtrl.MV_USB_DEVICE)
                {
                    MVCameraCtrl.MV_USB3_DEVICE_INFO usbInfo = (MVCameraCtrl.MV_USB3_DEVICE_INFO)MVCameraCtrl.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MVCameraCtrl.MV_USB3_DEVICE_INFO));
                    listsn.Add(usbInfo.chSerialNumber);
                }
            }

            return listsn;
        }

        public override bool InitDevice(string CamSN)
        {
            if (string.IsNullOrEmpty(CamSN)) return false;
            MVCameraCtrl.MV_CC_DEVICE_INFO camerainfo = new MVCameraCtrl.MV_CC_DEVICE_INFO();
            var infolist = GetListInfoEnum();
            if (infolist.Count < 1) return false;

            bool selectSNflag = false;
            foreach (var item in infolist)
            {
                if (item.nTLayerType == MVCameraCtrl.MV_GIGE_DEVICE)
                {
                    MVCameraCtrl.MV_GIGE_DEVICE_INFO gigeInfo = (MVCameraCtrl.MV_GIGE_DEVICE_INFO)MVCameraCtrl.ByteToStruct(item.SpecialInfo.stGigEInfo, typeof(MVCameraCtrl.MV_GIGE_DEVICE_INFO));
                    if (gigeInfo.chSerialNumber.Equals(CamSN))
                    {
                        camerainfo = item;
                        selectSNflag = true;
                        break;
                    }

                }
                else if (item.nTLayerType == MVCameraCtrl.MV_USB_DEVICE)
                {
                    MVCameraCtrl.MV_USB3_DEVICE_INFO usbInfo = (MVCameraCtrl.MV_USB3_DEVICE_INFO)MVCameraCtrl.ByteToStruct(item.SpecialInfo.stUsb3VInfo, typeof(MVCameraCtrl.MV_USB3_DEVICE_INFO));
                    if (usbInfo.chSerialNumber.Equals(CamSN))
                    {
                        camerainfo = item;
                        selectSNflag = true;
                        break;
                    }
                }
            }

            if (!selectSNflag) return false;

            // ch:打开设备 | en:Open device
            if (null == _myCamera)
            {
                _myCamera = new MVCameraCtrl();
                if (null == _myCamera)
                {
                    Debug.WriteLine("Applying resource fail!", MVCameraCtrl.MV_E_RESOURCE);
                    return false;
                }
            }

            int nRet = _myCamera.MV_CC_CreateDevice_NET(ref camerainfo);
            if (MVCameraCtrl.MV_OK != nRet)
            {
                Debug.WriteLine("Create device fail!", nRet);
                return false;
            }

            nRet = _myCamera.MV_CC_OpenDevice_NET();
            if (MVCameraCtrl.MV_OK != nRet)
            {
                _myCamera.MV_CC_DestroyDevice_NET();
                Debug.WriteLine("Device open fail!", nRet);
                return false;
            }

            // Register image acquisition call back
            _imageCallbackDelegate = ImageCallback;
            nRet = _myCamera.MV_CC_RegisterImageCallBackEx_NET(_imageCallbackDelegate, IntPtr.Zero);
            if (nRet != 0)
            {
                Debug.WriteLine("Register image acquisition call back failed");
                _myCamera.MV_CC_DestroyDevice_NET();
                return false;
            }

            // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
            if (camerainfo.nTLayerType == MVCameraCtrl.MV_GIGE_DEVICE)
            {
                int nPacketSize = _myCamera.MV_CC_GetOptimalPacketSize_NET();
                if (nPacketSize > 0)
                {
                    nRet = _myCamera.MV_CC_SetIntValueEx_NET("GevSCPSPacketSize", nPacketSize);
                    if (nRet != MVCameraCtrl.MV_OK)
                    {
                        Debug.WriteLine("Set Packet Size failed!", nRet);
                    }
                }
                else
                {
                    Debug.WriteLine("Get Packet Size failed!", nPacketSize);
                }

                //设置心跳时间1000ms  
                nRet = _myCamera.MV_CC_SetHeartBeatTimeout_NET(1000);
                if (nRet != MVCameraCtrl.MV_OK)
                {
                    Debug.WriteLine("Set HeartBeatTimeout  failed!", nRet);
                }
            }

            //更新图像Buff大小；
            NecessaryOperBeforeGrab();
            SN = CamSN;

            return true;
        }

        public override void CloseDevice()
        {
            StopGrabbing();

            if (m_BufForDriver != IntPtr.Zero)
            {
                Marshal.Release(m_BufForDriver);
            }
            if (IntPtr.Zero != m_ConvertDstBuf)
            {
                Marshal.Release(m_ConvertDstBuf);
                m_ConvertDstBuf = IntPtr.Zero;
            }
            var nRet = _myCamera.MV_CC_CloseDevice_NET();
            if (MVCameraCtrl.MV_OK != nRet) return;
            nRet = _myCamera.MV_CC_DestroyDevice_NET();
            if (MVCameraCtrl.MV_OK != nRet) return;
        }

        public override bool SoftTrigger() => _myCamera.MV_CC_SetCommandValue_NET("TriggerSoftware") == MVCameraCtrl.MV_OK;

        #endregion


        #region SettingConfig
        public override bool SetTriggerMode(TriggerMode mode, TriggerSource triggerEnum = TriggerSource.Line0)
        {
            int rec;
            switch (mode)
            {
                case TriggerMode.Off:
                    rec = _myCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MVCameraCtrl.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                    break;
                case TriggerMode.On:
                    rec = _myCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MVCameraCtrl.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
                    break;
                default:
                    rec = _myCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MVCameraCtrl.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
                    break;
            }
            bool flag1 = MVCameraCtrl.MV_OK == rec;
            switch (triggerEnum)
            {
                case TriggerSource.Software:
                    rec = _myCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MVCameraCtrl.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                    break;
                case TriggerSource.Line0:
                    rec = _myCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MVCameraCtrl.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
                    break;
                case TriggerSource.Line1:
                    rec = _myCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MVCameraCtrl.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE1);
                    break;
                case TriggerSource.Line2:
                    rec = _myCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MVCameraCtrl.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE2);
                    break;
                case TriggerSource.Line3:
                    rec = _myCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MVCameraCtrl.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE3);
                    break;
                default:
                    rec = _myCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MVCameraCtrl.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
                    break;
            }
            bool flag2 = MVCameraCtrl.MV_OK == rec;
            return flag1 && flag2;
        }

        public override bool GetTriggerMode(out TriggerMode mode, out TriggerSource hardTriggerModel)
        {
            mode = TriggerMode.On;
            hardTriggerModel = TriggerSource.Line0;
            MVCameraCtrl.MVCC_ENUMVALUE stParam = new MVCameraCtrl.MVCC_ENUMVALUE();

            int nRet = _myCamera.MV_CC_GetEnumValue_NET("TriggerMode", ref stParam);
            MVCameraCtrl.MV_CAM_TRIGGER_MODE Mode = (MVCameraCtrl.MV_CAM_TRIGGER_MODE)stParam.nCurValue;
            bool flag1 = MVCameraCtrl.MV_OK == nRet;

            switch (Mode)
            {
                case MVCameraCtrl.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF:
                    mode = TriggerMode.Off;
                    break;
                case MVCameraCtrl.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON:
                    mode = TriggerMode.On;
                    break;
                default:
                    mode = TriggerMode.On;
                    break;
            }

            nRet = _myCamera.MV_CC_GetEnumValue_NET("TriggerSource", ref stParam);
            MVCameraCtrl.MV_CAM_TRIGGER_SOURCE Source = (MVCameraCtrl.MV_CAM_TRIGGER_SOURCE)stParam.nCurValue;
            bool flag2 = MVCameraCtrl.MV_OK == nRet;
            switch (Source)
            {
                case MVCameraCtrl.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0:
                    hardTriggerModel = TriggerSource.Line0;
                    break;
                case MVCameraCtrl.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE1:
                    hardTriggerModel = TriggerSource.Line1;
                    break;
                case MVCameraCtrl.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE2:
                    hardTriggerModel = TriggerSource.Line2;
                    break;
                case MVCameraCtrl.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE:
                    hardTriggerModel = TriggerSource.Software;
                    break;
                default:
                    hardTriggerModel = TriggerSource.Line0;
                    break;
            }

            return flag1 && flag2;
        }


        public override bool SetExpouseTime(ulong value) => _myCamera.MV_CC_SetFloatValue_NET("ExposureTime", value) == MVCameraCtrl.MV_OK;

        public override bool GetExpouseTime(out ulong value)
        {
            MVCameraCtrl.MVCC_FLOATVALUE stParam = new MVCameraCtrl.MVCC_FLOATVALUE();
            int nRet = _myCamera.MV_CC_GetFloatValue_NET("ExposureTime", ref stParam);
            value = (ulong)stParam.fCurValue;
            return MVCameraCtrl.MV_OK == nRet;
        }

        //1下降沿 0 上升沿
        public override bool SetTriggerPolarity(TriggerPolarity polarity)
            => _myCamera.MV_CC_SetEnumValueByString_NET("TriggerActivation", polarity.ToString()) == MVCameraCtrl.MV_OK;


        public override bool GetTriggerPolarity(out TriggerPolarity polarity)
        {
            polarity = TriggerPolarity.RisingEdge;
            MVCameraCtrl.MVCC_ENUMVALUE stParam = new MVCameraCtrl.MVCC_ENUMVALUE();
            int nRet = _myCamera.MV_CC_GetEnumValue_NET("TriggerActivation", ref stParam);

            ushort activate = (ushort)stParam.nCurValue;
            //1下降沿 0 上升沿
            if (activate == 0)
            { //上升沿
                polarity = TriggerPolarity.RisingEdge;
            }
            else if (activate == 1)
            { //下降沿
                polarity = TriggerPolarity.FallingEdge;
            }
            return MVCameraCtrl.MV_OK == nRet;
        }


        public override bool SetTriggerFliter(ushort flitertime) => _myCamera.MV_CC_SetIntValue_NET("LineDebouncerTime", flitertime) == MVCameraCtrl.MV_OK;

        public override bool GetTriggerFliter(out ushort flitertime)
        {
            flitertime = 1000;
            MVCameraCtrl.MVCC_INTVALUE stParam = new MVCameraCtrl.MVCC_INTVALUE();
            int nRet = _myCamera.MV_CC_GetIntValue_NET("LineDebouncerTime", ref stParam);
            flitertime = (ushort)stParam.nCurValue;
            return MVCameraCtrl.MV_OK == nRet;
        }


        public override bool SetTriggerDelay(ushort delay) => _myCamera.MV_CC_SetFloatValue_NET("TriggerDelay", delay) == MVCameraCtrl.MV_OK;

        public override bool GetTriggerDelay(out ushort delay)
        {
            delay = 0;
            MVCameraCtrl.MVCC_FLOATVALUE stParam = new MVCameraCtrl.MVCC_FLOATVALUE();
            int nRet = _myCamera.MV_CC_GetFloatValue_NET("TriggerDelay", ref stParam);
            delay = (ushort)stParam.fCurValue;
            return MVCameraCtrl.MV_OK == nRet;
        }

        public override bool SetGain(float gain) => _myCamera.MV_CC_SetFloatValue_NET("Gain", gain) == MVCameraCtrl.MV_OK;

        public override bool GetGain(out float gain)
        {
            MVCameraCtrl.MVCC_FLOATVALUE stParam = new MVCameraCtrl.MVCC_FLOATVALUE();
            int nRet = _myCamera.MV_CC_GetFloatValue_NET("Gain", ref stParam);
            gain = stParam.fCurValue;
            return MVCameraCtrl.MV_OK == nRet;
        }

        public override bool SetLineMode(IOLines line, LineMode mode)
            => _myCamera.MV_CC_SetEnumValueByString_NET(line.ToString(), mode.ToString()) == MVCameraCtrl.MV_OK;

        public override bool SetLineStatus(IOLines line, LineStatus linestatus)
              => _myCamera.MV_CC_SetBoolValue_NET(line.ToString(), linestatus.Equals(LineStatus.Hight)) == MVCameraCtrl.MV_OK;

        public override bool GetLineStatus(IOLines line, out LineStatus linestatus)
        {
            bool resultsignal = false;
            int nRet = _myCamera.MV_CC_GetBoolValue_NET(line.ToString(), ref resultsignal);
            linestatus = resultsignal ? LineStatus.Hight : LineStatus.Low;
            return MVCameraCtrl.MV_OK == nRet;
        }

        public override bool AutoBalanceWhite() => _myCamera.MV_CC_SetEnumValueByString_NET("BalanceWhiteAuto", "Once") == MVCameraCtrl.MV_OK;

        #endregion


        #region helper 

        /// <summary>
        ///  // Set default state after grabbing starts
        // Turn off real-time mode which is default
        // 0: real-time
        // 1: trigger
        /// </summary>
        /// <returns></returns>
        protected override bool StartGrabbing() => _myCamera.MV_CC_StartGrabbing_NET() == MVCameraCtrl.MV_OK;

        protected override bool StopGrabbing() => _myCamera.MV_CC_StopGrabbing_NET() == MVCameraCtrl.MV_OK;

        private List<MVCameraCtrl.MV_CC_DEVICE_INFO> GetListInfoEnum()
        {
            List<string> listsn = new List<string>();
            MVCameraCtrl.MV_CC_DEVICE_INFO_LIST m_stDeviceList = new MVCameraCtrl.MV_CC_DEVICE_INFO_LIST();
            List<MVCameraCtrl.MV_CC_DEVICE_INFO> deviceList = new List<MVCameraCtrl.MV_CC_DEVICE_INFO>();
            m_stDeviceList.nDeviceNum = 0;
            MVCameraCtrl.MV_CC_EnumDevices_NET(MVCameraCtrl.MV_GIGE_DEVICE | MVCameraCtrl.MV_USB_DEVICE, ref m_stDeviceList);
            for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
            {
                MVCameraCtrl.MV_CC_DEVICE_INFO device = (MVCameraCtrl.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i], typeof(MVCameraCtrl.MV_CC_DEVICE_INFO));
                deviceList.Add(device);
            }
            return deviceList;
        }

        private Bitmap ParseRawImageDatacallback(IntPtr pData, MVCameraCtrl.MV_FRAME_OUT_INFO_EX stFrameInfo)
        {
            lock (BufForDriverLock)
            {
                if (m_BufForDriver == IntPtr.Zero || stFrameInfo.nFrameLen > m_nBufSizeForDriver)
                {
                    if (m_BufForDriver != IntPtr.Zero)
                    {
                        Marshal.Release(m_BufForDriver);
                        m_BufForDriver = IntPtr.Zero;
                    }

                    m_BufForDriver = Marshal.AllocHGlobal((Int32)stFrameInfo.nFrameLen);
                    if (m_BufForDriver == IntPtr.Zero)
                    {
                        return null;
                    }
                    m_nBufSizeForDriver = stFrameInfo.nFrameLen;
                }

                CopyMemory(m_BufForDriver, pData, stFrameInfo.nFrameLen);

                ///  // ch:转换像素格式 | en:Convert Pixel Format
                MVCameraCtrl.MV_PIXEL_CONVERT_PARAM stConvertInfo = new MVCameraCtrl.MV_PIXEL_CONVERT_PARAM();
                stConvertInfo.nWidth = stFrameInfo.nWidth;
                stConvertInfo.nHeight = stFrameInfo.nHeight;
                stConvertInfo.enSrcPixelType = stFrameInfo.enPixelType;
                stConvertInfo.pSrcData = pData;
                stConvertInfo.nSrcDataLen = stFrameInfo.nFrameLen;
                stConvertInfo.pDstBuffer = m_ConvertDstBuf;
                stConvertInfo.nDstBufferSize = m_nConvertDstBufLen;
                if (PixelFormat.Format8bppIndexed == m_bitmap.PixelFormat)
                {
                    stConvertInfo.enDstPixelType = MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono8;
                    _myCamera.MV_CC_ConvertPixelType_NET(ref stConvertInfo);
                }
                else
                {
                    stConvertInfo.enDstPixelType = MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BGR8_Packed;
                    _myCamera.MV_CC_ConvertPixelType_NET(ref stConvertInfo);
                }

                // ch:保存Bitmap数据 | en:Save Bitmap Data
                BitmapData bitmapData = m_bitmap.LockBits(new Rectangle(0, 0, stConvertInfo.nWidth, stConvertInfo.nHeight), ImageLockMode.ReadWrite, m_bitmap.PixelFormat);
                CopyMemory(bitmapData.Scan0, stConvertInfo.pDstBuffer, (UInt32)(bitmapData.Stride * m_bitmap.Height));

                m_bitmap.UnlockBits(bitmapData);

                _myCamera.MV_CC_ClearImageBuffer_NET();
            }


            return m_bitmap;
        }

        /// <summary>
        /// ch:取图前的必要操作步骤 | en:Necessary operation before grab
        /// </summary>
        /// <returns></returns>
        private Int32 NecessaryOperBeforeGrab()
        {
            // ch:取图像宽 | en:Get Iamge Width
            MVCameraCtrl.MVCC_INTVALUE_EX stWidth = new MVCameraCtrl.MVCC_INTVALUE_EX();
            int nRet = _myCamera.MV_CC_GetIntValueEx_NET("Width", ref stWidth);
            if (MVCameraCtrl.MV_OK != nRet)
            {
                return nRet;
            }
            // ch:取图像高 | en:Get Iamge Height
            MVCameraCtrl.MVCC_INTVALUE_EX stHeight = new MVCameraCtrl.MVCC_INTVALUE_EX();
            nRet = _myCamera.MV_CC_GetIntValueEx_NET("Height", ref stHeight);
            if (MVCameraCtrl.MV_OK != nRet)
            {
                return nRet;
            }
            // ch:取像素格式 | en:Get Pixel Format
            MVCameraCtrl.MVCC_ENUMVALUE stPixelFormat = new MVCameraCtrl.MVCC_ENUMVALUE();
            nRet = _myCamera.MV_CC_GetEnumValue_NET("PixelFormat", ref stPixelFormat);
            if (MVCameraCtrl.MV_OK != nRet)
            {
                return nRet;
            }

            // ch:设置bitmap像素格式，申请相应大小内存 | en:Set Bitmap Pixel Format, alloc memory
            if ((Int32)MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Undefined == (Int32)stPixelFormat.nCurValue)
            {
                return MVCameraCtrl.MV_E_UNKNOW;
            }
            else if (IsMonoData((MVCameraCtrl.MvGvspPixelType)stPixelFormat.nCurValue))
            {
                m_bitmapPixelFormat = PixelFormat.Format8bppIndexed;
                //m_bitmapPixelFormat = PixelFormat.Format16bppGrayScale;
                if (IntPtr.Zero != m_ConvertDstBuf)
                {
                    Marshal.Release(m_ConvertDstBuf);
                    m_ConvertDstBuf = IntPtr.Zero;
                }
                m_nConvertDstBufLen = (UInt32)(stWidth.nCurValue * stHeight.nCurValue);
                m_ConvertDstBuf = Marshal.AllocHGlobal((Int32)m_nConvertDstBufLen);
                if (IntPtr.Zero == m_ConvertDstBuf)
                {
                    Debug.WriteLine("Malloc Memory Fail!");
                    return MVCameraCtrl.MV_E_RESOURCE;
                }
            }
            else
            {
                m_bitmapPixelFormat = PixelFormat.Format24bppRgb;
                if (IntPtr.Zero != m_ConvertDstBuf)
                {
                    Marshal.FreeHGlobal(m_ConvertDstBuf);
                    m_ConvertDstBuf = IntPtr.Zero;
                }

                // RGB为三通道
                m_nConvertDstBufLen = (UInt32)(3 * stWidth.nCurValue * stHeight.nCurValue);
                m_ConvertDstBuf = Marshal.AllocHGlobal((Int32)m_nConvertDstBufLen);
                if (IntPtr.Zero == m_ConvertDstBuf)
                {
                    Debug.WriteLine("Malloc Memory Fail!");
                    return MVCameraCtrl.MV_E_RESOURCE;
                }
            }

            // 确保释放保存了旧图像数据的bitmap实例，用新图像宽高等信息new一个新的bitmap实例
            if (null != m_bitmap)
            {
                m_bitmap.Dispose();
                m_bitmap = null;
            }
            m_bitmap = new Bitmap((Int32)stWidth.nCurValue, (Int32)stHeight.nCurValue, m_bitmapPixelFormat);

            // ch:Mono8格式，设置为标准调色板 | en:Set Standard Palette in Mono8 Format
            if (PixelFormat.Format8bppIndexed == m_bitmapPixelFormat)
            {
                ColorPalette palette = m_bitmap.Palette;
                for (int i = 0; i < palette.Entries.Length; i++)
                {
                    palette.Entries[i] = Color.FromArgb(i, i, i);
                }
                m_bitmap.Palette = palette;
            }

            return MVCameraCtrl.MV_OK;
        }

        private bool IsColorData(MVCameraCtrl.MvGvspPixelType enGvspPixelType)
        {
            switch (enGvspPixelType)
            {
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerGR10:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerRG10:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerGB10:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerBG10:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerGR12:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerRG12:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerGB12:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerBG12:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerGR10_Packed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerRG10_Packed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerGB10_Packed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerBG10_Packed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerGR12_Packed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerRG12_Packed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerGB12_Packed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_BayerBG12_Packed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_YUV422_Packed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_YUV422_YUYV_Packed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_YCBCR411_8_CBYYCRYY:
                    return true;

                default:
                    return false;
            }
        }

        private bool IsMonoData(MVCameraCtrl.MvGvspPixelType enGvspPixelType)
        {
            switch (enGvspPixelType)
            {
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono1p:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono2p:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono4p:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono8_Signed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono14:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono16:
                    return true;

                default:
                    return false;
            }
        }

        private void ImageCallback(IntPtr pdata, ref MVCameraCtrl.MV_FRAME_OUT_INFO_EX pframeinfo, IntPtr puser)
        {
            var bitMap = ParseRawImageDatacallback(pdata, pframeinfo);
            if (bitMap == null) return;

            ActionGetImage?.Invoke(bitMap.Clone() as Bitmap);

        }
        #endregion
    }
}


