using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace ImageOrganizer.Contracts
{
    public static class ContractsModule
    {

        //static ContractsModule()
        //{
        //    var Catalog = new AggregateCatalog
        //        (
        //            new ComposablePartCatalog[] { new AssemblyCatalog(typeof(ContractsModule).Assembly) } 
        //        );
        //    Container = new CompositionContainer(Catalog);
        //    var Batch = new CompositionBatch();
        //    Batch.AddExportedValue(Container);
        //    Container.Compose(Batch);
        //}

        //public static CompositionContainer Container { get; private set; }
    }
}