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
    /// This program demonstrates how the PartCreationPolicy attribute.  It will throw an error if multiple readers are enabled.
    /// Use this with the ConsoleMessageWriter and the NonSharedConsoleWriter.
    /// </summary>
    class Program
    {

        static void Main(string[] args)
        {
            CompositionContainer Container = CreateContainer();
            IMessageWriter MessageWriter = Container.GetExportedValue<IMessageWriter>();
            MessageWriter.Write("Hello");

            MessageWriter = Container.GetExportedValue<IMessageWriter>();
            MessageWriter.Write("Hello");

            MessageWriter = Container.GetExportedValue<IMessageWriter>();
            MessageWriter.Write("Hello");

            MessageWriter = Container.GetExportedValue<IMessageWriter>();
            MessageWriter.Write("Hello");

            MessageWriter = Container.GetExportedValue<IMessageWriter>();
            MessageWriter.Write("Hello");

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