using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MEFContracts
{
    /// <summary>
    /// The basic interface for a message writer
    /// </summary>
    public interface IMessageWriter
    {

        #region Methods

        void Write(string Message);

        #endregion

    }
}