using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using FolderCrawlerDemo.Controllers;

namespace FolderCrawlerDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IDisposable MainWindowController;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var Controller = new MainWindowController();
            this.MainWindowController = Controller;

            var Window = new MainWindow();
            Controller.Owner = Window;
            Window.DataContext = Controller;
            
            Window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var Controller = this.MainWindowController;
            if (Controller != null)
            {
                try
                {
                    Controller.Dispose();
                }
                catch { }
            }
            base.OnExit(e);
        }
    }
}
