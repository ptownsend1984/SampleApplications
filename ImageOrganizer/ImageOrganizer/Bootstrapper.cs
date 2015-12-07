using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.MefExtensions;
using System.ComponentModel.Composition.Hosting;
using ImageOrganizer.Windows;
using ImageOrganizer.Contracts;

namespace ImageOrganizer
{
    public class Bootstrapper : MefBootstrapper
    {        
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Bootstrapper).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ContractsModule).Assembly));
        }

        protected override System.Windows.DependencyObject CreateShell()
        {
            return this.Container.GetExportedValue<MainWindow>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            System.Windows.Application.Current.MainWindow = (System.Windows.Window)this.Shell;
            System.Windows.Application.Current.MainWindow.Show();
        }

    }
}