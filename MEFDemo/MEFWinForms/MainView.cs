using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using MEFContracts;
using System.ComponentModel.Composition;

namespace MEFWinForms
{
    [Export(typeof(IMainView))]
    public class MainView : Form, IMainView
    {
        private Button button1;
        private Label label1;

        [Import]
        public IMainViewModel ViewModel
        {
            set
            {
                label1.DataBindings.Add("Text", value, "Message", false, DataSourceUpdateMode.OnPropertyChanged);
                button1.Click += (s, e) => { value.OKCommand.Execute(null); };
            }
        }

        public MainView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 162);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MainView
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "MainView";
            this.Text = "Main View";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void ShowView()
        {
            this.ShowDialog();
        }
    }
}