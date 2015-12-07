using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Interop;

namespace XAMLMagicks.Forms
{
    public class FormOwnerHelper : System.Windows.Forms.IWin32Window
    {

        #region Global Variables

        private readonly WindowInteropHelper WindowInteropHelper;

        #endregion

        #region Properties

        public IntPtr Handle
        {
            get { return WindowInteropHelper.Handle; }
        }
        public IntPtr WindowOwner
        {
            get { return WindowInteropHelper.Owner; }
            set { WindowInteropHelper.Owner = value; }
        }

        #endregion

        #region Constructor

        public FormOwnerHelper(System.Windows.Window Window)
        {
            WindowInteropHelper = new WindowInteropHelper(Window);
        }

        #endregion

    }
}