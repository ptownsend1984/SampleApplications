using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImageOrganizer.ViewModels;
using ImageOrganizer.ViewModels.Common;

namespace ImageOrganizer.Presentation
{
    public class ImageOrganizerWindow : System.Windows.Window
    {

        #region Properties

        #endregion

        #region Events


        #endregion

        #region Constructor

        public ImageOrganizerWindow()
        {
            this.DataContextChanged += (s, e) =>
                {
                    var OldValue = e.OldValue as ViewModel;
                    if (OldValue != null)
                        ClearDataContext(OldValue);
                    var NewValue = e.NewValue as ViewModel;
                    if (NewValue != null)
                        SetDataContext(NewValue);
                };            
        }

        #endregion

        #region Event Handlers

        protected virtual void OnRequestClose(object sender, RequestCloseEventArgs e)
        {
            //http://social.msdn.microsoft.com/Forums/en-US/wpf/thread/c95f1acb-5dee-4670-b779-b07b06afafff/
            if (System.Windows.Interop.ComponentDispatcher.IsThreadModal)
                this.DialogResult = e.DialogResult;
            this.Close();
        }

        #endregion

        #region Methods

        protected virtual void SetDataContext(ViewModel ViewModel)
        {
            if (ViewModel == null)
                return;
            var WindowViewModel = ViewModel as WindowViewModel;
            if (WindowViewModel != null)
                SetWindowViewModel(WindowViewModel);
        }
        protected virtual void SetWindowViewModel(WindowViewModel WindowViewModel)
        {
            if (WindowViewModel == null)
                return;
            WindowViewModel.Owner = this;
            WindowViewModel.RequestClose += OnRequestClose;            
        }
        protected virtual void ClearDataContext(ViewModel ViewModel)
        {
            if (ViewModel == null)
                return;
            var WindowViewModel = ViewModel as WindowViewModel;
            if (WindowViewModel != null)
                ClearWindowViewModel(WindowViewModel);
        }
        protected virtual void ClearWindowViewModel(WindowViewModel WindowViewModel)
        {
            if (WindowViewModel == null)
                return;
            WindowViewModel.RequestClose -= OnRequestClose;
            WindowViewModel.Owner = null;
        }

        #endregion

    }
}