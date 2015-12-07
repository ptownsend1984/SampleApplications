using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Windows.Controls.Ribbon;
using System.ComponentModel.Composition;
using ImageOrganizer.ViewModels;
using System.Windows.Interop;

namespace ImageOrganizer.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export]
    public partial class MainWindow : RibbonWindow, System.Windows.Interop.IWin32Window
    {
        [Import]
        public MainWindowViewModel ViewModel 
        { 
            set 
            {
                value.Owner = this;
                this.DataContext = value; 
            } 
        }

        private readonly WindowInteropHelper WindowInteropHelper;

        public MainWindow()
        {
            InitializeComponent();

            // Insert code required on object creation below this point.
            this.WindowInteropHelper = new WindowInteropHelper(this);
        }

        internal Ribbon Ribbon { get { return this._Ribbon; } }

        public IntPtr Handle
        {
            get { return this.WindowInteropHelper.Handle; }
        }
    }
}
