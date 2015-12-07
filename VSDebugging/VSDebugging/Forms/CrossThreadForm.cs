using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace VSDebugging.Forms
{
    public class CrossThreadForm : System.Windows.Forms.Form, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void SafeRaisePropertyChanged(string propertyName)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<string>(this.RaisePropertyChanged), new object[] { propertyName });
            else
                RaisePropertyChanged(propertyName);
        }
        private void RaisePropertyChanged(string propertyName)
        {
            var Handler = this.PropertyChanged;
            if (Handler != null)
                Handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private int ThreadCount;

        private volatile bool IsRunning;
        private int _Value;
        public int Value { get { return _Value; } }

        private System.Threading.Tasks.Task[] Tasks;

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnBreak;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Button btnStart;

        public CrossThreadForm()
        {
            InitializeComponent();

            txtValue.DataBindings.Add("Text", this, "Value");
        }
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnBreak = new System.Windows.Forms.Button();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(80, 25);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(104, 51);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start Thread";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(80, 97);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(104, 51);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop Thread";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnBreak
            // 
            this.btnBreak.Location = new System.Drawing.Point(80, 165);
            this.btnBreak.Name = "btnBreak";
            this.btnBreak.Size = new System.Drawing.Size(104, 51);
            this.btnBreak.TabIndex = 2;
            this.btnBreak.Text = "Break Thread";
            this.btnBreak.UseVisualStyleBackColor = true;
            this.btnBreak.Click += new System.EventHandler(this.btnBreak_Click);
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(80, 236);
            this.txtValue.Name = "txtValue";
            this.txtValue.ReadOnly = true;
            this.txtValue.Size = new System.Drawing.Size(100, 20);
            this.txtValue.TabIndex = 3;
            // 
            // CrossThreadForm
            // 
            this.ClientSize = new System.Drawing.Size(294, 372);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.btnBreak);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Name = "CrossThreadForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => Start());
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => Stop());
        }
        private void btnBreak_Click(object sender, EventArgs e)
        {
            Break();
        }

        private void Start()
        {
            Stop();

            IsRunning = true;
            var Tasks = new List<System.Threading.Tasks.Task>();
            for (int i = 0; i < 5; i++)
            {
                Tasks.Add(System.Threading.Tasks.Task.Factory.StartNew(Work, System.Threading.Tasks.TaskCreationOptions.LongRunning));
            }
            this.Tasks = Tasks.ToArray();
        }
        private void Stop()
        {
            if (!IsRunning)
                return;

            IsRunning = false;
            System.Threading.Tasks.Task.WaitAll(this.Tasks);
            this.Tasks = null;
        }
        private void Break()
        {
            System.Diagnostics.Debugger.Break();
        }

        private void Work()
        {
            if (string.IsNullOrEmpty(System.Threading.Thread.CurrentThread.Name))
                System.Threading.Thread.CurrentThread.Name = string.Format("Thread #{0}", System.Threading.Interlocked.Increment(ref ThreadCount));

            var Random = new Random();
            while (IsRunning)
            {
                if (Random.Next(0, 3) % 3 > 0)
                    System.Threading.Interlocked.Increment(ref _Value);
                else
                    System.Threading.Interlocked.Decrement(ref _Value);
                this.SafeRaisePropertyChanged("Value");
                System.Threading.Thread.Sleep(1);
            }
        }


    }

}