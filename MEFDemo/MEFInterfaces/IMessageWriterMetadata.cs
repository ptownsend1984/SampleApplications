using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MEFContracts
{
    /// <summary>
    /// This interface is used for MEF export attribute metadata generation.  See the MultiLazyMetaDataProgram.
    /// </summary>
    public interface IMessageWriterMetadata
    {

        #region Properties

        string Destination { get; }

        #endregion
    }
}