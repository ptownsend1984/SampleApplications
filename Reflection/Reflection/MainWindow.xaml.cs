using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Reflection.Controls;
using Reflection.Helper2;

namespace Reflection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Controls.Ribbon.RibbonWindow
    {

        public readonly System.Windows.Interop.WindowInteropHelper InteropHelper;

        public MainWindow()
        {
            InitializeComponent();

            this.InteropHelper = new System.Windows.Interop.WindowInteropHelper(this);
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ListLoadedTypesButton_Click(object sender, RoutedEventArgs e)
        {
            ClearMainView();

            var helper = new ListLoadedTypesHelper();
            var typesListBox = new TypesListBoxControl();
            typesListBox.Types = helper.ListLoadedTypes(AppDomain.CurrentDomain)
                .ToObservableCollection();
            this.MainView.Children.Add(typesListBox);
        }

        private void ClearMainView()
        {
            this.MainView.Children.Clear();
        }

        private void SearchTypeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            ClearMainView();

            var filterText = this.SearchTypeTextBox.Text;
            if (string.IsNullOrWhiteSpace(filterText))
                return;

            var helper = new ListLoadedTypesHelper();
            var typesListBox = new TypesListBoxControl();
            typesListBox.Types = helper.ListLoadedTypes(AppDomain.CurrentDomain)
                .Where(o => o.Name.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                .ToObservableCollection();
            this.MainView.Children.Add(typesListBox);
        }

        private void PerformanceUriButton_Click(object sender, RoutedEventArgs e)
        {
            ClearMainView();

            var helper = new PerformanceHelper();
            helper.CountUriConstructions(new TimeSpan(0, 0, 5));

            var control = new PerformanceControl();
            control.DataContext = helper;
            this.MainView.Children.Add(control);
        }

        private void ClearViewItem_Click(object sender, RoutedEventArgs e)
        {
            var evilClass = new EvilClass();
            evilClass.ClearMainWindow();
        }

    }
}
