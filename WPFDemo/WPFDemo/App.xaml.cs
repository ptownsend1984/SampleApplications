using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WPFDemo.ViewModels.Windows;
using WPFDemo.Windows;

namespace WPFDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs args)
        {
            this.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;

            var ViewModel = new MainWindowViewModel();
            var View = new MainWindow();
            View.ViewModel = ViewModel;
            View.Show();

            View.Closed += (s, e) =>
                {
                    View.Dispose();
                    ViewModel.Dispose();  
                };

            base.OnStartup(args);
        }

    }
}
