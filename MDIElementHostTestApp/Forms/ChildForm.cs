using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace MDITestApp.Forms
{
    public class ChildForm : Form
    {

        #region Constructor

        public ChildForm()
        {
            this.Text = "ChildForm";

            this.Controls.Add(new TextBox() { Dock = DockStyle.Top });
            this.Controls.Add(new TextBox() { Dock = DockStyle.Bottom });
        }

        #endregion

    }
}