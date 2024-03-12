﻿using MG.CamCtrl.Common.Enum;
using MG.CamCtrl.Common.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace MG.CamCtrl.Mode
{
    internal  abstract class BaseCamera: ICamera
    {
        protected BaseCamera() { ActionGetImage += ResetActionImageSignal; }


        #region Parm
        public string SN { get; set; } = string.Empty;

        /// <summary>
        /// 回调委托，获取图像数据，+= 赋值,子类要添加到回调中
        /// </summary>
        protected Action<Bitmap> ActionGetImage { get; set; }

        protected AutoResetEvent ResetGetImageSignal = new AutoResetEvent(false);
        protected Bitmap CallBaclImg { get; set; }

        #endregion


        #region  operate

        public abstract void CloseDevice();

        public abstract List<string> GetListEnum();

        public abstract bool InitDevice(string CamSN);

        public bool StartWith_Continue_SetCallback(Action<Bitmap> callbackfunc)
        {
            SetTriggerMode(TriggerMode.Off);
            if (callbackfunc != null) ActionGetImage += callbackfunc;
            return StartGrabbing();
        }


        public bool StartWith_SoftTriggerModel()
        {
            SetTriggerMode(TriggerMode.On, TriggerSource.Software);
            return StartGrabbing();
        }

        public bool StartWith_HardTriggerModel(TriggerSource hardtriggeritem)
        {
            if (hardtriggeritem == TriggerSource.Software) hardtriggeritem = TriggerSource.Line0;
            SetTriggerMode(TriggerMode.On, hardtriggeritem);
            return StartGrabbing();
        }

        public bool StartWith_HardTriggerModel_SetCallback(TriggerSource hardtriggeritem, Action<Bitmap> callbackfunc)
        {
            if (hardtriggeritem == TriggerSource.Software) hardtriggeritem = TriggerSource.Line0;
            SetTriggerMode(TriggerMode.On, hardtriggeritem);
            if (callbackfunc != null) ActionGetImage += callbackfunc;
            return StartGrabbing();
        }

        public bool StartWith_SoftTriggerModel_SetCallback(Action<Bitmap> callbackfunc)
        {
            SetTriggerMode(TriggerMode.On, TriggerSource.Software);
            if (callbackfunc != null) ActionGetImage += callbackfunc;
            return StartGrabbing();
        }

        /// <summary>
        /// 等待硬触发获取图像
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="outtime"></param>
        /// <returns></returns>
        public bool GetImage(out Bitmap bitmap, int outtime = 3000)
        {
            bitmap = null;
            if (ResetGetImageSignal.WaitOne(outtime))
            {
                bitmap = CallBaclImg.Clone() as Bitmap;
                CallBaclImg?.Dispose();
                return true;
            }
            CallBaclImg?.Dispose();
            return false;
        }

        /// <summary>
        /// 软触发获取图像
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="outtime"></param>
        /// <returns></returns>
        public bool GetImageWithSoftTrigger(out Bitmap bitmap, int outtime = 3000)
        {
            bitmap = null;
            if (!SoftTrigger()) return false;

            if (ResetGetImageSignal.WaitOne(outtime))
            {
                bitmap = CallBaclImg.Clone() as Bitmap;
                CallBaclImg?.Dispose();
                return true;
            }
            CallBaclImg?.Dispose();
            return false;
        }

        /// <summary>
        /// 软触发
        /// </summary>
        /// <returns></returns>
        public abstract bool SoftTrigger();

        #endregion


        #region SettingConfig
        public void SetCamConfig(CamConfig config)
        {
            if (config == null) return;
            SetExpouseTime(config.ExpouseTime);
            SetTriggerMode(config.triggerMode, config.triggeSource);
            SetTriggerPolarity(config.triggerPolarity);
            SetTriggerFliter(config.TriggerFilter);
            SetGain(config.Gain);
            SetTriggerDelay(config.TriggerDelay);
        }

        public void GetCamConfig(out CamConfig config)
        {
            GetExpouseTime(out ushort expouseTime);
            GetTriggerMode(out TriggerMode triggerMode, out TriggerSource hardwareTriggerModel);
            GetTriggerPolarity(out TriggerPolarity triggerPolarity);
            GetTriggerFliter(out ushort triggerfilter);
            GetGain(out short gain);
            GetTriggerDelay(out ushort triggerdelay);

            config = new CamConfig()
            {
                triggerMode = triggerMode,
                triggeSource = hardwareTriggerModel,
                triggerPolarity = triggerPolarity,
                TriggerFilter = triggerfilter,
                TriggerDelay = triggerdelay,
                ExpouseTime = expouseTime,
                Gain = gain
            };
        }


        /// <summary>
        /// 设置触发模式及触发源
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="triggerEnum"></param>
        /// <returns></returns>
        public abstract bool SetTriggerMode(TriggerMode mode, TriggerSource triggerEnum = TriggerSource.Line0);

        public abstract bool GetTriggerMode(out TriggerMode mode, out TriggerSource hardTriggerModel);



        public abstract bool SetExpouseTime(ushort value);

        public abstract bool GetExpouseTime(out ushort value);



        public abstract bool SetTriggerPolarity(TriggerPolarity polarity);

        public abstract bool GetTriggerPolarity(out TriggerPolarity polarity);



        /// <summary>
        /// 设置触发滤波时间 （us）
        /// </summary>
        /// <param name="flitertime"></param>
        /// <returns></returns>
        public abstract bool SetTriggerFliter(ushort flitertime);

        /// <summary>
        /// 获取触发参数时间 （us）
        /// </summary>
        /// <param name="flitertime"></param>
        /// <returns></returns>
        public abstract bool GetTriggerFliter(out ushort flitertime);


        public abstract bool SetTriggerDelay(ushort delay);

        public abstract bool GetTriggerDelay(out ushort delay);


        public abstract bool SetGain(short gain);

        public abstract bool GetGain(out short gain);

        public abstract bool SetLineMode(IOLines line, LineMode mode);
        public abstract bool SetLineStatus(IOLines line, LineStatus linestatus);
        public abstract bool GetLineStatus(IOLines line, out LineStatus lineStatus);

        public abstract bool AutoBalanceWhite();

        #endregion


        #region  protected abstract


        /// <summary>
        /// 开始采图
        /// </summary>
        /// <returns></returns>
        protected abstract bool StartGrabbing();

        /// <summary>
        /// 停止采图
        /// </summary>
        /// <returns></returns>
        protected abstract bool StopGrabbing();

        private void ResetActionImageSignal(Bitmap bitmap)
        {
            CallBaclImg = bitmap;
            ResetGetImageSignal.Set();
        }
        public void Dispose()
        {

            CallBaclImg?.Dispose();


        }
        #endregion
    }
}


