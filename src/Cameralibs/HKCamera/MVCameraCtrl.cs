using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MG.CamCtrl.Cameralibs.HKCamera
{
    internal class MVCameraCtrl
    { /// <summary>
      /// Constructor
      /// </summary>
        public MVCameraCtrl()
        {
            handle = IntPtr.Zero;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~MVCameraCtrl()
        {
            handle = IntPtr.Zero;
        }

        /// <summary>
        /// 设备句柄
        /// </summary>
        IntPtr handle;

        #region ch:委托声明 | en:delegate

        /// <summary>
        /// Grab callback
        /// </summary>
        /// <param name="pData">Image data</param>
        /// <param name="pFrameInfo">Frame info</param>
        /// <param name="pUser">User defined variable</param>
        public delegate void cbOutputdelegate(IntPtr pData, ref MV_FRAME_OUT_INFO pFrameInfo, IntPtr pUser);

        /// <summary>
        /// Grab callback
        /// </summary>
        /// <param name="pData">Image data</param>
        /// <param name="pFrameInfo">Frame info</param>
        /// <param name="pUser">User defined variable</param>
        public delegate void cbOutputExdelegate(IntPtr pData, ref MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser);

        /// <summary>
        /// Xml Update callback(Interfaces not recommended)
        /// </summary>
        /// <param name="enType">Node type</param>
        /// <param name="pstFeature">Current node feature structure</param>
        /// <param name="pstNodesList">Nodes list</param>
        /// <param name="pUser">User defined variable</param>
        public delegate void cbXmlUpdatedelegate(MV_XML_InterfaceType enType, IntPtr pstFeature, ref MV_XML_NODES_LIST pstNodesList, IntPtr pUser);

        /// <summary>
        /// Exception callback
        /// </summary>
        /// <param name="nMsgType">Msg type</param>
        /// <param name="pUser">User defined variable</param>
        public delegate void cbExceptiondelegate(UInt32 nMsgType, IntPtr pUser);

        /// <summary>
        /// Event callback (Interfaces not recommended)
        /// </summary>
        /// <param name="nUserDefinedId">User defined ID</param>
        /// <param name="pUser">User defined variable</param>
        public delegate void cbEventdelegate(UInt32 nUserDefinedId, IntPtr pUser);

        /// <summary>
        /// Event callback
        /// </summary>
        /// <param name="pEventInfo">Event Info</param>
        /// <param name="pUser">User defined variable</param>
        public delegate void cbEventdelegateEx(ref MV_EVENT_OUT_INFO pEventInfo, IntPtr pUser);

        /// <summary>
        /// Stream Exception callback
        /// </summary>
        /// <param name="enExceptionType">Msg type</param>
        /// <param name="pUser">User defined variable</param>
        public delegate void cbStreamException(MV_CC_STREAM_EXCEPTION_TYPE enExceptionType, IntPtr pUser);
        #endregion

        /// <summary>
        /// Initialize
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public static Int32 MV_CC_Initialize_NET()
        {
            return MV_CC_Initialize();
        }

        /// <summary>
        /// Finalize
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public static Int32 MV_CC_Finalize_NET()
        {
            return MV_CC_Finalize();
        }

        #region 采集卡接口
        /// <summary>
        /// 枚举采集卡设备信息
        /// </summary>
        /// <param name="nTLayerType">采集卡类型</param>
        /// <param name="pInterfaceInfoList">设备信息</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public static Int32 MV_CC_EnumInterfaces_NET(UInt32 nTLayerType, ref MV_INTERFACE_INFO_LIST pInterfaceInfoList)
        {
            return MV_CC_EnumInterfaces(nTLayerType, ref pInterfaceInfoList);
        }

        /// <summary>
        /// 创建采集卡设备句柄
        /// </summary>
        /// <param name="pInterfaceInfo">采集卡设备信息</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_CreateInterface_NET(ref MV_INTERFACE_INFO pInterfaceInfo)
        {
            if (IntPtr.Zero != handle)
            {
                MV_CC_DestroyInterface(handle);
                handle = IntPtr.Zero;
            }

            return MV_CC_CreateInterface(ref handle, ref pInterfaceInfo);
        }

        /// <summary>
        /// 通过采集卡ID创建设备句柄
        /// </summary>
        /// <param name="pInterfaceID">采集卡ID</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_CreateInterfaceByID_NET(String pInterfaceID)
        {
            if (IntPtr.Zero != handle)
            {
                MV_CC_DestroyInterface(handle);
                handle = IntPtr.Zero;
            }

            return MV_CC_CreateInterfaceByID(ref handle, pInterfaceID);
        }

        /// <summary>
        /// 打开采集卡设备
        /// </summary>
        /// <param name="strConfigFile">采集卡信息配置文件(目前不支持传配置文件)</param>
        /// <returns></returns>
        public Int32 MV_CC_OpenInterface_NET(String strConfigFile)
        {
            return MV_CC_OpenInterface(handle, strConfigFile);
        }

        /// <summary>
        /// 关闭采集卡
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_CloseInterface_NET()
        {
            return MV_CC_CloseInterface(handle);
        }

        /// <summary>
        /// 销毁采集卡句柄
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_DestroyInterface_NET()
        {
            Int32 nRet = MV_CC_DestroyInterface(handle);
            handle = IntPtr.Zero;
            return nRet;
        }
        #endregion


        #region ch:相机的控制和取流接口 | en:Camera control and streaming
        /// <summary>
        /// Get Camera Handle
        /// </summary>
        /// <returns></returns>
        public IntPtr GetCameraHandle()
        {
            return handle;
        }

        /// <summary>
        /// Get SDK Version
        /// </summary>
        /// <returns>Always return 4 Bytes of version number |Main  |Sub   |Rev   |Test|
        ///                                                   8bits  8bits  8bits  8bits 
        /// </returns>
        public static UInt32 MV_CC_GetSDKVersion_NET()
        {
            return MV_CC_GetSDKVersion();
        }

        /// <summary>
        /// Get supported Transport Layer
        /// </summary>
        /// <returns>Supported Transport Layer number</returns>
        public static Int32 MV_CC_EnumerateTls_NET()
        {
            return MV_CC_EnumerateTls();
        }

        /// <summary>
        /// Enumerate Device
        /// </summary>
        /// <param name="nTLayerType">Enumerate TLs</param>
        /// <param name="stDevList">Device List</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public static Int32 MV_CC_EnumDevices_NET(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList)
        {
            return MV_CC_EnumDevices(nTLayerType, ref stDevList);
        }

        /// <summary>
        /// Enumerate device according to manufacture name
        /// </summary>
        /// <param name="nTLayerType">Enumerate TLs</param>
        /// <param name="stDevList">Device List</param>
        /// <param name="pManufacturerName">Manufacture Name</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public static Int32 MV_CC_EnumDevicesEx_NET(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList, string pManufacturerName)
        {
            return MV_CC_EnumDevicesEx(nTLayerType, ref stDevList, pManufacturerName);
        }

        /// <summary>
        /// Enumerate device according to the specified ordering
        /// </summary>
        /// <param name="nTLayerType">Transmission layer of enumeration(All layer protocol type can input)</param>
        /// <param name="stDevList">Device list</param>
        /// <param name="pManufacturerName">Manufacture Name</param>
        /// <param name="enSortMethod">Sorting Method</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public static Int32 MV_CC_EnumDevicesEx2_NET(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList, string pManufacturerName, MV_SORT_METHOD enSortMethod)
        {
            return MV_CC_EnumDevicesEx2(nTLayerType, ref stDevList, pManufacturerName, enSortMethod);
        }

        /// <summary>
        /// Is the device accessible
        /// </summary>
        /// <param name="stDevInfo">Device Information</param>
        /// <param name="nAccessMode">Access Right</param>
        /// <returns>Access, return true. Not access, return false</returns>
        public static Boolean MV_CC_IsDeviceAccessible_NET(ref MV_CC_DEVICE_INFO stDevInfo, UInt32 nAccessMode)
        {
            Byte bRet = MV_CC_IsDeviceAccessible(ref stDevInfo, nAccessMode);
            if (bRet != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set SDK log path (Interfaces not recommended)
        /// If the logging service MvLogServer is enabled, the interface is invalid and The logging service is enabled by default
        /// </summary>
        /// <param name="pSDKLogPath"></param>
        /// <returns></returns>
        public static Int32 MV_CC_SetSDKLogPath_NET(String pSDKLogPath)
        {
            return MV_CC_SetSDKLogPath(pSDKLogPath);
        }

        /// <summary>
        /// Create Device
        /// </summary>
        /// <param name="stDevInfo">Device Information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_CreateDevice_NET(ref MV_CC_DEVICE_INFO stDevInfo)
        {
            if (IntPtr.Zero != handle)
            {
                MV_CC_DestroyHandle(handle);
                handle = IntPtr.Zero;
            }

            return MV_CC_CreateHandle(ref handle, ref stDevInfo);
        }

        /// <summary>
        /// Create Device without log
        /// </summary>
        /// <param name="stDevInfo">Device Information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_CreateDeviceWithoutLog_NET(ref MV_CC_DEVICE_INFO stDevInfo)
        {
            if (IntPtr.Zero != handle)
            {
                MV_CC_DestroyHandle(handle);
                handle = IntPtr.Zero;
            }

            return MV_CC_CreateHandleWithoutLog(ref handle, ref stDevInfo);
        }

        /// <summary>
        /// Destroy Device
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_DestroyDevice_NET()
        {
            Int32 nRet = MV_CC_DestroyHandle(handle);
            handle = IntPtr.Zero;
            return nRet;
        }

        /// <summary>
        /// Open Device
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_OpenDevice_NET()
        {
            return MV_CC_OpenDevice(handle, 1, 0);
        }

        /// <summary>
        /// Open Device
        /// </summary>
        /// <param name="nAccessMode">Access Right</param>
        /// <param name="nSwitchoverKey">Switch key of access right</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_OpenDevice_NET(UInt32 nAccessMode, UInt16 nSwitchoverKey)
        {
            return MV_CC_OpenDevice(handle, nAccessMode, nSwitchoverKey);
        }

        /// <summary>
        /// Close Device
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_CloseDevice_NET()
        {
            return MV_CC_CloseDevice(handle);
        }

        /// <summary>
        /// Is the device connected
        /// </summary>
        /// <returns>Connected, return true. Not Connected or DIsconnected, return false</returns>
        public Boolean MV_CC_IsDeviceConnected_NET()
        {
            if (handle == IntPtr.Zero)
            {
                return false;
            }

            Byte bRet = MV_CC_IsDeviceConnected(handle);
            if (bRet != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Register the image callback function
        /// </summary>
        /// <param name="cbOutput">Callback function pointer</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_RegisterImageCallBackEx_NET(cbOutputExdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBackEx(handle, cbOutput, pUser);
        }

        /// <summary>
        /// Register the RGB image callback function
        /// </summary>
        /// <param name="cbOutput">Callback function pointer</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_RegisterImageCallBackForRGB_NET(cbOutputExdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBackForRGB(handle, cbOutput, pUser);
        }

        /// <summary>
        /// Register the BGR image callback function
        /// </summary>
        /// <param name="cbOutput">Callback function pointer</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_RegisterImageCallBackForBGR_NET(cbOutputExdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBackForBGR(handle, cbOutput, pUser);
        }

        /// <summary>
        /// Start Grabbing
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_StartGrabbing_NET()
        {
            return MV_CC_StartGrabbing(handle);
        }

        /// <summary>
        /// Stop Grabbing
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_StopGrabbing_NET()
        {
            return MV_CC_StopGrabbing(handle);
        }

        /// <summary>
        /// Get one frame of RGB image, this function is using query to get data
        /// query whether the internal cache has data, get data if there has, return error code if no data
        /// </summary>
        /// <param name="pData">Image data receiving buffer</param>
        /// <param name="nDataSize">Buffer size</param>
        /// <param name="pFrameInfo">Image information</param>
        /// <param name="nMsec">Waiting timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetImageForRGB_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec)
        {
            return MV_CC_GetImageForRGB(handle, pData, nDataSize, ref pFrameInfo, nMsec);
        }

        /// <summary>
        /// Get one frame of BGR image, this function is using query to get data
        /// query whether the internal cache has data, get data if there has, return error code if no data
        /// </summary>
        /// <param name="pData">Image data receiving buffer</param>
        /// <param name="nDataSize">Buffer size</param>
        /// <param name="pFrameInfo">Image information</param>
        /// <param name="nMsec">Waiting timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error cod</returns>
        public Int32 MV_CC_GetImageForBGR_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec)
        {
            return MV_CC_GetImageForBGR(handle, pData, nDataSize, ref pFrameInfo, nMsec);
        }

        /// <summary>
        /// Get a frame of an image using an internal cache
        /// </summary>
        /// <param name="pFrame">Image data and image information</param>
        /// <param name="nMsec">Waiting timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetImageBuffer_NET(ref MV_FRAME_OUT pFrame, Int32 nMsec)
        {
            return MV_CC_GetImageBuffer(handle, ref pFrame, nMsec);
        }

        /// <summary>
        /// Free image buffer（used with MV_CC_GetImageBuffer）
        /// </summary>
        /// <param name="pFrame">Image data and image information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_FreeImageBuffer_NET(ref MV_FRAME_OUT pFrame)
        {
            return MV_CC_FreeImageBuffer(handle, ref pFrame);
        }

        /// <summary>
        /// Get a frame of an image
        /// </summary>
        /// <param name="pData">Image data receiving buffer</param>
        /// <param name="nDataSize">Buffer size</param>
        /// <param name="pFrameInfo">Image information</param>
        /// <param name="nMsec">Waiting timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetOneFrameTimeout_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec)
        {
            return MV_CC_GetOneFrameTimeout(handle, pData, nDataSize, ref pFrameInfo, nMsec);
        }

        /// <summary>
        /// Clear image Buffers to clear old data
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_ClearImageBuffer_NET()
        {
            return MV_CC_ClearImageBuffer(handle);
        }

        /// <summary>
        /// Get the number of valid images in the current image buffer
        /// </summary>
        /// <param name="pnValidImageNum">The number of valid images in the current image buffer</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetValidImageNum_NET(ref UInt32 pnValidImageNum)
        {
            return MV_CC_GetValidImageNum(handle, ref pnValidImageNum);
        }

        /// <summary>
        /// Display one frame image
        /// </summary>
        /// <param name="pDisplayInfo">Image information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_DisplayOneFrame_NET(ref MV_DISPLAY_FRAME_INFO pDisplayInfo)
        {
            return MV_CC_DisplayOneFrame(handle, ref pDisplayInfo);
        }

        /// <summary>
        /// Display one frame image Ex
        /// </summary>
        /// <param name="pDisplayHandle">dispaly Handle</param>
        /// <param name="pDisplayInfoEx">Image information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_DisplayOneFrameEx_NET(IntPtr pDisplayHandle, ref MV_DISPLAY_FRAME_INFO_EX pDisplayInfoEx)
        {
            if (IntPtr.Zero == pDisplayHandle)
            {
                return MV_E_PARAMETER;
            }
            return MV_CC_DisplayOneFrameEx(handle, pDisplayHandle, ref pDisplayInfoEx);
        }

        /// <summary>
        /// Set the number of the internal image cache nodes in SDK(Greater than or equal to 1, to be called before the capture)
        /// </summary>
        /// <param name="nNum">Number of cache nodes</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetImageNodeNum_NET(UInt32 nNum)
        {
            return MV_CC_SetImageNodeNum(handle, nNum);
        }

        /// <summary>
        /// Set Grab Strategy
        /// </summary>
        /// <param name="enGrabStrategy">The value of grab strategy</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetGrabStrategy_NET(MV_GRAB_STRATEGY enGrabStrategy)
        {
            return MV_CC_SetGrabStrategy(handle, enGrabStrategy);
        }

        /// <summary>
        /// Set The Size of Output Queue(Only work under the strategy of MV_GrabStrategy_LatestImages，rang：1-ImageNodeNum)
        /// </summary>
        /// <param name="nOutputQueueSize">The Size of Output Queue</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetOutputQueueSize_NET(UInt32 nOutputQueueSize)
        {
            return MV_CC_SetOutputQueueSize(handle, nOutputQueueSize);
        }

        /// <summary>
        /// Get device information(Called before start grabbing)
        /// </summary>
        /// <param name="pstDevInfo">device information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetDeviceInfo_NET(ref MV_CC_DEVICE_INFO pstDevInfo)
        {
            return MV_CC_GetDeviceInfo(handle, ref pstDevInfo);
        }

        /// <summary>
        /// Get various type of information
        /// </summary>
        /// <param name="pstInfo">Various type of information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetAllMatchInfo_NET(ref MV_ALL_MATCH_INFO pstInfo)
        {
            return MV_CC_GetAllMatchInfo(handle, ref pstInfo);
        }
        #endregion

        #region ch:相机属性万能配置接口&读写寄存器接口 | en: Camera attribute nodes set and obtained universal interface
        /// <summary>
        /// Get Integer value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "Width" to get width</param>
        /// <param name="pstValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetIntValueEx_NET(String strKey, ref MVCC_INTVALUE_EX pstValue)
        {
            return MV_CC_GetIntValueEx(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// Set Integer value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "Width" to set width</param>
        /// <param name="nValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetIntValueEx_NET(String strKey, Int64 nValue)
        {
            return MV_CC_SetIntValueEx(handle, strKey, nValue);
        }

        /// <summary>
        /// Get Enum value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "PixelFormat" to get pixel format</param>
        /// <param name="pstValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetEnumValue_NET(String strKey, ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetEnumValue(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// Set Enum value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "PixelFormat" to set pixel format</param>
        /// <param name="nValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetEnumValue_NET(String strKey, UInt32 nValue)
        {
            return MV_CC_SetEnumValue(handle, strKey, nValue);
        }

        /// <summary>
        /// Get the symbolic of the specified value of the Enum type node
        /// </summary>
        /// <param name="strKey">Key value, for example, using "PixelFormat" to set pixel format</param>
        /// <param name="pstEnumEntry">Symbolic to get</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetEnumEntrySymbolic_NET(String strKey, ref MVCC_ENUMENTRY pstEnumEntry)
        {
            return MV_CC_GetEnumEntrySymbolic(handle, strKey, ref pstEnumEntry);
        }

        /// <summary>
        /// Set Enum value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "PixelFormat" to set pixel format</param>
        /// <param name="sValue">Feature String to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetEnumValueByString_NET(String strKey, String sValue)
        {
            return MV_CC_SetEnumValueByString(handle, strKey, sValue);
        }

        /// <summary>
        /// Get Float value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="pstValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetFloatValue_NET(String strKey, ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetFloatValue(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// Set float value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="fValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetFloatValue_NET(String strKey, Single fValue)
        {
            return MV_CC_SetFloatValue(handle, strKey, fValue);
        }

        /// <summary>
        /// Get Boolean value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="pbValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetBoolValue_NET(String strKey, ref Boolean pbValue)
        {
            return MV_CC_GetBoolValue(handle, strKey, ref pbValue);
        }

        /// <summary>
        /// Set Boolean value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="bValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetBoolValue_NET(String strKey, Boolean bValue)
        {
            return MV_CC_SetBoolValue(handle, strKey, bValue);
        }

        /// <summary>
        /// Get String value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="pstValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetStringValue_NET(String strKey, ref MVCC_STRINGVALUE pstValue)
        {
            return MV_CC_GetStringValue(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// Set String value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="strValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetStringValue_NET(String strKey, String strValue)
        {
            return MV_CC_SetStringValue(handle, strKey, strValue);
        }

        /// <summary>
        /// Send Command
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetCommandValue_NET(String strKey)
        {
            return MV_CC_SetCommandValue(handle, strKey);
        }

        /// <summary>
        /// Read Memory
        /// </summary>
        /// <param name="pBuffer">Used as a return value, save the read-in memory value(Memory value is stored in accordance with the big end model)</param>
        /// <param name="nAddress">Memory address to be read, which can be obtained from the Camera.xml file of the device, the form xml node value of xxx_RegAddr</param>
        /// <param name="nLength">Length of the memory to be read</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_ReadMemory_NET(IntPtr pBuffer, Int64 nAddress, Int64 nLength)
        {
            return MV_CC_ReadMemory(handle, pBuffer, nAddress, nLength);
        }

        /// <summary>
        /// Write Memory
        /// </summary>
        /// <param name="pBuffer">Memory value to be written ( Note the memory value to be stored in accordance with the big end model)</param>
        /// <param name="nAddress">Memory address to be written, which can be obtained from the Camera.xml file of the device, the form xml node value of xxx_RegAddr</param>
        /// <param name="nLength">Length of the memory to be written</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_WriteMemory_NET(IntPtr pBuffer, Int64 nAddress, Int64 nLength)
        {
            return MV_CC_WriteMemory(handle, pBuffer, nAddress, nLength);
        }

        /// <summary>
        /// Invalidate GenICam Nodes
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_InvalidateNodes_NET()
        {
            return MV_CC_InvalidateNodes(handle);
        }

        /// <summary>
        /// Get camera feature tree XML
        /// </summary>
        /// <param name="pData">XML data receiving buffer</param>
        /// <param name="nDataSize">Buffer size</param>
        /// <param name="pnDataLen">Actual data length</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_XML_GetGenICamXML_NET(IntPtr pData, UInt32 nDataSize, ref UInt32 pnDataLen)
        {
            return MV_XML_GetGenICamXML(handle, pData, nDataSize, ref pnDataLen);
        }

        /// <summary>
        /// Get Access mode of cur node
        /// </summary>
        /// <param name="pstrName">Name of node</param>
        /// <param name="pAccessMode">Access mode of the node</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_XML_GetNodeAccessMode_NET(String pstrName, ref MV_XML_AccessMode pAccessMode)
        {
            return MV_XML_GetNodeAccessMode(handle, pstrName, ref pAccessMode);
        }

        /// <summary>
        /// Get Interface Type of cur node
        /// </summary>
        /// <param name="pstrName">Name of node</param>
        /// <param name="pInterfaceType">Interface Type of the node</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_XML_GetNodeInterfaceType_NET(String pstrName, ref MV_XML_InterfaceType pInterfaceType)
        {
            return MV_XML_GetNodeInterfaceType(handle, pstrName, ref pInterfaceType);
        }

        /// <summary>
        /// Save camera feature
        /// </summary>
        /// <param name="pFileName">File name</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_FeatureSave_NET(String pFileName)
        {
            return MV_CC_FeatureSave(handle, pFileName);
        }

        /// <summary>
        /// Load camera feature
        /// </summary>
        /// <param name="pFileName">File name</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_FeatureLoad_NET(String pFileName)
        {
            return MV_CC_FeatureLoad(handle, pFileName);
        }

        /// <summary>
        /// Read the file from the camera
        /// </summary>
        /// <param name="pstFileAccess">File access structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_FileAccessRead_NET(ref MV_CC_FILE_ACCESS pstFileAccess)
        {
            return MV_CC_FileAccessRead(handle, ref pstFileAccess);
        }

        /// <summary>
        /// Read the file from the camera
        /// </summary>
        /// <param name="pstFileAccessEx">File access structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_FileAccessReadEx_NET(ref MV_CC_FILE_ACCESS_EX pstFileAccessEx)
        {
            return MV_CC_FileAccessReadEx(handle, ref pstFileAccessEx);
        }

        /// <summary>
        /// Write the file to camera
        /// </summary>
        /// <param name="pstFileAccess">File access structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_FileAccessWrite_NET(ref MV_CC_FILE_ACCESS pstFileAccess)
        {
            return MV_CC_FileAccessWrite(handle, ref pstFileAccess);
        }

        /// <summary>
        /// Write the file to camera
        /// </summary>
        /// <param name="pstFileAccessEx">File access structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_FileAccessWriteEx_NET(ref MV_CC_FILE_ACCESS_EX pstFileAccessEx)
        {
            return MV_CC_FileAccessWriteEx(handle, ref pstFileAccessEx);
        }

        /// <summary>
        /// Get File Access Progress 
        /// </summary>
        /// <param name="pstFileAccessProgress">File access Progress</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_GetFileAccessProgress_NET(ref MV_CC_FILE_ACCESS_PROGRESS pstFileAccessProgress)
        {
            return MV_CC_GetFileAccessProgress(handle, ref pstFileAccessProgress);
        }
        #endregion

        #region ch: 设备升级 | en: Device upgrade interface
        /// <summary>
        /// Device Local Upgrade
        /// </summary>
        /// <param name="pFilePathName">File path and name</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_LocalUpgrade_NET(String pFilePathName)
        {
            return MV_CC_LocalUpgrade(handle, pFilePathName);
        }

        /// <summary>
        /// Get Upgrade Progress
        /// </summary>
        /// <param name="pnProcess">Value of Progress</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_GetUpgradeProcess_NET(ref UInt32 pnProcess)
        {
            return MV_CC_GetUpgradeProcess(handle, ref pnProcess);
        }
        #endregion

        #region ch: 注册异常回调和事件接口 | en: Enrol abnormal callbacks and event interface
        /// <summary>
        /// Register Exception Message CallBack, call after open device
        /// </summary>
        /// <param name="cbException">Exception Message CallBack Function</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_RegisterExceptionCallBack_NET(cbExceptiondelegate cbException, IntPtr pUser)
        {
            return MV_CC_RegisterExceptionCallBack(handle, cbException, pUser);
        }

        /// <summary>
        /// Register event callback, which is called after the device is opened
        /// </summary>
        /// <param name="cbEvent">Event CallBack Function</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_RegisterAllEventCallBack_NET(cbEventdelegateEx cbEvent, IntPtr pUser)
        {
            return MV_CC_RegisterAllEventCallBack(handle, cbEvent, pUser);
        }

        /// <summary>
        /// Register single event callback, which is called after the device is opened
        /// </summary>
        /// <param name="pEventName">Event name</param>
        /// <param name="cbEvent">Event CallBack Function</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_RegisterEventCallBackEx_NET(String pEventName, cbEventdelegateEx cbEvent, IntPtr pUser)
        {
            return MV_CC_RegisterEventCallBackEx(handle, pEventName, cbEvent, pUser);
        }
        #endregion

        #region ch: 仅GigE设备支持的接口 | en: Only support GigE device interface
        /// <summary>
        /// Set enumerate device timeout
        /// </summary>
        /// <param name="nMilTimeout">time out,default 100ms</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public static Int32 MV_GIGE_SetEnumDevTimeout_NET(UInt32 nMilTimeout)
        {
            return MV_GIGE_SetEnumDevTimeout(nMilTimeout);
        }

        /// <summary>
        /// Force IP
        /// </summary>
        /// <param name="nIP">IP to set</param>
        /// <param name="nSubNetMask">Subnet mask</param>
        /// <param name="nDefaultGateWay">Default gateway</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_ForceIpEx_NET(UInt32 nIP, UInt32 nSubNetMask, UInt32 nDefaultGateWay)
        {
            return MV_GIGE_ForceIpEx(handle, nIP, nSubNetMask, nDefaultGateWay);
        }

        /// <summary>
        /// IP configuration method
        /// </summary>
        /// <param name="nType">IP type, refer to MV_IP_CFG_x</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetIpConfig_NET(UInt32 nType)
        {
            return MV_GIGE_SetIpConfig(handle, nType);
        }

        /// <summary>
        /// Set to use only one mode,type: MV_NET_TRANS_x. When do not set, priority is to use driver by default
        /// </summary>
        /// <param name="nType">Net transmission mode, refer to MV_NET_TRANS_x</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetNetTransMode_NET(UInt32 nType)
        {
            return MV_GIGE_SetNetTransMode(handle, nType);
        }

        /// <summary>
        /// Get net transmission information
        /// </summary>
        /// <param name="pstInfo">Transmission information</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_GetNetTransInfo_NET(ref MV_NETTRANS_INFO pstInfo)
        {
            return MV_GIGE_GetNetTransInfo(handle, ref pstInfo);
        }

        /// <summary>
        /// Setting the ACK mode of devices Discovery
        /// </summary>
        /// <param name="nMode">ACK mode（Default-Broadcast）,0-Unicast,1-Broadcast</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public static Int32 MV_GIGE_SetDiscoveryMode_NET(UInt32 nMode)
        {
            return MV_GIGE_SetDiscoveryMode(nMode);
        }

        /// <summary>
        /// Set GVSP streaming timeout
        /// </summary>
        /// <param name="nMillisec">Timeout, default 300ms, range: >10ms</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetGvspTimeout_NET(UInt32 nMillisec)
        {
            return MV_GIGE_SetGvspTimeout(handle, nMillisec);
        }

        /// <summary>
        /// Get GVSP streaming timeout
        /// </summary>
        /// <param name="pMillisec">Timeout, ms as unit</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_GetGvspTimeout_NET(ref UInt32 pMillisec)
        {
            return MV_GIGE_GetGvspTimeout(handle, ref pMillisec);
        }

        /// <summary>
        /// Set GVCP cammand timeout
        /// </summary>
        /// <param name="nMillisec">Timeout, ms as unit, range: 0-10000</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetGvcpTimeout_NET(UInt32 nMillisec)
        {
            return MV_GIGE_SetGvcpTimeout(handle, nMillisec);
        }

        /// <summary>
        /// Get GVCP cammand timeout
        /// </summary>
        /// <param name="pMillisec">Timeout, ms as unit</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_GetGvcpTimeout_NET(ref UInt32 pMillisec)
        {
            return MV_GIGE_GetGvcpTimeout(handle, ref pMillisec);
        }

        /// <summary>
        /// Set the number of retry GVCP cammand
        /// </summary>
        /// <param name="nRetryGvcpTimes">The number of retries，rang：0-100</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetRetryGvcpTimes_NET(UInt32 nRetryGvcpTimes)
        {
            return MV_GIGE_SetRetryGvcpTimes(handle, nRetryGvcpTimes);
        }

        /// <summary>
        /// Get the number of retry GVCP cammand
        /// </summary>
        /// <param name="pRetryGvcpTimes">The number of retries</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_GetRetryGvcpTimes_NET(ref UInt32 pRetryGvcpTimes)
        {
            return MV_GIGE_GetRetryGvcpTimes(handle, ref pRetryGvcpTimes);
        }

        /// <summary>
        /// Get the optimal Packet Size, Only support GigE Camera
        /// </summary>
        /// <returns>Optimal packet size</returns>
        public Int32 MV_CC_GetOptimalPacketSize_NET()
        {
            return MV_CC_GetOptimalPacketSize(handle);
        }

        /// <summary>
        /// Set whethe to enable resend, and set resend
        /// </summary>
        /// <param name="bEnable">Enable resend</param>
        /// <param name="nMaxResendPercent">Max resend persent</param>
        /// <param name="nResendTimeout">Resend timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetResend_NET(UInt32 bEnable, UInt32 nMaxResendPercent, UInt32 nResendTimeout)
        {
            return MV_GIGE_SetResend(handle, bEnable, nMaxResendPercent, nResendTimeout);
        }

        /// <summary>
        /// Set the max resend retry times
        /// </summary>
        /// <param name="nRetryTimes">The max times to retry resending lost packets，default 20</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetResendMaxRetryTimes_NET(UInt32 nRetryTimes)
        {
            return MV_GIGE_SetResendMaxRetryTimes(handle, nRetryTimes);
        }

        /// <summary>
        /// Get the max resend retry times
        /// </summary>
        /// <param name="pnRetryTimes">the max times to retry resending lost packets</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_GetResendMaxRetryTimes_NET(ref UInt32 pnRetryTimes)
        {
            return MV_GIGE_GetResendMaxRetryTimes(handle, ref pnRetryTimes);
        }

        /// <summary>
        /// Set time interval between same resend requests
        /// </summary>
        /// <param name="nMillisec">The time interval between same resend requests,default 10ms</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetResendTimeInterval_NET(UInt32 nMillisec)
        {
            return MV_GIGE_SetResendTimeInterval(handle, nMillisec);
        }

        /// <summary>
        /// Get time interval between same resend requests
        /// </summary>
        /// <param name="pnMillisec">The time interval between same resend requests</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_GetResendTimeInterval_NET(ref UInt32 pnMillisec)
        {
            return MV_GIGE_GetResendTimeInterval(handle, ref pnMillisec);
        }

        /// <summary>
        /// Set transmission type,Unicast or Multicast
        /// </summary>
        /// <param name="pstTransmissionType">Struct of transmission type</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_GIGE_SetTransmissionType_NET(ref MV_CC_TRANSMISSION_TYPE pstTransmissionType)
        {
            return MV_GIGE_SetTransmissionType(handle, ref pstTransmissionType);
        }

        /// <summary>
        /// Issue Action Command
        /// </summary>
        /// <param name="pstActionCmdInfo">Action Command info</param>
        /// <param name="pstActionCmdResults">Action Command Result List</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_GIGE_IssueActionCommand_NET(ref MV_ACTION_CMD_INFO pstActionCmdInfo, ref MV_ACTION_CMD_RESULT_LIST pstActionCmdResults)
        {
            return MV_GIGE_IssueActionCommand(ref pstActionCmdInfo, ref pstActionCmdResults);
        }

        /// <summary>
        /// Get Multicast Status
        /// </summary>
        /// <param name="pstDevInfo">Device Information</param>
        /// <param name="pStatus">Status of Multicast</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public static Int32 MV_GIGE_GetMulticastStatus_NET(ref MV_CC_DEVICE_INFO pstDevInfo, ref Boolean pStatus)
        {
            return MV_GIGE_GetMulticastStatus(ref pstDevInfo, ref pStatus);
        }
        #endregion

        #region ch: 仅CameraLink 设备支持的接口 | en: Only support camlink device interface
        /// <summary>
        /// Set device baudrate using one of the CL_BAUDRATE_XXXX value
        /// </summary>
        /// <param name="nBaudrate">Baudrate to set. Refer to the 'CameraParams.h' for parameter definitions, for example, #define MV_CAML_BAUDRATE_9600  0x00000001</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CAML_SetDeviceBauderate_NET(UInt32 nBaudrate)
        {
            return MV_CAML_SetDeviceBaudrate(handle, nBaudrate);
        }

        /// <summary>
        /// Get device baudrate, using one of the CL_BAUDRATE_XXXX value
        /// </summary>
        /// <param name="pnCurrentBaudrate">Return pointer of baud rate to user. 
        ///                                 Refer to the 'CameraParams.h' for parameter definitions, for example, #define MV_CAML_BAUDRATE_9600  0x00000001</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CAML_GetDeviceBauderate_NET(ref UInt32 pnCurrentBaudrate)
        {
            return MV_CAML_GetDeviceBaudrate(handle, ref pnCurrentBaudrate);
        }

        /// <summary>
        /// Get supported baudrates of the combined device and host interface
        /// </summary>
        /// <param name="pnBaudrateAblity">Return pointer of the supported baudrates to user. 'OR' operation results of the supported baudrates. 
        ///                                Refer to the 'CameraParams.h' for single value definitions, for example, #define MV_CAML_BAUDRATE_9600  0x00000001</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CAML_GetSupportBauderates_NET(ref UInt32 pnBaudrateAblity)
        {
            return MV_CAML_GetSupportBaudrates(handle, ref pnBaudrateAblity);
        }

        /// <summary>
        /// Sets the timeout for operations on the serial port
        /// </summary>
        /// <param name="nMillisec">Timeout in [ms] for operations on the serial port.</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CAML_SetGenCPTimeOut_NET(UInt32 nMillisec)
        {
            return MV_CAML_SetGenCPTimeOut(handle, nMillisec);
        }
        #endregion

        #region ch: 仅U3V设备支持的接口 | en: Only support U3V device interface
        /// <summary>
        /// Set transfer size of U3V device
        /// </summary>
        /// <param name="nTransferSize">Transfer size，Byte，default：1M，rang：>=0x10000</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_USB_SetTransferSize_NET(UInt32 nTransferSize)
        {
            return MV_USB_SetTransferSize(handle, nTransferSize);
        }

        /// <summary>
        /// Get transfer size of U3V device
        /// </summary>
        /// <param name="pTransferSize">Transfer size，Byte</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_USB_GetTransferSize_NET(ref UInt32 pTransferSize)
        {
            return MV_USB_GetTransferSize(handle, ref pTransferSize);
        }

        /// <summary>
        /// Set transfer ways of U3V device
        /// </summary>
        /// <param name="nTransferWays">Transfer ways，rang：1-10</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_USB_SetTransferWays_NET(UInt32 nTransferWays)
        {
            return MV_USB_SetTransferWays(handle, nTransferWays);
        }

        /// <summary>
        /// Get transfer ways of U3V device
        /// </summary>
        /// <param name="pTransferWays">Transfer ways</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_USB_GetTransferWays_NET(ref UInt32 pTransferWays)
        {
            return MV_USB_GetTransferWays(handle, ref pTransferWays);
        }

        /// <summary>
        /// Register Stream Exception Message CallBack
        /// </summary>
        /// <param name="cbException">Stream Exception Message CallBack Function</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_USB_RegisterStreamExceptionCallBack_NET(cbStreamException cbException, IntPtr pUser)
        {
            return MV_USB_RegisterStreamExceptionCallBack(handle, cbException, pUser);
        }

        /// <summary>
        /// Set the number of U3V device event cache nodes
        /// </summary>
        /// <param name="nEventNodeNum">Event Node Number</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_USB_SetEventNodeNum_NET(UInt32 nEventNodeNum)
        {
            return MV_USB_SetEventNodeNum(handle, nEventNodeNum);
        }

        /// <summary>
        /// Set U3V Camera Synchronisation timeout
        /// </summary>
        /// <param name="nMills">Synchronisation time(ms), default 1000ms</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_USB_SetSyncTimeOut_NET(UInt32 nMills)
        {
            return MV_USB_SetSyncTimeOut(handle, nMills);
        }

        /// <summary>
        /// Get U3V Camera Synchronisation timeout
        /// </summary>
        /// <param name="pnMills">Synchronisation time(ms), default 1000ms</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_USB_GetSyncTimeOut_NET(ref UInt32 pnMills)
        {
            return MV_USB_GetSyncTimeOut(handle, ref pnMills);
        }
        #endregion

        #region ch: GenTL相关接口 | en: GenTL related interface
        /// <summary>
        /// Enumerate interfaces by GenTL
        /// </summary>
        /// <param name="stIFInfoList"> Interface information list</param>
        /// <param name="pGenTLPath">Path of GenTL's cti file</param>
        /// <returns></returns>
        public static Int32 MV_CC_EnumInterfacesByGenTL_NET(ref MV_GENTL_IF_INFO_LIST stIFInfoList, String pGenTLPath)
        {
            return MV_CC_EnumInterfacesByGenTL(ref stIFInfoList, pGenTLPath);
        }

        /// <summary>
        /// Unload cti library
        /// </summary>
        /// <param name="strGenTLPath">GenTL cti file path</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public static Int32 MV_CC_UnloadGenTLLibrary_NET(String strGenTLPath)
        {
            return MV_CC_UnloadGenTLLibrary(strGenTLPath);
        }

        /// <summary>
        /// Enumerate Device Based On GenTL
        /// </summary>
        /// <param name="stIFInfo">Interface information</param>
        /// <param name="stDevList">Device List</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public static Int32 MV_CC_EnumDevicesByGenTL_NET(ref MV_GENTL_IF_INFO stIFInfo, ref MV_GENTL_DEV_INFO_LIST stDevList)
        {
            return MV_CC_EnumDevicesByGenTL(ref stIFInfo, ref stDevList);
        }

        /// <summary>
        /// Create Device Handle Based On GenTL Device Info
        /// </summary>
        /// <param name="stDevInfo">Device Information Structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_CreateDeviceByGenTL_NET(ref MV_GENTL_DEV_INFO stDevInfo)
        {
            if (IntPtr.Zero != handle)
            {
                MV_CC_DestroyHandle(handle);
                handle = IntPtr.Zero;
            }

            return MV_CC_CreateHandleByGenTL(ref handle, ref stDevInfo);
        }
        #endregion

        #region ch: 图像保存、格式转换等相关接口 | en: Related image save and format convert interface
        /// <summary>
        /// Save image, support Bmp and Jpeg.
        /// </summary>
        /// <param name="stSaveParam">Save image parameters structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_SaveImageEx3_NET(ref MV_SAVE_IMAGE_PARAM_EX3 stSaveParam)
        {
            return MV_CC_SaveImageEx3(handle, ref stSaveParam);
        }

        /// <summary>
        /// Save the image file, support Bmp、 Jpeg、Png and Tiff. Encoding quality(50-99]
        /// </summary>
        /// <param name="pstSaveFileParam">Save the image file parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_SaveImageToFileEx_NET(ref MV_SAVE_IMG_TO_FILE_PARAM_EX pstSaveFileParam)
        {
            return MV_CC_SaveImageToFileEx(handle, ref pstSaveFileParam);
        }

        /// <summary>
        /// Save 3D point data, support PLY、CSV and OBJ
        /// </summary>
        /// <param name="pstPointDataParam">Save 3D point data parameters structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SavePointCloudData_NET(ref MV_SAVE_POINT_CLOUD_PARAM pstPointDataParam)
        {
            return MV_CC_SavePointCloudData(handle, ref pstPointDataParam);
        }

        /// <summary>
        /// Rotate Image
        /// </summary>
        /// <param name="pstRotateParam">Rotate image parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_RotateImage_NET(ref MV_CC_ROTATE_IMAGE_PARAM pstRotateParam)
        {
            return MV_CC_RotateImage(handle, ref pstRotateParam);
        }

        /// <summary>
        /// Flip Image
        /// </summary>
        /// <param name="pstFlipParam">Flip image parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_FlipImage_NET(ref MV_CC_FLIP_IMAGE_PARAM pstFlipParam)
        {
            return MV_CC_FlipImage(handle, ref pstFlipParam);
        }

        /// <summary>
        /// Pixel format conversion
        /// </summary>
        /// <param name="pstCvtParam">Convert Pixel Type parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_ConvertPixelTypeEx_NET(ref MV_CC_PIXEL_CONVERT_PARAM_EX pstCvtParam)
        {
            return MV_CC_ConvertPixelTypeEx(handle, ref pstCvtParam);
        }

        /// <summary>
        /// Interpolation algorithm type setting
        /// </summary>
        /// <param name="BayerCvtQuality">Bayer interpolation method  0-Fast 1-Equilibrium 2-Optimal</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_SetBayerCvtQuality_NET(UInt32 BayerCvtQuality)
        {
            return MV_CC_SetBayerCvtQuality(handle, BayerCvtQuality);
        }

        /// <summary>
        /// Filter type of the bell interpolation quality algorithm setting
        /// </summary>
        /// <param name="bFilterEnable">Filter type enable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetBayerFilterEnable_NET(Boolean bFilterEnable)
        {
            return MV_CC_SetBayerFilterEnable(handle, bFilterEnable);
        }

        /// <summary>
        /// Set Bayer Gamma value
        /// </summary>
        /// <param name="fBayerGammaValue">Gamma value[0.1,4.0]</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_SetBayerGammaValue_NET(Single fBayerGammaValue)
        {
            return MV_CC_SetBayerGammaValue(handle, fBayerGammaValue);
        }

        /// <summary>
        /// Set Mono8/Bayer Gamma value
        /// </summary>
        /// <param name="enPixelType">PixelType</param>
        /// <param name="fGammaValue">Gamma value[0.1,4.0]</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetGammaValue_NET(MvGvspPixelType enPixelType, Single fGammaValue)
        {
            return MV_CC_SetGammaValue(handle, enPixelType, fGammaValue);
        }

        /// <summary>
        /// Set Gamma param
        /// </summary>
        /// <param name="pstGammaParam">Gamma parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetBayerGammaParam_NET(ref MV_CC_GAMMA_PARAM pstGammaParam)
        {
            return MV_CC_SetBayerGammaParam(handle, ref pstGammaParam);
        }

        /// <summary>
        /// Set CCM param
        /// </summary>
        /// <param name="pstCCMParam">CCM parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetBayerCCMParam_NET(ref MV_CC_CCM_PARAM pstCCMParam)
        {
            return MV_CC_SetBayerCCMParam(handle, ref pstCCMParam);
        }

        /// <summary>
        /// Set CCM param
        /// </summary>
        /// <param name="pstCCMParam">CCM parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetBayerCCMParamEx_NET(ref MV_CC_CCM_PARAM_EX pstCCMParam)
        {
            return MV_CC_SetBayerCCMParamEx(handle, ref pstCCMParam);
        }

        /// <summary>
        /// Adjust image contrast
        /// </summary>
        /// <param name="pstContrastParam">Contrast parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_ImageContrast_NET(ref MV_CC_CONTRAST_PARAM pstContrastParam)
        {
            return MV_CC_ImageContrast(handle, ref pstContrastParam);
        }

        /// <summary>
        /// High Bandwidth Decode
        /// </summary>
        /// <param name="pstDecodeParam">High Bandwidth Decode parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_HB_Decode_NET(ref MV_CC_HB_DECODE_PARAM pstDecodeParam)
        {
            return MV_CC_HB_Decode(handle, ref pstDecodeParam);
        }

        /// <summary>
        /// Draw Rect Auxiliary Line
        /// </summary>
        /// <param name="pstRectInfo">Rect Auxiliary Line Info</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_DrawRect_NET(ref MVCC_RECT_INFO pstRectInfo)
        {
            return MV_CC_DrawRect(handle, ref pstRectInfo);
        }

        /// <summary>
        /// Draw Circle Auxiliary Line
        /// </summary>
        /// <param name="pstCircleInfo">Circle Auxiliary Line Info</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_DrawCircle_NET(ref MVCC_CIRCLE_INFO pstCircleInfo)
        {
            return MV_CC_DrawCircle(handle, ref pstCircleInfo);
        }

        /// <summary>
        /// Draw Line Auxiliary Line
        /// </summary>
        /// <param name="pstLinesInfo">Linear Auxiliary Line Info</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_DrawLines_NET(ref MVCC_LINES_INFO pstLinesInfo)
        {
            return MV_CC_DrawLines(handle, ref pstLinesInfo);
        }

        /// <summary>
        /// Start Record
        /// </summary>
        /// <param name="pstRecordParam">Record param structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_StartRecord_NET(ref MV_CC_RECORD_PARAM pstRecordParam)
        {
            return MV_CC_StartRecord(handle, ref pstRecordParam);
        }

        /// <summary>
        /// Input RAW data to Record
        /// </summary>
        /// <param name="pstInputFrameInfo">Record data structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_InputOneFrame_NET(ref MV_CC_INPUT_FRAME_INFO pstInputFrameInfo)
        {
            return MV_CC_InputOneFrame(handle, ref pstInputFrameInfo);
        }

        /// <summary>
        /// Stop Record
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_StopRecord_NET()
        {
            return MV_CC_StopRecord(handle);
        }

        /// <summary>
        /// Open the GUI interface for getting or setting camera parameters
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_OpenParamsGUI_NET()
        {
            return MV_CC_OpenParamsGUI(handle);
        }

        /// <summary>
        /// Reconstruct Image(For time-division exposure function)
        /// </summary>
        /// <param name="pstReconstructParam">Reconstruct image parameters</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_ReconstructImage_NET(ref MV_RECONSTRUCT_IMAGE_PARAM pstReconstructParam)
        {
            return MV_CC_ReconstructImage(handle, ref pstReconstructParam);
        }
        #endregion

        #region ch: 内部使用的公共功能接口 | en: Inner Interface
        /// <summary>
        /// Byte array to struct
        /// </summary>
        /// <param name="bytes">Byte array</param>
        /// <param name="type">Struct type</param>
        /// <returns>Struct object</returns>
        public static object ByteToStruct(Byte[] bytes, Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return null;
            }

            // 分配结构体内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);

            // 将byte数组拷贝到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);

            // 将内存空间转换为目标结构体
            object obj = Marshal.PtrToStructure(structPtr, type);

            // 释放内存空间
            Marshal.FreeHGlobal(structPtr);

            return obj;
        }

        /// <summary>
        /// 判断字符数组是否为utf-8
        /// </summary>
        /// <param name="inputStream">字符数组</param>
        /// <returns></returns>
        public static bool IsTextUTF8(byte[] inputStream)
        {
            int encodingBytesCount = 0;
            bool allTextsAreASCIIChars = true;

            for (int i = 0; i < inputStream.Length; i++)
            {
                byte current = inputStream[i];

                if ((current & 0x80) == 0x80)
                {
                    allTextsAreASCIIChars = false;
                }
                // First byte
                if (encodingBytesCount == 0)
                {
                    if ((current & 0x80) == 0)
                    {
                        // ASCII chars, from 0x00-0x7F
                        continue;
                    }

                    if ((current & 0xC0) == 0xC0)
                    {
                        encodingBytesCount = 1;
                        current <<= 2;

                        // More than two bytes used to encoding a unicode char.
                        // Calculate the real length.
                        while ((current & 0x80) == 0x80)
                        {
                            current <<= 1;
                            encodingBytesCount++;
                        }
                    }
                    else
                    {
                        // Invalid bits structure for UTF8 encoding rule.
                        return false;
                    }
                }
                else
                {
                    // Following bytes, must start with 10.
                    if ((current & 0xC0) == 0x80)
                    {
                        encodingBytesCount--;
                    }
                    else
                    {
                        // Invalid bits structure for UTF8 encoding rule.
                        return false;
                    }
                }
            }

            if (encodingBytesCount != 0)
            {
                // Invalid bits structure for UTF8 encoding rule.
                // Wrong following bytes count.
                return false;
            }

            // Although UTF8 supports encoding for ASCII chars, we regard as a input stream, whose contents are all ASCII as default encoding.
            return !allTextsAreASCIIChars;
        }

        /// <summary>
        /// Write Error Message
        /// </summary>
        /// <param name="csMessage">Message</param>
        /// <param name="nErrorNum">ErrorNum</param>
        public static void WriteErrorMsg(string csMessage, int nErrorNum)
        {
            string errorMsg;
            if (nErrorNum == 0)
            {
                errorMsg = csMessage;
            }
            else
            {
                errorMsg = csMessage + ": Error =" + String.Format("{0:X}", nErrorNum);
            }

            //TextWriterTraceListener TraceListener = new System.Diagnostics.TextWriterTraceListener(@"debug.txt");
            //Debug.Listeners.Add(TraceListener);
            //Debug.WriteLine(System.DateTime.Now.ToString());
            //Debug.WriteLine(errorMsg);
            //TraceListener.Flush();
            Debug.WriteLine(errorMsg);
        }
        #endregion

        #region ch: 弃用的接口 | en: Abandoned interface

        /// <summary>
        /// Save image, support Bmp and Jpeg.
        /// </summary>
        /// <param name="stSaveParam">Save image parameters structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_SaveImageEx2_NET(ref MV_SAVE_IMAGE_PARAM_EX2 stSaveParam)
        {
            return MV_CC_SaveImageEx2(handle, ref stSaveParam);
        }

        /// <summary>
        /// Save the image file, support Bmp、 Jpeg、Png and Tiff. Encoding quality(50-99]
        /// </summary>
        /// <param name="pstSaveFileParam">Save the image file parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        public Int32 MV_CC_SaveImageToFile_NET(ref MV_SAVE_IMG_TO_FILE_PARAM pstSaveFileParam)
        {
            return MV_CC_SaveImageToFile(handle, ref pstSaveFileParam);
        }

        /// <summary>
        /// Pixel format conversion
        /// </summary>
        /// <param name="pstCvtParam">Convert Pixel Type parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_ConvertPixelType_NET(ref MV_PIXEL_CONVERT_PARAM pstCvtParam)
        {
            return MV_CC_ConvertPixelType(handle, ref pstCvtParam);
        }
        /// <summary>
        /// Get basic information of image (Interfaces not recommended)
        /// </summary>
        /// <param name="pstInfo"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetImageInfo_NET(ref MV_IMAGE_BASIC_INFO pstInfo)
        {
            return MV_CC_GetImageInfo(handle, ref pstInfo);
        }

        /// <summary>
        /// Get GenICam proxy (Interfaces not recommended)
        /// </summary>
        /// <returns></returns>
        public IntPtr MV_CC_GetTlProxy_NET()
        {
            return MV_CC_GetTlProxy(handle);
        }

        /// <summary>
        /// Get root node (Interfaces not recommended)
        /// </summary>
        /// <param name="pstNode"></param>
        /// <returns></returns>
        public Int32 MV_XML_GetRootNode_NET(ref MV_XML_NODE_FEATURE pstNode)
        {
            return MV_XML_GetRootNode(handle, ref pstNode);
        }

        /// <summary>
        /// Get all children node of specific node from xml, root node is Root (Interfaces not recommended)
        /// </summary>
        /// <param name="pstNode"></param>
        /// <param name="pstNodesList"></param>
        /// <returns></returns>
        public Int32 MV_XML_GetChildren_NET(ref MV_XML_NODE_FEATURE pstNode, IntPtr pstNodesList)
        {
            return MV_XML_GetChildren(handle, ref pstNode, pstNodesList);
        }

        /// <summary>
        /// Get all children node of specific node from xml, root node is Root (Interfaces not recommended)
        /// </summary>
        /// <param name="pstNode"></param>
        /// <param name="pstNodesList"></param>
        /// <returns></returns>
        public Int32 MV_XML_GetChildren_NET(ref MV_XML_NODE_FEATURE pstNode, ref MV_XML_NODES_LIST pstNodesList)
        {
            return MV_XML_GetChildren(handle, ref pstNode, ref pstNodesList);
        }

        /// <summary>
        /// Get current node feature (Interfaces not recommended)
        /// </summary>
        /// <param name="pstNode"></param>
        /// <param name="pstFeature"></param>
        /// <returns></returns>
        public Int32 MV_XML_GetNodeFeature_NET(ref MV_XML_NODE_FEATURE pstNode, IntPtr pstFeature)
        {
            return MV_XML_GetNodeFeature(handle, ref pstNode, pstFeature);
        }

        /// <summary>
        /// Update node (Interfaces not recommended)
        /// </summary>
        /// <param name="enType"></param>
        /// <param name="pstFeature"></param>
        /// <returns></returns>
        public Int32 MV_XML_UpdateNodeFeature_NET(MV_XML_InterfaceType enType, IntPtr pstFeature)
        {
            return MV_XML_UpdateNodeFeature(handle, enType, pstFeature);
        }

        /// <summary>
        /// Register update callback (Interfaces not recommended)
        /// </summary>
        /// <param name="cbXmlUpdate"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        public Int32 MV_XML_RegisterUpdateCallBack_NET(cbXmlUpdatedelegate cbXmlUpdate, IntPtr pUser)
        {
            return MV_XML_RegisterUpdateCallBack(handle, cbXmlUpdate, pUser);
        }

        /// <summary>
        /// Noise estimate of Bayer format
        /// </summary>
        /// <param name="pstNoiseEstimateParam">Noise estimate parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_BayerNoiseEstimate_NET(ref MV_CC_BAYER_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam)
        {
            return MV_CC_BayerNoiseEstimate(handle, ref pstNoiseEstimateParam);
        }

        /// <summary>
        /// Spatial Denoise of Bayer format
        /// </summary>
        /// <param name="pstSpatialDenoiseParam">Spatial Denoise parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_BayerSpatialDenoise_NET(ref MV_CC_BAYER_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam)
        {
            return MV_CC_BayerSpatialDenoise(handle, ref pstSpatialDenoiseParam);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_DisplayOneFrame
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public Int32 MV_CC_Display_NET(IntPtr hWnd)
        {
            return MV_CC_Display(handle, hWnd);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_GetOneFrameTimeOut
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="nDataSize"></param>
        /// <param name="pFrameInfo"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetOneFrame_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO pFrameInfo)
        {
            return MV_CC_GetOneFrame(handle, pData, nDataSize, ref pFrameInfo);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_GetOneFrameTimeOut
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="nDataSize"></param>
        /// <param name="pFrameInfo"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetOneFrameEx_NET(IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo)
        {
            return MV_CC_GetOneFrameEx(handle, pData, nDataSize, ref pFrameInfo);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_SaveImageEx
        /// </summary>
        /// <param name="stSaveParam"></param>
        /// <returns></returns>
        public Int32 MV_CC_SaveImage_NET(ref MV_SAVE_IMAGE_PARAM stSaveParam)
        {
            return MV_CC_SaveImage(ref stSaveParam);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_GIGE_ForceIpEx
        /// </summary>
        /// <param name="nIP"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_ForceIp_NET(UInt32 nIP)
        {
            return MV_GIGE_ForceIp(handle, nIP);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_RegisterEventCallBackEx
        /// </summary>
        /// <param name="cbEvent"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        public Int32 MV_CC_RegisterEventCallBack_NET(cbEventdelegate cbEvent, IntPtr pUser)
        {
            return MV_CC_RegisterEventCallBack(handle, cbEvent, pUser);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_GetIntValueEx
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetIntValue_NET(String strKey, ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetIntValue(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_SetIntValueEx
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetIntValue_NET(String strKey, UInt32 nValue)
        {
            return MV_CC_SetIntValue(handle, strKey, nValue);
        }

        /// <summary>
        /// Set CLUT param
        /// </summary>
        /// <param name="pstCLUTParam">CLUT parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SetBayerCLUTParam_NET(ref MV_CC_CLUT_PARAM pstCLUTParam)
        {
            return MV_CC_SetBayerCLUTParam(handle, ref pstCLUTParam);
        }

        /// <summary>
        /// Image sharpen
        /// </summary>
        /// <param name="pstSharpenParam">Sharpen parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_ImageSharpen_NET(ref MV_CC_SHARPEN_PARAM pstSharpenParam)
        {
            return MV_CC_ImageSharpen(handle, ref pstSharpenParam);
        }

        /// <summary>
        /// Color Correct(include CCM and CLUT)
        /// </summary>
        /// <param name="pstColorCorrectParam">Color Correct parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_ColorCorrect_NET(ref MV_CC_COLOR_CORRECT_PARAM pstColorCorrectParam)
        {
            return MV_CC_ColorCorrect(handle, ref pstColorCorrectParam);
        }

        /// <summary>
        /// Noise Estimate
        /// </summary>
        /// <param name="pstNoiseEstimateParam">Noise Estimate parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_NoiseEstimate_NET(ref MV_CC_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam)
        {
            return MV_CC_NoiseEstimate(handle, ref pstNoiseEstimateParam);
        }

        /// <summary>
        /// Spatial Denoise
        /// </summary>
        /// <param name="pstSpatialDenoiseParam">Spatial Denoise parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_SpatialDenoise_NET(ref MV_CC_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam)
        {
            return MV_CC_SpatialDenoise(handle, ref pstSpatialDenoiseParam);
        }

        /// <summary>
        /// LSC Calib
        /// </summary>
        /// <param name="pstLSCCalibParam">LSC Calib parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_LSCCalib_NET(ref MV_CC_LSC_CALIB_PARAM pstLSCCalibParam)
        {
            return MV_CC_LSCCalib(handle, ref pstLSCCalibParam);
        }

        /// <summary>
        /// LSC Correct
        /// </summary>
        /// <param name="pstLSCCorrectParam">LSC Correct parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        public Int32 MV_CC_LSCCorrect_NET(ref MV_CC_LSC_CORRECT_PARAM pstLSCCorrectParam)
        {
            return MV_CC_LSCCorrect(handle, ref pstLSCCorrectParam);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetWidth_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetWidth(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetWidth_NET(UInt32 nValue)
        {
            return MV_CC_SetWidth(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetHeight_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetHeight(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetHeight_NET(UInt32 nValue)
        {
            return MV_CC_SetHeight(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetAOIoffsetX_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAOIoffsetX(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetAOIoffsetX_NET(UInt32 nValue)
        {
            return MV_CC_SetAOIoffsetX(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetAOIoffsetY_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAOIoffsetY(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetAOIoffsetY_NET(UInt32 nValue)
        {
            return MV_CC_SetAOIoffsetY(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetAutoExposureTimeLower_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAutoExposureTimeLower(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetAutoExposureTimeLower_NET(UInt32 nValue)
        {
            return MV_CC_SetAutoExposureTimeLower(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetAutoExposureTimeUpper_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAutoExposureTimeUpper(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetAutoExposureTimeUpper_NET(UInt32 nValue)
        {
            return MV_CC_SetAutoExposureTimeUpper(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetBrightness_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBrightness(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetBrightness_NET(UInt32 nValue)
        {
            return MV_CC_SetBrightness(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetFrameRate_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetFrameRate(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetFrameRate_NET(Single fValue)
        {
            return MV_CC_SetFrameRate(handle, fValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetGain_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetGain(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetGain_NET(Single fValue)
        {
            return MV_CC_SetGain(handle, fValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetExposureTime_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetExposureTime(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetExposureTime_NET(Single fValue)
        {
            return MV_CC_SetExposureTime(handle, fValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetPixelFormat_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetPixelFormat(handle, ref pstValue);
        }

        /// <summary>
        /// Set PixelFormat
        /// </summary>
        /// <param name="nValue">PixelFormat</param>
        /// <returns></returns>
        public Int32 MV_CC_SetPixelFormat_NET(UInt32 nValue)
        {
            return MV_CC_SetPixelFormat(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetAcquisitionMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetAcquisitionMode(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetAcquisitionMode_NET(UInt32 nValue)
        {
            return MV_CC_SetAcquisitionMode(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetGainMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetGainMode(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetGainMode_NET(UInt32 nValue)
        {
            return MV_CC_SetGainMode(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetExposureAutoMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetExposureAutoMode(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetExposureAutoMode_NET(UInt32 nValue)
        {
            return MV_CC_SetExposureAutoMode(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetTriggerMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetTriggerMode(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetTriggerMode_NET(UInt32 nValue)
        {
            return MV_CC_SetTriggerMode(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetTriggerDelay_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetTriggerDelay(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetTriggerDelay_NET(Single fValue)
        {
            return MV_CC_SetTriggerDelay(handle, fValue);
        }

        /// <summary>
        /// Get Trigger Source
        /// </summary>
        /// <param name="pstValue">Trigger Source</param>
        /// <returns></returns>
        public Int32 MV_CC_GetTriggerSource_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetTriggerSource(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetTriggerSource_NET(UInt32 nValue)
        {
            return MV_CC_SetTriggerSource(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <returns></returns>
        public Int32 MV_CC_TriggerSoftwareExecute_NET()
        {
            return MV_CC_TriggerSoftwareExecute(handle);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetGammaSelector_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetGammaSelector(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetGammaSelector_NET(UInt32 nValue)
        {
            return MV_CC_SetGammaSelector(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetGamma_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetGamma(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetGamma_NET(Single fValue)
        {
            return MV_CC_SetGamma(handle, fValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetSharpness_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetSharpness(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetSharpness_NET(UInt32 nValue)
        {
            return MV_CC_SetSharpness(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetHue_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetHue(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetHue_NET(UInt32 nValue)
        {
            return MV_CC_SetHue(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetSaturation_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetSaturation(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetSaturation_NET(UInt32 nValue)
        {
            return MV_CC_SetSaturation(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetBalanceWhiteAuto_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetBalanceWhiteAuto(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetBalanceWhiteAuto_NET(UInt32 nValue)
        {
            return MV_CC_SetBalanceWhiteAuto(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetBalanceRatioRed_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBalanceRatioRed(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetBalanceRatioRed_NET(UInt32 nValue)
        {
            return MV_CC_SetBalanceRatioRed(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetBalanceRatioGreen_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBalanceRatioGreen(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetBalanceRatioGreen_NET(UInt32 nValue)
        {
            return MV_CC_SetBalanceRatioGreen(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetBalanceRatioBlue_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBalanceRatioBlue(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetBalanceRatioBlue_NET(UInt32 nValue)
        {
            return MV_CC_SetBalanceRatioBlue(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetDeviceUserID_NET(ref MVCC_STRINGVALUE pstValue)
        {
            return MV_CC_GetDeviceUserID(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="chValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetDeviceUserID_NET(string chValue)
        {
            return MV_CC_SetDeviceUserID(handle, chValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetBurstFrameCount_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBurstFrameCount(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetBurstFrameCount_NET(UInt32 nValue)
        {
            return MV_CC_SetBurstFrameCount(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetAcquisitionLineRate_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAcquisitionLineRate(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetAcquisitionLineRate_NET(UInt32 nValue)
        {
            return MV_CC_SetAcquisitionLineRate(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_GetHeartBeatTimeout_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetHeartBeatTimeout(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_CC_SetHeartBeatTimeout_NET(UInt32 nValue)
        {
            return MV_CC_SetHeartBeatTimeout(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_GetGevSCPSPacketSize_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_GIGE_GetGevSCPSPacketSize(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_SetGevSCPSPacketSize_NET(UInt32 nValue)
        {
            return MV_GIGE_SetGevSCPSPacketSize(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_GetGevSCPD_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_GIGE_GetGevSCPD(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_SetGevSCPD_NET(UInt32 nValue)
        {
            return MV_GIGE_SetGevSCPD(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pnIP"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_GetGevSCDA_NET(ref UInt32 pnIP)
        {
            return MV_GIGE_GetGevSCDA(handle, ref pnIP);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nIP"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_SetGevSCDA_NET(UInt32 nIP)
        {
            return MV_GIGE_SetGevSCDA(handle, nIP);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pnPort"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_GetGevSCSP_NET(ref UInt32 pnPort)
        {
            return MV_GIGE_GetGevSCSP(handle, ref pnPort);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        public Int32 MV_GIGE_SetGevSCSP_NET(UInt32 nPort)
        {
            return MV_GIGE_SetGevSCSP(handle, nPort);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_RegisterImageCallBackEx
        /// </summary>
        /// <param name="cbOutput"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        public Int32 MV_CC_RegisterImageCallBack_NET(cbOutputdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBack(handle, cbOutput, pUser);
        }
        #endregion

        #region ch: 参数定义 | en: Paramera Define

        #region ch采集卡类型 |en Interface type
        /// <summary>ch GigE Vision采集卡 |en GigE Vision interface</summary>
        public const Int32 MV_GIGE_INTERFACE = unchecked((Int32)0x00000001);
        /// <summary>ch Camera Link采集卡 |en Camera Link interface</summary>
        public const Int32 MV_CAMERALINK_INTERFACE = unchecked((Int32)0x00000004);
        /// <summary>ch CoaXPress采集卡 |en CoaXPress interface</summary>
        public const Int32 MV_CXP_INTERFACE = unchecked((Int32)0x00000008);
        /// <summary>ch XoFLink采集卡 |en XoFLink interface</summary>
        public const Int32 MV_XOF_INTERFACE = unchecked((Int32)0x00000010);
        #endregion

        #region 设备类型定义
        /// <summary>Unknown Device Type, Reserved</summary>
        public const Int32 MV_UNKNOW_DEVICE = unchecked((Int32)0x00000000);
        /// <summary>GigE Device</summary>
        public const Int32 MV_GIGE_DEVICE = unchecked((Int32)0x00000001);
        /// <summary>1394-a/b Device</summary>
        public const Int32 MV_1394_DEVICE = unchecked((Int32)0x00000002);
        /// <summary>USB3.0 Device</summary>
        public const Int32 MV_USB_DEVICE = unchecked((Int32)0x00000004);
        /// <summary>CameraLink Device</summary>
        public const Int32 MV_CAMERALINK_DEVICE = unchecked((Int32)0x00000008);
        /// <summary>Virtual GigE Device</summary>
        public const Int32 MV_VIR_GIGE_DEVICE = unchecked((Int32)0x00000010);
        /// <summary>Virtual USB Device</summary>
        public const Int32 MV_VIR_USB_DEVICE = unchecked((Int32)0x00000020);
        /// <summary>GenTL GigE Device</summary>
        public const Int32 MV_GENTL_GIGE_DEVICE = unchecked((Int32)0x00000040);
        /// <summary>GenTL CML Device</summary>
        public const Int32 MV_GENTL_CAMERALINK_DEVICE = unchecked((Int32)0x00000080);
        /// <summary>GenTL CXP Device</summary>
        public const Int32 MV_GENTL_CXP_DEVICE = unchecked((Int32)0x00000100);
        /// <summary>GenTL XOF Device</summary>
        public const Int32 MV_GENTL_XOF_DEVICE = unchecked((Int32)0x00000200);

        /// <summary>
        /// ch:信息结构体的最大缓存 | en: Max buffer size of information structs
        /// </summary>
        public const Int32 INFO_MAX_BUFFER_SIZE = 64;

        /// <summary>
        /// 最大的相机数量
        /// </summary>
        public const Int32 MV_MAX_DEVICE_NUM = 256;

        /// <summary>
        /// ch:最大Interface数量 | en:Max num of interfaces
        /// </summary>
        public const Int32 MV_MAX_GENTL_IF_NUM = 256;

        /// <summary>
        /// ch:最大GenTL设备数量 | en:Max num of GenTL devices
        /// </summary>
        public const Int32 MV_MAX_GENTL_DEV_NUM = 256;

        /// <summary>
        /// XML节点描述最大长度
        /// </summary>
        public const Int32 MV_MAX_XML_DISC_STRLEN_C = 512;

        /// <summary>
        /// XML节点最大长度
        /// </summary>
        public const Int32 MV_MAX_XML_NODE_STRLEN_C = 64;

        /// <summary>
        /// XML节点最大数量
        /// </summary>
        public const Int32 MV_MAX_XML_NODE_NUM_C = 128;

        /// <summary>
        /// XML节点显示名最大数量
        /// </summary>
        public const Int32 MV_MAX_XML_SYMBOLIC_NUM = 64;

        /// <summary>
        /// string类型节点值的最大长度
        /// </summary>
        public const Int32 MV_MAX_XML_STRVALUE_STRLEN_C = 64;

        /// <summary>
        /// 最大父节点数
        /// </summary>
        public const Int32 MV_MAX_XML_PARENTS_NUM = 8;

        /// <summary>
        /// 最大节点描述长度
        /// </summary>
        public const Int32 MV_MAX_XML_SYMBOLIC_STRLEN_C = 64;

        //  异常消息类型
        /// <summary>
        /// 设备断开连接
        /// </summary>
        public const Int32 MV_EXCEPTION_DEV_DISCONNECT = 0x00008001;

        /// <summary>
        /// SDK与驱动版本不匹配
        /// </summary>
        public const Int32 MV_EXCEPTION_VERSION_CHECK = 0x00008002;

        //Event事件回调信息
        /// <summary>
        /// 相机Event事件名称最大长度
        /// </summary>
        public const Int32 MAX_EVENT_NAME_SIZE = 128;

        /// <summary>最大枚举条目对应的符号长度</summary>
        public const Int32 MV_MAX_SYMBOLIC_LEN = 64;

        /// <summary>分时曝光时最多将源图像拆分的个数</summary>
        public const Int32 MV_MAX_SPLIT_NUM = 8;

        /// <summary>
        /// ch:最大支持的采集卡数量 | en:The maximum number of Frame Grabber interface supported
        /// </summary>
        public const Int32 MV_MAX_INTERFACE_NUM = 64;

        // ch GigEVision IP配置 |en GigEVision IP Configuration
        /// <summary>
        /// ch 静态 |en Static
        /// </summary>
        public const Int32 MV_IP_CFG_STATIC = 0x05000000;
        /// <summary>
        /// ch DHCP |en DHCP
        /// </summary>
        public const Int32 MV_IP_CFG_DHCP = 0x06000000;
        /// <summary>
        /// ch LLA  |en LLA
        /// </summary>
        public const Int32 MV_IP_CFG_LLA = 0x04000000;


        // ch CameraLink波特率 |en CameraLink Baud Rates (CLUINT32)
        /// <summary>
        /// 9600
        /// </summary>
        public const Int32 MV_CAML_BAUDRATE_9600 = 0x00000001;
        /// <summary>
        /// 19200
        /// </summary>
        public const Int32 MV_CAML_BAUDRATE_19200 = 0x00000002;
        /// <summary>
        /// 38400
        /// </summary>
        public const Int32 MV_CAML_BAUDRATE_38400 = 0x00000004;
        /// <summary>
        /// 57600
        /// </summary>
        public const Int32 MV_CAML_BAUDRATE_57600 = 0x00000008;
        /// <summary>
        /// 115200
        /// </summary>
        public const Int32 MV_CAML_BAUDRATE_115200 = 0x00000010;
        /// <summary>
        /// 230400
        /// </summary>
        public const Int32 MV_CAML_BAUDRATE_230400 = 0x00000020;
        /// <summary>
        /// 460800
        /// </summary>
        public const Int32 MV_CAML_BAUDRATE_460800 = 0x00000040;
        /// <summary>
        /// 921600
        /// </summary>
        public const Int32 MV_CAML_BAUDRATE_921600 = 0x00000080;
        /// <summary>
        /// ch 最大值 |en Auto Max
        /// </summary>
        public const Int32 MV_CAML_BAUDRATE_AUTOMAX = 0x40000000;

        // chinese 信息类型 |en Information Type
        /// <summary>
        /// ch 网络流量和丢包信息 |en Network traffic and packet loss information
        /// </summary>
        public const Int32 MV_MATCH_TYPE_NET_DETECT = 0x00000001;
        /// <summary>
        /// ch host接收到来自U3V设备的字节总数 |en The total number of bytes host received from U3V device
        /// </summary>
        public const Int32 MV_MATCH_TYPE_USB_DETECT = 0x00000002;

        // 设备的访问模式
        /// <summary>
        /// ch独占权限，其他APP只允许读CCP寄存器 |en Exclusive authority, other APP is only allowed to read the CCP register
        /// </summary>
        public const Int32 MV_ACCESS_Exclusive = 1;
        /// <summary>
        /// ch 可以从5模式下抢占权限，然后以独占权限打开 |en You can seize the authority from the 5 mode, and then open with exclusive authority
        /// </summary>
        public const Int32 MV_ACCESS_ExclusiveWithSwitch = 2;
        /// <summary>
        /// ch 控制权限，其他APP允许读所有寄存器 |en Control authority, allows other APP reading all registers
        /// </summary>
        public const Int32 MV_ACCESS_Control = 3;
        /// <summary>
        /// ch 可以从5的模式下抢占权限，然后以控制权限打开 |en You can seize the authority from the 5 mode, and then open with control authority
        /// </summary>
        public const Int32 MV_ACCESS_ControlWithSwitch = 4;
        /// <summary>
        /// ch 以可被抢占的控制权限打开 |en Open with seized control authority
        /// </summary>
        public const Int32 MV_ACCESS_ControlSwitchEnable = 5;
        /// <summary>
        /// ch 可以从5的模式下抢占权限，然后以可被抢占的控制权限打开 |en You can seize the authority from the 5 mode, and then open with seized control authority
        /// </summary>
        public const Int32 MV_ACCESS_ControlSwitchEnableWithKey = 6;
        /// <summary>
        /// ch 读模式打开设备，适用于控制权限下 |en Open with read mode and is available under control authority
        /// </summary>
        public const Int32 MV_ACCESS_Monitor = 7;
        #endregion

        #region 相机参数结构体定义
        /// <summary>
        /// ch:采集卡信息列表 | en: Interface Information List
        /// </summary>
        public struct MV_INTERFACE_INFO_LIST
        {
            /// <summary>
            /// ch:在线设备数量 | en:Online Interface Number
            /// </summary>
            public UInt32 nInterfaceNum;

            /// <summary>
            /// ch:支持最多64个设备 | en:Support up to 64 Interfaces
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_INTERFACE_NUM)]
            public IntPtr[] pInterfaceInfo;
        };

        /// <summary>
        /// ch:采集卡信息 | en: Interface information
        /// </summary>
        public struct MV_INTERFACE_INFO
        {
            /// <summary>
            /// ch: 采集卡类型; 低16位有效: bits(0~2)代表功能, bits(3~7)代表相机, bits(8-15)代表总线| en: Interface type
            /// </summary>
            public UInt32 nTLayerType;
            /// <summary>
            /// ch: 采集卡的PCIE插槽信息 | en: PCIe slot information of interface
            /// </summary>
            public UInt32 nPCIEInfo;
            /// <summary>
            /// ch: 采集卡ID  | en: Interface ID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chInterfaceID;
            /// <summary>
            /// ch 显示名称 | en: Display name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chDisplayName;
            /// <summary>
            /// ch 序列号 |en: Serial number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chSerialNumber;
            /// <summary>
            /// ch 型号 | en: model name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chModelName;
            /// <summary>
            /// ch: 厂商 |en: manufacturer name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chManufacturer;
            /// <summary>
            /// ch: 版本号| en: device version
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chDeviceVersion;
            /// <summary>
            /// ch: 自定义名称 |en: user defined name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public Byte[] chUserDefinedName;
            /// <summary>
            /// ch 保留字段 | en Reserved
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public UInt32[] nReserved;
        };



        /// <summary>
        /// 排序方式
        /// </summary>
        public enum MV_SORT_METHOD
        {
            /// <summary>
            /// 按序列号排序
            /// </summary>
            SORTMETHOD_SERIALNUMBER = 0,

            /// <summary>
            /// 按用户自定义名字排序
            /// </summary>
            SORTMETHOD_USERID = 1,

            /// <summary>
            /// 按当前IP地址排序（升序）
            /// </summary>
            SORTMETHOD_CURRENTIP_ASC = 2,

            /// <summary>
            /// 按当前IP地址排序（降序）
            /// </summary>
            SORTMETHOD_CURRENTIP_DESC = 3,
        };

        /// <summary>
        /// ch: GigE设备信息 | en: GigE device information
        /// </summary>
        public struct MV_GIGE_DEVICE_INFO
        {
            /// <summary>
            /// IP 配置选项
            /// </summary>
            public UInt32 nIpCfgOption;
            /// <summary>
            /// IP configuration:bit31-static bit30-dhcp bit29-lla
            /// </summary>
            public UInt32 nIpCfgCurrent;
            /// <summary>
            /// curtent ip
            /// </summary>
            public UInt32 nCurrentIp;
            /// <summary>
            /// curtent subnet mask
            /// </summary>
            public UInt32 nCurrentSubNetMask;
            /// <summary>
            /// current gateway
            /// </summary>
            public UInt32 nDefultGateWay;
            /// <summary>
            /// 制造商名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chManufacturerName;
            /// <summary>
            /// 型号名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chModelName;
            /// <summary>
            /// 设备版本信息
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chDeviceVersion;
            /// <summary>
            /// 制造商特殊信息
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public String chManufacturerSpecificInfo;
            /// <summary>
            /// 序列号
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public String chSerialNumber;
            /// <summary>
            /// 用户自定义名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public String chUserDefinedName;
            /// <summary>
            /// 网口IP地址
            /// </summary>
            public UInt32 nNetExport;
            /// <summary>
            /// 预留
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// ch: GigE设备信息 | en: GigE device information
        /// </summary>
        public struct MV_GIGE_DEVICE_INFO_EX
        {
            /// <summary>
            /// IP 配置选项
            /// </summary>
            public UInt32 nIpCfgOption;
            /// <summary>
            /// IP configuration:bit31-static bit30-dhcp bit29-lla
            /// </summary>
            public UInt32 nIpCfgCurrent;
            /// <summary>
            /// curtent ip
            /// </summary>
            public UInt32 nCurrentIp;
            /// <summary>
            /// curtent subnet mask
            /// </summary>
            public UInt32 nCurrentSubNetMask;
            /// <summary>
            /// current gateway
            /// </summary>
            public UInt32 nDefultGateWay;
            /// <summary>
            /// 制造商名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chManufacturerName;
            /// <summary>
            /// 型号名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chModelName;
            /// <summary>
            /// 设备版本信息
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String chDeviceVersion;
            /// <summary>
            /// 制造商特殊信息
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public String chManufacturerSpecificInfo;
            /// <summary>
            /// 序列号
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public String chSerialNumber;
            /// <summary>
            /// 用户自定义名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public Byte[] chUserDefinedName;
            /// <summary>
            /// 网口IP地址
            /// </summary>
            public UInt32 nNetExport;
            /// <summary>
            /// 预留
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// ch:USB3 设备信息 | en:USB3 device information
        /// </summary>
        public struct MV_USB3_DEVICE_INFO
        {
            /// <summary>
            /// 控制输入端点
            /// </summary>
            public Byte CrtlInEndPoint;
            /// <summary>
            /// 控制输出端点
            /// </summary>
            public Byte CrtlOutEndPoint;
            /// <summary>
            /// 流端点
            /// </summary>
            public Byte StreamEndPoint;
            /// <summary>
            /// 事件端点
            /// </summary>
            public Byte EventEndPoint;
            /// <summary>
            /// 供应商ID号
            /// </summary>
            public UInt16 idVendor;
            /// <summary>
            /// 产品ID号
            /// </summary>
            public UInt16 idProduct;
            /// <summary>
            /// 设备索引号
            /// </summary>
            public UInt32 nDeviceNumber;
            /// <summary>
            /// 设备GUID号
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceGUID;
            /// <summary>
            /// 供应商名字
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;
            /// <summary>
            /// 型号名字
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            /// <summary>
            /// 家族名字
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chFamilyName;
            /// <summary>
            /// 设备版本号
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            /// <summary>
            /// 制造商名字
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerName;
            /// <summary>
            /// 序列号
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            /// <summary>
            /// 用户自定义名字
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chUserDefinedName;
            /// <summary>
            /// 支持的USB协议
            /// </summary>
            public UInt32 nbcdUSB;
            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// ch:USB3 设备信息 | en:USB3 device information
        /// </summary>
        public struct MV_USB3_DEVICE_INFO_EX
        {
            /// <summary>
            /// 控制输入端点
            /// </summary>
            public Byte CrtlInEndPoint;
            /// <summary>
            /// 控制输出端点
            /// </summary>
            public Byte CrtlOutEndPoint;
            /// <summary>
            /// 流端点
            /// </summary>
            public Byte StreamEndPoint;
            /// <summary>
            /// 事件端点
            /// </summary>
            public Byte EventEndPoint;
            /// <summary>
            /// 供应商ID号
            /// </summary>
            public UInt16 idVendor;
            /// <summary>
            /// 产品ID号
            /// </summary>
            public UInt16 idProduct;
            /// <summary>
            /// 设备索引号
            /// </summary>
            public UInt32 nDeviceNumber;
            /// <summary>
            /// 设备GUID号
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceGUID;
            /// <summary>
            /// 供应商名字
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;
            /// <summary>
            /// 型号名字
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            /// <summary>
            /// 家族名字
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chFamilyName;
            /// <summary>
            /// 设备版本号
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            /// <summary>
            /// 制造商名字
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerName;
            /// <summary>
            /// 序列号
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            /// <summary>
            /// 用户自定义名字
            /// </summary>   
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public Byte[] chUserDefinedName;
            /// <summary>
            /// 支持的USB协议
            /// </summary>
            public UInt32 nbcdUSB;
            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// ch:CamLink设备信息 | en:CamLink device information
        /// </summary>
        public struct MV_CamL_DEV_INFO
        {
            /// <summary>
            /// 端口号ID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chPortID;
            /// <summary>
            /// 模型名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            /// <summary>
            /// 家族名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chFamilyName;
            /// <summary>
            /// 设备版本信息
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            /// <summary>
            /// 制造商名字
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerName;
            /// <summary>
            /// 序列号
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 38)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// ch:采集卡Camera Link相机信息 | en:Camera Link device information on frame grabber
        /// </summary>
        public struct MV_CML_DEVICE_INFO
        {
            /// <summary>
            /// ch 采集卡ID |en Interface ID of Frame Grabber
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chInterfaceID;
            /// <summary>
            /// ch 供应商名字 |en Vendor name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;
            /// <summary>
            /// ch 型号名字 |en Model name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            /// <summary>
            /// ch 厂商信息 |en Manufacturer information
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerInfo;
            /// <summary>
            /// ch 相机版本 |en Device version
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            /// <summary>
            /// ch 序列号 |en Serial number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            /// <summary>
            /// ch 用户自定义名字 |en User defined name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public Byte[] chUserDefinedName;

            /// <summary>
            /// ch 相机ID |en Device ID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceID;

            /// <summary>
            /// ch 保留字段 |en Reserved
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public UInt32[] nReserved;                                          // 保留字节
        };

        /// <summary>
        /// ch:CoaXPress相机信息 | en:CoaXPress device information
        /// </summary>
        public struct MV_CXP_DEVICE_INFO
        {
            /// <summary>
            /// ch 采集卡ID |en Interface ID of Frame Grabber
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chInterfaceID;
            /// <summary>
            /// ch 供应商名字 |en Vendor name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;
            /// <summary>
            /// ch 型号名字 |en Model name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            /// <summary>
            /// ch 厂商信息 |en Manufacturer information
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerInfo;
            /// <summary>
            /// ch 相机版本 |en Device version
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            /// <summary>
            /// ch 序列号 |en Serial number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            /// <summary>
            /// ch 用户自定义名字 |en User defined name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public Byte[] chUserDefinedName;
            /// <summary>
            /// ch 相机ID |en Device ID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceID;
            /// <summary>
            /// ch 保留字段 |en Reserved
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public UInt32[] nReserved;                                          // 保留字节
        };

        /// <summary>
        /// ch:XoFLink相机信息 | en:XoFLink device information
        /// </summary>
        public struct MV_XOF_DEVICE_INFO
        {
            /// <summary>
            /// ch 采集卡ID |en Interface ID of Frame Grabber
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chInterfaceID;
            /// <summary>
            /// ch 供应商名字 |en Vendor name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;
            /// <summary>
            /// ch 型号名字 |en Model name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;
            /// <summary>
            /// ch 厂商信息 |en Manufacturer information
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chManufacturerInfo;
            /// <summary>
            /// ch 相机版本 |en Device version
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;
            /// <summary>
            /// ch 序列号 |en Serial number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;
            /// <summary>
            /// ch 用户自定义名字 |en User defined name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public Byte[] chUserDefinedName;
            /// <summary>
            /// ch 相机ID |en Device ID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceID;
            /// <summary>
            /// ch 保留字段 |en Reserved
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public UInt32[] nReserved;                                          // 保留字节
        };

        /// <summary>
        /// ch:设备信息 | en:Device information
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]   //结构体顺序布局
        public struct MV_CC_DEVICE_INFO
        {
            /// <summary>
            /// 主版本号
            /// </summary>
            public UInt16 nMajorVer;

            /// <summary>
            /// 次版本号
            /// </summary>
            public UInt16 nMinorVer;

            /// <summary>
            /// MAC高地址
            /// </summary>
            public UInt32 nMacAddrHigh;

            /// <summary>
            /// MAC低地址
            /// </summary>
            public UInt32 nMacAddrLow;

            /// <summary>
            /// 设备传输层协议类型，e.g. MV_GIGE_DEVICE
            /// </summary>
            public UInt32 nTLayerType;

            /// <summary>
            /// ch 设备类型信息 | en Device Type Info
            /// </summary>
            public UInt32 nDevTypeInfo;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public UInt32[] nReserved;

            /// <summary>
            /// ch:特定类型的设备信息 | en:Special devcie information
            /// </summary>
            [StructLayout(LayoutKind.Explicit, Size = 540)]
            public struct SPECIAL_INFO
            {
                /// <summary>
                /// GigE
                /// </summary>
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 216)]
                public Byte[] stGigEInfo;

                /// <summary>
                /// Camera Link
                /// </summary>
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 536)]
                public Byte[] stCamLInfo;
                /// <summary>
                /// Usb
                /// </summary>
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 540)]
                public Byte[] stUsb3VInfo;
                /// <summary>
                /// CML
                /// </summary>
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 540)]
                public Byte[] stCMLInfo;
                /// <summary>
                /// CXP
                /// </summary>
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 540)]
                public Byte[] stCXPInfo;
                /// <summary>
                /// XOF
                /// </summary>
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 540)]
                public Byte[] stXoFInfo;
            };

            /// <summary>
            /// 设备类型
            /// </summary>
            public SPECIAL_INFO SpecialInfo;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="nAnyNum">输入任意数，因为不接受无参构造函数</param>
            public MV_CC_DEVICE_INFO(UInt32 nAnyNum)
            {
                nMajorVer = 0;
                nMinorVer = 0;
                nMacAddrHigh = 0;
                nMacAddrLow = 0;
                nTLayerType = 0;
                nDevTypeInfo = 0;
                nReserved = new uint[3];
                SpecialInfo.stGigEInfo = new byte[216];
                SpecialInfo.stCamLInfo = new byte[536];
                SpecialInfo.stUsb3VInfo = new byte[540];
                SpecialInfo.stCMLInfo = new byte[540];
                SpecialInfo.stCXPInfo = new byte[540];
                SpecialInfo.stXoFInfo = new byte[540];
            }
        };

        /// <summary>
        /// 相机列表
        /// </summary>
        public struct MV_CC_DEVICE_INFO_LIST
        {
            /// <summary>
            /// 在线设备数量
            /// </summary>
            public UInt32 nDeviceNum;

            /// <summary>
            /// 支持最多256个设备
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_DEVICE_NUM)]
            public IntPtr[] pDeviceInfo;
        };

        /// <summary>
        /// ch:通过GenTL枚举到的Interface信息 | en:Interface Information with GenTL
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MV_GENTL_IF_INFO
        {
            /// <summary>
            /// GenTL接口ID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chInterfaceID;

            /// <summary>
            /// 传输层类型
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chTLType;

            /// <summary>
            /// 设备显示名称
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public String chDisplayName;

            /// <summary>
            /// GenTL的cti文件索引
            /// </summary>
            public UInt32 nCtiIndex;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// ch:通过GenTL枚举到的设备信息列表 | en:Interface Information List with GenTL
        /// </summary>
        public struct MV_GENTL_IF_INFO_LIST
        {
            /// <summary>
            /// ch:在线设备数量 | en:Online Interface Number
            /// </summary>
            public UInt32 nInterfaceNum;

            /// <summary>
            /// ch:支持最多256个设备 | en:Support up to 256 Interfaces
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_GENTL_IF_NUM)]
            public IntPtr[] pIFInfo;
        };

        /// <summary>
        /// ch:通过GenTL枚举到的设备信息 | en:Device Information discovered by with GenTL
        /// </summary>
        public struct MV_GENTL_DEV_INFO
        {
            /// <summary>
            /// 采集卡ID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chInterfaceID;

            /// <summary>
            /// 设备ID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceID;

            /// <summary>
            /// 供应商名字
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;

            /// <summary>
            /// 模型名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;

            /// <summary>
            /// 传输类型
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chTLType;

            /// <summary>
            /// 显示名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDisplayName;

            /// <summary>
            /// 用户自定义名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chUserDefinedName;

            /// <summary>
            /// 序列号
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;

            /// <summary>
            /// 设备版本信息
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;

            /// <summary>
            /// cti文件序号
            /// </summary>
            public UInt32 nCtiIndex;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// ch:通过GenTL枚举到的设备信息 | en:Device Information discovered by with GenTL
        /// </summary>
        public struct MV_GENTL_DEV_INFO_EX
        {
            /// <summary>
            /// 采集卡ID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chInterfaceID;

            /// <summary>
            /// 设备ID
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceID;

            /// <summary>
            /// 供应商名字
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chVendorName;

            /// <summary>
            /// 模型名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chModelName;

            /// <summary>
            /// 传输类型
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chTLType;

            /// <summary>
            /// 显示名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDisplayName;

            /// <summary>
            /// 用户自定义名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public Byte[] chUserDefinedName;

            /// <summary>
            /// 序列号
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chSerialNumber;

            /// <summary>
            /// 设备版本信息
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = INFO_MAX_BUFFER_SIZE)]
            public string chDeviceVersion;

            /// <summary>
            /// cti文件序号
            /// </summary>
            public UInt32 nCtiIndex;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// ch:GenTL设备列表 | en:GenTL devices list
        /// </summary>
        public struct MV_GENTL_DEV_INFO_LIST
        {
            /// <summary>
            /// 在线设备数量
            /// </summary>
            public UInt32 nDeviceNum;

            /// <summary>
            /// 支持最多256个设备
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_GENTL_DEV_NUM)]
            public IntPtr[] pDeviceInfo;
        };

        /// <summary>
        /// Net Trans Info
        /// </summary>
        public struct MV_NETTRANS_INFO
        {
            /// <summary>
            /// 已接收数据大小  [统计StartGrabbing和StopGrabbing之间的数据量]
            /// </summary>
            public Int64 nReviceDataSize;

            /// <summary>
            /// 丢帧数量
            /// </summary>
            public Int32 nThrowFrameCount;

            /// <summary>
            /// 接收帧数
            /// </summary>
            public UInt32 nNetRecvFrameCount;

            /// <summary>
            /// 请求重发包数
            /// </summary>
            public Int64 nRequestResendPacketCount;

            /// <summary>
            /// 重发包数
            /// </summary>
            public Int64 nResendPacketCount;
        };

        /// <summary>
        /// Frame Out Info
        /// </summary>
        public struct MV_FRAME_OUT_INFO
        {
            /// <summary>
            /// 图像宽
            /// </summary>
            public UInt16 nWidth;

            /// <summary>
            /// 图像高
            /// </summary>
            public UInt16 nHeight;

            /// <summary>
            /// 像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// 帧号
            /// </summary>
            public UInt32 nFrameNum;

            /// <summary>
            /// 时间戳高32位
            /// </summary>
            public UInt32 nDevTimeStampHigh;

            /// <summary>
            /// 时间戳低32位
            /// </summary>
            public UInt32 nDevTimeStampLow;

            /// <summary>
            /// 保留，8字节对齐
            /// </summary>
            public UInt32 nReserved0;

            /// <summary>
            /// 主机生成的时间戳
            /// </summary>
            public Int64 nHostTimeStamp;

            /// <summary>
            /// 帧数据大小
            /// </summary>
            public UInt32 nFrameLen;

            /// <summary>
            /// 丢包数量
            /// </summary>
            public UInt32 nLostPacket;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// Chunk数据信息
        /// </summary>
        public struct MV_CHUNK_DATA_CONTENT
        {
            /// <summary>
            /// Chunk数据
            /// </summary>
            public IntPtr pChunkData;

            /// <summary>
            /// ChunkID
            /// </summary>
            public UInt32 nChunkID;

            /// <summary>
            /// Chunk大小
            /// </summary>
            public UInt32 nChunkLen;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        };
        /// <summary>
        /// Frame Out Info Ex
        /// </summary>
        public struct MV_FRAME_OUT_INFO_EX
        {
            /// <summary>
            /// 图像宽
            /// </summary>
            public UInt16 nWidth;

            /// <summary>
            /// 图像高
            /// </summary>
            public UInt16 nHeight;

            /// <summary>
            /// 像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// 帧号
            /// </summary>
            public UInt32 nFrameNum;

            /// <summary>
            /// 时间戳高32位
            /// </summary>
            public UInt32 nDevTimeStampHigh;

            /// <summary>
            /// 时间戳低32位
            /// </summary>
            public UInt32 nDevTimeStampLow;

            /// <summary>
            /// 保留，8字节对齐
            /// </summary>
            public UInt32 nReserved0;

            /// <summary>
            /// 主机生成的时间戳
            /// </summary>
            public Int64 nHostTimeStamp;

            /// <summary>
            /// Frame大小
            /// </summary>
            public UInt32 nFrameLen;

            // 以下为chunk新增水印信息
            // 设备水印时标
            /// <summary>
            /// 秒数
            /// </summary>
            public UInt32 nSecondCount;

            /// <summary>
            /// 周期数
            /// </summary>
            public UInt32 nCycleCount;

            /// <summary>
            /// 周期偏移量
            /// </summary>
            public UInt32 nCycleOffset;

            /// <summary>
            /// 增益
            /// </summary>
            public Single fGain;

            /// <summary>
            /// 曝光时间
            /// </summary>
            public Single fExposureTime;

            /// <summary>
            /// 平均亮度
            /// </summary>
            public UInt32 nAverageBrightness;

            // 白平衡相关
            /// <summary>
            /// Red
            /// </summary>
            public UInt32 nRed;

            /// <summary>
            /// Green
            /// </summary>
            public UInt32 nGreen;

            /// <summary>
            /// Blue
            /// </summary>
            public UInt32 nBlue;

            /// <summary>
            /// 帧计数器
            /// </summary>
            public UInt32 nFrameCounter;

            /// <summary>
            /// 触发计数
            /// </summary>
            public UInt32 nTriggerIndex;

            //Line 输入/输出
            /// <summary>
            /// 输入
            /// </summary>
            public UInt32 nInput;

            /// <summary>
            /// 输出
            /// </summary>
            public UInt32 nOutput;

            // ROI区域
            /// <summary>
            /// 水平偏移量
            /// </summary>
            public UInt16 nOffsetX;

            /// <summary>
            /// 垂直偏移量
            /// </summary>
            public UInt16 nOffsetY;

            /// <summary>
            /// Chunk宽度
            /// </summary>
            public UInt16 nChunkWidth;

            /// <summary>
            /// Chunk高度
            /// </summary>
            public UInt16 nChunkHeight;

            /// <summary>
            /// 丢包数
            /// </summary>
            public UInt32 nLostPacket;

            /// <summary>
            /// 为解析的Chunk数量
            /// </summary>
            public UInt32 nUnparsedChunkNum;

            /// <summary>
            /// 为解析的Chunk列表
            /// </summary>
            [StructLayout(LayoutKind.Explicit)]
            public struct UNPARSED_CHUNK_LIST
            {
                /// <summary>
                /// 为解析的Chunk内容
                /// </summary>
                [FieldOffset(0)]
                public IntPtr pUnparsedChunkContent;

                /// <summary>
                /// 对齐结构体，无实际用途
                /// </summary>
                [FieldOffset(0)]
                public Int64 nAligning;
            }

            /// <summary>
            /// 为解析的Chunk列表
            /// </summary>
            public UNPARSED_CHUNK_LIST UnparsedChunkList;

            /// <summary>
            /// 图像宽扩展
            /// </summary>
            public UInt32 nExtendWidth;

            /// <summary>
            /// 图像高扩展
            /// </summary>
            public UInt32 nExtendHeight;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 34)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// 输出帧信息
        /// </summary>
        public struct MV_FRAME_OUT
        {
            /// <summary>
            /// 帧数据地址
            /// </summary>
            public IntPtr pBufAddr;

            /// <summary>
            /// 帧信息
            /// </summary>
            public MV_FRAME_OUT_INFO_EX stFrameInfo;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// 取流策略
        /// </summary>
        public enum MV_GRAB_STRATEGY
        {
            /// <summary>
            /// 从旧到新一帧一帧的获取图像（默认为该策略）
            /// </summary>
            MV_GrabStrategy_OneByOne = 0,

            /// <summary>
            /// 获取列表中最新的一帧图像（同时清除列表中的其余图像）
            /// </summary>
            MV_GrabStrategy_LatestImagesOnly = 1,

            /// <summary>
            /// 获取列表中最新的图像，个数由OutputQueueSize决定，范围为1-ImageNodeNum，设置成1等同于LatestImagesOnly，设置成ImageNodeNum等同于OneByOne
            /// </summary>
            MV_GrabStrategy_LatestImages = 2,

            /// <summary>
            /// 等待下一帧图像
            /// </summary>
            MV_GrabStrategy_UpcomingImage = 3,
        };

        /// <summary>
        /// 显示帧信息
        /// </summary>
        public struct MV_DISPLAY_FRAME_INFO
        {
            /// <summary>
            /// 显示窗口的句柄
            /// </summary>
            public IntPtr hWnd;

            /// <summary>
            /// 显示的帧数据
            /// </summary>
            public IntPtr pData;

            /// <summary>
            /// 显示的帧数据大小
            /// </summary>
            public UInt32 nDataLen;

            /// <summary>
            /// 图像宽
            /// </summary>
            public UInt16 nWidth;

            /// <summary>
            /// 图像高
            /// </summary>
            public UInt16 nHeight;

            /// <summary>
            /// 像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// 显示帧信息
        /// </summary>
        public struct MV_DISPLAY_FRAME_INFO_EX
        {
            /// <summary>
            /// 图像宽
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// 图像高
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// 像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// 显示的帧数据
            /// </summary>
            public IntPtr pData;

            /// <summary>
            /// 显示的帧数据大小
            /// </summary>
            public UInt32 nDataLen;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// ch:保存3D数据格式 | en:Save 3D file
        /// </summary>
        public enum MV_SAVE_POINT_CLOUD_FILE_TYPE
        {
            /// <summary>
            /// 未定义数据格式
            /// </summary>
            MV_PointCloudFile_Undefined = 0,

            /// <summary>
            /// PLY数据格式
            /// </summary>
            MV_PointCloudFile_PLY = 1,

            /// <summary>
            /// CSV数据格式
            /// </summary>
            MV_PointCloudFile_CSV = 2,

            /// <summary>
            /// OBJ数据格式
            /// </summary>
            MV_PointCloudFile_OBJ = 3,
        };

        /// <summary>
        /// 保存的点阵参数
        /// </summary>
        public struct MV_SAVE_POINT_CLOUD_PARAM
        {
            /// <summary>
            /// [IN]     每一行点的数量
            /// </summary>
            public UInt32 nLinePntNum;

            /// <summary>
            /// [IN]     行数
            /// </summary>
            public UInt32 nLineNum;

            /// <summary>
            /// [IN]     输入数据的像素格式
            /// </summary>
            public MvGvspPixelType enSrcPixelType;

            /// <summary>
            /// [IN]     输入数据缓存
            /// </summary>
            public IntPtr pSrcData;

            /// <summary>
            /// [IN]     输入数据大小
            /// </summary>
            public UInt32 nSrcDataLen;

            /// <summary>
            /// [OUT]    输出像素数据缓存
            /// </summary>
            public IntPtr pDstBuf;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小(nLinePntNum * nLineNum * (16*3 + 4) + 2048)
            /// </summary>
            public UInt32 nDstBufSize;

            /// <summary>
            /// [OUT]    输出像素数据缓存长度
            /// </summary>
            public UInt32 nDstBufLen;

            /// <summary>
            /// 保存的点阵文件类型
            /// </summary>
            public MV_SAVE_POINT_CLOUD_FILE_TYPE enPointCloudFileType;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 保存的图像格式
        /// </summary>
        public enum MV_SAVE_IAMGE_TYPE
        {
            /// <summary>
            /// 未定义类型
            /// </summary>
            MV_Image_Undefined = 0,

            /// <summary>
            /// Bmp图像格式
            /// </summary>
            MV_Image_Bmp = 1,

            /// <summary>
            /// Jpeg图像格式
            /// </summary>
            MV_Image_Jpeg = 2,

            /// <summary>
            /// Png图像格式
            /// </summary>
            MV_Image_Png = 3,

            /// <summary>
            /// Tif图像格式
            /// </summary>
            MV_Image_Tif = 4,
        };

        /// <summary>
        /// 保存的图像参数
        /// </summary>
        public struct MV_SAVE_IMAGE_PARAM
        {
            /// <summary>
            /// [IN]     输入数据缓存
            /// </summary>
            public IntPtr pData;

            /// <summary>
            /// [IN]     输入数据大小
            /// </summary>
            public UInt32 nDataLen;

            /// <summary>
            /// [IN]     输入数据的像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [IN]     图像宽
            /// </summary>
            public UInt16 nWidth;

            /// <summary>
            /// [IN]     图像高
            /// </summary>
            public UInt16 nHeight;

            /// <summary>
            /// [OUT]    输出图片缓存
            /// </summary>
            public IntPtr pImageBuffer;

            /// <summary>
            /// [OUT]    输出图片大小
            /// </summary>
            public UInt32 nImageLen;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nBufferSize;

            /// <summary>
            /// [IN]     输出图片格式
            /// </summary>
            public MV_SAVE_IAMGE_TYPE enImageType;
        };

        /// <summary>
        /// 保存的图像参数
        /// </summary>
        public struct MV_SAVE_IMAGE_PARAM_EX2
        {
            /// <summary>
            /// [IN]     输入数据缓存
            /// </summary>
            public IntPtr pData;

            /// <summary>
            /// [IN]     输入数据大小
            /// </summary>
            public UInt32 nDataLen;

            /// <summary>
            /// [IN]     输入数据的像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [IN]     图像宽
            /// </summary>
            public UInt16 nWidth;

            /// <summary>
            /// [IN]     图像高
            /// </summary>
            public UInt16 nHeight;

            /// <summary>
            /// [OUT]    输出图片缓存
            /// </summary>
            public IntPtr pImageBuffer;

            /// <summary>
            /// [OUT]    输出图片大小
            /// </summary>
            public UInt32 nImageLen;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nBufferSize;

            /// <summary>
            /// [IN]     输出图片格式
            /// </summary>
            public MV_SAVE_IAMGE_TYPE enImageType;

            /// <summary>
            /// [IN]     编码质量, (50-99]
            /// </summary>
            public UInt32 nJpgQuality;

            /// <summary>
            /// [IN]     Bayer的插值方法 0-快速 1-均衡 2-最优（如果传入其它值则默认为最优）
            /// </summary>
            public UInt32 iMethodValue;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public UInt32[] nReserved;
        };

        /// <summary>
        /// 保存的图像信息扩展
        /// </summary>
        public struct MV_SAVE_IMAGE_PARAM_EX3
        {
            /// <summary>
            /// [IN]     输入数据缓存
            /// </summary>
            public IntPtr pData;

            /// <summary>
            /// [IN]     输入数据大小
            /// </summary>
            public UInt32 nDataLen;

            /// <summary>
            /// [IN]     输入数据的像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [IN]     图像宽
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN]     图像高
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [OUT]    输出图片缓存
            /// </summary>
            public IntPtr pImageBuffer;

            /// <summary>
            /// [OUT]    输出图片大小
            /// </summary>
            public UInt32 nImageLen;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nBufferSize;

            /// <summary>
            /// [IN]     输出图片格式
            /// </summary>
            public MV_SAVE_IAMGE_TYPE enImageType;

            /// <summary>
            /// [IN]     编码质量, (50-99]
            /// </summary>
            public UInt32 nJpgQuality;

            /// <summary>
            /// [IN]     Bayer的插值方法 0-快速 1-均衡 2-最优（如果传入其它值则默认为最优）
            /// </summary>
            public UInt32 iMethodValue;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 保存图像到文件的参数
        /// </summary>
        public struct MV_SAVE_IMG_TO_FILE_PARAM
        {
            /// <summary>
            /// [IN]     输入数据的像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [IN]     输入数据缓存
            /// </summary>
            public IntPtr pData;

            /// <summary>
            /// [IN]     输入数据大小
            /// </summary>
            public UInt32 nDataLen;

            /// <summary>
            /// [IN]     图像宽
            /// </summary>
            public UInt16 nWidth;

            /// <summary>
            /// [IN]     图像高
            /// </summary>
            public UInt16 nHeight;

            /// <summary>
            /// [IN]     输入图片格式
            /// </summary>
            public MV_SAVE_IAMGE_TYPE enImageType;

            /// <summary>
            /// [IN]     编码质量, (0-100]
            /// </summary>
            public UInt32 nQuality;

            /// <summary>
            /// [IN]     输入文件路径
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string pImagePath;

            /// <summary>
            /// [IN]     Bayer的插值方法 0-快速 1-均衡 2-最优（如果传入其它值则默认为最优）
            /// </summary>
            public UInt32 iMethodValue;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 保存图像到文件信息扩展
        /// </summary>
        public struct MV_SAVE_IMG_TO_FILE_PARAM_EX
        {
            /// <summary>
            /// [IN]     图像宽
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN]     图像高
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [IN]     输入数据的像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [IN]     输入数据缓存
            /// </summary>
            public IntPtr pData;

            /// <summary>
            /// [IN]     输入数据大小
            /// </summary>
            public UInt32 nDataLen;

            /// <summary>
            /// [IN]     输入图片格式
            /// </summary>
            public MV_SAVE_IAMGE_TYPE enImageType;

            /// <summary>
            /// [IN]     输入文件路径
            /// </summary>
            public string pImagePath;

            /// <summary>
            /// [IN]     编码质量, (0-100]
            /// </summary>
            public UInt32 nQuality;

            /// <summary>
            /// [IN]     Bayer的插值方法 0-快速 1-均衡 2-最优（如果传入其它值则默认为最优）
            /// </summary>
            public UInt32 iMethodValue;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 旋转角度
        /// </summary>
        public enum MV_IMG_ROTATION_ANGLE
        {
            /// <summary>
            /// 旋转90度
            /// </summary>
            MV_IMAGE_ROTATE_90 = 1,

            /// <summary>
            /// 旋转180度
            /// </summary>
            MV_IMAGE_ROTATE_180 = 2,

            /// <summary>
            /// 旋转270度
            /// </summary>
            MV_IMAGE_ROTATE_270 = 3,
        }

        /// <summary>
        /// 旋转图像参数
        /// </summary>
        public struct MV_CC_ROTATE_IMAGE_PARAM
        {
            /// <summary>
            /// [IN]     像素格式(仅支持Mono8/RGB24/BGR24)
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [IN][OUT]     图像宽
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN][OUT]     图像高
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [IN]     输入数据缓存
            /// </summary>
            public IntPtr pSrcData;

            /// <summary>
            /// [IN]     输入数据大小
            /// </summary>
            public UInt32 nSrcDataLen;

            /// <summary>
            /// [OUT]    输出图片缓存
            /// </summary>
            public IntPtr pDstBuf;

            /// <summary>
            /// [OUT]    输出图片大小
            /// </summary>
            public UInt32 nDstBufLen;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nDstBufSize;

            /// <summary>
            /// [IN]     旋转角度
            /// </summary>
            public MV_IMG_ROTATION_ANGLE enRotationAngle;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 图像翻转类型
        /// </summary>
        public enum MV_IMG_FLIP_TYPE
        {
            /// <summary>
            /// 垂直方向翻转
            /// </summary>
            MV_FLIP_VERTICAL = 1,

            /// <summary>
            /// 水平方向翻转
            /// </summary>
            MV_FLIP_HORIZONTAL = 2,
        }

        /// <summary>
        /// 翻转图像参数
        /// </summary>
        public struct MV_CC_FLIP_IMAGE_PARAM
        {
            /// <summary>
            /// [IN]     像素格式(仅支持Mono8/RGB24/BGR24)
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [IN]     图像宽
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN]     图像高
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [IN]     输入数据缓存
            /// </summary>
            public IntPtr pSrcData;

            /// <summary>
            /// [IN]     输入数据大小
            /// </summary>
            public UInt32 nSrcDataLen;

            /// <summary>
            /// [OUT]    输出图片缓存
            /// </summary>
            public IntPtr pDstBuf;

            /// <summary>
            /// [OUT]    输出图片大小
            /// </summary>
            public UInt32 nDstBufLen;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nDstBufSize;

            /// <summary>
            /// [IN]     翻转类型
            /// </summary>
            public MV_IMG_FLIP_TYPE enFlipType;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 像素转换参数
        /// </summary>
        public struct MV_PIXEL_CONVERT_PARAM
        {
            /// <summary>
            /// [IN]     图像宽
            /// </summary>
            public UInt16 nWidth;

            /// <summary>
            /// [IN]     图像高
            /// </summary>
            public UInt16 nHeight;

            /// <summary>
            /// [IN]     源像素格式
            /// </summary>
            public MvGvspPixelType enSrcPixelType;

            /// <summary>
            /// [IN]     输入数据缓存
            /// </summary>
            public IntPtr pSrcData;

            /// <summary>
            /// [IN]     输入数据大小
            /// </summary>
            public UInt32 nSrcDataLen;

            /// <summary>
            /// [IN]     目标像素格式
            /// </summary>
            public MvGvspPixelType enDstPixelType;

            /// <summary>
            /// [OUT]    输出数据缓存
            /// </summary>
            public IntPtr pDstBuffer;

            /// <summary>
            /// [OUT]    输出数据大小
            /// </summary>
            public UInt32 nDstLen;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nDstBufferSize;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nRes;
        }

        /// <summary>
        /// 图像像素转换信息扩展
        /// </summary>
        public struct MV_CC_PIXEL_CONVERT_PARAM_EX
        {
            /// <summary>
            /// [IN]     图像宽
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN]     图像高
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [IN]     源像素格式
            /// </summary>
            public MvGvspPixelType enSrcPixelType;

            /// <summary>
            /// [IN]     输入数据缓存
            /// </summary>
            public IntPtr pSrcData;

            /// <summary>
            /// [IN]     输入数据大小
            /// </summary>
            public UInt32 nSrcDataLen;

            /// <summary>
            /// [IN]     目标像素格式
            /// </summary>
            public MvGvspPixelType enDstPixelType;

            /// <summary>
            /// [OUT]    输出数据缓存
            /// </summary>
            public IntPtr pDstBuffer;

            /// <summary>
            /// [OUT]    输出数据大小
            /// </summary>
            public UInt32 nDstLen;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nDstBufferSize;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nRes;
        }

        /// <summary>
        /// Gamma类型
        /// </summary>
        public enum MV_CC_GAMMA_TYPE
        {
            /// <summary>
            /// 不启用
            /// </summary>
            MV_CC_GAMMA_TYPE_NONE = 0,

            /// <summary>
            /// GAMMA值
            /// </summary>
            MV_CC_GAMMA_TYPE_VALUE = 1,

            /// <summary>
            /// GAMMA曲线，8位需要的长度：256*sizeof(unsigned char)
            /// 10位需要的长度：1024*sizeof(unsigned short)
            /// 12位需要的长度：4096*sizeof(unsigned short)
            /// 16位需要的长度：65536*sizeof(unsigned short)
            /// </summary>
            MV_CC_GAMMA_TYPE_USER_CURVE = 2,

            /// <summary>
            /// 线性RGB转非线性RGB
            /// </summary>
            MV_CC_GAMMA_TYPE_LRGB2SRGB = 3,

            /// <summary>
            /// 非线性RGB转线性RGB
            /// </summary>
            MV_CC_GAMMA_TYPE_SRGB2LRGB = 4,
        }


        /// <summary>
        /// Gamma参数
        /// </summary>
        public struct MV_CC_GAMMA_PARAM
        {
            /// <summary>
            /// [IN]     Gamma类型
            /// </summary>
            public MV_CC_GAMMA_TYPE enGammaType;

            /// <summary>
            /// [IN]     Gamma值
            /// </summary>
            public Single fGammaValue;

            /// <summary>
            /// [IN]     Gamma曲线缓存
            /// </summary>
            public IntPtr pGammaCurveBuf;

            /// <summary>
            /// [IN]     Gamma曲线长度
            /// </summary>
            public UInt32 nGammaCurveBufLen;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// CCM参数
        /// </summary>
        public struct MV_CC_CCM_PARAM
        {
            /// <summary>
            /// [IN]     是否启用CCM
            /// </summary>
            public Boolean bCCMEnable;

            /// <summary>
            /// [IN]     CCM矩阵(-8192~8192)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public Int32[] nCCMat;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// CCM参数
        /// </summary>
        public struct MV_CC_CCM_PARAM_EX
        {
            /// <summary>
            /// [IN]     是否启用CCM
            /// </summary>
            public Boolean bCCMEnable;

            /// <summary>
            /// [IN]     量化3x3矩阵
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public Int32[] nCCMat;

            /// <summary>
            /// [IN]     量化系数（2的整数幂）
            /// </summary>
            public UInt32 nCCMScale;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// CLUT参数
        /// </summary>
        public struct MV_CC_CLUT_PARAM
        {
            /// <summary>
            /// [IN]     是否启用CLUT
            /// </summary>
            public Boolean bCLUTEnable;

            /// <summary>
            /// [IN]     量化系数(2的整数幂)
            /// </summary>
            public UInt32 nCLUTScale;

            /// <summary>
            /// [IN]     CLUT大小，建议值17
            /// </summary>
            public UInt32 nCLUTSize;

            /// <summary>
            /// [OUT]    量化CLUT
            /// </summary>
            public IntPtr pCLUTBuf;

            /// <summary>
            /// [IN]     量化CLUT缓存大小（nCLUTSize*nCLUTSize*nCLUTSize*sizeof(int)*3）
            /// </summary>
            public UInt32 nCLUTBufLen;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 对比度调节参数
        /// </summary>
        public struct MV_CC_CONTRAST_PARAM
        {
            /// <summary>
            /// [IN]     图像宽度(最小8)
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN]     图像高度(最小8)
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [IN]     输入图像缓存
            /// </summary>
            public IntPtr pSrcBuf;

            /// <summary>
            /// [IN]     输入图像缓存长度
            /// </summary>
            public UInt32 nSrcBufLen;

            /// <summary>
            /// [IN]     输入的像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [OUT]    输出像素数据缓存
            /// </summary>
            public IntPtr pDstBuf;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nDstBufSize;

            /// <summary>
            /// [OUT]    输出像素数据缓存长度
            /// </summary>
            public UInt32 nDstBufLen;

            /// <summary>
            /// [IN]     对比度值，范围:[1, 10000]
            /// </summary>
            public UInt32 nContrastFactor;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 锐化参数
        /// </summary>
        public struct MV_CC_SHARPEN_PARAM
        {
            /// <summary>
            /// [IN]     图像宽度(最小8)
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN]     图像高度(最小8)
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [IN]     输入图像缓存
            /// </summary>
            public IntPtr pSrcBuf;

            /// <summary>
            /// [IN]     输入图像缓存长度
            /// </summary>
            public UInt32 nSrcBufLen;

            /// <summary>
            /// [IN]     输入的像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [OUT]    输出像素数据缓存
            /// </summary>
            public IntPtr pDstBuf;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nDstBufSize;

            /// <summary>
            /// [OUT]    输出像素数据缓存长度
            /// </summary>
            public UInt32 nDstBufLen;

            /// <summary>
            /// [IN]     锐度调节强度，范围:[0, 500]
            /// </summary>
            public UInt32 nSharpenAmount;

            /// <summary>
            /// [IN]     锐度调节半径（半径越大，耗时越长），范围:[1, 21]
            /// </summary>
            public UInt32 nSharpenRadius;

            /// <summary>
            /// [IN]     锐度调节阈值，范围:[0, 255]
            /// </summary>
            public UInt32 nSharpenThreshold;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 色彩校正参数（包括CCM和CLUT）
        /// </summary>
        public struct MV_CC_COLOR_CORRECT_PARAM
        {
            /// <summary>
            /// [IN]     图像宽度
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN]     图像高度
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [IN]     输入图像缓存
            /// </summary>
            public IntPtr pSrcBuf;

            /// <summary>
            /// [IN]     输入图像缓存长度
            /// </summary>
            public UInt32 nSrcBufLen;

            /// <summary>
            /// [IN]     输入的像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [OUT]    输出像素数据缓存
            /// </summary>
            public IntPtr pDstBuf;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nDstBufSize;

            /// <summary>
            /// [OUT]    输出像素数据缓存长度
            /// </summary>
            public UInt32 nDstBufLen;

            /// <summary>
            /// [IN]     输入有效图像位数，8 or 10 or 12 or 16
            /// </summary>
            public UInt32 nImageBit;

            /// <summary>
            /// [IN]     输入Gamma信息
            /// </summary>
            public MV_CC_GAMMA_PARAM stGammaParam;

            /// <summary>
            /// [IN]     输入CCM信息
            /// </summary>
            public MV_CC_CCM_PARAM_EX stCCMParam;

            /// <summary>
            /// [IN]     输入CLUT信息
            /// </summary>
            public MV_CC_CLUT_PARAM stCLUTParam;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 矩形ROI参数
        /// </summary>
        public struct MV_CC_RECT_I
        {
            /// <summary>
            /// [IN]     矩形左上角X轴坐标
            /// </summary>
            public UInt32 nX;

            /// <summary>
            /// [IN]     矩形左上角Y轴坐标
            /// </summary>
            public UInt32 nY;

            /// <summary>
            /// [IN]     矩形宽度
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN]     矩形高度
            /// </summary>
            public UInt32 nHeight;
        };

        /// <summary>
        /// 噪声估计参数
        /// </summary>
        public struct MV_CC_NOISE_ESTIMATE_PARAM
        {
            /// <summary>
            /// [IN]     图像宽度
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN]     图像高度
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [IN]     输入的像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [IN]     输入图像缓存
            /// </summary>
            public IntPtr pSrcBuf;

            /// <summary>
            /// [IN]     输入图像缓存长度
            /// </summary>
            public UInt32 nSrcBufLen;

            /// <summary>
            /// [IN]     图像ROI
            /// </summary>
            public IntPtr pstROIRect;

            /// <summary>
            /// [IN]     ROI个数
            /// </summary>
            public UInt32 nROINum;

            //Bayer域噪声估计参数，Mono8/RGB域无效
            /// <summary>
            /// [IN]     噪声阈值[0-4095]
            /// </summary>
            public UInt32 nNoiseThreshold;

            /// <summary>
            /// [OUT]    输出噪声特性
            /// </summary>
            public IntPtr pNoiseProfile;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nNoiseProfileSize;

            /// <summary>
            /// [OUT]    输出噪声特性长度
            /// </summary>
            public UInt32 nNoiseProfileLen;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 空域降噪参数
        /// </summary>
        public struct MV_CC_SPATIAL_DENOISE_PARAM
        {
            /// <summary>
            /// [IN]     图像宽度
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN]     图像高度
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [IN]     输入的像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [IN]     输入图像缓存
            /// </summary>
            public IntPtr pSrcBuf;

            /// <summary>
            /// [IN]     输入图像缓存长度
            /// </summary>
            public UInt32 nSrcBufLen;

            /// <summary>
            /// [OUT]    输出降噪后的数据
            /// </summary>
            public IntPtr pDstBuf;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nDstBufSize;

            /// <summary>
            /// [OUT]    输出降噪后的数据长度
            /// </summary>
            public UInt32 nDstBufLen;

            /// <summary>
            /// [IN]     输入噪声特性
            /// </summary>
            public IntPtr pNoiseProfile;

            /// <summary>
            /// [IN]     输入噪声特性长度
            /// </summary>
            public UInt32 nNoiseProfileLen;

            //Bayer域空域降噪算法参数，Mono8/RGB域无效
            /// <summary>
            /// [IN]     降噪强度(0-100)
            /// </summary>
            public UInt32 nBayerDenoiseStrength;

            /// <summary>
            /// [IN]     锐化强度(0-32)
            /// </summary>
            public UInt32 nBayerSharpenStrength;

            /// <summary>
            /// [IN]     噪声校正系数(0-1280)
            /// </summary>
            public UInt32 nBayerNoiseCorrect;

            //Mono8/RGB域空域降噪算法参数，Bayer域无效
            /// <summary>
            /// [IN]     亮度校正系数(1-2000)
            /// </summary>
            public UInt32 nNoiseCorrectLum;

            /// <summary>
            /// [IN]     色调校正系数(1-2000)
            /// </summary>
            public UInt32 nNoiseCorrectChrom;

            /// <summary>
            /// [IN]     亮度降噪强度(0-100)
            /// </summary>
            public UInt32 nStrengthLum;

            /// <summary>
            /// [IN]     色调降噪强度(0-100)
            /// </summary>
            public UInt32 nStrengthChrom;

            /// <summary>
            /// [IN]     锐化强度(1-1000)
            /// </summary>
            public UInt32 nStrengthSharpen;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// LSC标定参数
        /// </summary>
        public struct MV_CC_LSC_CALIB_PARAM
        {
            /// <summary>
            /// [IN]     图像宽度(16~65536)
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN]     图像高度(16~65536)
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [IN]     输入的像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [IN]     输入图像缓存
            /// </summary>
            public IntPtr pSrcBuf;

            /// <summary>
            /// [IN]     输入图像缓存长度
            /// </summary>
            public UInt32 nSrcBufLen;

            /// <summary>
            /// [OUT]    输出标定表缓存
            /// </summary>
            public IntPtr pCalibBuf;

            /// <summary>
            /// [IN]     提供的标定表缓冲大小（nWidth*nHeight*sizeof(unsigned short)）
            /// </summary>
            public UInt32 nCalibBufSize;

            /// <summary>
            /// [OUT]    输出标定表缓存长度
            /// </summary>
            public UInt32 nCalibBufLen;

            /// <summary>
            /// [IN]     宽度分块数
            /// </summary>
            public UInt32 nSecNumW;

            /// <summary>
            /// [IN]     高度分块数
            /// </summary>
            public UInt32 nSecNumH;

            /// <summary>
            /// [IN]     边缘填充系数，范围1~5
            /// </summary>
            public UInt32 nPadCoef;

            /// <summary>
            /// [IN]     标定方式，0-中心为基准
            ///                    1-最亮区域为基准
            ///                    2-目标亮度
            /// </summary>
            public UInt32 nCalibMethod;

            /// <summary>
            /// [IN]     目标亮度（8bits，[0,255])
            ///         （10bits，[0,1023])
            ///         （12bits，[0,4095])
            ///         （16bits，[0,65535])
            /// </summary>
            public UInt32 nTargetGray;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// LSC校正参数
        /// </summary>
        public struct MV_CC_LSC_CORRECT_PARAM
        {
            /// <summary>
            /// [IN]     图像宽度(16~65536)
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN]     图像高度(16~65536)
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [IN]     输入的像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [IN]     输入图像缓存
            /// </summary>
            public IntPtr pSrcBuf;

            /// <summary>
            /// [IN]     输入图像缓存长度
            /// </summary>
            public UInt32 nSrcBufLen;

            /// <summary>
            /// [OUT]    输出像素数据缓存
            /// </summary>
            public IntPtr pDstBuf;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nDstBufSize;

            /// <summary>
            /// [OUT]    输出像素数据缓存长度
            /// </summary>
            public UInt32 nDstBufLen;

            /// <summary>
            /// [IN]     输入校正表缓存
            /// </summary>
            public IntPtr pCalibBuf;

            /// <summary>
            /// [IN]     输入校正表缓存长度
            /// </summary>
            public UInt32 nCalibBufLen;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 噪声特性类型
        /// </summary>
        public enum MV_CC_BAYER_NOISE_FEATURE_TYPE
        {
            /// <summary>
            /// 无效 
            /// </summary>
            MV_CC_BAYER_NOISE_FEATURE_TYPE_INVALID = 0,

            /// <summary>
            /// 噪声曲线
            /// </summary>
            MV_CC_BAYER_NOISE_FEATURE_TYPE_PROFILE = 1,

            /// <summary>
            /// 噪声水平
            /// </summary>
            MV_CC_BAYER_NOISE_FEATURE_TYPE_LEVEL = 2,

            /// <summary>
            /// 默认值
            /// </summary>
            MV_CC_BAYER_NOISE_FEATURE_TYPE_DEFAULT = 2,
        };

        /// <summary>
        /// 噪声基本信息
        /// </summary>
        public struct MV_CC_BAYER_NOISE_PROFILE_INFO
        {
            /// <summary>
            /// 版本
            /// </summary>
            public UInt32 nVersion;

            /// <summary>
            /// 噪声特性类型
            /// </summary>
            public MV_CC_BAYER_NOISE_FEATURE_TYPE enNoiseFeatureType;

            /// <summary>
            /// 图像格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// 平均噪声水平
            /// </summary>
            public Int32 nNoiseLevel;

            /// <summary>
            /// 曲线点数
            /// </summary>
            public UInt32 nCurvePointNum;

            /// <summary>
            /// 噪声曲线
            /// </summary>
            public IntPtr nNoiseCurve;

            /// <summary>
            /// 亮度曲线
            /// </summary>
            public IntPtr nLumCurve;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 噪声估计参数
        /// </summary>
        public struct MV_CC_BAYER_NOISE_ESTIMATE_PARAM
        {
            /// <summary>
            /// [IN]     图像宽(大于等于8)
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN]     图像高(大于等于8)
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [IN]     像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [IN]     输入数据缓存
            /// </summary>
            public IntPtr pSrcData;

            /// <summary>
            /// [IN]     输入数据大小
            /// </summary>
            public UInt32 nSrcDataLen;

            /// <summary>
            /// [IN]     噪声阈值(0-4095)
            /// </summary>
            public UInt32 nNoiseThreshold;

            /// <summary>
            /// [IN]     用于存储噪声曲线和亮度曲线（需要外部分配，缓存大小：4096 * sizeof(int) * 2）
            /// </summary>
            public IntPtr pCurveBuf;

            /// <summary>
            /// [OUT]    降噪特性信息
            /// </summary>
            public MV_CC_BAYER_NOISE_PROFILE_INFO stNoiseProfile;

            /// <summary>
            /// [IN]     线程数量，0表示算法库根据硬件自适应；1表示单线程（默认）；大于1表示线程数目
            /// </summary>
            public UInt32 nThreadNum;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 降噪参数
        /// </summary>
        public struct MV_CC_BAYER_SPATIAL_DENOISE_PARAM
        {
            /// <summary>
            /// [IN]     图像宽(大于等于8)
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [IN]     图像高(大于等于8)
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [IN]     像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [IN]     输入数据缓存
            /// </summary>
            public IntPtr pSrcData;

            /// <summary>
            /// [IN]     输入数据大小
            /// </summary>
            public UInt32 nSrcDataLen;

            /// <summary>
            /// [OUT]    输出降噪后的数据
            /// </summary>
            public IntPtr pDstBuf;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nDstBufSize;

            /// <summary>
            /// [OUT]    输出降噪后的数据长度
            /// </summary>
            public UInt32 nDstBufLen;

            /// <summary>
            /// [IN]    降噪特性信息(来源于噪声估计)
            /// </summary>
            public MV_CC_BAYER_NOISE_PROFILE_INFO stNoiseProfile;

            /// <summary>
            /// [IN]     降噪强度(0-100) 
            /// </summary>
            public UInt32 nDenoiseStrength;

            /// <summary>
            /// [IN]     锐化强度(0-32)
            /// </summary>
            public UInt32 nSharpenStrength;

            /// <summary>
            /// [IN]     噪声校正系数(0-1280)
            /// </summary>
            public UInt32 nNoiseCorrect;

            /// <summary>
            /// [IN]     线程数量，0表示算法库根据硬件自适应；1表示单线程（默认）；大于1表示线程数目
            /// </summary>
            public UInt32 nThreadNum;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 帧特殊信息
        /// </summary>
        public struct MV_CC_FRAME_SPEC_INFO
        {
            //设备水印时标
            /// <summary>
            /// [OUT]     秒数
            /// </summary>
            public UInt32 nSecondCount;

            /// <summary>
            /// [OUT]     周期数
            /// </summary>
            public UInt32 nCycleCount;

            /// <summary>
            /// [OUT]     周期偏移量
            /// </summary>
            public UInt32 nCycleOffset;

            /// <summary>
            /// [OUT]     增益
            /// </summary>
            public Single fGain;

            /// <summary>
            /// [OUT]     曝光时间
            /// </summary>
            public Single fExposureTime;

            /// <summary>
            /// [OUT]     平均亮度
            /// </summary>
            public UInt32 nAverageBrightness;

            //白平衡相关
            /// <summary>
            /// [OUT]     红色
            /// </summary>
            public UInt32 nRed;

            /// <summary>
            /// [OUT]     绿色
            /// </summary>
            public UInt32 nGreen;

            /// <summary>
            /// [OUT]     蓝色
            /// </summary>
            public UInt32 nBlue;

            /// <summary>
            /// [OUT]     总帧数
            /// </summary>
            public UInt32 nFrameCounter;

            /// <summary>
            /// [OUT]     触发计数
            /// </summary>
            public UInt32 nTriggerIndex;

            /// <summary>
            /// [OUT]     输入
            /// </summary>
            public UInt32 nInput;

            /// <summary>
            /// [OUT]     输出
            /// </summary>
            public UInt32 nOutput;

            /// <summary>
            /// [OUT]     水平偏移量
            /// </summary>
            public UInt16 nOffsetX;

            /// <summary>
            /// [OUT]     垂直偏移量
            /// </summary>
            public UInt16 nOffsetY;

            /// <summary>
            /// [OUT]     水印宽
            /// </summary>
            public UInt16 nFrameWidth;

            /// <summary>
            /// [OUT]     水印高
            /// </summary>
            public UInt16 nFrameHeight;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// HB解码参数
        /// </summary>
        public struct MV_CC_HB_DECODE_PARAM
        {
            /// <summary>
            /// [IN]     输入数据缓存
            /// </summary>
            public IntPtr pSrcBuf;

            /// <summary>
            /// [IN]     输入数据大小
            /// </summary>
            public UInt32 nSrcLen;

            /// <summary>
            /// [OUT]    图像宽
            /// </summary>
            public UInt32 nWidth;

            /// <summary>
            /// [OUT]    图像高
            /// </summary>
            public UInt32 nHeight;

            /// <summary>
            /// [OUT]    输出数据缓存
            /// </summary>
            public IntPtr pDstBuf;

            /// <summary>
            /// [IN]     提供的输出缓冲区大小
            /// </summary>
            public UInt32 nDstBufSize;

            /// <summary>
            /// [OUT]    输出数据大小
            /// </summary>
            public UInt32 nDstBufLen;

            /// <summary>
            /// [OUT]     输出的像素格式
            /// </summary>
            public MvGvspPixelType enDstPixelType;

            /// <summary>
            /// [OUT]    水印信息
            /// </summary>
            public MV_CC_FRAME_SPEC_INFO stFrameSpecInfo;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 录像格式定义
        /// </summary>
        public enum MV_RECORD_FORMAT_TYPE
        {
            /// <summary>
            /// 未定义格式
            /// </summary>
            MV_FormatType_Undefined = 0,

            /// <summary>
            /// AVI格式
            /// </summary>
            MV_FormatType_AVI = 1,
        }

        /// <summary>
        /// 录像参数
        /// </summary>
        public struct MV_CC_RECORD_PARAM
        {
            /// <summary>
            /// [IN]     输入数据的像素格式
            /// </summary>
            public MvGvspPixelType enPixelType;

            /// <summary>
            /// [IN]     图像宽(指定目标参数时需为8的倍数)
            /// </summary>
            public UInt16 nWidth;

            /// <summary>
            /// [IN]     图像高(指定目标参数时需为8的倍数)
            /// </summary>
            public UInt16 nHeight;

            /// <summary>
            /// [IN]     帧率fps(大于1/16)
            /// </summary>
            public Single fFrameRate;

            /// <summary>
            /// [IN]     码率kbps(128kbps-16Mbps)
            /// </summary>
            public UInt32 nBitRate;

            /// <summary>
            /// [IN]     录像格式
            /// </summary>
            public MV_RECORD_FORMAT_TYPE enRecordFmtType;

            /// <summary>
            /// [IN]     录像文件存放路径
            /// </summary>
            public String strFilePath;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 输入帧信息
        /// </summary>
        public struct MV_CC_INPUT_FRAME_INFO
        {
            /// <summary>
            /// [IN]     图像数据指针
            /// </summary>
            public IntPtr pData;

            /// <summary>
            /// [IN]     图像大小
            /// </summary>
            public UInt32 nDataLen;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nRes;
        };

        /// <summary>
        /// 采集模式
        /// </summary>
        public enum MV_CAM_ACQUISITION_MODE
        {
            /// <summary>
            /// 单帧模式
            /// </summary>
            MV_ACQ_MODE_SINGLE = 0,

            /// <summary>
            /// 多帧模式
            /// </summary>
            MV_ACQ_MODE_MUTLI = 1,

            /// <summary>
            /// 持续采集模式
            /// </summary>
            MV_ACQ_MODE_CONTINUOUS = 2,
        };

        /// <summary>
        /// 增益模式
        /// </summary>
        public enum MV_CAM_GAIN_MODE
        {
            /// <summary>
            /// 关闭
            /// </summary>
            MV_GAIN_MODE_OFF = 0,

            /// <summary>
            /// 一次
            /// </summary>
            MV_GAIN_MODE_ONCE = 1,

            /// <summary>
            /// 连续
            /// </summary>
            MV_GAIN_MODE_CONTINUOUS = 2,
        };

        /// <summary>
        /// 曝光模式
        /// </summary>
        public enum MV_CAM_EXPOSURE_MODE
        {
            /// <summary>
            /// Timed
            /// </summary>
            MV_EXPOSURE_MODE_TIMED = 0,

            /// <summary>
            /// TriggerWidth
            /// </summary>
            MV_EXPOSURE_MODE_TRIGGER_WIDTH = 1,
        };

        /// <summary>
        /// 自动曝光模式
        /// </summary>
        public enum MV_CAM_EXPOSURE_AUTO_MODE
        {
            /// <summary>
            /// 关闭
            /// </summary>
            MV_EXPOSURE_AUTO_MODE_OFF = 0,

            /// <summary>
            /// 一次
            /// </summary>
            MV_EXPOSURE_AUTO_MODE_ONCE = 1,

            /// <summary>
            /// 连续
            /// </summary>
            MV_EXPOSURE_AUTO_MODE_CONTINUOUS = 2,
        };

        /// <summary>
        /// 相机触发模式
        /// </summary>
        public enum MV_CAM_TRIGGER_MODE
        {
            /// <summary>
            /// 关闭
            /// </summary>
            MV_TRIGGER_MODE_OFF = 0,

            /// <summary>
            /// 打开
            /// </summary>
            MV_TRIGGER_MODE_ON = 1,
        };

        /// <summary>
        /// Gamma选择器
        /// </summary>
        public enum MV_CAM_GAMMA_SELECTOR
        {
            /// <summary>
            /// USER
            /// </summary>
            MV_GAMMA_SELECTOR_USER = 1,

            /// <summary>
            /// SRGB
            /// </summary>
            MV_GAMMA_SELECTOR_SRGB = 2,
        };

        /// <summary>
        /// 自动白平衡
        /// </summary>
        public enum MV_CAM_BALANCEWHITE_AUTO
        {
            /// <summary>
            /// 关闭自动白平衡
            /// </summary>
            MV_BALANCEWHITE_AUTO_OFF = 0,

            /// <summary>
            /// 一次自动白平衡
            /// </summary>
            MV_BALANCEWHITE_AUTO_ONCE = 2,

            /// <summary>
            /// 连续自动白平衡
            /// </summary>
            MV_BALANCEWHITE_AUTO_CONTINUOUS = 1,
        }

        /// <summary>
        /// 触发源
        /// </summary>
        public enum MV_CAM_TRIGGER_SOURCE
        {
            /// <summary>
            /// LINE0
            /// </summary>
            MV_TRIGGER_SOURCE_LINE0 = 0,

            /// <summary>
            /// LINE1
            /// </summary>
            MV_TRIGGER_SOURCE_LINE1 = 1,

            /// <summary>
            /// LINE2
            /// </summary>
            MV_TRIGGER_SOURCE_LINE2 = 2,

            /// <summary>
            /// LINE3
            /// </summary>
            MV_TRIGGER_SOURCE_LINE3 = 3,

            /// <summary>
            /// COUNTER0
            /// </summary>
            MV_TRIGGER_SOURCE_COUNTER0 = 4,

            /// <summary>
            /// SOFTWARE
            /// </summary>
            MV_TRIGGER_SOURCE_SOFTWARE = 7,

            /// <summary>
            /// FrequencyConverter
            /// </summary>
            MV_TRIGGER_SOURCE_FrequencyConverter = 8,
        }

        /// <summary>
        /// ALL MATHCH INFO
        /// </summary>
        public struct MV_ALL_MATCH_INFO
        {
            /// <summary>
            /// 需要输出的信息类型，e.g. MV_MATCH_TYPE_NET_DETECT
            /// </summary>
            public UInt32 nType;

            /// <summary>
            /// 输出的信息缓存，由调用者分配
            /// </summary>
            public IntPtr pInfo;

            /// <summary>
            /// 信息缓存的大小
            /// </summary>
            public UInt32 nInfoSize;
        }

        /// <summary>
        /// 
        /// </summary>
        public struct MV_MATCH_INFO_NET_DETECT
        {
            /// <summary>
            /// 已接收数据大小  [统计StartGrabbing和StopGrabbing之间的数据量]
            /// </summary>
            public Int64 nReviceDataSize;

            /// <summary>
            /// 丢失的包数量
            /// </summary>
            public Int64 nLostPacketCount;

            /// <summary>
            /// 丢帧数量
            /// </summary>
            public UInt32 nLostFrameCount;

            /// <summary>
            /// 帧数
            /// </summary>
            public UInt32 nNetRecvFrameCount;

            /// <summary>
            /// 请求重发包数
            /// </summary>
            public Int64 nRequestResendPacketCount;

            /// <summary>
            /// 重发包数
            /// </summary>
            public Int64 nResendPacketCount;
        }

        /// <summary>
        /// USB
        /// </summary>
        public struct MV_MATCH_INFO_USB_DETECT
        {
            /// <summary>
            /// 已接收数据大小    [统计OpenDevicce和CloseDevice之间的数据量]
            /// </summary>
            public Int64 nReviceDataSize;

            /// <summary>
            /// 已收到的帧数
            /// </summary>
            public UInt32 nRevicedFrameCount;

            /// <summary>
            /// 错误帧数
            /// </summary>
            public UInt32 nErrorFrameCount;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 图像的基本信息
        /// </summary>
        public struct MV_IMAGE_BASIC_INFO
        {
            /// <summary>
            /// 宽度值
            /// </summary>
            public UInt16 nWidthValue;

            /// <summary>
            /// 宽度最小值
            /// </summary>
            public UInt16 nWidthMin;

            /// <summary>
            /// 宽度最大值
            /// </summary>
            public UInt32 nWidthMax;

            /// <summary>
            /// Width Inc
            /// </summary>
            public UInt32 nWidthInc;

            /// <summary>
            /// 高度值
            /// </summary>
            public UInt32 nHeightValue;

            /// <summary>
            /// 高度最小值
            /// </summary>
            public UInt32 nHeightMin;

            /// <summary>
            /// 高度最大值
            /// </summary>
            public UInt32 nHeightMax;

            /// <summary>
            /// Height Inc
            /// </summary>
            public UInt32 nHeightInc;

            /// <summary>
            /// 帧率
            /// </summary>
            public Single fFrameRateValue;

            /// <summary>
            /// 最小帧率
            /// </summary>
            public Single fFrameRateMin;

            /// <summary>
            /// 最大帧率
            /// </summary>
            public Single fFrameRateMax;

            /// <summary>
            /// 当前的像素格式
            /// </summary>
            public UInt32 enPixelType;

            /// <summary>
            /// 支持的像素格式种类
            /// </summary>
            public UInt32 nSupportedPixelFmtNum;

            /// <summary>
            /// 像素列表
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_SYMBOLIC_NUM)]
            public UInt32[] enPixelList;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 节点是否可见的权限等级
        /// </summary>
        public enum MV_XML_Visibility
        {
            /// <summary>
            /// Always visible
            /// </summary>
            V_Beginner = 0,

            /// <summary>
            /// Visible for experts or Gurus
            /// </summary>        
            V_Expert = 1,

            /// <summary>
            /// Visible for Gurus
            /// </summary>
            V_Guru = 2,

            /// <summary>
            /// Not Visible
            /// </summary>
            V_Invisible = 3,

            /// <summary>
            /// Object is not yet initialized
            /// </summary>
            V_Undefined = 99
        }

        /// <summary>
        /// 事件信息
        /// </summary>
        public struct MV_EVENT_OUT_INFO
        {
            /// <summary>
            /// 事件名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_EVENT_NAME_SIZE)]
            public string EventName;

            /// <summary>
            /// Event号
            /// </summary>
            public UInt16 nEventID;

            /// <summary>
            /// 流通到序号
            /// </summary>
            public UInt16 nStreamChannel;

            /// <summary>
            /// 帧号高位
            /// </summary>
            public UInt32 nBlockIdHigh;

            /// <summary>
            /// 帧号低位
            /// </summary>
            public UInt32 nBlockIdLow;

            /// <summary>
            /// 时间戳高位
            /// </summary>
            public UInt32 nTimestampHigh;

            /// <summary>
            /// 时间戳低位
            /// </summary>
            public UInt32 nTimestampLow;

            /// <summary>
            /// Event数据
            /// </summary>
            public IntPtr pEventData;

            /// <summary>
            /// Event数据长度
            /// </summary>
            public UInt32 nEventDataSize;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 文件存取
        /// </summary>
        public struct MV_CC_FILE_ACCESS
        {
            /// <summary>
            /// 用户文件名
            /// </summary>
            public String pUserFileName;

            /// <summary>
            /// 设备文件名
            /// </summary>
            public String pDevFileName;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 文件存取
        /// </summary>
        public struct MV_CC_FILE_ACCESS_EX
        {
            /// <summary>
            /// 用户文件数据缓存空间
            /// </summary>
            public IntPtr pUserFileBuf;

            /// <summary>
            /// 用户数据缓存大小
            /// </summary>
            public UInt32 nFileBufSize;

            /// <summary>
            /// 文件实际缓存大小
            /// </summary>
            public UInt32 nFileBufLen;

            /// <summary>
            /// 设备文件名
            /// </summary>
            public String pDevFileName;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 文件存取进度
        /// </summary>
        public struct MV_CC_FILE_ACCESS_PROGRESS
        {
            /// <summary>
            /// 已完成的长度
            /// </summary>
            public Int64 nCompleted;

            /// <summary>
            /// 总长度
            /// </summary>
            public Int64 nTotal;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// GigE传输类型
        /// </summary>
        public enum MV_GIGE_TRANSMISSION_TYPE
        {
            /// <summary>
            /// 表示单播(默认)
            /// </summary>
            MV_GIGE_TRANSTYPE_UNICAST = 0x0,

            /// <summary>
            /// 表示组播
            /// </summary>
            MV_GIGE_TRANSTYPE_MULTICAST = 0x1,

            /// <summary>
            /// 表示局域网内广播，暂不支持
            /// </summary>
            MV_GIGE_TRANSTYPE_LIMITEDBROADCAST = 0x2,

            /// <summary>
            /// 表示子网内广播，暂不支持
            /// </summary>
            MV_GIGE_TRANSTYPE_SUBNETBROADCAST = 0x3,

            /// <summary>
            /// 表示从相机获取，暂不支持
            /// </summary>
            MV_GIGE_TRANSTYPE_CAMERADEFINED = 0x4,

            /// <summary>
            /// 表示用户自定义应用端接收图像数据Port号
            /// </summary>
            MV_GIGE_TRANSTYPE_UNICAST_DEFINED_PORT = 0x5,

            /// <summary>
            /// 表示设置了单播，但本实例不接收图像数据
            /// </summary>
            MV_GIGE_TRANSTYPE_UNICAST_WITHOUT_RECV = 0x00010000,

            /// <summary>
            /// 表示组播模式，但本实例不接收图像数据
            /// </summary>
            MV_GIGE_TRANSTYPE_MULTICAST_WITHOUT_RECV = 0x00010001,
        }


        /// <summary>
        /// 传输模式，可以为单播模式、组播模式等
        /// </summary>
        public struct MV_CC_TRANSMISSION_TYPE
        {
            /// <summary>
            /// 传输模式
            /// </summary>
            public MV_GIGE_TRANSMISSION_TYPE enTransmissionType;

            /// <summary>
            /// 目标IP，组播模式下有意义
            /// </summary>
            public UInt32 nDestIp;

            /// <summary>
            /// 目标Port，组播模式下有意义
            /// </summary>
            public UInt16 nDestPort;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 动作命令信息
        /// </summary>
        public struct MV_ACTION_CMD_INFO
        {
            /// <summary>
            /// 设备密钥
            /// </summary>
            public UInt32 nDeviceKey;

            /// <summary>
            /// 组键
            /// </summary>
            public UInt32 nGroupKey;

            /// <summary>
            /// 组掩码
            /// </summary>
            public UInt32 nGroupMask;

            /// <summary>
            /// 只有设置成1时Action Time才有效，非1时无效
            /// </summary>
            public UInt32 bActionTimeEnable;

            /// <summary>
            /// 预定的时间，和主频有关
            /// </summary>
            public Int64 nActionTime;

            /// <summary>
            /// 广播包地址
            /// </summary>
            public String pBroadcastAddress;

            /// <summary>
            /// 等待ACK的超时时间，如果为0表示不需要ACK
            /// </summary>
            public UInt32 nTimeOut;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 动作命令结果
        /// </summary>
        public struct MV_ACTION_CMD_RESULT
        {
            /// <summary>
            /// IP address of the device
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public String strDeviceAddress;

            /// <summary>
            /// status code returned by the device
            /// </summary>
            public Int32 nStatus;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 动作命令结果列表
        /// </summary>
        public struct MV_ACTION_CMD_RESULT_LIST
        {
            /// <summary>
            /// 返回值个数
            /// </summary>
            public UInt32 nNumResults;

            /// <summary>
            /// 返回的结果
            /// </summary>
            public IntPtr pResults;
        }

        /// <summary>
        /// 每个节点对应的接口类型
        /// </summary>
        public enum MV_XML_InterfaceType
        {
            /// <summary>
            /// IValue接口类型
            /// </summary>
            IFT_IValue,

            /// <summary>
            /// IBase接口类型
            /// </summary>
            IFT_IBase,

            /// <summary>
            /// IInteger接口类型
            /// </summary>
            IFT_IInteger,

            /// <summary>
            /// IBoolean接口类型
            /// </summary>
            IFT_IBoolean,

            /// <summary>
            /// ICommand接口类型
            /// </summary>
            IFT_ICommand,

            /// <summary>
            /// IFloat接口类型
            /// </summary>
            IFT_IFloat,

            /// <summary>
            /// IString接口类型
            /// </summary>
            IFT_IString,

            /// <summary>
            /// IRegister接口类型
            /// </summary>
            IFT_IRegister,

            /// <summary>
            /// ICategory接口类型
            /// </summary>
            IFT_ICategory,

            /// <summary>
            /// IEnumeration接口类型
            /// </summary>
            IFT_IEnumeration,

            /// <summary>
            /// IEnumEntry接口类型
            /// </summary>
            IFT_IEnumEntry,

            /// <summary>
            /// IPort接口类型
            /// </summary>
            IFT_IPort,
        }


        /// <summary>
        /// XML节点特点
        /// </summary>
        public struct MV_XML_NODE_FEATURE
        {
            /// <summary>
            /// 节点类型
            /// </summary>
            public MV_XML_InterfaceType enType;

            /// <summary>
            /// 是否可见
            /// </summary>
            public MV_XML_Visibility enVisivility;

            /// <summary>
            /// 节点描述
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;

            /// <summary>
            /// 显示名称
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;

            /// <summary>
            /// 节点名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;

            /// <summary>
            /// 提示
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// XML节点列表
        /// </summary>
        public struct MV_XML_NODES_LIST
        {
            /// <summary>
            /// 节点个数
            /// </summary>
            public UInt32 nNodeNum;

            /// <summary>
            /// 节点列表
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_NODE_NUM_C)]
            public MV_XML_NODE_FEATURE[] stNodes;
        }

        /// <summary>
        /// 整型节点值
        /// </summary>
        public struct MVCC_INTVALUE
        {
            /// <summary>
            /// 当前值
            /// </summary>
            public UInt32 nCurValue;

            /// <summary>
            /// 最大值
            /// </summary> 
            public UInt32 nMax;

            /// <summary>
            /// 最小值
            /// </summary>
            public UInt32 nMin;

            /// <summary>
            /// Inc
            /// </summary>
            public UInt32 nInc;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 整型节点值
        /// </summary>
        public struct MVCC_INTVALUE_EX
        {
            /// <summary>
            /// 当前值
            /// </summary>
            public Int64 nCurValue;

            /// <summary>
            /// 最大值
            /// </summary>
            public Int64 nMax;

            /// <summary>
            /// 最小值
            /// </summary>
            public Int64 nMin;

            /// <summary>
            /// Inc
            /// </summary>
            public Int64 nInc;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 浮点型节点值
        /// </summary>
        public struct MVCC_FLOATVALUE
        {
            /// <summary>
            /// 当前值
            /// </summary>
            public Single fCurValue;

            /// <summary>
            /// 最大值
            /// </summary>
            public Single fMax;

            /// <summary>
            /// 最小值
            /// </summary>
            public Single fMin;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 枚举型节点值
        /// </summary>
        public struct MVCC_ENUMVALUE
        {
            /// <summary>
            /// 当前值
            /// </summary>
            public UInt32 nCurValue;

            /// <summary>
            /// 有效数据个数
            /// </summary>
            public UInt32 nSupportedNum;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_SYMBOLIC_NUM)]
            public UInt32[] nSupportValue;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 字符串型节点值
        /// </summary>
        public struct MVCC_STRINGVALUE
        {
            /// <summary>
            /// 当前值
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string chCurValue;

            /// <summary>
            /// 节点值的最大长度
            /// </summary>
            public Int64 nMaxLength;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 节点的读写性
        /// </summary>
        public enum MV_XML_AccessMode
        {
            /// <summary>
            /// 未实现
            /// </summary>
            AM_NI,

            /// <summary>
            /// 不可获取
            /// </summary>
            AM_NA,

            /// <summary>
            /// 只写
            /// </summary>
            AM_WO,

            /// <summary>
            /// 只读
            /// </summary>
            AM_RO,

            /// <summary>
            /// 可读可写
            /// </summary>
            AM_RW,

            /// <summary>
            /// 未定义
            /// </summary>
            AM_Undefined,

            /// <summary>
            /// 内部用于AccessMode循环检测
            /// </summary>
            AM_CycleDetect
        }

        /// <summary>
        /// 整型节点
        /// </summary>
        public struct MV_XML_FEATURE_Integer
        {
            /// <summary>
            /// 节点名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;

            /// <summary>
            /// 显示名称
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;

            /// <summary>
            /// 节点描述
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;

            /// <summary>
            /// 提示
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;

            /// <summary>
            /// 是否可见
            /// </summary>
            public MV_XML_Visibility enVisivility;

            /// <summary>
            /// 访问模式
            /// </summary>
            public MV_XML_AccessMode enAccessMode;

            /// <summary>
            /// 是否锁定。0-否；1-是
            /// </summary>
            public Int32 bIsLocked;

            /// <summary>
            /// 当前值
            /// </summary>
            public Int64 nValue;

            /// <summary>
            /// 最小值
            /// </summary>
            public Int64 nMinValue;

            /// <summary>
            /// 最大值
            /// </summary>
            public Int64 nMaxValue;

            /// <summary>
            /// 增量
            /// </summary>
            public Int64 nIncrement;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 布尔型节点
        /// </summary>
        public struct MV_XML_FEATURE_Boolean
        {
            /// <summary>
            /// 节点名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;

            /// <summary>
            /// 显示名称
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;

            /// <summary>
            /// 节点描述
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;

            /// <summary>
            /// 提示
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;

            /// <summary>
            /// 是否可见
            /// </summary>
            public MV_XML_Visibility enVisivility;

            /// <summary>
            /// 访问模式
            /// </summary>
            public MV_XML_AccessMode enAccessMode;

            /// <summary>
            /// 是否锁定。0-否；1-是
            /// </summary>
            public Int32 bIsLocked;

            /// <summary>
            /// 当前值
            /// </summary>
            public bool bValue;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 命令型节点
        /// </summary>
        public struct MV_XML_FEATURE_Command
        {
            /// <summary>
            /// 节点名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;

            /// <summary>
            /// 显示名称
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;

            /// <summary>
            /// 节点描述
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;

            /// <summary>
            /// 提示
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;

            /// <summary>
            /// 是否可见
            /// </summary>
            public MV_XML_Visibility enVisivility;

            /// <summary>
            /// 访问模式
            /// </summary>
            public MV_XML_AccessMode enAccessMode;

            /// <summary>
            /// 是否锁定。0-否；1-是
            /// </summary>
            public Int32 bIsLocked;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 浮点型节点
        /// </summary>
        public struct MV_XML_FEATURE_Float
        {
            /// <summary>
            /// 节点名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;

            /// <summary>
            /// 显示名称
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;

            /// <summary>
            /// 节点描述
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;

            /// <summary>
            /// 提示
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;

            /// <summary>
            /// 是否可见
            /// </summary>
            public MV_XML_Visibility enVisivility;

            /// <summary>
            /// 访问模式
            /// </summary>
            public MV_XML_AccessMode enAccessMode;

            /// <summary>
            /// 是否锁定。0-否；1-是
            /// </summary>
            public Int32 bIsLocked;

            /// <summary>
            /// 当前值
            /// </summary>
            public Double dfValue;

            /// <summary>
            /// 最小值
            /// </summary>
            public Double dfMinValue;

            /// <summary>
            /// 最大值
            /// </summary>
            public Double dfMaxValue;

            /// <summary>
            /// 增量
            /// </summary>
            public Double dfIncrement;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 字符串类型节点
        /// </summary>
        public struct MV_XML_FEATURE_String
        {
            /// <summary>
            /// 节点名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;

            /// <summary>
            /// 显示名称
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;

            /// <summary>
            /// 节点描述
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;

            /// <summary>
            /// 提示
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;

            /// <summary>
            /// 是否可见
            /// </summary>
            public MV_XML_Visibility enVisivility;

            /// <summary>
            /// 访问模式
            /// </summary>
            public MV_XML_AccessMode enAccessMode;

            /// <summary>
            /// 是否锁定。0-否；1-是
            /// </summary>
            public Int32 bIsLocked;

            /// <summary>
            /// 当前值
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_STRVALUE_STRLEN_C)]
            public string strValue;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 寄存器型节点
        /// </summary>
        public struct MV_XML_FEATURE_Register
        {
            /// <summary>
            /// 节点名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;

            /// <summary>
            /// 显示名称
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;

            /// <summary>
            /// 节点描述
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;

            /// <summary>
            /// 提示
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;

            /// <summary>
            /// 是否可见
            /// </summary>
            public MV_XML_Visibility enVisivility;

            /// <summary>
            /// 访问模式
            /// </summary>
            public MV_XML_AccessMode enAccessMode;

            /// <summary>
            /// 是否锁定。0-否；1-是
            /// </summary>
            public Int32 bIsLocked;

            /// <summary>
            /// 当前值
            /// </summary>
            public Int64 nAddrValue;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 类别属性
        /// </summary>
        public struct MV_XML_FEATURE_Category
        {
            /// <summary>
            /// 节点描述
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;

            /// <summary>
            /// 显示名称
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;

            /// <summary>
            /// 节点名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;

            /// <summary>
            /// 提示
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;

            /// <summary>
            /// 是否可见
            /// </summary>
            public MV_XML_Visibility enVisivility;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// EnumEntry属性节点
        /// </summary>
        public struct MV_XML_FEATURE_EnumEntry
        {
            /// <summary>
            /// 节点名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;

            /// <summary>
            /// 显示名称
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;

            /// <summary>
            /// 节点描述
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;

            /// <summary>
            /// 提示
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;

            /// <summary>
            /// 
            /// </summary>
            public Int32 bIsImplemented;

            /// <summary>
            /// 父节点数
            /// </summary>
            public Int32 nParentsNum;

            /// <summary>
            /// 父节点列表
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_PARENTS_NUM)]
            public MV_XML_NODE_FEATURE[] stParentsList;

            /// <summary>
            /// 是否可见
            /// </summary>
            public MV_XML_Visibility enVisivility;

            /// <summary>
            /// 当前值
            /// </summary>
            public Int64 nValue;

            /// <summary>
            /// 访问模式
            /// </summary>
            public MV_XML_AccessMode enAccessMode;

            /// <summary>
            /// 是否锁定。0-否；1-是
            /// </summary>
            public Int32 bIsLocked;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 节点描述
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct StrSymbolic
        {
            /// <summary>
            /// 节点描述
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_SYMBOLIC_STRLEN_C)]
            public string str;
        }

        /// <summary>
        /// Enumeration属性节点
        /// </summary>
        public struct MV_XML_FEATURE_Enumeration
        {
            /// <summary>
            /// 是否可见
            /// </summary>
            public MV_XML_Visibility enVisivility;

            /// <summary>
            /// 节点描述
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;

            /// <summary>
            /// 显示名称
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;

            /// <summary>
            /// 节点名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;

            /// <summary>
            /// 提示
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;

            /// <summary>
            /// Symbolic数
            /// </summary>
            public Int32 nSymbolicNum;

            /// <summary>
            /// 当前Symbolic索引
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_SYMBOLIC_STRLEN_C)]
            public string strCurrentSymbolic;

            /// <summary>
            /// Symbolic索引
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_XML_SYMBOLIC_NUM)]
            public StrSymbolic[] strSymbolic;

            /// <summary>
            /// 访问模式
            /// </summary>
            public MV_XML_AccessMode enAccessMode;

            /// <summary>
            /// 是否锁定。0-否；1-是
            /// </summary>
            public Int32 bIsLocked;

            /// <summary>
            /// 当前值
            /// </summary>
            public Int64 nValue;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// Port属性节点
        /// </summary>
        public struct MV_XML_FEATURE_Port
        {
            /// <summary>
            /// 是否可见
            /// </summary>
            public MV_XML_Visibility enVisivility;

            /// <summary>
            /// 节点描述
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strDescription;

            /// <summary>
            /// 显示名称
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strDisplayName;

            /// <summary>
            /// 节点名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_NODE_STRLEN_C)]
            public string strName;

            /// <summary>
            /// 提示
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MV_MAX_XML_DISC_STRLEN_C)]
            public string strToolTip;

            /// <summary>
            /// 访问模式
            /// </summary>
            public MV_XML_AccessMode enAccessMode;

            /// <summary>
            /// 是否锁定。0-否；1-是
            /// </summary>
            public Int32 bIsLocked;

            /// <summary>
            /// 保留字节
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>辅助线颜色</summary>
        public struct MVCC_COLORF
        {
            /// <summary>[0.0 , 1.0]</summary>
            public float fR;

            /// <summary>[0.0 , 1.0]</summary>
            public float fG;

            /// <summary>[0.0 , 1.0]</summary>
            public float fB;

            /// <summary>[0.0 , 1.0]</summary>
            public float fAlpha;

            /// <summary>预留字节</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>自定义点坐标</summary>
        public struct MVCC_POINTF
        {
            /// <summary>[0.0 , 1.0]</summary>
            public float fX;

            /// <summary>[0.0 , 1.0]</summary>
            public float fY;

            /// <summary>预留字节</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>矩形框区域信息</summary>
        public struct MVCC_RECT_INFO
        {
            /// <summary>[0.0 , 1.0]</summary>
            public float fTop;

            /// <summary>[0.0 , 1.0]</summary>
            public float fBottom;

            /// <summary>[0.0 , 1.0]</summary>
            public float fLeft;

            /// <summary>[0.0 , 1.0]</summary>
            public float fRight;

            /// <summary>辅助线颜色</summary>
            public MVCC_COLORF stColor;

            /// <summary>辅助线宽度</summary>
            public UInt32 nLineWidth;

            /// <summary>预留字节</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>圆形框区域信息</summary>
        public struct MVCC_CIRCLE_INFO
        {
            /// <summary>圆心信息</summary>
            public MVCC_POINTF stCenterPoint;

            /// <summary>宽向半径，根据图像的相对位置[0, 1.0]</summary>
            public float fR1;

            /// <summary>高向半径，根据图像的相对位置[0, 1.0]</summary>
            public float fR2;

            /// <summary>辅助线颜色信息</summary>
            public MVCC_COLORF stColor;

            /// <summary>辅助线宽度</summary>
            public UInt32 nLineWidth;

            /// <summary>预留字节</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>线条辅助线信息</summary>
        public struct MVCC_LINES_INFO
        {
            /// <summary>线条辅助线的起始点坐标</summary>
            public MVCC_POINTF stStartPoint;

            /// <summary>线条辅助线的终点坐标</summary>
            public MVCC_POINTF stEndPoint;

            /// <summary>辅助线颜色信息</summary>
            public MVCC_COLORF stColor;

            /// <summary>辅助线宽度</summary>
            public UInt32 nLineWidth;

            /// <summary>预留字节</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>枚举类型指定条目信息</summary>
        public struct MVCC_ENUMENTRY
        {
            /// <summary>指定值</summary>
            public UInt32 nValue;

            /// <summary>指定值对应的符号</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_SYMBOLIC_LEN)]
            public Byte[] chSymbolic;

            /// <summary>预留字节</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }

        /// <summary>U3V流异常类型</summary>
        public enum MV_CC_STREAM_EXCEPTION_TYPE
        {
            /// <summary>异常的图像，该帧被丢弃</summary>
            MV_CC_STREAM_EXCEPTION_ABNORMAL_IMAGE = 0x4001,

            /// <summary>缓存列表溢出，清除最旧的一帧</summary>
            MV_CC_STREAM_EXCEPTION_LIST_OVERFLOW = 0x4002,

            /// <summary>缓存列表为空，该帧被丢弃</summary>
            MV_CC_STREAM_EXCEPTION_LIST_EMPTY = 0x4003,

            /// <summary>断流恢复</summary>
            MV_CC_STREAM_EXCEPTION_RECONNECTION = 0x4004,

            /// <summary>断流,恢复失败,取流被中止</summary>
            MV_CC_STREAM_EXCEPTION_DISCONNECTED = 0x4005,

            /// <summary>设备异常,取流被中止</summary>
            MV_CC_STREAM_EXCEPTION_DEVICE = 0x4006,
        }

        /// <summary>重构后的图像列表</summary>
        public struct MV_OUTPUT_IMAGE_INFO
        {
            /// <summary>源图像宽</summary>
            public UInt32 nWidth;

            /// <summary>源图像高</summary>
            public UInt32 nHeight;

            /// <summary>像素格式</summary>
            public MvGvspPixelType enPixelType;

            /// <summary>输出数据缓存</summary>
            public IntPtr pBuf;

            /// <summary>输出数据长度</summary>
            public UInt32 nBufLen;

            /// <summary>提供的输出缓冲区大小</summary>
            public UInt32 nBufSize;

            /// <summary>预留字节</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public UInt32[] nReserved;
        }

        /// <summary>
        /// 分时曝光的图像处理方式
        /// </summary>
        public enum MV_IMAGE_RECONSTRUCTION_METHOD
        {
            /// <summary>
            /// 源图像按行拆分成多张图像
            /// </summary>
            MV_SPLIT_BY_LINE = 1,
        }

        /// <summary>重构图像参数信息</summary>
        public struct MV_RECONSTRUCT_IMAGE_PARAM
        {
            /// <summary>源图像宽</summary>
            public UInt32 nWidth;

            /// <summary>源图像高</summary>
            public UInt32 nHeight;

            /// <summary>像素格式</summary>
            public MvGvspPixelType enPixelType;

            /// <summary>输入数据缓存</summary>
            public IntPtr pSrcData;

            /// <summary>输入数据长度</summary>
            public UInt32 nSrcDataLen;

            /// <summary>曝光个数(1-8]</summary>
            public UInt32 nExposureNum;

            /// <summary>图像重构方式</summary>
            public MV_IMAGE_RECONSTRUCTION_METHOD enReconstructMethod;

            /// <summary>
            /// 输出数据缓存信息
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MV_MAX_SPLIT_NUM)]
            public MV_OUTPUT_IMAGE_INFO[] stDstBufList;

            /// <summary>预留字节</summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt32[] nReserved;
        }
        #endregion

        #region 像素格式定义
        /// <summary>
        /// 像素格式定义
        /// </summary>
        public enum MvGvspPixelType
        {
            /// <summary>
            /// 未定义像素格式
            /// </summary>
            PixelType_Gvsp_Undefined = -1,

            /// <summary>
            /// Mono1p
            /// </summary>
            PixelType_Gvsp_Mono1p = 0x01010037,

            /// <summary>
            /// Mono2p
            /// </summary>
            PixelType_Gvsp_Mono2p = 0x01020038,

            /// <summary>
            /// Mono4p
            /// </summary>
            PixelType_Gvsp_Mono4p = 0x01040039,

            /// <summary>
            /// Mono8
            /// </summary>
            PixelType_Gvsp_Mono8 = 0x01080001,

            /// <summary>
            /// Mono8_Signed
            /// </summary>
            PixelType_Gvsp_Mono8_Signed = 0x01080002,

            /// <summary>
            /// Mono10
            /// </summary>
            PixelType_Gvsp_Mono10 = 0x01100003,

            /// <summary>
            /// Mono10_Packed
            /// </summary>
            PixelType_Gvsp_Mono10_Packed = 0x010c0004,

            /// <summary>
            /// Mono12
            /// </summary>
            PixelType_Gvsp_Mono12 = 0x01100005,

            /// <summary>
            /// Mono12_Packed
            /// </summary>
            PixelType_Gvsp_Mono12_Packed = 0x010c0006,

            /// <summary>
            /// Mono14
            /// </summary>
            PixelType_Gvsp_Mono14 = 0x01100025,

            /// <summary>
            /// Mono16
            /// </summary>
            PixelType_Gvsp_Mono16 = 0x01100007,

            /// <summary>
            /// BayerGR8
            /// </summary>
            PixelType_Gvsp_BayerGR8 = 0x01080008,

            /// <summary>
            /// BayerRG8
            /// </summary>
            PixelType_Gvsp_BayerRG8 = 0x01080009,

            /// <summary>
            /// BayerGB8
            /// </summary>
            PixelType_Gvsp_BayerGB8 = 0x0108000a,

            /// <summary>
            /// BayerBG8
            /// </summary>
            PixelType_Gvsp_BayerBG8 = 0x0108000b,

            /// <summary>
            /// BayerRBGG8
            /// </summary>
            PixelType_Gvsp_BayerRBGG8 = 0x01080046,

            /// <summary>
            /// BayerGR10
            /// </summary>
            PixelType_Gvsp_BayerGR10 = 0x0110000c,

            /// <summary>
            /// BayerRG10
            /// </summary>
            PixelType_Gvsp_BayerRG10 = 0x0110000d,

            /// <summary>
            /// BayerGB10
            /// </summary>
            PixelType_Gvsp_BayerGB10 = 0x0110000e,

            /// <summary>
            /// BayerBG10
            /// </summary>
            PixelType_Gvsp_BayerBG10 = 0x0110000f,

            /// <summary>
            /// BayerGR12
            /// </summary>
            PixelType_Gvsp_BayerGR12 = 0x01100010,

            /// <summary>
            /// BayerRG12
            /// </summary>
            PixelType_Gvsp_BayerRG12 = 0x01100011,

            /// <summary>
            /// BayerGB12
            /// </summary>
            PixelType_Gvsp_BayerGB12 = 0x01100012,

            /// <summary>
            /// BayerBG12
            /// </summary>
            PixelType_Gvsp_BayerBG12 = 0x01100013,

            /// <summary>
            /// BayerGR10_Packed
            /// </summary>
            PixelType_Gvsp_BayerGR10_Packed = 0x010c0026,

            /// <summary>
            /// BayerRG10_Packed
            /// </summary>
            PixelType_Gvsp_BayerRG10_Packed = 0x010c0027,

            /// <summary>
            /// BayerGB10_Packed
            /// </summary>
            PixelType_Gvsp_BayerGB10_Packed = 0x010c0028,

            /// <summary>
            /// BayerBG10_Packed
            /// </summary>
            PixelType_Gvsp_BayerBG10_Packed = 0x010c0029,

            /// <summary>
            /// BayerGR12_Packed
            /// </summary>
            PixelType_Gvsp_BayerGR12_Packed = 0x010c002a,

            /// <summary>BayerRG12_Packed</summary>
            PixelType_Gvsp_BayerRG12_Packed = 0x010c002b,

            /// <summary>BayerGB12_Packed</summary>
            PixelType_Gvsp_BayerGB12_Packed = 0x010c002c,

            /// <summary>BayerBG12_Packed</summary>
            PixelType_Gvsp_BayerBG12_Packed = 0x010c002d,

            /// <summary>BayerGR16</summary>
            PixelType_Gvsp_BayerGR16 = 0x0110002e,

            /// <summary>BayerRG16</summary>
            PixelType_Gvsp_BayerRG16 = 0x0110002f,

            /// <summary>BayerGB16</summary>
            PixelType_Gvsp_BayerGB16 = 0x01100030,

            /// <summary>BayerBG16</summary>
            PixelType_Gvsp_BayerBG16 = 0x01100031,

            /// <summary>RGB8_Packed</summary>
            PixelType_Gvsp_RGB8_Packed = 0x02180014,

            /// <summary>BGR8_Packed</summary>
            PixelType_Gvsp_BGR8_Packed = 0x02180015,

            /// <summary>RGBA8_Packed</summary>
            PixelType_Gvsp_RGBA8_Packed = 0x02200016,

            /// <summary>BGRA8_Packed</summary>
            PixelType_Gvsp_BGRA8_Packed = 0x02200017,

            /// <summary>RGB10_Packed</summary>
            PixelType_Gvsp_RGB10_Packed = 0x02300018,

            /// <summary>BGR10_Packed</summary>
            PixelType_Gvsp_BGR10_Packed = 0x02300019,

            /// <summary>RGB12_Packed</summary>
            PixelType_Gvsp_RGB12_Packed = 0x0230001a,

            /// <summary>BGR12_Packed</summary>
            PixelType_Gvsp_BGR12_Packed = 0x0230001b,

            /// <summary>RGB16_Packed</summary>
            PixelType_Gvsp_RGB16_Packed = 0x02300033,

            /// <summary>BGR16_Packed/// </summary>
            PixelType_Gvsp_BGR16_Packed = 0x0230004b,

            /// <summary>RGBA16_Packed</summary>
            PixelType_Gvsp_RGBA16_Packed = 0x02400040,

            /// <summary>BGRA16_Packed</summary>
            PixelType_Gvsp_BGRA16_Packed = 0x02400051,

            /// <summary>RGB10V1_Packe</summary>
            PixelType_Gvsp_RGB10V1_Packed = 0x0220001c,

            /// <summary>RGB10V2_Packed</summary>
            PixelType_Gvsp_RGB10V2_Packed = 0x0220001d,

            /// <summary>RGB12V1_Packed</summary>
            PixelType_Gvsp_RGB12V1_Packed = 0x02240034,

            /// <summary>RGB565_Packed</summary>
            PixelType_Gvsp_RGB565_Packed = 0x02100035,

            /// <summary>BGR565_Packed</summary>
            PixelType_Gvsp_BGR565_Packed = 0x02100036,

            /// <summary>YUV411_Packed</summary>
            PixelType_Gvsp_YUV411_Packed = 0x020c001e,

            /// <summary>YUV422_Packed</summary>
            PixelType_Gvsp_YUV422_Packed = 0x0210001f,

            /// <summary>YUV422_YUYV_Packed</summary>
            PixelType_Gvsp_YUV422_YUYV_Packed = 0x02100032,

            /// <summary>YUV444_Packed</summary>
            PixelType_Gvsp_YUV444_Packed = 0x02180020,

            /// <summary>YCBCR8_CBYCR</summary>
            PixelType_Gvsp_YCBCR8_CBYCR = 0x0218003a,

            /// <summary>YCBCR422_8</summary>
            PixelType_Gvsp_YCBCR422_8 = 0x0210003b,

            /// <summary>YCBCR422_8_CBYCRY</summary>
            PixelType_Gvsp_YCBCR422_8_CBYCRY = 0x02100043,

            /// <summary>YCBCR411_8_CBYYCRYY</summary>
            PixelType_Gvsp_YCBCR411_8_CBYYCRYY = 0x020c003c,

            /// <summary>YCBCR601_8_CBYCR</summary>
            PixelType_Gvsp_YCBCR601_8_CBYCR = 0x0218003d,

            /// <summary>YCBCR601_422_8</summary>
            PixelType_Gvsp_YCBCR601_422_8 = 0x0210003e,

            /// <summary>YCBCR601_422_8_CBYCRY</summary>
            PixelType_Gvsp_YCBCR601_422_8_CBYCRY = 0x02100044,

            /// <summary>YCBCR601_411_8_CBYYCRYY</summary>
            PixelType_Gvsp_YCBCR601_411_8_CBYYCRYY = 0x020c003f,

            /// <summary>YCBCR709_8_CBYCR</summary>
            PixelType_Gvsp_YCBCR709_8_CBYCR = 0x02180040,

            /// <summary>YCBCR709_422_8</summary>
            PixelType_Gvsp_YCBCR709_422_8 = 0x02100041,

            /// <summary>YCBCR709_422_8_CBYCRY</summary>
            PixelType_Gvsp_YCBCR709_422_8_CBYCRY = 0x02100045,

            /// <summary>YCBCR709_411_8_CBYYCRYY</summary>
            PixelType_Gvsp_YCBCR709_411_8_CBYYCRYY = 0x020c0042,

            /// <summary>YUV420SP_NV12</summary>
            PixelType_Gvsp_YUV420SP_NV12 = 0X020c8001,

            /// <summary>YUV420SP_NV21</summary>
            PixelType_Gvsp_YUV420SP_NV21 = 0X020c8002,

            /// <summary>RGB8_Planar</summary>
            PixelType_Gvsp_RGB8_Planar = 0x02180021,

            /// <summary>RGB10_Planar</summary>
            PixelType_Gvsp_RGB10_Planar = 0x02300022,

            /// <summary>RGB12_Planar</summary>
            PixelType_Gvsp_RGB12_Planar = 0x02300023,

            /// <summary>RGB16_Planar</summary>
            PixelType_Gvsp_RGB16_Planar = 0x02300024,

            /// <summary>Jpeg</summary>
            PixelType_Gvsp_Jpeg = unchecked((Int32)0x80180001),

            /// <summary>Coord3D_ABC32f</summary>
            PixelType_Gvsp_Coord3D_ABC32f = 0x026000C0,

            /// <summary>Coord3D_ABC32f_Planar</summary>
            PixelType_Gvsp_Coord3D_ABC32f_Planar = 0x026000C1,

            /// <summary>Coord3D_AC32f</summary>
            PixelType_Gvsp_Coord3D_AC32f = 0x024000C2,//3D coordinate A-C 32-bit floating point

            /// <summary>COORD3D_DEPTH_PLUS_MASK</summary>
            PixelType_Gvsp_COORD3D_DEPTH_PLUS_MASK = unchecked((Int32)0x821c0001),

            /// <summary>Coord3D_ABC32</summary>
            PixelType_Gvsp_Coord3D_ABC32 = unchecked((Int32)0x82603001),

            /// <summary>Coord3D_AB32f</summary>
            PixelType_Gvsp_Coord3D_AB32f = unchecked((Int32)0x82403002),

            /// <summary>Coord3D_AB32</summary>
            PixelType_Gvsp_Coord3D_AB32 = unchecked((Int32)0x82403003),

            /// <summary>Coord3D_AC32f_64</summary>
            PixelType_Gvsp_Coord3D_AC32f_64 = unchecked((Int32)0x024000C2),

            /// <summary>Coord3D_AC32f_Planar</summary>
            PixelType_Gvsp_Coord3D_AC32f_Planar = 0x024000C3,

            /// <summary>Coord3D_AC32</summary>
            PixelType_Gvsp_Coord3D_AC32 = unchecked((Int32)0x82403004),

            /// <summary>Coord3D_A32f</summary>
            PixelType_Gvsp_Coord3D_A32f = 0x012000BD,

            /// <summary>Coord3D_A32</summary>
            PixelType_Gvsp_Coord3D_A32 = unchecked((Int32)0x81203005),

            /// <summary>Coord3D_C32f</summary>
            PixelType_Gvsp_Coord3D_C32f = 0x012000BF,

            /// <summary>Coord3D_C32</summary>
            PixelType_Gvsp_Coord3D_C32 = unchecked((Int32)0x81203006),

            /// <summary>Coord3D_ABC16</summary>
            PixelType_Gvsp_Coord3D_ABC16 = 0x023000b9,

            /// <summary>Coord3D_C16</summary>
            PixelType_Gvsp_Coord3D_C16 = 0x011000b8,

            /// <summary>Float32</summary>
            PixelType_Gvsp_Float32 = unchecked((Int32)0x81200001),

            //无损压缩像素格式定义
            /// <summary>HB_Mono8</summary>
            PixelType_Gvsp_HB_Mono8 = unchecked((Int32)0x81080001),

            /// <summary>HB_Mono10</summary>
            PixelType_Gvsp_HB_Mono10 = unchecked((Int32)0x81100003),

            /// <summary>HB_Mono10_Packed</summary>
            PixelType_Gvsp_HB_Mono10_Packed = unchecked((Int32)0x810c0004),

            /// <summary>HB_Mono12</summary>
            PixelType_Gvsp_HB_Mono12 = unchecked((Int32)0x81100005),

            /// <summary>HB_Mono12_Packed</summary>
            PixelType_Gvsp_HB_Mono12_Packed = unchecked((Int32)0x810c0006),

            /// <summary>HB_Mono16</summary>
            PixelType_Gvsp_HB_Mono16 = unchecked((Int32)0x81100007),

            /// <summary>HB_BayerGR8</summary>
            PixelType_Gvsp_HB_BayerGR8 = unchecked((Int32)0x81080008),

            /// <summary>HB_BayerRG8</summary>
            PixelType_Gvsp_HB_BayerRG8 = unchecked((Int32)0x81080009),

            /// <summary>HB_BayerGB8</summary>
            PixelType_Gvsp_HB_BayerGB8 = unchecked((Int32)0x8108000a),

            /// <summary>HB_BayerBG8</summary>
            PixelType_Gvsp_HB_BayerBG8 = unchecked((Int32)0x8108000b),

            /// <summary>HB_BayerRBGG8</summary>
            PixelType_Gvsp_HB_BayerRBGG8 = unchecked((Int32)0x81080046),

            /// <summary>HB_BayerGR10</summary>
            PixelType_Gvsp_HB_BayerGR10 = unchecked((Int32)0x8110000c),

            /// <summary>HB_BayerRG10</summary>
            PixelType_Gvsp_HB_BayerRG10 = unchecked((Int32)0x8110000d),

            /// <summary>HB_BayerGB10</summary>
            PixelType_Gvsp_HB_BayerGB10 = unchecked((Int32)0x8110000e),

            /// <summary>HB_BayerBG10</summary>
            PixelType_Gvsp_HB_BayerBG10 = unchecked((Int32)0x8110000f),

            /// <summary>HB_BayerGR12</summary>
            PixelType_Gvsp_HB_BayerGR12 = unchecked((Int32)0x81100010),

            /// <summary>HB_BayerRG12</summary>
            PixelType_Gvsp_HB_BayerRG12 = unchecked((Int32)0x81100011),

            /// <summary>HB_BayerGB12</summary>
            PixelType_Gvsp_HB_BayerGB12 = unchecked((Int32)0x81100012),

            /// <summary>HB_BayerBG12</summary>
            PixelType_Gvsp_HB_BayerBG12 = unchecked((Int32)0x81100013),

            /// <summary>HB_BayerGR10_Packed</summary>
            PixelType_Gvsp_HB_BayerGR10_Packed = unchecked((Int32)0x810c0026),

            /// <summary>HB_BayerRG10_Packed</summary>
            PixelType_Gvsp_HB_BayerRG10_Packed = unchecked((Int32)0x810c0027),

            /// <summary>HB_BayerGB10_Packed</summary>
            PixelType_Gvsp_HB_BayerGB10_Packed = unchecked((Int32)0x810c0028),

            /// <summary>HB_BayerBG10_Packed</summary>
            PixelType_Gvsp_HB_BayerBG10_Packed = unchecked((Int32)0x810c0029),

            /// <summary>HB_BayerGR12_Packed</summary>
            PixelType_Gvsp_HB_BayerGR12_Packed = unchecked((Int32)0x810c002a),

            /// <summary>HB_BayerRG12_Packed</summary>
            PixelType_Gvsp_HB_BayerRG12_Packed = unchecked((Int32)0x810c002b),

            /// <summary>HB_BayerGB12_Packed</summary>
            PixelType_Gvsp_HB_BayerGB12_Packed = unchecked((Int32)0x810c002c),

            /// <summary>HB_BayerBG12_Packed</summary>
            PixelType_Gvsp_HB_BayerBG12_Packed = unchecked((Int32)0x810c002d),

            /// <summary>HB_YUV422_Packed</summary>
            PixelType_Gvsp_HB_YUV422_Packed = unchecked((Int32)0x8210001f),

            /// <summary>HB_YUV422_YUYV_Packed</summary>
            PixelType_Gvsp_HB_YUV422_YUYV_Packed = unchecked((Int32)0x82100032),

            /// <summary>HB_RGB8_Packed</summary>
            PixelType_Gvsp_HB_RGB8_Packed = unchecked((Int32)0x82180014),

            /// <summary>HB_BGR8_Packed</summary>
            PixelType_Gvsp_HB_BGR8_Packed = unchecked((Int32)0x82180015),

            /// <summary>HB_RGBA8_Packed</summary>
            PixelType_Gvsp_HB_RGBA8_Packed = unchecked((Int32)0x82200016),

            /// <summary>HB_BGRA8_Packed</summary>
            PixelType_Gvsp_HB_BGRA8_Packed = unchecked((Int32)0x82200017),

            /// <summary>HB_RGB16_Packed</summary>
            PixelType_Gvsp_HB_RGB16_Packed = unchecked((Int32)0x82300033),

            /// <summary>HB_BGR16_Packed</summary>
            PixelType_Gvsp_HB_BGR16_Packed = unchecked((Int32)0x8230004B),

            /// <summary>HB_RGBA16_Packed</summary>
            PixelType_Gvsp_HB_RGBA16_Packed = unchecked((Int32)0x82400064),

            /// <summary>HB_BGRA16_Packed</summary>
            PixelType_Gvsp_HB_BGRA16_Packed = unchecked((Int32)0x82400051),
        }
        #endregion

        #region 设备错误码定义
        /// <summary>成功，无错误</summary>
        public const Int32 MV_OK = unchecked((Int32)0x00000000);

        // 通用错误码定义:范围0x80000000-0x800000FF
        /// <summary>错误或无效的句柄</summary>
        public const Int32 MV_E_HANDLE = unchecked((Int32)0x80000000);
        /// <summary>不支持的功能</summary>
        public const Int32 MV_E_SUPPORT = unchecked((Int32)0x80000001);
        /// <summary>缓存已满</summary>
        public const Int32 MV_E_BUFOVER = unchecked((Int32)0x80000002);
        /// <summary>函数调用顺序错误</summary>
        public const Int32 MV_E_CALLORDER = unchecked((Int32)0x80000003);
        /// <summary>错误的参数</summary>
        public const Int32 MV_E_PARAMETER = unchecked((Int32)0x80000004);
        /// <summary>资源申请失败</summary>
        public const Int32 MV_E_RESOURCE = unchecked((Int32)0x80000006);
        /// <summary>无数据</summary>
        public const Int32 MV_E_NODATA = unchecked((Int32)0x80000007);
        /// <summary>前置条件有误，或运行环境已发生变化</summary>
        public const Int32 MV_E_PRECONDITION = unchecked((Int32)0x80000008);
        /// <summary>版本不匹配</summary>
        public const Int32 MV_E_VERSION = unchecked((Int32)0x80000009);
        /// <summary>传入的内存空间不足</summary>
        public const Int32 MV_E_NOENOUGH_BUF = unchecked((Int32)0x8000000A);
        /// <summary>异常图像，可能是丢包导致图像不完整</summary>
        public const Int32 MV_E_ABNORMAL_IMAGE = unchecked((Int32)0x8000000B);
        /// <summary>动态导入DLL失败</summary>
        public const Int32 MV_E_LOAD_LIBRARY = unchecked((Int32)0x8000000C);
        /// <summary>没有可输出的缓存</summary>
        public const Int32 MV_E_NOOUTBUF = unchecked((Int32)0x8000000D);
        /// <summary>加密错误</summary>
        public const Int32 MV_E_ENCRYPT = unchecked((Int32)0x8000000E);
        /// <summary>打开文件出现错误</summary>
        public const Int32 MV_E_OPENFILE = unchecked((Int32)0x8000000F);
        /// <summary>未知的错误</summary>
        public const Int32 MV_E_UNKNOW = unchecked((Int32)0x800000FF);

        // GenICam系列错误:范围0x80000100-0x800001FF
        /// <summary>通用错误</summary>
        public const Int32 MV_E_GC_GENERIC = unchecked((Int32)0x80000100);
        /// <summary>参数非法</summary>
        public const Int32 MV_E_GC_ARGUMENT = unchecked((Int32)0x80000101);
        /// <summary>值超出范围</summary>
        public const Int32 MV_E_GC_RANGE = unchecked((Int32)0x80000102);
        /// <summary>属性</summary>
        public const Int32 MV_E_GC_PROPERTY = unchecked((Int32)0x80000103);
        /// <summary>运行环境有问题</summary>
        public const Int32 MV_E_GC_RUNTIME = unchecked((Int32)0x80000104);
        /// <summary>逻辑错误</summary>
        public const Int32 MV_E_GC_LOGICAL = unchecked((Int32)0x80000105);
        /// <summary>节点访问条件有误</summary>
        public const Int32 MV_E_GC_ACCESS = unchecked((Int32)0x80000106);
        /// <summary>超时</summary>
        public const Int32 MV_E_GC_TIMEOUT = unchecked((Int32)0x80000107);
        /// <summary>转换异常</summary>
        public const Int32 MV_E_GC_DYNAMICCAST = unchecked((Int32)0x80000108);
        /// <summary>GenICam未知错误</summary>
        public const Int32 MV_E_GC_UNKNOW = unchecked((Int32)0x800001FF);

        // GigE_STATUS对应的错误码:范围0x80000200-0x800002FF
        /// <summary>命令不被设备支持</summary>
        public const Int32 MV_E_NOT_IMPLEMENTED = unchecked((Int32)0x80000200);
        /// <summary>访问的目标地址不存在</summary>
        public const Int32 MV_E_INVALID_ADDRESS = unchecked((Int32)0x80000201);
        /// <summary>目标地址不可写</summary>
        public const Int32 MV_E_WRITE_PROTECT = unchecked((Int32)0x80000202);
        /// <summary>设备无访问权限</summary>
        public const Int32 MV_E_ACCESS_DENIED = unchecked((Int32)0x80000203);
        /// <summary>设备忙，或网络断开</summary>
        public const Int32 MV_E_BUSY = unchecked((Int32)0x80000204);
        /// <summary>网络包数据错误</summary>
        public const Int32 MV_E_PACKET = unchecked((Int32)0x80000205);
        /// <summary>网络相关错误</summary>
        public const Int32 MV_E_NETER = unchecked((Int32)0x80000206);
        /// <summary>设备IP冲突</summary>
        public const Int32 MV_E_IP_CONFLICT = unchecked((Int32)0x80000221);

        // USB_STATUS对应的错误码:范围0x80000300-0x800003FF
        /// <summary>读usb出错</summary>
        public const Int32 MV_E_USB_READ = unchecked((Int32)0x80000300);
        /// <summary>写usb出错</summary>
        public const Int32 MV_E_USB_WRITE = unchecked((Int32)0x80000301);
        /// <summary>设备异常</summary>
        public const Int32 MV_E_USB_DEVICE = unchecked((Int32)0x80000302);
        /// <summary>GenICam相关错误</summary>
        public const Int32 MV_E_USB_GENICAM = unchecked((Int32)0x80000303);
        /// <summary>带宽不足</summary>
        public const Int32 MV_E_USB_BANDWIDTH = unchecked((Int32)0x80000304);
        /// <summary>驱动不匹配或者未装驱动</summary>
        public const Int32 MV_E_USB_DRIVER = unchecked((Int32)0x80000305);
        /// <summary>USB未知的错误</summary>
        public const Int32 MV_E_USB_UNKNOW = unchecked((Int32)0x800003FF);

        // 升级时对应的错误码:范围0x80000400-0x800004FF
        /// <summary>升级固件不匹配</summary>
        public const Int32 MV_E_UPG_FILE_MISMATCH = unchecked((Int32)0x80000400);
        /// <summary>升级固件语言不匹配</summary>
        public const Int32 MV_E_UPG_LANGUSGE_MISMATCH = unchecked((Int32)0x80000401);
        /// <summary>升级冲突（设备已经在升级了再次请求升级即返回此错误）</summary>
        public const Int32 MV_E_UPG_CONFLICT = unchecked((Int32)0x80000402);
        /// <summary>升级时设备内部出现错误</summary>
        public const Int32 MV_E_UPG_INNER_ERR = unchecked((Int32)0x80000403);
        /// <summary>升级时未知错误</summary>
        public const Int32 MV_E_UPG_UNKNOW = unchecked((Int32)0x800004FF);
        #endregion

        #region 来自ISP算法库的错误码
        // 通用类型
        /// <summary>处理正确</summary>
        public const Int32 MV_ALG_OK = unchecked((Int32)0x00000000);
        /// <summary>不确定类型错误</summary>
        public const Int32 MV_ALG_ERR = unchecked((Int32)0x10000000);

        // 能力检查
        /// <summary>能力集中存在无效参数</summary>
        public const Int32 MV_ALG_E_ABILITY_ARG = unchecked((Int32)0x10000001);

        // 内存检查
        /// <summary>内存地址为空</summary>
        public const Int32 MV_ALG_E_MEM_NULL = unchecked((Int32)0x10000002);
        /// <summary>内存对齐不满足要求</summary>
        public const Int32 MV_ALG_E_MEM_ALIGN = unchecked((Int32)0x10000003);
        /// <summary>内存空间大小不够</summary>
        public const Int32 MV_ALG_E_MEM_LACK = unchecked((Int32)0x10000004);
        /// <summary>内存空间大小不满足对齐要求</summary>
        public const Int32 MV_ALG_E_MEM_SIZE_ALIGN = unchecked((Int32)0x10000005);
        /// <summary>内存地址不满足对齐要求</summary>
        public const Int32 MV_ALG_E_MEM_ADDR_ALIGN = unchecked((Int32)0x10000006);

        // 图像检查
        /// <summary>图像格式不正确或者不支持</summary>
        public const Int32 MV_ALG_E_IMG_FORMAT = unchecked((Int32)0x10000007);
        /// <summary>图像宽高不正确或者超出范围</summary>
        public const Int32 MV_ALG_E_IMG_SIZE = unchecked((Int32)0x10000008);
        /// <summary>图像宽高与step参数不匹配</summary>
        public const Int32 MV_ALG_E_IMG_STEP = unchecked((Int32)0x10000009);
        /// <summary>图像数据存储地址为空</summary>
        public const Int32 MV_ALG_E_IMG_DATA_NULL = unchecked((Int32)0x1000000A);

        // 输入输出参数检查
        /// <summary>设置或者获取参数类型不正确</summary>
        public const Int32 MV_ALG_E_CFG_TYPE = unchecked((Int32)0x1000000B);
        /// <summary>设置或者获取参数的输入、输出结构体大小不正确</summary>
        public const Int32 MV_ALG_E_CFG_SIZE = unchecked((Int32)0x1000000C);
        /// <summary>处理类型不正确</summary>
        public const Int32 MV_ALG_E_PRC_TYPE = unchecked((Int32)0x1000000D);
        /// <summary>处理时输入、输出参数大小不正确</summary>
        public const Int32 MV_ALG_E_PRC_SIZE = unchecked((Int32)0x1000000E);
        /// <summary>子处理类型不正确</summary>
        public const Int32 MV_ALG_E_FUNC_TYPE = unchecked((Int32)0x1000000F);
        /// <summary>子处理时输入、输出参数大小不正确</summary>
        public const Int32 MV_ALG_E_FUNC_SIZE = unchecked((Int32)0x10000010);

        // 运行参数检查
        /// <summary>index参数不正确</summary>
        public const Int32 MV_ALG_E_PARAM_INDEX = unchecked((Int32)0x10000011);
        /// <summary>value参数不正确或者超出范围</summary>
        public const Int32 MV_ALG_E_PARAM_VALUE = unchecked((Int32)0x10000012);
        /// <summary>param_num参数不正确</summary>
        public const Int32 MV_ALG_E_PARAM_NUM = unchecked((Int32)0x10000013);

        // 接口调用检查
        /// <summary>函数参数指针为空</summary>
        public const Int32 MV_ALG_E_NULL_PTR = unchecked((Int32)0x10000014);
        /// <summary>超过限定的最大内存</summary>
        public const Int32 MV_ALG_E_OVER_MAX_MEM = unchecked((Int32)0x10000015);
        /// <summary>回调函数出错</summary>
        public const Int32 MV_ALG_E_CALL_BACK = unchecked((Int32)0x10000016);

        // 算法库加密相关检查
        /// <summary>加密错误</summary>
        public const Int32 MV_ALG_E_ENCRYPT = unchecked((Int32)0x10000017);
        /// <summary>算法库使用期限错误</summary>
        public const Int32 MV_ALG_E_EXPIRE = unchecked((Int32)0x10000018);

        // 内部模块返回的基本错误类型
        /// <summary>参数范围不正确</summary>
        public const Int32 MV_ALG_E_BAD_ARG = unchecked((Int32)0x10000019);
        /// <summary>数据大小不正确</summary>
        public const Int32 MV_ALG_E_DATA_SIZE = unchecked((Int32)0x1000001A);
        /// <summary>数据step不正确</summary>
        public const Int32 MV_ALG_E_STEP = unchecked((Int32)0x1000001B);

        // cpu指令集支持错误码
        /// <summary>cpu不支持优化代码中的指令集</summary>
        public const Int32 MV_ALG_E_CPUID = unchecked((Int32)0x1000001C);

        /// <summary>警告</summary>
        public const Int32 MV_ALG_WARNING = unchecked((Int32)0x1000001D);

        /// <summary>算法库超时</summary>
        public const Int32 MV_ALG_E_TIME_OUT = unchecked((Int32)0x1000001E);
        /// <summary>算法版本号出错</summary>
        public const Int32 MV_ALG_E_LIB_VERSION = unchecked((Int32)0x1000001F);
        /// <summary>模型版本号出错</summary>
        public const Int32 MV_ALG_E_MODEL_VERSION = unchecked((Int32)0x10000020);
        /// <summary>GPU内存分配错误</summary>
        public const Int32 MV_ALG_E_GPU_MEM_ALLOC = unchecked((Int32)0x10000021);
        /// <summary>文件不存在</summary>
        public const Int32 MV_ALG_E_FILE_NON_EXIST = unchecked((Int32)0x10000022);
        /// <summary>字符串为空</summary>
        public const Int32 MV_ALG_E_NONE_STRING = unchecked((Int32)0x10000023);
        /// <summary>图像解码器错误</summary>
        public const Int32 MV_ALG_E_IMAGE_CODEC = unchecked((Int32)0x10000024);
        /// <summary>打开文件错误</summary>
        public const Int32 MV_ALG_E_FILE_OPEN = unchecked((Int32)0x10000025);
        /// <summary>文件读取错误</summary>
        public const Int32 MV_ALG_E_FILE_READ = unchecked((Int32)0x10000026);
        /// <summary>文件写错误</summary>
        public const Int32 MV_ALG_E_FILE_WRITE = unchecked((Int32)0x10000027);
        /// <summary>文件读取大小错误</summary>
        public const Int32 MV_ALG_E_FILE_READ_SIZE = unchecked((Int32)0x10000028);
        /// <summary>文件类型错误</summary>
        public const Int32 MV_ALG_E_FILE_TYPE = unchecked((Int32)0x10000029);
        /// <summary>模型类型错误</summary>
        public const Int32 MV_ALG_E_MODEL_TYPE = unchecked((Int32)0x1000002A);
        /// <summary>分配内存错误</summary>
        public const Int32 MV_ALG_E_MALLOC_MEM = unchecked((Int32)0x1000002B);
        /// <summary>线程绑核失败</summary>
        public const Int32 MV_ALG_E_BIND_CORE_FAILED = unchecked((Int32)0x1000002C);

        // 降噪特有错误码
        /// <summary>噪声特性图像格式错误</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_IMG_FORMAT = unchecked((Int32)0x10402001);
        /// <summary>噪声特性类型错误</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_FEATURE_TYPE = unchecked((Int32)0x10402002);
        /// <summary>噪声特性个数错误</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_PROFILE_NUM = unchecked((Int32)0x10402003);
        /// <summary>噪声特性增益个数错误</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_GAIN_NUM = unchecked((Int32)0x10402004);
        /// <summary>噪声曲线增益值输入错误</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_GAIN_VAL = unchecked((Int32)0x10402005);
        /// <summary>噪声曲线柱数错误</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_BIN_NUM = unchecked((Int32)0x10402006);
        /// <summary>噪声估计初始化增益设置错误</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_INIT_GAIN = unchecked((Int32)0x10402007);
        /// <summary>噪声估计未初始化</summary>
        public const Int32 MV_ALG_E_DENOISE_NE_NOT_INIT = unchecked((Int32)0x10402008);
        /// <summary>颜色空间模式错误</summary>
        public const Int32 MV_ALG_E_DENOISE_COLOR_MODE = unchecked((Int32)0x10402009);
        /// <summary>图像ROI个数错误</summary>
        public const Int32 MV_ALG_E_DENOISE_ROI_NUM = unchecked((Int32)0x1040200a);
        /// <summary>图像ROI原点错误</summary>
        public const Int32 MV_ALG_E_DENOISE_ROI_ORI_PT = unchecked((Int32)0x1040200b);
        /// <summary>图像ROI大小错误</summary>
        public const Int32 MV_ALG_E_DENOISE_ROI_SIZE = unchecked((Int32)0x1040200c);
        /// <summary>输入的相机增益不存在(增益个数已达上限)</summary>
        public const Int32 MV_ALG_E_DENOISE_GAIN_NOT_EXIST = unchecked((Int32)0x1040200d);
        /// <summary>输入的相机增益不在范围内</summary>
        public const Int32 MV_ALG_E_DENOISE_GAIN_BEYOND_RANGE = unchecked((Int32)0x1040200e);
        /// <summary>输入的噪声特性内存大小错误</summary>
        public const Int32 MV_ALG_E_DENOISE_NP_BUF_SIZE = unchecked((Int32)0x1040200f);
        #endregion

        #endregion

        #region ch: 从C/C++接口库导出的函数 | en: C/C++ Load Function
        [DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern void OutputDebugString(string message);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumInterfaces")]
        private static extern Int32 MV_CC_EnumInterfaces(UInt32 nTLayerType, ref MV_INTERFACE_INFO_LIST pInterfaceInfoList);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CreateInterface")]
        private static extern Int32 MV_CC_CreateInterface(ref IntPtr handle, ref MV_INTERFACE_INFO pInterfaceInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CreateInterfaceByID")]
        private static extern Int32 MV_CC_CreateInterfaceByID(ref IntPtr handle, String pInterfaceID);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_OpenInterface")]
        private static extern Int32 MV_CC_OpenInterface(IntPtr handle, String pConfigFile);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CloseInterface")]
        private static extern Int32 MV_CC_CloseInterface(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DestroyInterface")]
        private static extern Int32 MV_CC_DestroyInterface(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_Initialize")]
        private static extern Int32 MV_CC_Initialize();

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_Finalize")]
        private static extern Int32 MV_CC_Finalize();

        #region 相机的基本指令和操作函数
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetSDKVersion")]
        private static extern UInt32 MV_CC_GetSDKVersion();

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumerateTls")]
        private static extern Int32 MV_CC_EnumerateTls();

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumDevices")]
        private static extern Int32 MV_CC_EnumDevices(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumDevicesEx")]
        private static extern Int32 MV_CC_EnumDevicesEx(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList, String pManufacturerName);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumDevicesEx2")]
        private static extern Int32 MV_CC_EnumDevicesEx2(UInt32 nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList, String pManufacturerName, MV_SORT_METHOD enSortMethod);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_IsDeviceAccessible")]
        private static extern Byte MV_CC_IsDeviceAccessible(ref MV_CC_DEVICE_INFO stDevInfo, UInt32 nAccessMode);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetSDKLogPath")]
        private static extern Int32 MV_CC_SetSDKLogPath(String pSDKLogPath);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CreateHandle")]
        private static extern Int32 MV_CC_CreateHandle(ref IntPtr handle, ref MV_CC_DEVICE_INFO stDevInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CreateHandleWithoutLog")]
        private static extern Int32 MV_CC_CreateHandleWithoutLog(ref IntPtr handle, ref MV_CC_DEVICE_INFO stDevInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DestroyHandle")]
        private static extern Int32 MV_CC_DestroyHandle(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_OpenDevice")]
        private static extern Int32 MV_CC_OpenDevice(IntPtr handle, UInt32 nAccessMode, UInt16 nSwitchoverKey);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CloseDevice")]
        private static extern Int32 MV_CC_CloseDevice(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_IsDeviceConnected")]
        private static extern Byte MV_CC_IsDeviceConnected(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterImageCallBackEx")]
        private static extern Int32 MV_CC_RegisterImageCallBackEx(IntPtr handle, cbOutputExdelegate cbOutput, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterImageCallBackForRGB")]
        private static extern Int32 MV_CC_RegisterImageCallBackForRGB(IntPtr handle, cbOutputExdelegate cbOutput, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterImageCallBackForBGR")]
        private static extern Int32 MV_CC_RegisterImageCallBackForBGR(IntPtr handle, cbOutputExdelegate cbOutput, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_StartGrabbing")]
        private static extern Int32 MV_CC_StartGrabbing(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_StopGrabbing")]
        private static extern Int32 MV_CC_StopGrabbing(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetImageForRGB")]
        private static extern Int32 MV_CC_GetImageForRGB(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetImageForBGR")]
        private static extern Int32 MV_CC_GetImageForBGR(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetImageBuffer")]
        private static extern Int32 MV_CC_GetImageBuffer(IntPtr handle, ref MV_FRAME_OUT pFrame, Int32 nMsec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FreeImageBuffer")]
        private static extern Int32 MV_CC_FreeImageBuffer(IntPtr handle, ref MV_FRAME_OUT pFrame);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetOneFrameTimeout")]
        private static extern Int32 MV_CC_GetOneFrameTimeout(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, Int32 nMsec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ClearImageBuffer")]
        private static extern Int32 MV_CC_ClearImageBuffer(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetValidImageNum")]
        private static extern Int32 MV_CC_GetValidImageNum(IntPtr handle, ref UInt32 pnValidImageNum);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DisplayOneFrame")]
        private static extern Int32 MV_CC_DisplayOneFrame(IntPtr handle, ref MV_DISPLAY_FRAME_INFO pDisplayInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DisplayOneFrameEx")]
        private static extern Int32 MV_CC_DisplayOneFrameEx(IntPtr handle, IntPtr hWnd, ref MV_DISPLAY_FRAME_INFO_EX pDisplayInfoEx);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetImageNodeNum")]
        private static extern Int32 MV_CC_SetImageNodeNum(IntPtr handle, UInt32 nNum);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGrabStrategy")]
        private static extern Int32 MV_CC_SetGrabStrategy(IntPtr handle, MV_GRAB_STRATEGY enGrabStrategy);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetOutputQueueSize")]
        private static extern Int32 MV_CC_SetOutputQueueSize(IntPtr handle, UInt32 nOutputQueueSize);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetDeviceInfo")]
        private static extern Int32 MV_CC_GetDeviceInfo(IntPtr handle, ref MV_CC_DEVICE_INFO pstDevInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAllMatchInfo")]
        private static extern Int32 MV_CC_GetAllMatchInfo(IntPtr handle, ref MV_ALL_MATCH_INFO pstInfo);
        #endregion

        #region 设置和获取相机参数的万能接口
        /************************************************************************/
        /* 设置和获取相机参数的万能接口                                 */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetIntValueEx")]
        private static extern Int32 MV_CC_GetIntValueEx(IntPtr handle, String strValue, ref MVCC_INTVALUE_EX pIntValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetIntValueEx")]
        private static extern Int32 MV_CC_SetIntValueEx(IntPtr handle, String strValue, Int64 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetEnumValue")]
        private static extern Int32 MV_CC_GetEnumValue(IntPtr handle, String strValue, ref MVCC_ENUMVALUE pEnumValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetEnumValue")]
        private static extern Int32 MV_CC_SetEnumValue(IntPtr handle, String strValue, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetEnumEntrySymbolic")]
        private static extern Int32 MV_CC_GetEnumEntrySymbolic(IntPtr handle, string strKey, ref MVCC_ENUMENTRY pstEnumEntry);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetEnumValueByString")]
        private static extern Int32 MV_CC_SetEnumValueByString(IntPtr handle, String strValue, String sValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetFloatValue")]
        private static extern Int32 MV_CC_GetFloatValue(IntPtr handle, String strValue, ref MVCC_FLOATVALUE pFloatValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetFloatValue")]
        private static extern Int32 MV_CC_SetFloatValue(IntPtr handle, String strValue, Single fValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBoolValue")]
        private static extern Int32 MV_CC_GetBoolValue(IntPtr handle, String strValue, ref Boolean pBoolValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBoolValue")]
        private static extern Int32 MV_CC_SetBoolValue(IntPtr handle, String strValue, Boolean bValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetStringValue")]
        private static extern Int32 MV_CC_GetStringValue(IntPtr handle, String strKey, ref MVCC_STRINGVALUE pStringValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetStringValue")]
        private static extern Int32 MV_CC_SetStringValue(IntPtr handle, String strKey, String sValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetCommandValue")]
        private static extern Int32 MV_CC_SetCommandValue(IntPtr handle, String strValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_InvalidateNodes")]
        private static extern Int32 MV_CC_InvalidateNodes(IntPtr handle);
        #endregion

        #region 设备升级 和 寄存器读写 和异常、事件回调
        /************************************************************************/
        /* 设备升级 和 寄存器读写 和异常、事件回调                            */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_LocalUpgrade")]
        private static extern Int32 MV_CC_LocalUpgrade(IntPtr handle, String pFilePathName);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetUpgradeProcess")]
        private static extern Int32 MV_CC_GetUpgradeProcess(IntPtr handle, ref UInt32 pnProcess);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ReadMemory")]
        private static extern Int32 MV_CC_ReadMemory(IntPtr handle, IntPtr pBuffer, Int64 nAddress, Int64 nLength);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_WriteMemory")]
        private static extern Int32 MV_CC_WriteMemory(IntPtr handle, IntPtr pBuffer, Int64 nAddress, Int64 nLength);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterExceptionCallBack")]
        private static extern Int32 MV_CC_RegisterExceptionCallBack(IntPtr handle, cbExceptiondelegate cbException, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterEventCallBack")]
        private static extern Int32 MV_CC_RegisterEventCallBack(IntPtr handle, cbEventdelegate cbEvent, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterAllEventCallBack")]
        private static extern Int32 MV_CC_RegisterAllEventCallBack(IntPtr handle, cbEventdelegateEx cbEvent, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterEventCallBackEx")]
        private static extern Int32 MV_CC_RegisterEventCallBackEx(IntPtr handle, String pEventName, cbEventdelegateEx cbEvent, IntPtr pUser);
        #endregion

        #region GigEVision 设备独有的接口
        /************************************************************************/
        /* GigEVision 设备独有的接口                                     */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetEnumDevTimeout")]
        private static extern Int32 MV_GIGE_SetEnumDevTimeout(UInt32 nMilTimeout);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_ForceIpEx")]
        private static extern Int32 MV_GIGE_ForceIpEx(IntPtr handle, UInt32 nIP, UInt32 nSubNetMask, UInt32 nDefaultGateWay);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetIpConfig")]
        private static extern Int32 MV_GIGE_SetIpConfig(IntPtr handle, UInt32 nType);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetNetTransMode")]
        private static extern Int32 MV_GIGE_SetNetTransMode(IntPtr handle, UInt32 nType);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetNetTransInfo")]
        private static extern Int32 MV_GIGE_GetNetTransInfo(IntPtr handle, ref MV_NETTRANS_INFO pstInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetDiscoveryMode")]
        private static extern Int32 MV_GIGE_SetDiscoveryMode(UInt32 nMode);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGvspTimeout")]
        private static extern Int32 MV_GIGE_SetGvspTimeout(IntPtr handle, UInt32 nMillisec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGvspTimeout")]
        private static extern Int32 MV_GIGE_GetGvspTimeout(IntPtr handle, ref UInt32 pMillisec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGvcpTimeout")]
        private static extern Int32 MV_GIGE_SetGvcpTimeout(IntPtr handle, UInt32 nMillisec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGvcpTimeout")]
        private static extern Int32 MV_GIGE_GetGvcpTimeout(IntPtr handle, ref UInt32 pMillisec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetRetryGvcpTimes")]
        private static extern Int32 MV_GIGE_SetRetryGvcpTimes(IntPtr handle, UInt32 nRetryGvcpTimes);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetRetryGvcpTimes")]
        private static extern Int32 MV_GIGE_GetRetryGvcpTimes(IntPtr handle, ref UInt32 pRetryGvcpTimes);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetOptimalPacketSize")]
        private static extern Int32 MV_CC_GetOptimalPacketSize(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetResend")]
        private static extern Int32 MV_GIGE_SetResend(IntPtr handle, UInt32 bEnable, UInt32 nMaxResendPercent, UInt32 nResendTimeout);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetResendMaxRetryTimes")]
        private static extern Int32 MV_GIGE_SetResendMaxRetryTimes(IntPtr handle, UInt32 nRetryTimes);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetResendMaxRetryTimes")]
        private static extern Int32 MV_GIGE_GetResendMaxRetryTimes(IntPtr handle, ref UInt32 pnRetryTimes);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetResendTimeInterval")]
        private static extern Int32 MV_GIGE_SetResendTimeInterval(IntPtr handle, UInt32 nMillisec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetResendTimeInterval")]
        private static extern Int32 MV_GIGE_GetResendTimeInterval(IntPtr handle, ref UInt32 pnMillisec);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetTransmissionType")]
        private static extern Int32 MV_GIGE_SetTransmissionType(IntPtr handle, ref MV_CC_TRANSMISSION_TYPE pstTransmissionType);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_IssueActionCommand")]
        private static extern Int32 MV_GIGE_IssueActionCommand(ref MV_ACTION_CMD_INFO pstActionCmdInfo, ref MV_ACTION_CMD_RESULT_LIST pstActionCmdResults);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetMulticastStatus")]
        private static extern Int32 MV_GIGE_GetMulticastStatus(ref MV_CC_DEVICE_INFO pstDevInfo, ref Boolean pStatus);
        #endregion

        #region CameraLink独有的接口
        //CameraLink独有的接口
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_SetDeviceBauderate")]
        private static extern Int32 MV_CAML_SetDeviceBaudrate(IntPtr handle, UInt32 nBaudrate);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_GetDeviceBauderate")]
        private static extern Int32 MV_CAML_GetDeviceBaudrate(IntPtr handle, ref UInt32 pnCurrentBaudrate);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_GetSupportBauderates")]
        private static extern Int32 MV_CAML_GetSupportBaudrates(IntPtr handle, ref UInt32 pnBaudrateAblity);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_SetGenCPTimeOut")]
        private static extern Int32 MV_CAML_SetGenCPTimeOut(IntPtr handle, UInt32 nMillisec);
        #endregion

        #region U3V 设备独有的接口
        /************************************************************************/
        /* U3V 设备独有的接口                                            */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_SetTransferSize")]
        private static extern Int32 MV_USB_SetTransferSize(IntPtr handle, UInt32 nTransferSize);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_GetTransferSize")]
        private static extern Int32 MV_USB_GetTransferSize(IntPtr handle, ref UInt32 pTransferSize);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_SetTransferWays")]
        private static extern Int32 MV_USB_SetTransferWays(IntPtr handle, UInt32 nTransferWays);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_GetTransferWays")]
        private static extern Int32 MV_USB_GetTransferWays(IntPtr handle, ref UInt32 pTransferWays);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_RegisterStreamExceptionCallBack")]
        private static extern Int32 MV_USB_RegisterStreamExceptionCallBack(IntPtr handle, cbStreamException cbException, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_SetEventNodeNum")]
        private static extern Int32 MV_USB_SetEventNodeNum(IntPtr handle, UInt32 nEventNodeNum);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_SetSyncTimeOut")]
        private static extern Int32 MV_USB_SetSyncTimeOut(IntPtr handle, UInt32 nMills);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_USB_GetSyncTimeOut")]
        private static extern Int32 MV_USB_GetSyncTimeOut(IntPtr handle, ref UInt32 pnMills);
        #endregion

        #region GenTL相关接口，其它接口可以复用（部分接口不支持）
        /************************************************************************/
        /* GenTL相关接口，其它接口可以复用（部分接口不支持）                    */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumInterfacesByGenTL")]
        private static extern Int32 MV_CC_EnumInterfacesByGenTL(ref MV_GENTL_IF_INFO_LIST pstIFInfoList, String sGenTLPath);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_EnumDevicesByGenTL")]
        private static extern Int32 MV_CC_EnumDevicesByGenTL(ref MV_GENTL_IF_INFO stIFInfo, ref MV_GENTL_DEV_INFO_LIST pstDevList);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_UnloadGenTLLibrary")]
        private static extern Int32 MV_CC_UnloadGenTLLibrary(String strGenTLPath);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_CreateHandleByGenTL")]
        private static extern Int32 MV_CC_CreateHandleByGenTL(ref IntPtr handle, ref MV_GENTL_DEV_INFO stDevInfo);
        #endregion

        #region XML解析树的生成
        /************************************************************************/
        /* XML解析树的生成                                                         */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetGenICamXML")]
        private static extern Int32 MV_XML_GetGenICamXML(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref UInt32 pnDataLen);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetNodeAccessMode")]
        private static extern Int32 MV_XML_GetNodeAccessMode(IntPtr handle, String pstrName, ref MV_XML_AccessMode pAccessMode);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetNodeInterfaceType")]
        private static extern Int32 MV_XML_GetNodeInterfaceType(IntPtr handle, String pstrName, ref MV_XML_InterfaceType pInterfaceType);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetRootNode")]
        private static extern Int32 MV_XML_GetRootNode(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetChildren")]
        private static extern Int32 MV_XML_GetChildren(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode, IntPtr pstNodesList);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetChildren")]
        private static extern Int32 MV_XML_GetChildren(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode, ref MV_XML_NODES_LIST pstNodesList);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_GetNodeFeature")]
        private static extern Int32 MV_XML_GetNodeFeature(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode, IntPtr pstFeature);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_UpdateNodeFeature")]
        private static extern Int32 MV_XML_UpdateNodeFeature(IntPtr handle, MV_XML_InterfaceType enType, IntPtr pstFeature);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_XML_RegisterUpdateCallBack")]
        private static extern Int32 MV_XML_RegisterUpdateCallBack(IntPtr handle, cbXmlUpdatedelegate cbXmlUpdate, IntPtr pUser);
        #endregion

        #region 附加接口
        /************************************************************************/
        /* 附加接口                                   */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SaveImageEx3")]
        private static extern Int32 MV_CC_SaveImageEx3(IntPtr handle, ref MV_SAVE_IMAGE_PARAM_EX3 stSaveParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SaveImageToFileEx")]
        private static extern Int32 MV_CC_SaveImageToFileEx(IntPtr handle, ref MV_SAVE_IMG_TO_FILE_PARAM_EX pstSaveFileParamEx);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SavePointCloudData")]
        private static extern Int32 MV_CC_SavePointCloudData(IntPtr handle, ref MV_SAVE_POINT_CLOUD_PARAM pstPointDataParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RotateImage")]
        private static extern Int32 MV_CC_RotateImage(IntPtr handle, ref MV_CC_ROTATE_IMAGE_PARAM pstRotateParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FlipImage")]
        private static extern Int32 MV_CC_FlipImage(IntPtr handle, ref MV_CC_FLIP_IMAGE_PARAM pstFlipParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ConvertPixelTypeEx")]
        private static extern Int32 MV_CC_ConvertPixelTypeEx(IntPtr handle, ref MV_CC_PIXEL_CONVERT_PARAM_EX pstCvtParamEx);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGammaValue")]
        private static extern Int32 MV_CC_SetGammaValue(IntPtr handle, MvGvspPixelType enSrcPixelType, Single fGammaValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerCvtQuality")]
        private static extern Int32 MV_CC_SetBayerCvtQuality(IntPtr handle, UInt32 BayerCvtQuality);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerFilterEnable")]
        private static extern Int32 MV_CC_SetBayerFilterEnable(IntPtr handle, Boolean bFilterEnable);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerGammaParam")]
        private static extern Int32 MV_CC_SetBayerGammaParam(IntPtr handle, ref MV_CC_GAMMA_PARAM pstGammaParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerCCMParam")]
        private static extern Int32 MV_CC_SetBayerCCMParam(IntPtr handle, ref MV_CC_CCM_PARAM pstCCMParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerCCMParamEx")]
        private static extern Int32 MV_CC_SetBayerCCMParamEx(IntPtr handle, ref MV_CC_CCM_PARAM_EX pstCCMParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ImageContrast")]
        private static extern Int32 MV_CC_ImageContrast(IntPtr handle, ref MV_CC_CONTRAST_PARAM pstContrastParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_HB_Decode")]
        private static extern Int32 MV_CC_HB_Decode(IntPtr handle, ref MV_CC_HB_DECODE_PARAM pstDecodeParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DrawRect")]
        private static extern Int32 MV_CC_DrawRect(IntPtr handle, ref MVCC_RECT_INFO pRectInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DrawCircle")]
        private static extern Int32 MV_CC_DrawCircle(IntPtr handle, ref MVCC_CIRCLE_INFO pCircleInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_DrawLines")]
        private static extern Int32 MV_CC_DrawLines(IntPtr handle, ref MVCC_LINES_INFO pLinesInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FeatureSave")]
        private static extern Int32 MV_CC_FeatureSave(IntPtr handle, String pFileName);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FeatureLoad")]
        private static extern Int32 MV_CC_FeatureLoad(IntPtr handle, String pFileName);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FileAccessRead")]
        private static extern Int32 MV_CC_FileAccessRead(IntPtr handle, ref MV_CC_FILE_ACCESS pstFileAccess);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FileAccessReadEx")]
        private static extern Int32 MV_CC_FileAccessReadEx(IntPtr handle, ref MV_CC_FILE_ACCESS_EX pstFileAccessEx);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FileAccessWrite")]
        private static extern Int32 MV_CC_FileAccessWrite(IntPtr handle, ref MV_CC_FILE_ACCESS pstFileAccess);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_FileAccessWriteEx")]
        private static extern Int32 MV_CC_FileAccessWriteEx(IntPtr handle, ref MV_CC_FILE_ACCESS_EX pstFileAccessEx);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetFileAccessProgress")]
        private static extern Int32 MV_CC_GetFileAccessProgress(IntPtr handle, ref MV_CC_FILE_ACCESS_PROGRESS pstFileAccessProgress);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_StartRecord")]
        private static extern Int32 MV_CC_StartRecord(IntPtr handle, ref MV_CC_RECORD_PARAM pstRecordParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_InputOneFrame")]
        private static extern Int32 MV_CC_InputOneFrame(IntPtr handle, ref MV_CC_INPUT_FRAME_INFO pstInputFrameInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_StopRecord")]
        private static extern Int32 MV_CC_StopRecord(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_OpenParamsGUI")]
        private static extern Int32 MV_CC_OpenParamsGUI(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ReconstructImage")]
        private static extern Int32 MV_CC_ReconstructImage(IntPtr handle, ref MV_RECONSTRUCT_IMAGE_PARAM pstReconstructParam);
        #endregion

        #region 弃用的接口
        /************************************************************************/
        /* 弃用的接口                                 */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SaveImageEx2")]
        private static extern Int32 MV_CC_SaveImageEx2(IntPtr handle, ref MV_SAVE_IMAGE_PARAM_EX2 stSaveParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SaveImageToFile")]
        private static extern Int32 MV_CC_SaveImageToFile(IntPtr handle, ref MV_SAVE_IMG_TO_FILE_PARAM pstSaveFileParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ConvertPixelType")]
        private static extern Int32 MV_CC_ConvertPixelType(IntPtr handle, ref MV_PIXEL_CONVERT_PARAM pstCvtParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetOneFrame")]
        private static extern Int32 MV_CC_GetOneFrame(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO pFrameInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetOneFrameEx")]
        private static extern Int32 MV_CC_GetOneFrameEx(IntPtr handle, IntPtr pData, UInt32 nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_RegisterImageCallBack")]
        private static extern Int32 MV_CC_RegisterImageCallBack(IntPtr handle, cbOutputdelegate cbOutput, IntPtr pUser);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SaveImage")]
        private static extern Int32 MV_CC_SaveImage(ref MV_SAVE_IMAGE_PARAM stSaveParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_ForceIp")]
        private static extern Int32 MV_GIGE_ForceIp(IntPtr handle, UInt32 nIP);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_BayerNoiseEstimate")]
        private static extern Int32 MV_CC_BayerNoiseEstimate(IntPtr handle, ref MV_CC_BAYER_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_BayerSpatialDenoise")]
        private static extern Int32 MV_CC_BayerSpatialDenoise(IntPtr handle, ref MV_CC_BAYER_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_Display")]
        private static extern Int32 MV_CC_Display(IntPtr handle, IntPtr hWnd);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetImageInfo")]
        private static extern Int32 MV_CC_GetImageInfo(IntPtr handle, ref MV_IMAGE_BASIC_INFO pstInfo);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGevSCPSPacketSize")]
        private static extern Int32 MV_GIGE_GetGevSCPSPacketSize(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGevSCPSPacketSize")]
        private static extern Int32 MV_GIGE_SetGevSCPSPacketSize(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGevSCPD")]
        private static extern Int32 MV_GIGE_GetGevSCPD(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGevSCPD")]
        private static extern Int32 MV_GIGE_SetGevSCPD(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGevSCDA")]
        private static extern Int32 MV_GIGE_GetGevSCDA(IntPtr handle, ref UInt32 pnIP);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGevSCDA")]
        private static extern Int32 MV_GIGE_SetGevSCDA(IntPtr handle, UInt32 nIP);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_GetGevSCSP")]
        private static extern Int32 MV_GIGE_GetGevSCSP(IntPtr handle, ref UInt32 pnPort);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_GIGE_SetGevSCSP")]
        private static extern Int32 MV_GIGE_SetGevSCSP(IntPtr handle, UInt32 nPort);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerCLUTParam")]
        private static extern Int32 MV_CC_SetBayerCLUTParam(IntPtr handle, ref MV_CC_CLUT_PARAM pstCLUTParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ImageSharpen")]
        private static extern Int32 MV_CC_ImageSharpen(IntPtr handle, ref MV_CC_SHARPEN_PARAM pstSharpenParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_ColorCorrect")]
        private static extern Int32 MV_CC_ColorCorrect(IntPtr handle, ref MV_CC_COLOR_CORRECT_PARAM pstColorCorrectParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_NoiseEstimate")]
        private static extern Int32 MV_CC_NoiseEstimate(IntPtr handle, ref MV_CC_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SpatialDenoise")]
        private static extern Int32 MV_CC_SpatialDenoise(IntPtr handle, ref MV_CC_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_LSCCalib")]
        private static extern Int32 MV_CC_LSCCalib(IntPtr handle, ref MV_CC_LSC_CALIB_PARAM pstLSCCalibParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_LSCCorrect")]
        private static extern Int32 MV_CC_LSCCorrect(IntPtr handle, ref MV_CC_LSC_CORRECT_PARAM pstLSCCorrectParam);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetTlProxy")]
        private static extern IntPtr MV_CC_GetTlProxy(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_WriteLog")]
        private static extern Int32 MV_CC_WriteLog(string strLog);
        /************************************************************************/
        /* 相机参数获取和设置，此模块的所有接口，将逐步废弃，建议用上面的万能接口代替   */
        /************************************************************************/
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetIntValue")]
        private static extern Int32 MV_CC_GetIntValue(IntPtr handle, String strValue, ref MVCC_INTVALUE pIntValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetIntValue")]
        private static extern Int32 MV_CC_SetIntValue(IntPtr handle, String strValue, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetWidth")]
        private static extern Int32 MV_CC_GetWidth(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetWidth")]
        private static extern Int32 MV_CC_SetWidth(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetHeight")]
        private static extern Int32 MV_CC_GetHeight(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetHeight")]
        private static extern Int32 MV_CC_SetHeight(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAOIoffsetX")]
        private static extern Int32 MV_CC_GetAOIoffsetX(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAOIoffsetX")]
        private static extern Int32 MV_CC_SetAOIoffsetX(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAOIoffsetY")]
        private static extern Int32 MV_CC_GetAOIoffsetY(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAOIoffsetY")]
        private static extern Int32 MV_CC_SetAOIoffsetY(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAutoExposureTimeLower")]
        private static extern Int32 MV_CC_GetAutoExposureTimeLower(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAutoExposureTimeLower")]
        private static extern Int32 MV_CC_SetAutoExposureTimeLower(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAutoExposureTimeUpper")]
        private static extern Int32 MV_CC_GetAutoExposureTimeUpper(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAutoExposureTimeUpper")]
        private static extern Int32 MV_CC_SetAutoExposureTimeUpper(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBrightness")]
        private static extern Int32 MV_CC_GetBrightness(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBrightness")]
        private static extern Int32 MV_CC_SetBrightness(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetFrameRate")]
        private static extern Int32 MV_CC_GetFrameRate(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetFrameRate")]
        private static extern Int32 MV_CC_SetFrameRate(IntPtr handle, Single fValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetGain")]
        private static extern Int32 MV_CC_GetGain(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGain")]
        private static extern Int32 MV_CC_SetGain(IntPtr handle, Single fValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetExposureTime")]
        private static extern Int32 MV_CC_GetExposureTime(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetExposureTime")]
        private static extern Int32 MV_CC_SetExposureTime(IntPtr handle, Single fValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetPixelFormat")]
        private static extern Int32 MV_CC_GetPixelFormat(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetPixelFormat")]
        private static extern Int32 MV_CC_SetPixelFormat(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAcquisitionMode")]
        private static extern Int32 MV_CC_GetAcquisitionMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAcquisitionMode")]
        private static extern Int32 MV_CC_SetAcquisitionMode(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetGainMode")]
        private static extern Int32 MV_CC_GetGainMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGainMode")]
        private static extern Int32 MV_CC_SetGainMode(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetExposureAutoMode")]
        private static extern Int32 MV_CC_GetExposureAutoMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetExposureAutoMode")]
        private static extern Int32 MV_CC_SetExposureAutoMode(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetTriggerMode")]
        private static extern Int32 MV_CC_GetTriggerMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetTriggerMode")]
        private static extern Int32 MV_CC_SetTriggerMode(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetTriggerDelay")]
        private static extern Int32 MV_CC_GetTriggerDelay(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetTriggerDelay")]
        private static extern Int32 MV_CC_SetTriggerDelay(IntPtr handle, Single fValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetTriggerSource")]
        private static extern Int32 MV_CC_GetTriggerSource(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetTriggerSource")]
        private static extern Int32 MV_CC_SetTriggerSource(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_TriggerSoftwareExecute")]
        private static extern Int32 MV_CC_TriggerSoftwareExecute(IntPtr handle);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetGammaSelector")]
        private static extern Int32 MV_CC_GetGammaSelector(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGammaSelector")]
        private static extern Int32 MV_CC_SetGammaSelector(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetGamma")]
        private static extern Int32 MV_CC_GetGamma(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetGamma")]
        private static extern Int32 MV_CC_SetGamma(IntPtr handle, Single fValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetSharpness")]
        private static extern Int32 MV_CC_GetSharpness(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetSharpness")]
        private static extern Int32 MV_CC_SetSharpness(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetHue")]
        private static extern Int32 MV_CC_GetHue(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetHue")]
        private static extern Int32 MV_CC_SetHue(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetSaturation")]
        private static extern Int32 MV_CC_GetSaturation(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetSaturation")]
        private static extern Int32 MV_CC_SetSaturation(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBalanceWhiteAuto")]
        private static extern Int32 MV_CC_GetBalanceWhiteAuto(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBalanceWhiteAuto")]
        private static extern Int32 MV_CC_SetBalanceWhiteAuto(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBalanceRatioRed")]
        private static extern Int32 MV_CC_GetBalanceRatioRed(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBalanceRatioRed")]
        private static extern Int32 MV_CC_SetBalanceRatioRed(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBalanceRatioGreen")]
        private static extern Int32 MV_CC_GetBalanceRatioGreen(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBalanceRatioGreen")]
        private static extern Int32 MV_CC_SetBalanceRatioGreen(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBalanceRatioBlue")]
        private static extern Int32 MV_CC_GetBalanceRatioBlue(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBalanceRatioBlue")]
        private static extern Int32 MV_CC_SetBalanceRatioBlue(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetDeviceUserID")]
        private static extern Int32 MV_CC_GetDeviceUserID(IntPtr handle, ref MVCC_STRINGVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetDeviceUserID")]
        private static extern Int32 MV_CC_SetDeviceUserID(IntPtr handle, string chValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetBurstFrameCount")]
        private static extern Int32 MV_CC_GetBurstFrameCount(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBurstFrameCount")]
        private static extern Int32 MV_CC_SetBurstFrameCount(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetAcquisitionLineRate")]
        private static extern Int32 MV_CC_GetAcquisitionLineRate(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetAcquisitionLineRate")]
        private static extern Int32 MV_CC_SetAcquisitionLineRate(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_GetHeartBeatTimeout")]
        private static extern Int32 MV_CC_GetHeartBeatTimeout(IntPtr handle, ref MVCC_INTVALUE pstValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetHeartBeatTimeout")]
        private static extern Int32 MV_CC_SetHeartBeatTimeout(IntPtr handle, UInt32 nValue);

        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CC_SetBayerGammaValue")]
        private static extern Int32 MV_CC_SetBayerGammaValue(IntPtr handle, Single fBayerGammaValue);
        #endregion

        #endregion
    }
}


