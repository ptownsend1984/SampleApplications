using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageOrganizer.Common.Contracts
{
    public interface IViewModel : INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        void NotifyAllPropertiesChanged();
    }
}