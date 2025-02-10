using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Basler.Pylon;

namespace MG.CamCtrl.Mode
{
    internal class BaslerCamera : BaseCamera
    {
        #region Parm 
        List<ICameraInfo> listcaminf = new List<ICameraInfo>();
        ICameraInfo currcaminf { get; set; }
        Camera camera;
        bool iscreated = false;
        bool isgiveupfirst = false;
        private Object monitor = new Object();
        private Bitmap latestFrame = null;
        private PixelDataConverter converter = new PixelDataConverter();

        #endregion


        public BaslerCamera() : base() { }

        #region Opr
        public override List<string> GetListEnum()
        {
            listcaminf.Clear();
            listcaminf = CameraFinder.Enumerate();
            return listcaminf.Select(t => t[CameraInfoKey.SerialNumber]).ToList();
        }
        public override bool InitDevice(string CamSN)
        {
            var listsn = GetListEnum();
            if (listcaminf.Count < 1 || string.IsNullOrEmpty(CamSN)) return false;
            currcaminf = listcaminf.Where(t => t[CameraInfoKey.SerialNumber].Equals(CamSN)).FirstOrDefault();
            if (currcaminf == null) return false;

            camera = new Camera(CamSN);

            if (currcaminf != null)
            {
                camera.ConnectionLost += OnConnectionLost;
                camera.CameraOpened += OnCameraOpened;
                camera.CameraClosed += OnCameraClosed;
                camera.StreamGrabber.GrabStarted += OnGrabStarted;
                camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                camera.StreamGrabber.GrabStopped += OnGrabStopped;
            }
            try
            {
                camera.Open();
            }
            catch (Exception)
            {

                return false;
            }


            //var parmeter = camera.Parameters[PLCamera.TriggerSelector];
            //parmeter.SetValue("AcquisitionStart");

            return true;
        }
        public override void CloseDevice()
        {
            StopGrabbing();
            //  ClearLatestFrame();
            if (camera != null && camera.IsOpen)
            {
                isgiveupfirst = false;
                camera.StreamGrabber.Stop();
                camera.Close();
            }
            if (camera != null)
            {
                DisconnectFromCameraEvents();
                camera.Dispose();
                camera = null;
            }
        }

        private void DisconnectFromCameraEvents()
        {
            camera.ConnectionLost -= OnConnectionLost;
            camera.CameraOpened -= OnCameraOpened;
            camera.CameraClosed -= OnCameraClosed;
            camera.StreamGrabber.GrabStarted -= OnGrabStarted;
            camera.StreamGrabber.ImageGrabbed -= OnImageGrabbed;
            camera.StreamGrabber.GrabStopped -= OnGrabStopped;
        }

        public override bool SoftTrigger()
        {
            if (camera == null || !camera.IsConnected) return false;
            if (camera.CanWaitForFrameTriggerReady)
            {
                camera.WaitForFrameTriggerReady(1000, TimeoutHandling.ThrowException);
            }

            camera.ExecuteSoftwareTrigger();

            return true;
        }


        #endregion

        #region SettingConfig
        public override bool SetExpouseTime(ulong value)
        {
            if (camera == null || !camera.IsConnected) return false;
            try
            {
                IFloatParameter parmeter = camera.Parameters[camera.Parameters.Contains(PLCamera.ExposureTimeAbs) ? PLCamera.ExposureTimeAbs : PLCamera.ExposureTime];
                parmeter.SetValue(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool GetExpouseTime(out ulong value)
        {
            value = 0;
            if (camera == null || !camera.IsConnected) return false;
            try
            {
                var parmeter = camera.Parameters[camera.Parameters.Contains(PLCamera.ExposureTimeAbs) ? PLCamera.ExposureTimeAbs : PLCamera.ExposureTime];
                value = (ulong)parmeter.GetValue();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool SetTriggerMode(TriggerMode mode, TriggerSource triggerEnum = TriggerSource.Line0)
        {
            if (camera == null || !camera.IsConnected) return false;
            try
            {
                var par_triggmodel = camera.Parameters[PLCamera.TriggerMode];
                var par_triggsource = camera.Parameters[PLCamera.TriggerSource];
                switch (mode)
                {
                    case TriggerMode.Off:
                        Configuration.AcquireContinuous(camera, null);
                        camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                        par_triggmodel.SetValue("Off");
                        break;
                    case TriggerMode.On:
                        Configuration.AcquireContinuous(camera, null);
                        camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                        par_triggmodel.SetValue("On");
                        par_triggsource.SetValue(triggerEnum.ToString());
                        isgiveupfirst = triggerEnum.Equals(TriggerSource.Software);
                        break;
                    default:
                        Configuration.AcquireContinuous(camera, null);
                        camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);

                        par_triggsource.SetValue(TriggerSource.Line0.ToString());
                        par_triggmodel.SetValue("On");
                        break;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool GetTriggerMode(out TriggerMode mode, out TriggerSource triggersource)
        {
            mode = TriggerMode.On;
            triggersource = TriggerSource.Line0;
            if (camera == null || !camera.IsConnected) return false;

            try
            {
                var par_triggmodel = camera.Parameters[PLCamera.TriggerMode];
                var par_triggsource = camera.Parameters[PLCamera.TriggerSource];
                mode = (TriggerMode)Enum.Parse(typeof(TriggerMode), par_triggmodel.GetValue());
                triggersource = (TriggerSource)Enum.Parse(typeof(TriggerSource), par_triggmodel.GetValue());

                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool SetTriggerPolarity(TriggerPolarity polarity)
        {
            if (camera == null || !camera.IsConnected) return false;
            try
            {
                var parmeter = camera.Parameters[PLCamera.TriggerActivation];
                parmeter.SetValue(polarity.ToString());
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
            if (camera == null || !camera.IsConnected) return false;
            try
            {
                var parmeter = camera.Parameters[PLCamera.TriggerActivation];
                polarity = (TriggerPolarity)Enum.Parse(typeof(TriggerPolarity), parmeter.GetValue());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool SetTriggerFliter(ushort flitertime)
        {
            if (camera == null || !camera.IsConnected) return false;
            try
            {
                var parmeter = camera.Parameters[camera.Parameters.Contains(PLCamera.LineDebouncerTimeAbs) ? PLCamera.LineDebouncerTimeAbs : PLCamera.LineDebouncerTime];
                parmeter.SetValue(flitertime);
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
            if (camera == null || !camera.IsConnected) return false;
            try
            {
                var parmeter = camera.Parameters[camera.Parameters.Contains(PLCamera.LineDebouncerTimeAbs) ? PLCamera.LineDebouncerTimeAbs : PLCamera.LineDebouncerTime];
                flitertime = (ushort)parmeter.GetValue();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public override bool SetTriggerDelay(ushort delay)
        {
            if (camera == null || !camera.IsConnected) return false;
            try
            {
                var parmeter = camera.Parameters[camera.Parameters.Contains(PLCamera.TriggerDelayAbs) ? PLCamera.TriggerDelayAbs : PLCamera.TriggerDelay];
                parmeter.SetValue(delay);
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
            if (camera == null || !camera.IsConnected) return false;
            try
            {
                var parmeter = camera.Parameters[camera.Parameters.Contains(PLCamera.TriggerDelayAbs) ? PLCamera.TriggerDelayAbs : PLCamera.TriggerDelay];
                delay = (ushort)parmeter.GetValue();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool SetGain(float gain)
        {
            if (camera == null || !camera.IsConnected) return false;
            try
            {
                var parmeter = camera.Parameters[camera.Parameters.Contains(PLCamera.GainAbs) ? PLCamera.GainAbs : PLCamera.Gain];
                parmeter.SetValue(gain);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool GetGain(out float gain)
        {
            gain = 0;
            if (camera == null || !camera.IsConnected) return false;
            try
            {
                var parmeter = camera.Parameters[camera.Parameters.Contains(PLCamera.GainAbs) ? PLCamera.GainAbs : PLCamera.Gain];
                gain = (float)parmeter.GetValue();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public override bool SetLineMode(IOLines line, LineMode mode)
        {
            if (camera == null || !camera.IsConnected) return false;
            var parmeter1 = camera.Parameters[PLCamera.LineSelector];
            parmeter1.SetValue(line.ToString());
            var parmeter2 = camera.Parameters[PLCamera.LineMode];
            parmeter2.SetValue(mode.ToString());
            return true;
        }

        public override bool SetLineStatus(IOLines line, LineStatus linestatus)
        {
            if (camera == null || !camera.IsConnected) return false;
            var parmeter1 = camera.Parameters[PLCamera.LineSelector];
            parmeter1.SetValue(line.ToString());
            var parmeter2 = camera.Parameters[PLCamera.LineLogic];
            switch (linestatus)
            {
                case LineStatus.Hight:
                    parmeter2.SetValue("Positive"); break;
                case LineStatus.Low:
                    parmeter2.SetValue("Negative"); break;
                default:
                    break;
            }

            return true;
        }

        public override bool GetLineStatus(IOLines line, out LineStatus linestatus)
        {
            line = IOLines.Line0;
            linestatus = LineStatus.Low;
            if (camera == null || !camera.IsConnected) return false;
            var parmeter1 = camera.Parameters[PLCamera.LineSelector];
            line = (IOLines)Enum.Parse(typeof(IOLines), parmeter1.GetValue());

            var parmeter2 = camera.Parameters[PLCamera.LineLogic];
            switch (parmeter2.GetValue())
            {
                case "Positive":
                    linestatus = LineStatus.Hight; break;
                case "Negative":
                    linestatus = LineStatus.Low; break;
                default:
                    linestatus = LineStatus.Low;
                    break;
            }
            return true;
        }


        public override bool AutoBalanceWhite()
        {
            try
            {
                if (camera == null || !camera.IsConnected) return false;
                var parmeter = camera.Parameters[PLCamera.BalanceWhiteAuto];
                parmeter.SetValue("Once");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region private

        protected override bool StartGrabbing() => true;

        protected override bool StopGrabbing()
        {
            if (camera == null) return false;
            if (camera.StreamGrabber.IsGrabbing)
                camera.StreamGrabber.Stop();
            return true;
        }



        private void OnGrabStopped(object sender, GrabStopEventArgs e)
        {

        }

        private void OnImageGrabbed(object sender, ImageGrabbedEventArgs e)
        {

            try
            {
                // Acquire the image from the camera. Only show the latest image. The camera may acquire images faster than the images can be displayed.

                // Get the grab result.
                IGrabResult grabResult = e.GrabResult;

                // Check if the image can be displayed.
                if (grabResult.IsValid)
                {
                    lock (monitor)
                    {

                        if (isgiveupfirst && grabResult.BlockID.Equals(1)) return;
                        latestFrame = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
                        // Lock the bits of the bitmap.
                        BitmapData bmpData = latestFrame.LockBits(new Rectangle(0, 0, latestFrame.Width, latestFrame.Height), ImageLockMode.ReadWrite, latestFrame.PixelFormat);
                        // Place the pointer to the buffer of the bitmap.
                        converter.OutputPixelFormat = PixelType.BGRA8packed;
                        IntPtr ptrBmp = bmpData.Scan0;
                        converter.Convert(ptrBmp, bmpData.Stride * latestFrame.Height, grabResult);
                        latestFrame.UnlockBits(bmpData);
                        //Debug.WriteLine("callback img");
                        ActionGetImage?.Invoke(latestFrame.Clone() as Bitmap);
                    }
                }
            }
            catch (Exception exception)
            {
                // ShowException(exception);
            }
            finally
            {
                // Dispose the grab result if needed for returning it to the grab loop.
                e.DisposeGrabResultIfClone();
                ClearLatestFrame();
            }
        }

        private void OnGrabStarted(object sender, EventArgs e)
        {

        }

        private void OnCameraClosed(object sender, EventArgs e)
        {

        }

        private void OnCameraOpened(object sender, EventArgs e)
        {

        }

        private void OnConnectionLost(object sender, EventArgs e)
        {

        }

        private void ClearLatestFrame()
        {
            lock (monitor)
            {
                if (latestFrame != null)
                {
                    latestFrame.Dispose();
                    latestFrame = null;
                }
            }
        }
        #endregion

    }
}
