﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using XAMLMagicks.Forms;

namespace XAMLMagicks.UserControls
{

    public partial class WinFormsDemoControl : UserControl
    {

        #region Constructor

        public WinFormsDemoControl()
        {
            InitializeComponent();
        }

        #endregion

        private void Button_Click(object sender, EventArgs e)
        {
            var Form = new DemoForm();
            var Helper = new FormOwnerHelper(System.Windows.Application.Current.MainWindow);            
            Form.ShowDialog(Helper);
        }

    }
}
