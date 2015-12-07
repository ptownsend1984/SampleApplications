using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using MEFContracts;

namespace MEFDemo
{
    /// <summary>
    /// This class demonstrates the use of metadata attributes and is supported by the IMessageWriterMetadata interface.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class WriterExportAttribute : ExportAttribute
    {

        #region Properties

        public string Destination { get; set; }

        #endregion

        #region Constructor

        public WriterExportAttribute()
            : base(typeof(IMessageWriter))
        {

        }

        #endregion

    }
}