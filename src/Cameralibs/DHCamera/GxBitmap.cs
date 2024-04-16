using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GxIAPINET;

namespace MG.CamCtrl.Cameralibs.DHCmera
{
    public class GxBitmap
    {

        IGXDevice m_objIGXDevice = null;                      ///<设备对像
        PictureBox m_pic_ShowImage = null;                      ///<图片显示控件
        bool m_bIsColor = false;                                ///<是否支持彩色相机
        byte[] m_byMonoBuffer = null;                           ///<黑白相机buffer
        byte[] m_byColorBuffer = null;                          ///<彩色相机buffer
        byte[] m_byRawBuffer = null;                            ///<用于存储Raw图的Buffer
        int m_nPayloadSize = 0;                                 ///<图像数据大小
        int m_nWidth = 0;                                       ///<图像宽度
        int m_nHeigh = 0;                                       ///<图像高度
        Bitmap m_bitmapForSave = null;                          ///<bitmap对象,仅供存储图像使用
        const uint PIXEL_FORMATE_BIT = 0x00FF0000;              ///<用于与当前的数据格式进行与运算得到当前的数据位数
        const uint GX_PIXEL_8BIT = 0x00080000;                  ///<8位数据图像格式
       // IImageProcessConfig m_objCfg = null;                           ///<图像配置参数对象
                                                                       ///
        const int COLORONCOLOR = 3;
        const uint DIB_RGB_COLORS = 0;
        const uint SRCCOPY = 0x00CC0020;
        CWin32Bitmaps.BITMAPINFO m_objBitmapInfo = new CWin32Bitmaps.BITMAPINFO();
        IntPtr m_pBitmapInfo = IntPtr.Zero;
        Graphics m_objGC = null;
        IntPtr m_pHDC = IntPtr.Zero;


       
        public GxBitmap(IGXDevice objIGXDevice )
        {
            m_objIGXDevice = objIGXDevice;
      
            if (null != objIGXDevice)
            {
                //获得图像原始数据大小、宽度、高度等
                m_nPayloadSize = (int)objIGXDevice.GetRemoteFeatureControl().GetIntFeature("PayloadSize").GetValue();
                m_nWidth = (int)objIGXDevice.GetRemoteFeatureControl().GetIntFeature("Width").GetValue();
                m_nHeigh = (int)objIGXDevice.GetRemoteFeatureControl().GetIntFeature("Height").GetValue();

                //获取是否为彩色相机
                __IsSupportColor(ref m_bIsColor);
            }

            //申请用于缓存图像数据的buffer
            m_byRawBuffer = new byte[m_nPayloadSize];
            m_byMonoBuffer = new byte[__GetStride(m_nWidth, m_bIsColor) * m_nHeigh];
            m_byColorBuffer = new byte[__GetStride(m_nWidth, m_bIsColor) * m_nHeigh];

            __CreateBitmap(out m_bitmapForSave, m_nWidth, m_nHeigh, m_bIsColor);

            //m_objGC = m_pic_ShowImage.CreateGraphics();
            //m_pHDC = m_objGC.GetHdc();
            if (m_bIsColor)
            {
                m_objBitmapInfo.bmiHeader.biSize = (uint)Marshal.SizeOf(typeof(CWin32Bitmaps.BITMAPINFOHEADER));
                m_objBitmapInfo.bmiHeader.biWidth = m_nWidth;
                m_objBitmapInfo.bmiHeader.biHeight = m_nHeigh;
                m_objBitmapInfo.bmiHeader.biPlanes = 1;
                m_objBitmapInfo.bmiHeader.biBitCount = 24;
                m_objBitmapInfo.bmiHeader.biCompression = 0;
                m_objBitmapInfo.bmiHeader.biSizeImage = 0;
                m_objBitmapInfo.bmiHeader.biXPelsPerMeter = 0;
                m_objBitmapInfo.bmiHeader.biYPelsPerMeter = 0;
                m_objBitmapInfo.bmiHeader.biClrUsed = 0;
                m_objBitmapInfo.bmiHeader.biClrImportant = 0;
            }
            else
            {
                m_objBitmapInfo.bmiHeader.biSize = (uint)Marshal.SizeOf(typeof(CWin32Bitmaps.BITMAPINFOHEADER));
                m_objBitmapInfo.bmiHeader.biWidth = m_nWidth;
                m_objBitmapInfo.bmiHeader.biHeight = m_nHeigh;
                m_objBitmapInfo.bmiHeader.biPlanes = 1;
                m_objBitmapInfo.bmiHeader.biBitCount = 8;
                m_objBitmapInfo.bmiHeader.biCompression = 0;
                m_objBitmapInfo.bmiHeader.biSizeImage = 0;
                m_objBitmapInfo.bmiHeader.biXPelsPerMeter = 0;
                m_objBitmapInfo.bmiHeader.biYPelsPerMeter = 0;
                m_objBitmapInfo.bmiHeader.biClrUsed = 0;
                m_objBitmapInfo.bmiHeader.biClrImportant = 0;

                m_objBitmapInfo.bmiColors = new CWin32Bitmaps.RGBQUAD[256];
                // 黑白图像需要初始化调色板
                for (int i = 0; i < 256; i++)
                {
                    m_objBitmapInfo.bmiColors[i].rgbBlue = (byte)i;
                    m_objBitmapInfo.bmiColors[i].rgbGreen = (byte)i;
                    m_objBitmapInfo.bmiColors[i].rgbRed = (byte)i;
                    m_objBitmapInfo.bmiColors[i].rgbReserved = 0;
                }
            }
            m_pBitmapInfo = Marshal.AllocHGlobal(2048);
            Marshal.StructureToPtr(m_objBitmapInfo, m_pBitmapInfo, false);
        }
         
        /// <summary>
        /// 存储图像
        /// </summary>
        /// <param name="objIBaseData">图像数据对象</param>
        /// <param name="strFilePath">显示图像文件名</param>
        public void SaveBmp(IBaseData objIBaseData, string strFilePath)
        {
            GX_VALID_BIT_LIST emValidBits = GX_VALID_BIT_LIST.GX_BIT_0_7;

            //检查图像是否改变并更新Buffer
            __UpdateBufferSize(objIBaseData);

            if (null != objIBaseData)
            {
                emValidBits = __GetBestValudBit(objIBaseData.GetPixelFormat());
                if (m_bIsColor)
                {
                    IntPtr pBufferColor = objIBaseData.ConvertToRGB24(emValidBits, GX_BAYER_CONVERT_TYPE_LIST.GX_RAW2RGB_NEIGHBOUR, false);
                    Marshal.Copy(pBufferColor, m_byColorBuffer, 0, __GetStride(m_nWidth, m_bIsColor) * m_nHeigh);
                    __UpdateBitmapForSave(m_byColorBuffer);
                }
                else
                {
                    IntPtr pBufferMono = IntPtr.Zero;
                    if (__IsPixelFormat8(objIBaseData.GetPixelFormat()))
                    {
                        pBufferMono = objIBaseData.GetBuffer();
                    }
                    else
                    {
                        pBufferMono = objIBaseData.ConvertToRaw8(emValidBits);
                    }
                    Marshal.Copy(pBufferMono, m_byMonoBuffer, 0, __GetStride(m_nWidth, m_bIsColor) * m_nHeigh);

                    __UpdateBitmapForSave(m_byMonoBuffer);
                }
                m_bitmapForSave.Save(strFilePath, ImageFormat.Bmp);
            }
        }


        /// <summary>
        /// 用于图像处理后并显示图像
        /// </summary>
        /// <param name="objCfg">图像处理参数配置对象</param>
        /// <param name="objIBaseData">图像数据对象</param>
        public Bitmap GetImageProcess(IImageProcessConfig objCfg, IBaseData objIBaseData)
        {
            //检查图像是否改变并更新Buffer
            __UpdateBufferSize(objIBaseData);

            if (null != objIBaseData)
            {
                if (GX_FRAME_STATUS_LIST.GX_FRAME_STATUS_SUCCESS == objIBaseData.GetStatus())
                {
                    if (m_bIsColor)
                    {
                        IntPtr pBufferColor = objIBaseData.ImageProcess(objCfg);
                        Marshal.Copy(pBufferColor, m_byColorBuffer, 0, __GetStride(m_nWidth, m_bIsColor) * m_nHeigh);
                        //__ShowImage(m_byColorBuffer);
                        __UpdateBitmapForSave(m_byColorBuffer);
                    }
                    else
                    {
                        IntPtr pBufferMono = objIBaseData.ImageProcess(objCfg);
                        Marshal.Copy(pBufferMono, m_byMonoBuffer, 0, __GetStride(m_nWidth, m_bIsColor) * m_nHeigh);

                        //__ShowImage(m_byMonoBuffer);
                        __UpdateBitmapForSave(m_byColorBuffer);
                    }
                   
                }
            }
            return GetBmp(objIBaseData);
        }
        /// <summary>
        /// 获取图像
        /// </summary>
        /// <param name="objIBaseData">图像数据对象</param>
        /// <param name="strFilePath">显示图像文件名</param>
        public Bitmap GetBmp(IBaseData objIBaseData )
        {
            GX_VALID_BIT_LIST emValidBits = GX_VALID_BIT_LIST.GX_BIT_0_7;

            //检查图像是否改变并更新Buffer
            __UpdateBufferSize(objIBaseData);

            if (null != objIBaseData)
            {
                emValidBits = __GetBestValudBit(objIBaseData.GetPixelFormat());
                if (m_bIsColor)
                {
                    IntPtr pBufferColor = objIBaseData.ConvertToRGB24(emValidBits, GX_BAYER_CONVERT_TYPE_LIST.GX_RAW2RGB_NEIGHBOUR, false);
                    Marshal.Copy(pBufferColor, m_byColorBuffer, 0, __GetStride(m_nWidth, m_bIsColor) * m_nHeigh);
                    __UpdateBitmapForSave(m_byColorBuffer);
                }
                else
                {
                    IntPtr pBufferMono = IntPtr.Zero;
                    if (__IsPixelFormat8(objIBaseData.GetPixelFormat()))
                    {
                        pBufferMono = objIBaseData.GetBuffer();
                    }
                    else
                    {
                        pBufferMono = objIBaseData.ConvertToRaw8(emValidBits);
                    }
                    Marshal.Copy(pBufferMono, m_byMonoBuffer, 0, __GetStride(m_nWidth, m_bIsColor) * m_nHeigh);

                    __UpdateBitmapForSave(m_byMonoBuffer);
                }
                // m_bitmapForSave.Save(strFilePath, ImageFormat.Bmp);
            }

            return DeepCopyBitmap(m_bitmapForSave);
            //return m_bitmapForSave.Clone(new Rectangle(0, 0, m_bitmapForSave.Width, m_bitmapForSave.Height), m_bitmapForSave.PixelFormat);/*m_bitmapForSave.Clone() as Bitmap;*/
        }


        public static Bitmap DeepCopyBitmap(Bitmap bitmap)

        {
            try
            {

                Bitmap dstBitmap = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, bitmap);
                    ms.Seek(0, SeekOrigin.Begin);
                    dstBitmap = (Bitmap)bf.Deserialize(ms);
                    ms.Close();
                }
                return dstBitmap;
            }
            catch (Exception ex)
            {
                
                return null;
            }

        }


        /// <summary>
        /// 存储Raw图像
        /// </summary>
        /// <param name="objIBaseData">图像数据对象</param>
        /// <param name="strFilePath">显示图像文件名</param>
        public void SaveRaw(IBaseData objIBaseData, string strFilePath)
        {
            Stream objFileStream = new FileStream(strFilePath, FileMode.Create);
            BinaryWriter objSW = new BinaryWriter(objFileStream);

            //检查图像是否改变并更新Buffer
            __UpdateBufferSize(objIBaseData);

            if (null != objIBaseData)
            {
                IntPtr pBufferRaw = objIBaseData.GetBuffer();
                Marshal.Copy(pBufferRaw, m_byRawBuffer, 0, m_nPayloadSize);
            }

            objSW.Write(m_byRawBuffer);
            objSW.Close();
            objFileStream.Close();
        }

        /// <summary>
        /// 检查图像是否改变并更新Buffer
        /// </summary>
        /// <param name="objIBaseData">图像数据对象</param>
        private void __UpdateBufferSize(IBaseData objIBaseData)
        {
            if (null != objIBaseData)
            {
                if (__IsCompatible(m_bitmapForSave, m_nWidth, m_nHeigh, m_bIsColor))
                {
                    m_nPayloadSize = (int)objIBaseData.GetPayloadSize();
                    m_nWidth = (int)objIBaseData.GetWidth();
                    m_nHeigh = (int)objIBaseData.GetHeight();
                }
                else
                {
                    m_nPayloadSize = (int)objIBaseData.GetPayloadSize();
                    m_nWidth = (int)objIBaseData.GetWidth();
                    m_nHeigh = (int)objIBaseData.GetHeight();

                    m_byRawBuffer = new byte[m_nPayloadSize];
                    m_byMonoBuffer = new byte[__GetStride(m_nWidth, m_bIsColor) * m_nHeigh];
                    m_byColorBuffer = new byte[__GetStride(m_nWidth, m_bIsColor) * m_nHeigh];

                    //更新BitmapInfo
                    m_objBitmapInfo.bmiHeader.biWidth = m_nWidth;
                    m_objBitmapInfo.bmiHeader.biHeight = m_nHeigh;
                    Marshal.StructureToPtr(m_objBitmapInfo, m_pBitmapInfo, false);
                }
            }
        }

        /// <summary>
        /// 更新存储数据
        /// </summary>
        /// <param name="byBuffer">图像buffer</param>
        private void __UpdateBitmapForSave(byte[] byBuffer)
        {
            if (__IsCompatible(m_bitmapForSave, m_nWidth, m_nHeigh, m_bIsColor))
            {
                __UpdateBitmap(m_bitmapForSave, byBuffer, m_nWidth, m_nHeigh, m_bIsColor);
            }
            else
            {
                __CreateBitmap(out m_bitmapForSave, m_nWidth, m_nHeigh, m_bIsColor);
                __UpdateBitmap(m_bitmapForSave, byBuffer, m_nWidth, m_nHeigh, m_bIsColor);
            }
        }

      

        /// <summary>
        /// 判断PixelFormat是否为8位
        /// </summary>
        /// <param name="emPixelFormatEntry">图像数据格式</param>
        /// <returns>true为8为数据，false为非8位数据</returns>
        private bool __IsPixelFormat8(GX_PIXEL_FORMAT_ENTRY emPixelFormatEntry)
        {
            bool bIsPixelFormat8 = false;
            uint uiPixelFormatEntry = (uint)emPixelFormatEntry;
            if ((uiPixelFormatEntry & PIXEL_FORMATE_BIT) == GX_PIXEL_8BIT)
            {
                bIsPixelFormat8 = true;
            }
            return bIsPixelFormat8;
        }

        /// <summary>
        /// 通过GX_PIXEL_FORMAT_ENTRY获取最优Bit位
        /// </summary>
        /// <param name="em">图像数据格式</param>
        /// <returns>最优Bit位</returns>
        private GX_VALID_BIT_LIST __GetBestValudBit(GX_PIXEL_FORMAT_ENTRY emPixelFormatEntry)
        {
            GX_VALID_BIT_LIST emValidBits = GX_VALID_BIT_LIST.GX_BIT_0_7;
            switch (emPixelFormatEntry)
            {
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO8:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR8:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG8:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB8:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG8:
                    {
                        emValidBits = GX_VALID_BIT_LIST.GX_BIT_0_7;
                        break;
                    }
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO10:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR10:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG10:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB10:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG10:
                    {
                        emValidBits = GX_VALID_BIT_LIST.GX_BIT_2_9;
                        break;
                    }
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO12:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR12:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG12:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB12:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG12:
                    {
                        emValidBits = GX_VALID_BIT_LIST.GX_BIT_4_11;
                        break;
                    }
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO14:
                    {
                        //暂时没有这样的数据格式待升级
                        break;
                    }
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO16:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR16:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG16:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB16:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG16:
                    {
                        //暂时没有这样的数据格式待升级
                        break;
                    }
                default:
                    break;
            }
            return emValidBits;
        }

        /// <summary>
        /// 获取图像显示格式
        /// </summary>
        /// <param name="bIsColor">是否为彩色相机</param>
        /// <returns>图像的数据格式</returns>
        private PixelFormat __GetFormat(bool bIsColor)
        {
            return bIsColor ? PixelFormat.Format24bppRgb : PixelFormat.Format8bppIndexed;
        }

        /// <summary>
        /// 计算宽度所占的字节数
        /// </summary>
        /// <param name="nWidth">图像宽度</param>
        /// <param name="bIsColor">是否是彩色相机</param>
        /// <returns>图像一行所占的字节数</returns>
        private int __GetStride(int nWidth, bool bIsColor)
        {
            return bIsColor ? nWidth * 3 : nWidth;
        }

        /// <summary>
        /// 判断是否兼容
        /// </summary>
        /// <param name="bitmap">Bitmap对象</param>
        /// <param name="nWidth">图像宽度</param>
        /// <param name="nHeight">图像高度</param>
        /// <param name="bIsColor">是否是彩色相机</param>
        /// <returns>true为一样，false不一样</returns>
        private bool __IsCompatible(Bitmap bitmap, int nWidth, int nHeight, bool bIsColor)
        {
            try
            {
                if (bitmap == null
               || bitmap.Height != nHeight
               || bitmap.Width != nWidth
               || bitmap.PixelFormat != __GetFormat(bIsColor)
            )
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
           
            return true;
        }

        /// <summary>
        /// 创建Bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap对象</param>
        /// <param name="nWidth">图像宽度</param>
        /// <param name="nHeight">图像高度</param>
        /// <param name="bIsColor">是否是彩色相机</param>
        private void __CreateBitmap(out Bitmap bitmap, int nWidth, int nHeight, bool bIsColor)
        {
            bitmap = new Bitmap(nWidth, nHeight, __GetFormat(bIsColor));
            if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                ColorPalette colorPalette = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    colorPalette.Entries[i] = Color.FromArgb(i, i, i);
                }
                bitmap.Palette = colorPalette;
            }
        }

        /// <summary>
        /// 更新和复制图像数据到Bitmap的buffer
        /// </summary>
        /// <param name="bitmap">Bitmap对象</param>
        /// <param name="nWidth">图像宽度</param>
        /// <param name="nHeight">图像高度</param>
        /// <param name="bIsColor">是否是彩色相机</param>
        private void __UpdateBitmap(Bitmap bitmap, byte[] byBuffer, int nWidth, int nHeight, bool bIsColor)
        {
            //给BitmapData加锁
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

            //得到一个指向Bitmap的buffer指针
            IntPtr ptrBmp = bmpData.Scan0;
            int nImageStride = __GetStride(m_nWidth, bIsColor);
            //图像宽能够被4整除直接copy
            if (nImageStride == bmpData.Stride)
            {
                Marshal.Copy(byBuffer, 0, ptrBmp, bmpData.Stride * bitmap.Height);
            }
            else//图像宽不能够被4整除按照行copy
            {
                for (int i = 0; i < bitmap.Height; ++i)
                {
                    Marshal.Copy(byBuffer, i * nImageStride, new IntPtr(ptrBmp.ToInt64() + i * bmpData.Stride), m_nWidth);
                }
            }
            //BitmapData解锁
            bitmap.UnlockBits(bmpData);
        }

        /// <summary>
        /// 是否支持彩色
        /// </summary>
        /// <param name="bIsColorFilter">是否支持彩色</param>
        private void __IsSupportColor(ref bool bIsColorFilter)
        {
            bool bIsImplemented = false;
            bool bIsMono = false;
            string strPixelFormat = "";

            strPixelFormat = m_objIGXDevice.GetRemoteFeatureControl().GetEnumFeature("PixelFormat").GetValue();
            if (0 == string.Compare(strPixelFormat, 0, "Mono", 0, 4))
            {
                bIsMono = true;
            }
            else
            {
                bIsMono = false;
            }

            bIsImplemented = m_objIGXDevice.GetRemoteFeatureControl().IsImplemented("PixelColorFilter");

            // 若当前为非黑白且支持PixelColorFilter则为彩色
            if ((!bIsMono) && (bIsImplemented))
            {
                bIsColorFilter = true;
            }
            else
            {
                bIsColorFilter = false;
            }
        }
    }
}


