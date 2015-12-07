using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace ImageOrganizer.Common.Utils
{
    public static class WinAPIUtils
    {

        #region Global Variables


        #endregion

        #region Static Properties

        /// <summary>
        /// Returns true if the process is running under Wow64
        /// </summary>
        public static bool IsWow64Process { get; private set; }
        /// <summary>
        /// Returns true if the process is running as 64bit
        /// </summary>
        /// <remarks>IntPtr.Size == 4 for 32bit processes, 8 for 64bit</remarks>
        public static bool Is64BitProcess { get { return IntPtr.Size == 8; } }
        /// <summary>
        /// Returns true if the process is 64bit or the process is Wow64
        /// </summary>
        public static bool Is64BitOS { get { return IsWow64Process || Is64BitProcess; } }

        private static bool CouldHave64BitVersion
        {
            get
            {
                return (System.Environment.OSVersion.Version.Major == 5 && System.Environment.OSVersion.Version.Minor >= 1) ||
                    System.Environment.OSVersion.Version.Major >= 6;
            }
        }

        #endregion

        #region Constructor

        static WinAPIUtils()
        {
            //Determine the 64bit-ness of this process
            if (CouldHave64BitVersion)
            {
                using (var Process = System.Diagnostics.Process.GetCurrentProcess())
                {
                    bool IsWow64;
                    if (NativeMethods.IsWow64Process(Process.Handle, out IsWow64))
                        IsWow64Process = IsWow64;
                }
            }
        }

        #endregion

        #region Static Methods

        //http://www.swart.ws/2009/03/kiosk-full-screen-wpf-applications.html
        public static System.Windows.Rect GetWindowWorkspace(System.Windows.Window Window)
        {
            var Interop = new System.Windows.Interop.WindowInteropHelper(Window);
            var Handle = Interop.Handle;
            if (Handle == IntPtr.Zero)
                return new System.Windows.Rect(0d, 0d, 0d, 0d);

            var MonitorHandle = NativeMethods.MonitorFromWindow(Handle, NativeMethods.MONITOR_DEFAULTTONEAREST);
            if (MonitorHandle == IntPtr.Zero)
                return new System.Windows.Rect(0d, 0d, 0d, 0d);
            var HandleRef = new HandleRef(Window, MonitorHandle);

            var MonitorInfo = new MONITORINFOEX();
            MonitorInfo.cbSize = Marshal.SizeOf(MonitorInfo);
            if (!NativeMethods.GetMonitorInfo(HandleRef, MonitorInfo))
                return new System.Windows.Rect(0d, 0d, 0d, 0d);

            return new System.Windows.Rect(MonitorInfo.rcWork.Left, MonitorInfo.rcWork.Top, MonitorInfo.rcWork.Right - MonitorInfo.rcWork.Left, MonitorInfo.rcWork.Bottom - MonitorInfo.rcWork.Top);
        }

        public static bool RecycleFile(string FullFilePath)
        {
            if (!System.IO.File.Exists(FullFilePath))
                return true;

            IntPtr FilePathPtr = IntPtr.Zero;
            try
            {
                var FullFilePathPtrValue = FullFilePath + Convert.ToChar(0) + Convert.ToChar(0);
                FilePathPtr = Marshal.StringToHGlobalAuto(FullFilePathPtrValue);
                if (Is64BitProcess)
                {
                    var Operation = new SHFILEOPSTRUCTx64();
                    Operation.wFunc = FO_Func.FO_DELETE;
                    Operation.pFrom = FilePathPtr;
                    Operation.fFlags = FO_FileOp.FOF_ALLOWUNDO | FO_FileOp.FOF_NOCONFIRMATION | FO_FileOp.FOF_SILENT | FO_FileOp.FOF_NOERRORUI;
                    int Result = NativeMethodsx64.SHFileOperation(ref Operation);
                    return Result == 0;
                }
                else
                {
                    var Operation = new SHFILEOPSTRUCTx86();
                    Operation.wFunc = FO_Func.FO_DELETE;
                    Operation.pFrom = FilePathPtr;
                    Operation.fFlags = FO_FileOp.FOF_ALLOWUNDO | FO_FileOp.FOF_NOCONFIRMATION | FO_FileOp.FOF_SILENT | FO_FileOp.FOF_NOERRORUI;
                    int Result = NativeMethods.SHFileOperation(ref Operation);
                    return Result == 0;
                }
            }
            finally
            {
                if (FilePathPtr != IntPtr.Zero)
                    Marshal.Release(FilePathPtr);
            }
        }

        #endregion

    }
}