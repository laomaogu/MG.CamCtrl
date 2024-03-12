# C#  MG.CamCtrl 相机库（开源） 海康 大恒

## 介绍

c# 相机库，含海康、大恒品牌2D相机的常用功能。
底层采用回调+信号量模式封装 ，最大程度减小线程资源，提高采图效率。

 
 功能持续完善中 

 ### 工厂模式创建实例

```csharp
ICamera myCamera;
myCamera= CamFactory.CreatCamera(CameraBrand.HIK);//CameraBrand.DaHeng
```
### 选取对应SN号的相机，初始化

```csharp
 //获取相机枚举
var devicelist = myCamera.GetListEnum();
//or
//var devicelist = CamFactory.GetDeviceEnum(CameraBrand.HIK); 

//选对应SN,初始化相机 
myCamera.InitDevice(devicelist.First());
```
### 启动相机
启动相机有以下几种方式：

 - 常规硬触发 
 - 常规软触发
 - 硬触发  +   回调
 - 软触发 + 回调

```csharp
 //硬触发模式  启动相机
myCamera.StartWith_HardTriggerModel(HardTriggerModel.Line0); 

//软触发模式  启动相机
//myCamera.StartWith_SoftTriggerModel(); 

//硬触发 + 回调模式  启动相机
// myCamera.StartWith_HardTriggerModel_SetCallback(HardTriggerModel.Line0, CameraCallBack); 

//软触发 + 回调模式  启动相机
//myCamera.StartWith_SoftTriggerModel_SetCallback(CameraCallBack); 
```
回调函数：

```csharp
/// <summary>
/// 回调函数
/// </summary>
/// <param name="bmp"></param>
private void CameraCallBack(Bitmap bmp)
{

	//执行取图后的操作
	bmp.Save("./test.bmp");
}
```

### 取图

```csharp
//等待硬触发 获取图片, 设定超时：5000ms
myCamera.GetImage(out Bitmap CaptureImage,5000);
//or 使用默认超时时间
//myCamera.GetImage(out Bitmap CaptureImage);

//软触发获取图像
//myCamera.GetImageWithSoftTrigger(out Bitmap CaptureImage);



/// <summary>
/// 回调函数
/// </summary>
/// <param name="bmp"></param>
private void CameraCallBack(Bitmap bmp)
{

	//执行取图后的操作
	bmp.Save("./test.bmp");
}

```

### 注销相机

```csharp  
//注销当前实例
myCamera.CloseDevice();
////or
//CamFactory.DestroyCamera(newcamera);
////or
//CamFactory.DestroyAll();   
 
```

### 参数设置/获取
含常用参数设置和获取
如曝光值、延时、硬触发方式等
详细见**接口**小节
 

### 接口 

```csharp
namespace MG.CamCtrl
{
    public interface ICamera : IDisposable
    {

        #region  operate
        /// <summary>
        /// 获取相机SN枚举
        /// </summary>
        /// <returns></returns>
        List<string> GetListEnum();

        /// <summary>
        /// 初始化相机
        /// </summary>
        /// <param name="CamSN"></param>
        /// <returns></returns>
        bool InitDevice(string CamSN);

        /// <summary>
        /// 注销相机
        /// </summary>
        void CloseDevice();

        /// <summary>
        /// 回调 + 循环采图 启动相机
        /// </summary>
        /// <param name="callbackfunc"></param>
        /// <returns></returns>
        bool StartWith_Continue_SetCallback(Action<Bitmap> callbackfunc);

        /// <summary>
        /// 软触发模式 启动相机
        /// </summary>
        /// <returns></returns>
        bool StartWith_SoftTriggerModel();

        /// <summary>
        /// 硬触发模式 启动相机
        /// </summary>
        /// <param name="hardtriggeritem"></param>
        /// <returns></returns>
        bool StartWith_HardTriggerModel(TriggerSource hardtriggeritem);

        /// <summary>
        /// 硬触发 + 回调 启动相机
        /// </summary>
        /// <param name="hardtriggeritem"></param>
        /// <param name="callbackfunc"></param>
        /// <returns></returns>
        bool StartWith_HardTriggerModel_SetCallback(TriggerSource hardtriggeritem, Action<Bitmap> callbackfunc);

        /// <summary>
        /// 软触发 + 回调 启动相机
        /// </summary>
        /// <param name="callbackfunc"></param>
        /// <returns></returns>
        bool StartWith_SoftTriggerModel_SetCallback(Action<Bitmap> callbackfunc);

        /// <summary>
        /// 等待硬触发获取图像
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="outtime"></param>
        /// <returns></returns>
        bool GetImage(out Bitmap bitmap, int outtime = 3000);

        /// <summary>
        /// 软触发获取图像
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="outtime"></param>
        /// <returns></returns>
        bool GetImageWithSoftTrigger(out Bitmap bitmap, int outtime = 3000);

        /// <summary>
        /// 软触发
        /// </summary>
        /// <returns></returns>
        bool SoftTrigger();

        #endregion


        #region SettingConfig
        /// <summary>
        /// 设置相机参数
        /// </summary>
        /// <param name="config"></param>
        void SetCamConfig(CamConfig config);
        /// <summary>
        /// 获取相机参数
        /// </summary>
        /// <param name="config"></param>
        void GetCamConfig(out CamConfig config);

        /// <summary>
        /// 设置触发模式及触发源
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="triggerEnum"></param>
        /// <returns></returns>
        bool SetTriggerMode(TriggerMode mode, TriggerSource triggerEnum = TriggerSource.Line0);

        /// <summary>
        /// 获取触发模式及触发源
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="hardTriggerModel"></param>
        /// <returns></returns>
        bool GetTriggerMode(out TriggerMode mode, out TriggerSource hardTriggerModel);

        /// <summary>
        /// 设置曝光时长
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool SetExpouseTime(ushort value);

        /// <summary>
        /// 获取曝光时长
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool GetExpouseTime(out ushort value);

        /// <summary>
        /// 设置硬触发极性
        /// </summary>
        /// <param name="polarity"></param>
        /// <returns></returns>
        bool SetTriggerPolarity(TriggerPolarity polarity);

        /// <summary>
        /// 获取硬触发极性
        /// </summary>
        /// <param name="polarity"></param>
        /// <returns></returns>
        bool GetTriggerPolarity(out TriggerPolarity polarity);

        /// <summary>
        /// 设置触发滤波时间 （us）
        /// </summary>
        /// <param name="flitertime"></param>
        /// <returns></returns>
        bool SetTriggerFliter(ushort flitertime);

        /// <summary>
        /// 获取触发滤波时间 （us）
        /// </summary>
        /// <param name="flitertime"></param>
        /// <returns></returns>
        bool GetTriggerFliter(out ushort flitertime);

        /// <summary>
        /// 设置触发延时
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        bool SetTriggerDelay(ushort delay);

        /// <summary>
        /// 获取触发延时
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        bool GetTriggerDelay(out ushort delay);

        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="gain"></param>
        /// <returns></returns>
        bool SetGain(short gain);

        /// <summary>
        /// 获取增益值
        /// </summary>
        /// <param name="gain"></param>
        /// <returns></returns>
        bool GetGain(out short gain);

        /// <summary>
        /// 设置信号线模式
        /// </summary>
        /// <param name="line"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        bool SetLineMode(IOLines line, LineMode mode);

        /// <summary>
        /// 设置信号线电平状态
        /// </summary>
        /// <param name="line"></param>
        /// <param name="linestatus"></param>
        /// <returns></returns>
        bool SetLineStatus(IOLines line, LineStatus linestatus);

        /// <summary>
        /// 获取信号线电平状态
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lineStatus"></param>
        /// <returns></returns>
        bool GetLineStatus(IOLines line, out LineStatus lineStatus);

        /// <summary>
        /// 自动白平衡
        /// </summary>
        /// <returns></returns>
        bool AutoBalanceWhite();

        #endregion

    }
}

```

