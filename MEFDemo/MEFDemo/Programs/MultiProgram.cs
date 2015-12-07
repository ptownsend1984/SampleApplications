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
    /// This program demonstrates the usage of multiple contract definitions.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            CompositionContainer Container = CreateContainer();
            IEnumerable<IMessageWriter> MessageWriters = Container.GetExportedValues<IMessageWriter>();
            foreach (IMessageWriter Writer in MessageWriters)
            {
                Console.WriteLine("Testing write");
                Writer.Write("Hello " + Writer.GetType().AssemblyQualifiedName);
            }
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