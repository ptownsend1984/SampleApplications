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

namespace FolderCrawlerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, System.Windows.Forms.IWin32Window
    {
        private readonly System.Windows.Interop.WindowInteropHelper WindowInteropHelper;

        public MainWindow()
        {
            InitializeComponent();

            this.WindowInteropHelper = new System.Windows.Interop.WindowInteropHelper(this);
        }

        //Needed for the choose folder dialog owner
        public IntPtr Handle
        {
            get { return this.WindowInteropHelper.Handle; }
        }

        private void MessageTextTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var Sender = sender as TextBox;
            if (Sender == null)
                return;

            //Scroll to the end when the text changes
            Sender.ScrollToEnd();
        }
    }
}
