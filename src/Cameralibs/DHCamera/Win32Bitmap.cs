using System.Runtime.InteropServices;
//String

//UInt32
using DWORD = System.UInt32;
//alias Win32 types to Framework types
//IntPtr
using HDC = System.IntPtr;
using LPVOID = System.IntPtr;
using UINT = System.UInt32;

//Int32

//Other


namespace MG.CamCtrl.Cameralibs.DHCmera
{
    public class CWin32Bitmaps
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPFILEHEADER
        {
            public ushort bfType;
            public int bfSize;
            public ushort bfReserved1;
            public ushort bfReserved2;
            public uint bfOffBits;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public RGBQUAD[] bmiColors;
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int SetStretchBltMode(
            HDC hdc,          // handle to DC
            int iStretchMode  // bitmap stretching mode
            );

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int StretchDIBits(
            HDC hdc,                   // handle to DC
            int XDest,                 // x-coord of destination upper-left corner
            int YDest,                 // y-coord of destination upper-left corner
            int nDestWidth,            // width of destination rectangle
            int nDestHeight,           // height of destination rectangle
            int XSrc,                  // x-coord of source upper-left corner
            int YSrc,                  // y-coord of source upper-left corner
            int nSrcWidth,             // width of source rectangle
            int nSrcHeight,            // height of source rectangle
            byte[] lpBits,             // bitmap bits
            LPVOID lpBitsInfo,         // bitmap data            
            UINT iUsage,               // usage options
            DWORD dwRop                // raster operation code
            );
    }
}

