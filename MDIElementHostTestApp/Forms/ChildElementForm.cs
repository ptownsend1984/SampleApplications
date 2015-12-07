using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using MDITestApp.Controls;
using System.Windows.Interop;

namespace MDITestApp.Forms
{
    public class ChildElementForm : Form
    {

        #region Constructor

        public ChildElementForm(bool Fixed)
        {
            //Tabbing forward through a form with an element host as its only control
            //will result in tabbing through to the next MDI child instead
            //of rotating around.
            //Tabbing backward is seemingly uneffected.
            //The quick fix adds a hidden focusable control behind it that redirects
            //back to the element host.
            //The internal HwndSource is reflected out in order to call
            //IKeyboardInputSink.TabInto()
            if (Fixed)
            {
                var ElementHost = new FormElementHost();
                ElementHost.Dock = DockStyle.Fill;
                ElementHost.Child = new ChildElementControl();
                ElementHost.TabIndex = 0;
                this.Controls.Add(ElementHost);

                var Panel = new TextBox();
                Panel.TabIndex = 1;
                Panel.GotFocus += (s, e) =>
                {
                    ElementHost.TabInto();
                };
                this.Controls.Add(Panel);
                this.Text = "ChildElementForm Fixed";
            }
            else
            {
                var ElementHost = new ElementHost();
                ElementHost.Dock = DockStyle.Fill;
                ElementHost.Child = new ChildElementControl();
                ElementHost.TabIndex = 0;
                this.Controls.Add(ElementHost);
                this.Text = "ChildElementForm Broke";
            }
        }

        #endregion

    }

    public class FormElementHost : ElementHost
    {
        public void TabInto()
        {
            var HwndSource = this.GetType().GetProperty("HwndSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this, null) as HwndSource;
            ((IKeyboardInputSink)HwndSource).TabInto(new System.Windows.Input.TraversalRequest(System.Windows.Input.FocusNavigationDirection.First));

        }
    }
}