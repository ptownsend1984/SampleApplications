using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.MefExtensions;
using MEFContracts;
using System.Windows;
using System.ComponentModel.Composition.Hosting;
using MEFPresentation;


//http://compositewpf.codeplex.com/
namespace MEFPrism
{
    /// <summary>
    /// This class uses the MefBootstrapper from Prism to start the MEFPresentation MainView.
    /// </summary>
    public class Bootstrapper : MefBootstrapper
    {

        #region Methods

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(PresentationModule).Assembly));
        }
        protected override System.Windows.DependencyObject CreateShell()
        {
            return (Window)this.Container.GetExportedValue<IMainView>();
        }
        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
        }        

        #endregion

    }
}