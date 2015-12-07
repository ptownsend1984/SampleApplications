using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WPFDemo.ViewModels
{
    public class UIViewModel : ViewModel, IUIViewModel
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


        #endregion

        #region Events

        public event EventHandler<RequestCloseEventArgs> RequestClose;

        #endregion

        #region Constructor

        public UIViewModel()
        {

        }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods

        protected void OnRequestClose(bool? DialogResult)
        {
            var Handler = this.RequestClose;
            if (Handler != null)
                Handler(this, new RequestCloseEventArgs(DialogResult));
        }

        #endregion

    }

    public class RequestCloseEventArgs : System.EventArgs
    {
        public bool? DialogResult { get; private set; }
        public RequestCloseEventArgs(bool? DialogResult) { this.DialogResult = DialogResult; }
    }

}