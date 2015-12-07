using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace WPFDemo.Common
{
    public static class SystemUtils
    {

        private static bool? _Is64BitOS;
        public static bool IsRunningAs64Bit()
        {
            if (IntPtr.Size == 8)
                return true;
            else
                return false;
        }
        public static bool GetIs64BitOS()
        {
            //http://stackoverflow.com/a/336729
            if (_Is64BitOS == null)
            {
                if (!IsRunningAs64Bit())
                {
                    var VersionInfo = SystemUtils.VersionInfo;
                    if (VersionInfo.MajorVersion >= 6 || (VersionInfo.MajorVersion == 5 && VersionInfo.MinorVersion >= 1))
                    {
                        using (var Process = System.Diagnostics.Process.GetCurrentProcess())
                        {
                            bool Value;
                            if (!NativeMethods.IsWow64Process(Process.Handle, out Value))
                                Value = false;
                            _Is64BitOS = Value;
                        }
                    }
                }
                else
                    _Is64BitOS = true;
            }
            return _Is64BitOS.Value;
        }
        private static OSVersionInfoEx? _VersionInfo;
        private static readonly object VersionInfoLock = new object();
        public static OSVersionInfoEx VersionInfo
        {
            get
            {
                if (_VersionInfo == null)
                {
                    lock (VersionInfoLock)
                    {
                        if (_VersionInfo == null)
                        {
                            _VersionInfo = GetVersionEx();
                        }
                    }
                }
                return _VersionInfo.Value;
            }
        }
        private static OSVersionInfoEx GetVersionEx()
        {
            OSVersionInfoEx OSVersionInfoEx = new OSVersionInfoEx();
            OSVersionInfoEx.VersionInfoSize = Marshal.SizeOf(OSVersionInfoEx);
            NativeMethods.GetVersionEx(ref OSVersionInfoEx);
            return OSVersionInfoEx;
        }

        public static bool GetIsServerOS()
        {
            //http://msdn.microsoft.com/en-us/library/windows/desktop/ms724833%28v=vs.85%29.aspx
            var VersionInfo = SystemUtils.VersionInfo;
            if (VersionInfo.MajorVersion >= 6 || VersionInfo.MinorVersion >= 2)
                return VersionInfo.ProductType != (int)NativeMethods.OSProductType.VER_NT_WORKSTATION;
            else
                return false;
        }
        public static bool GetIsXPOS()
        {
            //5.1 and up
            var VersionInfo = SystemUtils.VersionInfo;
            return
                VersionInfo.MajorVersion >= 6 ||
                VersionInfo.MajorVersion == 5 && (VersionInfo.MinorVersion >= 1);
        }
        public static bool GetIsVistaOS()
        {
            //6.0 and up
            var VersionInfo = SystemUtils.VersionInfo;
            return VersionInfo.MajorVersion >= 6;
        }
        public static bool GetIsWindows7OS()
        {
            //6.1 and up
            var VersionInfo = SystemUtils.VersionInfo;
            return
                VersionInfo.MajorVersion > 6 ||
                VersionInfo.MajorVersion == 6 && VersionInfo.MinorVersion >= 1;
        }
        public static bool GetIsWindows8OS()
        {
            //6.2 and up
            var VersionInfo = SystemUtils.VersionInfo;
            return
                VersionInfo.MajorVersion > 6 ||
                VersionInfo.MajorVersion == 6 && VersionInfo.MinorVersion >= 2;
        }

    }
}