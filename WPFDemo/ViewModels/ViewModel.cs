using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using WPFDemo.Common.Extensions;

namespace WPFDemo.ViewModels
{
    public class ViewModel : IViewModel
    {

        #region Subclasses


        #endregion

        #region Constants


        #endregion

        #region Static Members


        #endregion

        #region Global Variables

        protected readonly DataAnnotationSupport DataAnnotationSupport;

        #endregion

        #region Properties

        public bool IsDisposed { get; private set; }
        public bool IsDisposing { get; private set; }

        public virtual bool HasErrors
        {
            get { return !string.IsNullOrEmpty(((IDataErrorInfo)this).Error); }
        }
        public virtual string Error
        {
            get
            {
                return this[string.Empty];
            }
        }
        public virtual string this[string columnName]
        {
            get
            {
                return DataAnnotationSupport[columnName];
            }
        }
        public bool IsValid
        {
            get
            {
                return !this.HasErrors;
            }
        }

        #endregion

        #region Events

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #endregion

        #region Constructor

        public ViewModel()
        {
            this.DataAnnotationSupport = new DataAnnotationSupport(this);
        }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods

        /// <summary>
        /// Raise PropertyChanged for all properties. 
        /// </summary>
        public virtual void NotifyAllPropertiesChanged()
        {
            this.OnPropertyChanged(string.Empty);
        }

        /// <summary>
        /// Raise the PropertyChangedEvent for the supplied property name
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string PropertyName)
        {
            RaisePropertyChanged(PropertyName);
        }
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> PropertyExpression)
        {
            OnPropertyChanged(PropertyExpression.GetPropertyName<T>());
        }
        
        private void RaisePropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            DisposeCore();
        }
        protected void DisposeCore()
        {
            if (IsDisposed || IsDisposing)
                return;

            this.IsDisposing = true;
            OnDispose();
            this.IsDisposing = false;
            IsDisposed = true;
        }
        protected virtual void OnDispose() { }

        #endregion

    }
}