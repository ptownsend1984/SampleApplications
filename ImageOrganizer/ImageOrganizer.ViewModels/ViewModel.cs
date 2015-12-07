using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;
using System.ComponentModel;
using System.Windows.Threading;
using ImageOrganizer.Common.Contracts;

namespace ImageOrganizer.ViewModels
{
    public class ViewModel : NotificationObject, IViewModel
    {

        #region Global Variables

        protected readonly DataAnnotationSupport DataAnnotationSupport;
        private readonly Dispatcher Dispatcher;

        private bool _IsValidating;

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
                if (!this.IsValidating) return string.Empty;
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

        public bool IsValidating
        {
            get { return _IsValidating; }
            set
            {
                if (_IsValidating != value)
                    _IsValidating = value;
                this.NotifyAllPropertiesChanged();
            }
        }
        protected bool IsValidatingInternal
        {
            set { _IsValidating = value; }
        }

        #endregion

        #region Events


        #endregion

        #region Constructor

        public ViewModel()
        {
            this.DataAnnotationSupport = new DataAnnotationSupport(this);
            this.Dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;

            this._IsValidating = true;
        }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods        

        /// <summary>
        /// Raise PropertyChanged for all properties. 
        /// </summary>
        [System.Diagnostics.DebuggerStepThrough]
        public virtual void NotifyAllPropertiesChanged()
        {
            this.RaisePropertyChanged(string.Empty);
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

        [System.Diagnostics.DebuggerStepThrough]
        protected object Invoke(Delegate Delegate, params object[] args)
        {
            if (!this.Dispatcher.CheckAccess())
                return this.Dispatcher.Invoke(Delegate, args);
            else
                return Delegate.DynamicInvoke(args);
        }
        [System.Diagnostics.DebuggerStepThrough]
        protected T Invoke<T>(Delegate Delegate, params object[] args)
        {
            return (T)Invoke(Delegate, args);
        }

        #endregion

    }
}