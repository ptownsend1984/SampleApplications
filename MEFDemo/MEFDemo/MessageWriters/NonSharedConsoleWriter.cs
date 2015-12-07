//#define IncludeThis

#if IncludeThis
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MEFContracts;
using System.ComponentModel.Composition;

namespace MEFDemo.MessageWriter
{
    /// <summary>
    /// This will write to the console, but every time it is instantiated, it will increment a global counter.
    /// The value of the InstanceCount will be written in the console.
    /// Note the PartCreationPolicy is set to NonShared, so a new writer will be created every time.
    /// </summary>
    [Export(typeof(IMessageWriter))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public class NonSharedConsoleWriter : IMessageWriter
    {
        private static int InstanceCount = 0;

        public NonSharedConsoleWriter()
        {
            InstanceCount++;
            Write("Initializing " + this.GetType().Name + Environment.NewLine + "Instance " + InstanceCount.ToString());
        }

        public void Write(string Message)
        {
            Console.WriteLine(Message);
        }

    }
}
#endif