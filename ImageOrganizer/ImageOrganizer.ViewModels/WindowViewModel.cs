using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImageOrganizer.ViewModels.Common;

namespace ImageOrganizer.ViewModels
{
    public class WindowViewModel : ViewModel
    {

        #region Global Variables


        #endregion

        #region Properties

        public System.Windows.Window Owner { get; set; }
        public string WindowTitle { get { return this.GetWindowTitle(); } }

        #endregion

        #region Events

        public event EventHandler<RequestCloseEventArgs> RequestClose;

        #endregion

        #region Constructor

        public WindowViewModel()
        {

        }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods

        protected virtual void RaiseRequestClose(bool? DialogResult)
        {
            var Handler = this.RequestClose;
            if (Handler != null)
                Handler(this, new RequestCloseEventArgs(DialogResult));
        }

        protected virtual string GetWindowTitle()
        {
            return string.Empty;
        }

        protected void ShowMessageBox(string Message, System.Windows.MessageBoxImage MessageBoxImage)
        {
            this.ShowMessageBox(Message, System.Windows.MessageBoxButton.OK, MessageBoxImage, System.Windows.MessageBoxResult.OK);
        }
        protected System.Windows.MessageBoxResult ShowMessageBox(string Message, System.Windows.MessageBoxButton MessageBoxButton, System.Windows.MessageBoxImage MessageBoxImage, System.Windows.MessageBoxResult DefaultResult)
        {
            return System.Windows.MessageBox.Show(this.Owner, Message, this.WindowTitle, MessageBoxButton, MessageBoxImage, DefaultResult);
        }

        #endregion

    }
}