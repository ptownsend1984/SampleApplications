#define IncludeThis

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
    /// This program demonstrates the most simple usage of MEF.  It will throw an error if you have multiple writers defined.
    /// </summary>
    class Program
    {

        static void Main(string[] args)
        {
            CompositionContainer Container = CreateContainer();
            IMessageWriter MessageWriter = Container.GetExportedValue<IMessageWriter>();
            Console.WriteLine("Testing write");
            MessageWriter.Write("Hello");
            Console.ReadKey();
        }

        private static CompositionContainer CreateContainer()
        {
            //Create assembly catalog from this assembly
            var Catalog = new AssemblyCatalog(System.Reflection.Assembly.GetEntryAssembly());            
            //Create a CompositionContainer from this assembly
            var Container = new CompositionContainer(Catalog);
            //Create a batch
            var Batch = new CompositionBatch();
            //Add the container to the batch
            Batch.AddExportedValue(Container);            
            Container.Compose(Batch);
            return Container;
        }
    }
}
#endif