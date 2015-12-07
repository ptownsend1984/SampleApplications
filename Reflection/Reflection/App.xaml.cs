using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Reflection
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private MainWindow mainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;

            LaunchApplication();
        }

        private void LaunchApplication()
        {
            HelperMe();

            mainWindow = new MainWindow();
            this.MainWindow = mainWindow;
            mainWindow.Show();
        }
        private void HelperMe()
        {
            var helper = new Helper.HelperUtility();
            Console.WriteLine(helper.ToString());
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var window = this.MainWindow;
            if (window != null)
                MessageBox.Show(window, e.Exception.Message);
            else
                MessageBox.Show(e.Exception.Message);
        }

        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Console.WriteLine("Resolving assembly: {0}", args.Name);
            System.Reflection.Assembly resolved = null;
            return resolved;
        }
        private void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            Console.WriteLine("Loaded assembly: {0}", args.LoadedAssembly.FullName);
        }


        
    }
}
