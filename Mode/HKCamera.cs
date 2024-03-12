#define MVCAMERA

using MG.CamCtrl.Cameralibs.HKCamera;
using MG.CamCtrl.Common.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static MG.CamCtrl.Cameralibs.HKCamera.MVCameraCtrl;

namespace MG.CamCtrl.Mode
{
    internal class HKCamera : BaseCamera
    {
        public HKCamera() : base() { }


        #region param
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        private MVCameraCtrl _myCamera = new MVCameraCtrl();
        private cbOutputExdelegate _imageCallbackDelegate = null;
        private bool m_bGrabbing = false;

        IntPtr m_BufForDriver = IntPtr.Zero;
        UInt32 _bufferSize = 3072 * 2048 * 3;
        private byte[] _buffer;
        private uint _buffSizeForSaveImage = 3072 * 2048 * (16 * 3 + 4) + 2048;
        private byte[] _bufForSaveImage;
        private Object m_BufForSaveImageLock = new Object();
        public IntPtr m_pSaveImageBuf = IntPtr.Zero;
        MVCameraCtrl.MV_FRAME_OUT_INFO_EX m_stFrameInfo = new MVCameraCtrl.MV_FRAME_OUT_INFO_EX();
        #endregion


        #region operate

        public override List<string> GetListEnum()
        {
            List<string> deviceList = new List<string>();
            foreach (var item in GetListInfoEnum())
            {
                MVCameraCtrl.MV_GIGE_DEVICE_INFO gigeInfo = (MVCameraCtrl.MV_GIGE_DEVICE_INFO)MVCameraCtrl.ByteToStruct(item.SpecialInfo.stGigEInfo, typeof(MVCameraCtrl.MV_GIGE_DEVICE_INFO));
                deviceList.Add(gigeInfo.chSerialNumber);
            }

            return deviceList;
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
                MVCameraCtrl.MV_GIGE_DEVICE_INFO gigeInfo = (MVCameraCtrl.MV_GIGE_DEVICE_INFO)MVCameraCtrl.ByteToStruct(item.SpecialInfo.stGigEInfo, typeof(MVCameraCtrl.MV_GIGE_DEVICE_INFO));
                if (gigeInfo.chSerialNumber.Equals(CamSN))
                {
                    camerainfo = item;
                    selectSNflag = true;
                    break;
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

            // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
            // _myCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MVCameraCtrl.MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
            // _myCamera.MV_CC_SetEnumValue_NET("triggerMode", (uint)MVCameraCtrl.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);

            // Reserve buffer
            _buffer = new byte[_bufferSize];
            _bufForSaveImage = new byte[_buffSizeForSaveImage];
            SN = CamSN;
            return true;
        }

        public override void CloseDevice()
        {
            if (m_BufForDriver != IntPtr.Zero)
            {
                Marshal.Release(m_BufForDriver);
            }

            var nRet = _myCamera.MV_CC_CloseDevice_NET();
            if (MVCameraCtrl.MV_OK != nRet) return;
            nRet = _myCamera.MV_CC_DestroyDevice_NET();
            if (MVCameraCtrl.MV_OK != nRet) return;
        }

        public override bool SoftTrigger()
        {
            int nRet = _myCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
            return (MVCameraCtrl.MV_OK == nRet);
        }

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
            bool flag1 = (MVCameraCtrl.MV_OK == rec);
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
            bool flag2 = (MVCameraCtrl.MV_OK == rec);
            return flag1 && flag2;
        }

        public override bool GetTriggerMode(out TriggerMode mode, out TriggerSource hardTriggerModel)
        {

            mode = TriggerMode.On;
            hardTriggerModel = TriggerSource.Line0;
            MVCameraCtrl.MVCC_ENUMVALUE stParam = new MVCameraCtrl.MVCC_ENUMVALUE();

            int nRet = _myCamera.MV_CC_GetEnumValue_NET("TriggerMode", ref stParam);
            MVCameraCtrl.MV_CAM_TRIGGER_MODE Mode = (MVCameraCtrl.MV_CAM_TRIGGER_MODE)stParam.nCurValue;
            bool flag1 = (MVCameraCtrl.MV_OK == nRet);

            switch (Mode)
            {
                case MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF:
                    mode = TriggerMode.Off;
                    break;
                case MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON:
                    mode = TriggerMode.On;
                    break;
                default:
                    mode = TriggerMode.On;
                    break;
            }

            nRet = _myCamera.MV_CC_GetEnumValue_NET("TriggerSource", ref stParam);
            MVCameraCtrl.MV_CAM_TRIGGER_SOURCE Source = (MVCameraCtrl.MV_CAM_TRIGGER_SOURCE)stParam.nCurValue;
            bool flag2 = (MVCameraCtrl.MV_OK == nRet);
            switch (Source)
            {
                case MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0:
                    hardTriggerModel = TriggerSource.Line0;
                    break;
                case MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE1:
                    hardTriggerModel = TriggerSource.Line1;
                    break;
                case MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE2:
                    hardTriggerModel = TriggerSource.Line2;
                    break;
                case MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE:
                    hardTriggerModel = TriggerSource.Software;
                    break;
                default:
                    hardTriggerModel = TriggerSource.Line0;
                    break;
            }

            return flag1 && flag2;
        }


        public override bool SetExpouseTime(ushort value)
        {
            int nRet = _myCamera.MV_CC_SetFloatValue_NET("ExposureTime", (float)value);
            return (MVCameraCtrl.MV_OK == nRet);
        }

        public override bool GetExpouseTime(out ushort value)
        {
            MVCameraCtrl.MVCC_FLOATVALUE stParam = new MVCameraCtrl.MVCC_FLOATVALUE();
            int nRet = _myCamera.MV_CC_GetFloatValue_NET("ExposureTime", ref stParam);
            value = (ushort)stParam.fCurValue;
            return (MVCameraCtrl.MV_OK == nRet);
        }


        public override bool SetTriggerPolarity(TriggerPolarity polarity)
        {
            int nRet = _myCamera.MV_CC_SetEnumValueByString_NET("TriggerActivation", polarity.ToString());//1下降沿 0 上升沿
            return (MVCameraCtrl.MV_OK == nRet);
        }

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
            return (MVCameraCtrl.MV_OK == nRet);
        }


        public override bool SetTriggerFliter(ushort flitertime)
        {
            int nRet = _myCamera.MV_CC_SetIntValue_NET("LineDebouncerTime", (uint)flitertime);
            return (MVCameraCtrl.MV_OK == nRet);
        }

        public override bool GetTriggerFliter(out ushort flitertime)
        {
            flitertime = 1000;
            MVCameraCtrl.MVCC_INTVALUE stParam = new MVCameraCtrl.MVCC_INTVALUE();
            int nRet = _myCamera.MV_CC_GetIntValue_NET("LineDebouncerTime", ref stParam);
            flitertime = (ushort)stParam.nCurValue;
            return (MVCameraCtrl.MV_OK == nRet);
        }


        public override bool SetTriggerDelay(ushort delay)
        {
            int nRet = _myCamera.MV_CC_SetFloatValue_NET("TriggerDelay", (float)delay);
            return (MVCameraCtrl.MV_OK == nRet);
        }

        public override bool GetTriggerDelay(out ushort delay)
        {
            delay = 0;
            MVCameraCtrl.MVCC_FLOATVALUE stParam = new MVCameraCtrl.MVCC_FLOATVALUE();
            int nRet = _myCamera.MV_CC_GetFloatValue_NET("TriggerDelay", ref stParam);
            delay = (ushort)stParam.fCurValue;
            return (MVCameraCtrl.MV_OK == nRet);

        }


        public override bool SetGain(short gain)
        {
            int nRet = _myCamera.MV_CC_SetFloatValue_NET("Gain", (float)gain);
            return (MVCameraCtrl.MV_OK == nRet);
        }

        public override bool GetGain(out short gain)
        {
            MVCameraCtrl.MVCC_FLOATVALUE stParam = new MVCameraCtrl.MVCC_FLOATVALUE();
            int nRet = _myCamera.MV_CC_GetFloatValue_NET("Gain", ref stParam);
            gain = (short)stParam.fCurValue;
            return (MVCameraCtrl.MV_OK == nRet);
        }

        public override bool SetLineMode(IOLines line, LineMode mode)
        {
            int nRet = _myCamera.MV_CC_SetEnumValueByString_NET(line.ToString(), mode.ToString());
            return (MVCameraCtrl.MV_OK == nRet);

        }
        public override bool SetLineStatus(IOLines line, LineStatus linestatus)
        {

            int nRet = _myCamera.MV_CC_SetBoolValue_NET(line.ToString(), linestatus.Equals(LineStatus.Hight));
            return (MVCameraCtrl.MV_OK == nRet);
        }

        public override bool GetLineStatus(IOLines line, out LineStatus linestatus)
        {
            bool resultsignal = false;
            int nRet = _myCamera.MV_CC_GetBoolValue_NET(line.ToString(), ref resultsignal);
            linestatus = resultsignal ? LineStatus.Hight : LineStatus.Low;
            return (MVCameraCtrl.MV_OK == nRet);
        }


        public override bool AutoBalanceWhite()
        {
            int nRet = _myCamera.MV_CC_SetEnumValueByString_NET("BalanceWhiteAuto", "Once");
            return (MVCameraCtrl.MV_OK == nRet);
        }


        #endregion


        #region helper 

        protected override bool StartGrabbing()
        {
            // Set default state after grabbing starts
            // Turn off real-time mode which is default
            // 0: real-time
            // 1: trigger

            var success = _myCamera.MV_CC_StartGrabbing_NET() == 0;
            if (!success) Debug.WriteLine("Grab start failed");

            return success;
        }

        protected override bool StopGrabbing()
        {
            var success = _myCamera.MV_CC_StopGrabbing_NET() == 0;
            if (!success) Debug.WriteLine("Grab stop failed");
            return success;
        }

        private List<MVCameraCtrl.MV_CC_DEVICE_INFO> GetListInfoEnum()
        {
            System.GC.Collect();
            MVCameraCtrl.MV_CC_DEVICE_INFO_LIST m_stDeviceList = new MVCameraCtrl.MV_CC_DEVICE_INFO_LIST();
            List<MVCameraCtrl.MV_CC_DEVICE_INFO> deviceList = new List<MVCameraCtrl.MV_CC_DEVICE_INFO>();
            m_stDeviceList.nDeviceNum = 0;
            MVCameraCtrl.MV_CC_EnumDevices_NET(MVCameraCtrl.MV_GIGE_DEVICE | MVCameraCtrl.MV_USB_DEVICE, ref m_stDeviceList);
            for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
            {
                MVCameraCtrl.MV_CC_DEVICE_INFO device = (MVCameraCtrl.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i], typeof(MVCameraCtrl.MV_CC_DEVICE_INFO));
                deviceList.Add(device);
                //if (device.nTLayerType == MVCameraCtrl.MV_GIGE_DEVICE)
                //{
                //    MVCameraCtrl.MV_GIGE_DEVICE_INFO gigeInfo = (MVCameraCtrl.MV_GIGE_DEVICE_INFO)MVCameraCtrl.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MVCameraCtrl.MV_GIGE_DEVICE_INFO));
                //}
                //else if (device.nTLayerType == MVCameraCtrl.MV_USB_DEVICE)
                //{
                //    MVCameraCtrl.MV_USB3_DEVICE_INFO usbInfo = (MVCameraCtrl.MV_USB3_DEVICE_INFO)MVCameraCtrl.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MVCameraCtrl.MV_USB3_DEVICE_INFO));
                //}
            }
            return deviceList;
        }

        private Bitmap ParseRawImageDatacallback(IntPtr pData, MVCameraCtrl.MV_FRAME_OUT_INFO_EX stFrameInfo)
        {
            Bitmap output = null;


            //int nIndex = (int)pUser;

            // ch:抓取的帧数 | en:Aquired Frame Number
            //++m_nFrames[nIndex];

            lock (m_BufForSaveImageLock)
            {
                if (m_pSaveImageBuf == IntPtr.Zero || stFrameInfo.nFrameLen > _buffSizeForSaveImage)
                {
                    if (m_pSaveImageBuf != IntPtr.Zero)
                    {
                        Marshal.Release(m_pSaveImageBuf);
                        m_pSaveImageBuf = IntPtr.Zero;
                    }

                    m_pSaveImageBuf = Marshal.AllocHGlobal((Int32)stFrameInfo.nFrameLen);
                    if (m_pSaveImageBuf == IntPtr.Zero)
                    {
                        return output;
                    }
                    _buffSizeForSaveImage = stFrameInfo.nFrameLen;
                }

                m_stFrameInfo = stFrameInfo;
                CopyMemory(m_pSaveImageBuf, pData, stFrameInfo.nFrameLen);
            }

            //MVCameraCtrl.MV_DISPLAY_FRAME_INFO stDisplayInfo = new MVCameraCtrl.MV_DISPLAY_FRAME_INFO();
            //stDisplayInfo.hWnd = m_hDisplayHandle[nIndex];
            //stDisplayInfo.pData = pData;
            //stDisplayInfo.nDataLen = stFrameInfo.nFrameLen;
            //stDisplayInfo.nWidth = stFrameInfo.nWidth;
            //stDisplayInfo.nHeight = stFrameInfo.nHeight;
            //stDisplayInfo.enPixelType = stFrameInfo.enPixelType;

            //_myCamera.MV_CC_DisplayOneFrame_NET(ref stDisplayInfo);


            MVCameraCtrl.MvGvspPixelType enDstPixelType;
            if (IsMonoData(stFrameInfo.enPixelType))
            {
                enDstPixelType = MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono8;
            }
            else if (IsColorData(stFrameInfo.enPixelType))
            {
                enDstPixelType = MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed;
            }
            else
            {
                throw new NotSupportedException("Can not support such pixel type currently");
            }

            IntPtr pImage = Marshal.UnsafeAddrOfPinnedArrayElement(_bufForSaveImage, 0);

            MVCameraCtrl.MV_PIXEL_CONVERT_PARAM stConverPixelParam = new MVCameraCtrl.MV_PIXEL_CONVERT_PARAM
            {
                nWidth = stFrameInfo.nWidth,
                nHeight = stFrameInfo.nHeight,
                pSrcData = pData,
                nSrcDataLen = stFrameInfo.nFrameLen,
                enSrcPixelType = stFrameInfo.enPixelType,
                enDstPixelType = enDstPixelType,
                pDstBuffer = pImage,
                nDstBufferSize = _buffSizeForSaveImage
            };

            int nRet = _myCamera.MV_CC_ConvertPixelType_NET(ref stConverPixelParam);
            if (MVCameraCtrl.MV_OK != nRet)
            {
                //throw new InvalidOperationException("Unable to convert pixel type");
                return null;
            }

            if (enDstPixelType == MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono8)
            {
                //************************Mono8 转 Bitmap*******************************
                output = new Bitmap(stFrameInfo.nWidth, stFrameInfo.nHeight, stFrameInfo.nWidth * 1,
                    PixelFormat.Format8bppIndexed, pImage);

                ColorPalette cp = output.Palette;
                // init palette
                for (int i = 0; i < 256; i++)
                {
                    cp.Entries[i] = Color.FromArgb(i, i, i);
                }

                output.Palette = cp;
            }
            else
            {
                //*********************RGB8 转 Bitmap**************************
                for (int i = 0; i < stFrameInfo.nHeight; i++)
                {
                    for (int j = 0; j < stFrameInfo.nWidth; j++)
                    {
                        byte chRed = _bufForSaveImage[i * stFrameInfo.nWidth * 3 + j * 3];
                        _bufForSaveImage[i * stFrameInfo.nWidth * 3 + j * 3] =
                            _bufForSaveImage[i * stFrameInfo.nWidth * 3 + j * 3 + 2];
                        _bufForSaveImage[i * stFrameInfo.nWidth * 3 + j * 3 + 2] = chRed;
                    }
                }

                output = new Bitmap(stFrameInfo.nWidth, stFrameInfo.nHeight, stFrameInfo.nWidth * 3,
                    PixelFormat.Format24bppRgb, pImage);
            }

            return output;
        }

        private Boolean IsColorData(MVCameraCtrl.MvGvspPixelType enGvspPixelType)
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
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MVCameraCtrl.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                    return true;

                default:
                    return false;
            }
        }

        private void ImageCallback(IntPtr pdata, ref MVCameraCtrl.MV_FRAME_OUT_INFO_EX pframeinfo, IntPtr puser)
        {
            //  var bitMap = ParseRawImageData(pdata, pframeinfo);
            var bitMap = ParseRawImageDatacallback(pdata, pframeinfo);
            if (bitMap == null) return;
            ActionGetImage?.Invoke(bitMap);
        }



        #endregion

    }
}


