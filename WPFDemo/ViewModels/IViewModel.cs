using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WPFDemo.ViewModels
{
    public interface IViewModel : IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        bool IsValid { get; }
    }
}