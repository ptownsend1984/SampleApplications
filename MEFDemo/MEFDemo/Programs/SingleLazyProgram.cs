//#define IncludeThis

#if IncludeThis
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MEFContracts;

namespace MEFDemo
{
    /// <summary>
    /// This program demonstrates lazy loading with MEF.  It will throw an error if you have multiple writers defined.
    /// </summary>
    class Program
    {

        static void Main(string[] args)
        {
            CompositionContainer Container = CreateContainer();
            Lazy<IMessageWriter> MessageWriter = Container.GetExport<IMessageWriter>();
            Console.WriteLine("Testing write");
            MessageWriter.Value.Write("Hello");
            Console.ReadKey();
        }

        private static CompositionContainer CreateContainer()
        {
            var Catalog = new AssemblyCatalog(System.Reflection.Assembly.GetEntryAssembly());            
            var Container = new CompositionContainer(Catalog);
            var Batch = new CompositionBatch();
            Batch.AddExportedValue(Container);            
            Container.Compose(Batch);
            return Container;
        }
    }
}
#endif