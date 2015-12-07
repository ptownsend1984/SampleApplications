using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WPFDemo.ViewModels;
using System.Windows;
using System.ComponentModel;

namespace WPFDemo.Presentation
{
    public class DemoWindow : System.Windows.Window, IDisposable
    {

        #region Subclasses


        #endregion

        #region Constants


        #endregion

        #region Static Members


        #endregion

        #region Global Variables


        #endregion

        #region Properties

        public bool IsDisposed { get; private set; }
        public bool IsDisposing { get; private set; }

        /// <summary>
        /// Get/Set the active ViewModel for this window
        /// </summary>
        public IViewModel ViewModel
        {
            get { return this.DataContext as IViewModel; }
            set
            {
                this.DataContext = value;                
            }
        }

        #endregion

        #region Events


        #endregion

        #region Constructor

        public DemoWindow()
        {
            this.DataContextChanged += (s, e) =>
            {
                var OldValue = e.OldValue as IViewModel;
                if (OldValue != null)
                    ClearManualDataBindings(OldValue);
                var NewValue = e.NewValue as IViewModel;
                if (NewValue != null)
                    SetManualDataBindings(NewValue);
            };
        }

        #endregion

        #region Event Handlers

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnViewModelPropertyChanged(e.PropertyName);
        }
        private void IUIViewModel_RequestClose(object sender, RequestCloseEventArgs e)
        {
            OnViewModelRequestClose(e);
        }

        #endregion

        #region Methods

        protected virtual void SetManualDataBindings(IViewModel ViewModel)
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            var IUIViewModel = ViewModel as IUIViewModel;
            if (IUIViewModel != null)
            {
                IUIViewModel.RequestClose += IUIViewModel_RequestClose;
            }
        }

        protected virtual void ClearManualDataBindings(IViewModel ViewModel)
        {
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }
        protected virtual void OnViewModelPropertyChanged(string PropertyName) { }
        protected virtual void OnViewModelRequestClose(RequestCloseEventArgs e) 
        {
            if (System.Windows.Interop.ComponentDispatcher.IsThreadModal)
                this.DialogResult = e.DialogResult;
            this.Close();
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
        protected virtual void OnDispose() { this.ViewModel = null; }

        #endregion

    }
}