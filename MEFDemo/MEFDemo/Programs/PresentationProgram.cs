//#define IncludeThis

#if IncludeThis
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MEFContracts;
using MEFPresentation;

namespace MEFDemo
{
    /// <summary>
    /// This program demonstrates using MEF with WPF.
    /// </summary>
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            CompositionContainer Container = CreateContainer();
            IMainView MainView = Container.GetExportedValue<IMainView>();
            MainView.ShowView();
        }

        private static CompositionContainer CreateContainer()
        {
            var Catalog = new AssemblyCatalog(typeof(PresentationModule).Assembly);            
            var Container = new CompositionContainer(Catalog);
            var Batch = new CompositionBatch();
            Batch.AddExportedValue(Container);            
            Container.Compose(Batch);
            return Container;
        }
    }
}
#endif