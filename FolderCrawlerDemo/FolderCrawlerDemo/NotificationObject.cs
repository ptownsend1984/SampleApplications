using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using FolderCrawlerDemo.Extensions;

namespace FolderCrawlerDemo
{
    public abstract class NotificationObject : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged<T>(Expression<Func<T>> PropertyExpression)
        {
            OnPropertyChanged(PropertyExpression.GetPropertyName<T>());
        }
        protected virtual void OnPropertyChanged(string PropertyName)
        {
            var Handler = this.PropertyChanged;
            if (Handler != null)
                Handler(this, new PropertyChangedEventArgs(PropertyName));
        }

    }
}