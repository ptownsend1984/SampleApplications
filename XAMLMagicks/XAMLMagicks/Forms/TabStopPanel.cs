using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;

namespace XAMLMagicks.Forms
{
    /// <summary>
    /// When WinForms hosts WPF hosts Winforms, the lowest level WinForms control does not relinquish tab focus.
    /// Place this panel as the content of a WindowsFormsHost and dock-fill the wanted control under it.
    /// </summary>
    /// <remarks>http://social.msdn.microsoft.com/Forums/eu/wpf/thread/054d8509-dd2d-4b60-9b0a-383b0147e2ac</remarks>
    public class TabStopPanel : Panel
    {
        private HwndHost HwndHost { get; set; }
        public bool OverrideTabStop { get; set; }

        public TabStopPanel()
        {
            this.OverrideTabStop = true;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            var Parent = this.Parent;
            if (Parent == null)
                HwndHost = null;
            else
            {
                var Adapter = FindWinFormsAdapter(Parent);
                if (Adapter != null)
                    //Reflect out the hidden _host field value
                    HwndHost = Adapter.GetType().GetField("_host", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(Parent) as HwndHost;
                else
                    HwndHost = null;
            }
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (base.ProcessDialogKey(keyData) || OverrideTabStop == false || HwndHost == null)
                return true;
            if ((keyData & (Keys.Alt | Keys.Control)) == Keys.None)
            {
                Keys keyCode = (Keys)keyData & Keys.KeyCode;
                if (keyCode == Keys.Tab)
                {
                    IKeyboardInputSink sink = HwndHost;
                    if (sink == null || sink.KeyboardInputSite == null)
                        return false;
                    return sink.KeyboardInputSite.OnNoMoreTabStops(
                      new TraversalRequest((keyData & Keys.Shift) == Keys.None ?
                        FocusNavigationDirection.Next : FocusNavigationDirection.Previous));
                }
            }
            return false;
        }

        private Control FindWinFormsAdapter(Control Control)
        {
            //.NET 3.5: 
            //WinFormsAdapter is an internal class in the System.Windows.Forms.Integration assembly
            if (Control == null)
                return null;
            if (Control.GetType().TypeIs(typeof(System.Windows.Forms.Integration.WindowsFormsHost).Assembly, "System.Windows.Forms.Integration.WinFormsAdapter"))
                return Control;
            else
                return FindWinFormsAdapter(Control.Parent);
        }
    }
}