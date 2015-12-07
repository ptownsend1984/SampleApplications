using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace XAMLMagicks
{
    public class NotificationObject : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string PropertyName)
        {
            var Handler = this.PropertyChanged;
            if (Handler != null)
                Handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        public string Error
        {
            get { return this[null]; }
        }
        public virtual string this[string columnName]
        {
            get { return string.Empty; }
        }
    }
}