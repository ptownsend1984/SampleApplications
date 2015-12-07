using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace ImageOrganizer.Common.Utils
{
    internal static class NativeMethodsx64
    {

        #region Subclasses


        #endregion

        #region Constants


        #endregion

        #region Static Global Variables


        #endregion

        #region Static Properties


        #endregion

        #region Static Methods

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int SHFileOperation(ref SHFILEOPSTRUCTx64 FileOp);

        #endregion

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct SHFILEOPSTRUCTx64
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

}