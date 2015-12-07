using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace WPFDemo.Common
{
    public static class NativeMethods
    {

        [DllImport("kernel32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetVersionEx(ref OSVersionInfoEx osvi);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetProductInfo(
          int osMajorVersion,
          int osMinorVersion,
          int spMajorVersion,
          int spMinorVersion,
          ref uint type);

        [DllImport("kernel32.dll")]
        internal static extern int GetSystemMetrics(
          int index);

        public enum OSProductType : uint
        {
            VER_NT_WORKSTATION = 0x01,
            VER_NT_DOMAIN_CONTROLLER = 0x02,
            VER_NT_SERVER = 0x03
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool wow64Process);

        public const uint SM_SERVERR2 = 89;

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern bool BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight, HandleRef hSrcDC, int xSrc, int ySrc, int dwRop);

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OSVersionInfoEx
    {
        public int VersionInfoSize;
        public int MajorVersion;
        public int MinorVersion;
        public int BuildNumber;
        public int PlatformId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string CSDVersion;
        public Int16 ServicePackMajor;
        public Int16 ServicePackMinor;
        public Int16 SuiteMask;
        public byte ProductType;
        public byte Reserved;
    }

}