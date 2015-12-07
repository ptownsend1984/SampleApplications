//#define IncludeThis

#if IncludeThis
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MEFContracts;

namespace MEFDemo.MessageWriter
{
    /// <summary>
    /// This writer pops up a message box
    /// </summary>
    [WriterExport(Destination="MessageBox")]
    public class MessageBoxWriter : IMessageWriter
    {
        public void Write(string Message)
        {
            System.Windows.MessageBox.Show(Message);
        }
    }
}
#endif