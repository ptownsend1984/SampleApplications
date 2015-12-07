using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace MDITestApp.Forms
{
    public class MainForm : Form
    {

        #region Constructor

        public MainForm()
        {
            this.IsMdiContainer = true;

            this.Size = new System.Drawing.Size(800, 600);
        }

        #endregion

        #region Methods

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var ChildForm = new ChildForm();
            ChildForm.MdiParent = this;
            ChildForm.Show();

            var ChildElementFormFixed = new ChildElementForm(true);
            ChildElementFormFixed.MdiParent = this;
            ChildElementFormFixed.Show();

            var ChildElementForm = new ChildElementForm(false);
            ChildElementForm.MdiParent = this;
            ChildElementForm.Show();

        }

        #endregion

    }
}