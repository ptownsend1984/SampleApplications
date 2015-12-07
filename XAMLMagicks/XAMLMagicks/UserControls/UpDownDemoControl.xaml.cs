using System;
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
using XAMLMagicks.ViewModels;

namespace XAMLMagicks.UserControls
{

    public partial class UpDownDemoControl : UserControl
    {

        #region Constructor

        public UpDownDemoControl()
        {
            InitializeComponent();

            var ViewModel = new UpDownDemoViewModel();
            this.DataContext = ViewModel;
        }

        #endregion

    }
}
