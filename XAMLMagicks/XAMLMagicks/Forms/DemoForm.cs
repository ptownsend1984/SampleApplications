using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.Integration;
using System.Windows;
using XAMLMagicks.UserControls;

namespace XAMLMagicks.Forms
{
    public class DemoForm : Form
    {

        #region Global Variables

        private FormElementHost _ElementHost;

        #endregion

        #region Properties

        public ElementHost ElementHost { get { return _ElementHost; } }
        public FrameworkElement ChildElement { get { return ElementHost.Child as FrameworkElement; } }

        #endregion

        #region Constructor

        public DemoForm()
        {
            InitializeComponent();

            this.ElementHost.Child = new WinFormsDemoControl();
        }
        private void InitializeComponent()
        {
            this._ElementHost = new FormElementHost();
            this.SuspendLayout();
            // 
            // _ElementHost
            // 
            this._ElementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ElementHost.Location = new System.Drawing.Point(0, 0);
            this._ElementHost.Name = "_ElementHost";
            this._ElementHost.Size = new System.Drawing.Size(292, 273);
            this._ElementHost.TabIndex = 0;
            this._ElementHost.Child = null;
            // 
            // CusiBaseElementHostForm
            // 
            this.ClientSize = new System.Drawing.Size(500, 400);
            this.Controls.Add(this._ElementHost);
            this.Name = "DemoForm";
            this.Text = "Demo Form";
            this.ResumeLayout(false);

        }

        #endregion

        #region Methods

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            //Winforms + WPF focus bug workaround
            this.ElementHost.Focus();
        }
        protected override void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    var ChildElement = this.ChildElement;
                    if (ChildElement != null)
                    {
                        this.ChildElement.DataContext = null;
                        if (ChildElement is IDisposable)
                            ((IDisposable)ChildElement).Dispose();
                    }
                }
            }
            base.Dispose(disposing);
        }

        #endregion

    }
}