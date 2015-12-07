using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MEFContracts;
using System.ComponentModel.Composition;

namespace MEFDemo
{
    /// <summary>
    /// This class demonstrates the use of ImportMany.  See the WriterBaronProgram.
    /// </summary>
    [Export]
    public class WriterBaron
    {

        #region Subclasses


        #endregion

        #region Constants


        #endregion

        #region Static Members


        #endregion

        #region Global Variables


        #endregion

        #region Properties

        [ImportMany]
        protected IEnumerable<Lazy<IMessageWriter>> MessageWriters { get; set; }

        #endregion

        #region Events


        #endregion

        #region Constructor

        public WriterBaron()
        {

        }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods

        public void WriteAll()
        {
            foreach (Lazy<IMessageWriter> Writer in MessageWriters)
            {
                Console.WriteLine("Testing write");
                Writer.Value.Write("Hello " + Writer.Value.GetType().AssemblyQualifiedName);
            }
        }
        #endregion

    }
}