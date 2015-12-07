#define IncludeThis

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
    /// This writer will write to the console window.
    /// </summary>
    [ExportMetadata("Destination", "Console")]
    [Export(typeof(IMessageWriter))]
    public class ConsoleMessageWriter : IMessageWriter
    {

        private static int InstanceCount = 0;

        public ConsoleMessageWriter()
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