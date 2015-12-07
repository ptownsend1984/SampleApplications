using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WPFDemo.ViewModels
{
    public interface IUIViewModel : IViewModel
    {

        event EventHandler<RequestCloseEventArgs> RequestClose;

    }
}