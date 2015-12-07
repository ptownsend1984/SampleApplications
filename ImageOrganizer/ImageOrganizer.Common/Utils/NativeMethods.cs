using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace ImageOrganizer.Common.Utils
{
    internal static class NativeMethods
    {

        #region Constants

        // Nearest monitor to window
        public const int MONITOR_DEFAULTTONEAREST = 2;

        #endregion

        #region Static Methods

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int SHFileOperation(ref SHFILEOPSTRUCTx86 FileOp);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr processHandle,
             [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

        // To get a handle to the specified monitor
        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

        // To get the working area of the specified monitor
        [DllImport("user32.dll")]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MONITORINFOEX monitorInfo);

        #endregion

    }

    #region Structs

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    internal struct SHFILEOPSTRUCTx86
    {
        public IntPtr hwnd;
        public FO_Func wFunc;
        public IntPtr pFrom;
        public IntPtr pTo;
        public FO_FileOp fFlags;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fAnyOperationsAborted;
        public IntPtr hNameMappings;
        public string lpszProgressTitle; //only used if FOF_SIMPLEPROGRESS
    }

    // Rectangle (used by MONITORINFOEX)
    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    // Monitor information (used by GetMonitorInfo())
    [StructLayout(LayoutKind.Sequential)]
    internal class MONITORINFOEX
    {
        public int cbSize;
        public RECT rcMonitor; // Total area
        public RECT rcWork; // Working area
        public int dwFlags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public char[] szDevice;
    }

    #endregion

    #region Enums

    internal enum FO_Func : uint
    {
        FO_MOVE = 0x1
        ,
        FO_COPY = 0x2
        ,
        FO_DELETE = 0x3
        ,
        FO_RENAME = 0x4
    }
    [Flags]
    internal enum FO_FileOp : ushort
    {
        FOF_MULTIDESTFILES = 0x1
            ,
        FOF_CONFIRMMOUSE = 0x2
            ,
        FOF_SILENT = 0x4  //don't create progress/report
            ,
        FOF_RENAMEONCOLLISION = 0x8
            ,
        FOF_NOCONFIRMATION = 0x10 //Don't prompt the user.
            ,
        FOF_WANTMAPPINGHANDLE = 0x20  //Fill in SHFILEOPSTRUCT.hNameMappings
            ,
        //Must be freed using SHFreeNameMappings
        FOF_ALLOWUNDO = 0x40
            ,
        FOF_FILESONLY = 0x80  //on *.*, do only files
            ,
        FOF_SIMPLEPROGRESS = 0x100  //means don't show names of files
            ,
        FOF_NOCONFIRMMKDIR = 0x200  //don't confirm making any needed dirs
            ,
        FOF_NOERRORUI = 0x400  //don't put up error UI
            ,
        FOF_NOCOPYSECURITYATTRIBS = 0x800  //dont copy NT file Security Attributes
            ,
        FOF_NORECURSION = 0x1000  //don't recurse into directories.
            ,
        FOF_NO_CONNECTED_ELEMENTS = 0x2000 //don't operate on connected elements.
            ,
        FOF_WANTNUKEWARNING = 0x4000 //during delete operation, warn if nuking instead of recycling (partially overrides FOF_NOCONFIRMATION)
            ,
        FOF_NORECURSEREPARSE = 0x8000 //treat reparse points as objects, not containers
    }

    #endregion
}