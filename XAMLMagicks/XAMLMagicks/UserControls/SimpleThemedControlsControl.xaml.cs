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

namespace XAMLMagicks.UserControls
{

    public partial class SimpleThemedControlsControl : UserControl
    {

        #region Subclasses


        #endregion

        #region Constants


        #endregion

        #region Static Members


        #endregion

        #region Global Variables


        #endregion

        #region Properties


        #endregion

        #region Events


        #endregion

        #region Constructor

        public SimpleThemedControlsControl()
        {
            InitializeComponent();
        }

        #endregion


        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CloseCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #region Event Handlers


        #endregion

        #region Methods


        #endregion

    }
}
