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
using System.ComponentModel.Composition;
using MEFContracts;

namespace MEFPresentation
{
    [Export(typeof(IMainView))]
    public partial class MainView : Window, IMainView
    {

        #region Constructor

        public MainView()
        {
            InitializeComponent();
        }

        #endregion

        [Import]
        public IMainViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }

        public void ShowView()
        {
            this.ShowDialog();
        }
    }
}
