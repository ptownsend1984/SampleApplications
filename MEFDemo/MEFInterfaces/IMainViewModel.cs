using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MEFContracts
{
    /// <summary>
    /// The basic interface for a main view model
    /// </summary>
    public interface IMainViewModel
    {

        #region Properties

        string Message { get; set; }
        ICommand OKCommand { get; }

        #endregion

    }
}