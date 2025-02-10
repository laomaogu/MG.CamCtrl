using System.Collections.Generic;
using MG.CamCtrl.Mode;

namespace MG.CamCtrl
{
    public class CamFactory
    {
        public CamFactory() { if (CameraList == null) CameraList = new List<ICamera>(); }

        private static List<ICamera> CameraList { get; set; } = new List<ICamera>() { };

        /// <summary>
        /// 按相机品牌获取相近SN枚举
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        public static List<string> GetDeviceEnum(CameraBrand brand)
        {
            ICamera camera = null;
            switch (brand)
            {
                case CameraBrand.DaHeng:
                    camera = new DaHengCamera();
                    break;
                case CameraBrand.HIK:
                    camera = new HIKCamera();
                    break;
                case CameraBrand.Basler:
                    camera = new BaslerCamera();
                    break;
                default: break;
            }
            return camera?.GetListEnum();
        }

        /// <summary>
        /// 按品牌创建相机
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        public static ICamera CreatCamera(CameraBrand brand)
        {
            ICamera returncamera = null;
            switch (brand)
            {
                case CameraBrand.DaHeng:
                    returncamera = new DaHengCamera();
                    break;
                case CameraBrand.HIK:
                    returncamera = new HIKCamera();
                    break;
                case CameraBrand.Basler:
                    returncamera = new BaslerCamera();
                    break;
                default:
                    break;
            }
            CameraList.Add(returncamera);
            return returncamera;
        }

        /// <summary>
        /// 获取对应SN的相机实例
        /// </summary>
        /// <param name="CamSN"></param>
        /// <returns></returns>
        public static ICamera GetItem(string CamSN)
        {
            ICamera cameraStandard = null;
            if (CameraList.Count < 1) return cameraStandard;

            foreach (var item in CameraList)
            {
                if ((item as BaseCamera).SN.Equals(CamSN))
                {
                    cameraStandard = item;
                    break;
                }
            }
            return cameraStandard;
        }

        /// <summary>
        /// 注销相机
        /// </summary>
        /// <param name="decamera"></param>
        public static void DestroyCamera(ICamera decamera)
        {
            CameraList?.Remove(decamera);
            decamera?.CloseDevice();
        }

        /// <summary>
        /// 注销所有相机
        /// </summary>
        public static void DestroyAll()
        {
            if (CameraList.Count < 1) return;
            foreach (var camereaitem in CameraList)
            {
                camereaitem?.CloseDevice();
            }
            CameraList?.Clear();
        }
    }

}



