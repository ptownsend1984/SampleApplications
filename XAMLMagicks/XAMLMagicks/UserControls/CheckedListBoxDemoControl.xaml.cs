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

    public partial class CheckedListBoxDemoControl : UserControl
    {

        #region Constructor

        public CheckedListBoxDemoControl()
        {
            InitializeComponent();

            var ViewModel = new CheckedListBoxDemoViewModel();
            ViewModel.SelectableItems.Add(new Models.SelectableItem { DisplayName = "One", IsSelected = false });
            ViewModel.SelectableItems.Add(new Models.SelectableItem { DisplayName = "Two", IsSelected = true });
            ViewModel.SelectableItems.Add(new Models.SelectableItem { DisplayName = "Three", IsSelected = false });
            this.DataContext = ViewModel;
        }

        #endregion

    }
}
