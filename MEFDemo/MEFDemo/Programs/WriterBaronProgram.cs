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
    /// This program demonstrates the ImportMany attribute.    
    /// </summary>
    class Program
    {

        static void Main(string[] args)
        {
            CompositionContainer Container = CreateContainer();
            WriterBaron WriterBaron = Container.GetExportedValue<WriterBaron>();
            WriterBaron.WriteAll();
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