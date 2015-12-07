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
    /// This writer will write to the debug window.
    /// </summary>
    [ExportMetadata("Destination", "Debug")]
    [Export(typeof(IMessageWriter))]
    public class DebugMessageWriter : IMessageWriter
    {

        public DebugMessageWriter()
        {
            Write("Initializing " + this.GetType().Name);
        }

        public void Write(string Message)
        {
            System.Diagnostics.Debug.WriteLine(Message);
        }
    }
}
#endif