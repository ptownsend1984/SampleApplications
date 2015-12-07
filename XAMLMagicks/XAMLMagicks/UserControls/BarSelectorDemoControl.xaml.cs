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

    public partial class BarSelectorDemoControl : UserControl
    {

        #region Constructor

        public BarSelectorDemoControl()
        {
            InitializeComponent();

            var ViewModel = new BarSelectorDemoViewModel();
            ViewModel.SelectableItems.Add(new Models.SelectableItem { DisplayName = "One" });
            ViewModel.SelectableItems.Add(new Models.SelectableItem { DisplayName = "Two" });
            ViewModel.SelectableItems.Add(new Models.SelectableItem { DisplayName = "Three" });
            ViewModel.SelectableItems.Add(new Models.SelectableItem { DisplayName = "Four" });
            ViewModel.SelectableItems.Add(new Models.SelectableItem { DisplayName = "Five" });

            this.DataContext = ViewModel;
        }

        #endregion

    }
}
