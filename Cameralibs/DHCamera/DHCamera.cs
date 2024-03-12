
#define TurnTable  //on:飞拍软触发 off:定拍硬触发

using GxIAPINET;
using System;
using System.Drawing;
using System.Threading;
using System.Windows;

namespace CamControl.Cameralibs.DHCmera
{
    internal class DHCamera
    {


        #region  camera
        public bool m_bIsOpen = false;                  ///<设备打开状态
        public bool m_bIsSnap = false;                  ///<发送开采命令标识 
        public bool m_bIsTrigValid = true;                 ///< 触发是否有效标志:当一次触发正在执行时,将该标志置为false   
        public IGXFactory m_objIGXFactory = null;                   ///<Factory对像
        private IGXDevice m_objIGXDevice = null;                   ///<设备对像
        private IGXStream m_objIGXStream = null;                   ///<流对像
        private IGXFeatureControl m_objIGXFeatureControl = null;                   ///<远端设备属性控制器对像
        private IGXFeatureControl m_objIGXStreamFeatureControl = null;                   ///<流层属性控制器对象
        private GX_FEATURE_CALLBACK_HANDLE m_hFeatureCallback = null;                 ///<Feature事件的句柄
        private Util_Bitmap Utilbitmap = null;                   ///<图像显示类对象


       public IGXDeviceInfo GXDeviceInfo ;

        #endregion camera

         
        /// <summary>
        /// 获取图像数据委托  注册用  +=  切记！！！
        /// </summary>
        public Action<Bitmap> Delegate_Camera ;

        private void DelegateCallBack(Bitmap caallimg)
        { 
            callbackimg = caallimg;
            ResetGetImageSignal.Set(); 
        }

        #region carmera function


        public DHCamera()
        {
            
        }

        public void Init()
        {
            try
            {
               //List<IGXDeviceInfo> listGXDeviceInfo = new List<IGXDeviceInfo>();

                //关闭流
                __CloseStream();
                // 如果设备已经打开则关闭，保证相机在初始化出错情况下能再次打开
                __CloseDevice();

                //m_objIGXFactory.UpdateDeviceList(200, listGXDeviceInfo);

                //// 判断当前连接设备个数
                //if (listGXDeviceInfo.Count <= 0)
                //{
                //    MessageBox.Show("未发现设备!");
                //    return;
                //}                // 判断当前连接设备个数
                //if (listGXDeviceInfo.Count <= 0)
                //{
                //    MessageBox.Show("未发现设备!");
                //    return;
                //}

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

                __InitDevice();

                Utilbitmap = new Util_Bitmap(m_objIGXDevice);

                // 更新设备打开标识
                m_bIsOpen = true;

                //刷新界面
                //__UpdateUI();

            }
            catch(Exception e)
            { /*MessageBox.Show(e.ToString());*/ }

        }
          
        public void Close()
        {
            try
            {
                // 停止采集关闭设备、关闭流
                __CloseAll();
                //刷新界面
                //__UpdateUI();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
         
        /// <summary>
        /// 设置软触发
        /// </summary>
        public void SetSoftTrigger()
        {
            m_objIGXFeatureControl?.GetEnumFeature("triggerMode").SetValue("On");

            ////选择触发源为软触发
            m_objIGXFeatureControl?.GetEnumFeature("SelectedTriggsource").SetValue("Software");
        }

        /// <summary>
        /// 设置硬件触发
        /// </summary>
        public void SetHardwareTrigger()
        {
            m_objIGXFeatureControl?.GetEnumFeature("triggerMode").SetValue("On");

            //选择触发源为外触发
            m_objIGXFeatureControl?.GetEnumFeature("SelectedTriggsource").SetValue("Line0");
        }

        /// <summary>
        /// 发送软触发命令
        /// </summary> 
        public void SoftTriggerCommand()
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
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //m_bIsTrigValid = true;
            }
        }






        AutoResetEvent ResetGetImageSignal = new AutoResetEvent(false);

        /// <summary>
        /// 以回调方式获取图像，会阻塞线程，
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="outtime"></param>
        /// <returns></returns>
        public bool getCallBackImage(out Bitmap bitmap, int outtime = 500)
        {
            bitmap = null;
            if (ResetGetImageSignal.WaitOne(outtime))
            {
                bitmap = callbackimg.Clone() as Bitmap;  
                return true;
            }
           
            return false;
        }

        private Bitmap callbackimg { get; set; }
        /// <summary>
        /// 界面画面采集
        /// </summary>
        public void Collect_Start()
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
                    Delegate_Camera += new Action<Bitmap>(DelegateCallBack);
                    m_objIGXStream.RegisterCaptureCallback(null, OnFrameCallbackFun);
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
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 界面停止采集画面
        /// </summary>
        public void Collect_Stop()
        {
            try
            {
                //发送停采命令 ----------------------
                if (null != m_objIGXFeatureControl)
                {
                    m_objIGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                }

                //关闭采集流通道
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.StopGrab();
                    //注销采集回调函数
                    m_objIGXStream.UnregisterCaptureCallback();
                }

                m_bIsSnap = false;

                // 更新界面UI
                //__UpdateUI();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

         
       
        /************************************************************************/

        /// <summary>
        /// 采单帧 委托传回数据
        /// </summary>
        public void Strigger_SignlePix()
        {
            try
            {
                ////设置触发模式为开
                m_objIGXFeatureControl.GetEnumFeature("triggerMode").SetValue("On");

                ////选择触发源为软触发
                m_objIGXFeatureControl.GetEnumFeature("SelectedTriggsource").SetValue("Software");

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


                IImageData objIImageData = null;
                //获取图像
                if (null != m_objIGXStream)
                {
                    objIImageData = m_objIGXStream.GetImage(500);

                }

                //m_objGxBitmap.Show(objIImageData);

                Bitmap single = Utilbitmap.GetBmp(objIImageData);

                //调试窗口
                //Delegate_debugShow(single.Clone() as Bitmap);


                if (null != m_objIGXStream && null != m_objIGXFeatureControl)
                {

                    //开始采集之前可设置buff个数
                    //开启采集流通道
                    m_objIGXStream.StartGrab();
                    //发送开采命令
                    m_objIGXFeatureControl.GetCommandFeature("AcquisitionStart").Execute();

                    //采单帧
                    IImageData objImageData = null;
                    //超时时间使用 500ms，用户可以自行设定
                    objImageData = m_objIGXStream.GetImage(100);
                    if (objImageData.GetStatus() == GX_FRAME_STATUS_LIST.GX_FRAME_STATUS_SUCCESS)
                    {
                        //采图成功而且是完整帧，可以进行图像处理...
                        //委托传回调试界面
                        Bitmap single1 = Utilbitmap.GetBmp(objImageData);
                        // Delegate_debugShow(single1.Clone() as Bitmap);

                    }
                    objImageData.Destroy();//销毁 objImageData 对象
                                           //停采
                    m_objIGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                    m_objIGXStream.StopGrab();
                    //关闭流通道

                }



                m_bIsSnap = true;
                //m_bIsTrigValid = true;

                // 更新界面UI
                // __UpdateUI();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }


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
                    m_objIGXStream.Close();
                    m_objIGXStream = null;
                    m_objIGXStreamFeatureControl = null;
                }
            }
            catch (Exception)
            {
            }

            //关闭设备
            __CloseDevice();
            m_bIsOpen = false;
        }


        /// <summary>
        /// 相机初始化
        /// </summary>
        private void __InitDevice()
        {
            if (null != m_objIGXFeatureControl)
            {
 
                //m_objIGXFeatureControl.GetEnumFeature("triggerMode").SetValue("On"); 
                ////选择触发源为软触发
                //m_objIGXFeatureControl.GetEnumFeature("SelectedTriggsource").SetValue("Software");

 
                m_objIGXFeatureControl.GetEnumFeature("triggerMode").SetValue("On");  
                m_objIGXFeatureControl.GetEnumFeature("SelectedTriggsource").SetValue("Line0");
                 
 
                //设置采集模式连续采集
                //m_objIGXFeatureControl.GetEnumFeature("AcquisitionMode").SetValue("Continuous");


                m_objIGXFeatureControl.GetFloatFeature("ExposureTime").SetValue(100);

            }
        }





        #endregion carmera function

        /// <summary>
        /// 主界面采集事件的委托函数
        /// </summary>
        /// <param name="objUserParam">用户私有参数</param>
        /// <param name="objIFrameData">图像信息对象</param>
        private void OnFrameCallbackFun(object objUserParam, IFrameData objIFrameData)
        {

            //用户私有参数 obj，用户在注册回调函数的时候传入了设备对象，在回调函数内部可以将此
            //参数还原为用户私有参数
            //IGXDevice objIGXDevice = objUserParam as IGXDevice;
            if (objIFrameData.GetStatus() == GX_FRAME_STATUS_LIST.GX_FRAME_STATUS_SUCCESS)  //完整帧 
            {
                Bitmap getbitmap = Utilbitmap.GetBmp(objIFrameData);

                //数据返回到主窗口 
                Delegate_Camera(getbitmap);
                //GC.Collect();
            }
        }


    }
}


