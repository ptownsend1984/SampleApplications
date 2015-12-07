using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;

namespace ImageOrganizer.ViewModels
{
    public class DialogWindowViewModel : WindowViewModel
    {

        #region Global Variables

        private readonly DelegateCommand _OKCommand;
        private readonly DelegateCommand _CloseCommand;

        #endregion

        #region Properties

        public ICommand OKCommand { get { return _OKCommand; } }
        public ICommand CloseCommand { get { return _CloseCommand; } }

        #endregion

        #region Events


        #endregion

        #region Constructor

        public DialogWindowViewModel()
        {
            _OKCommand = new DelegateCommand(VerifyDoOKCommand, CanDoOkCommand);
            _CloseCommand = new DelegateCommand(VerifyDoCloseCommand, CanDoCloseCommand);
        }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods

        protected void RaiseOKCommandCanExecuteChanged() { this._OKCommand.RaiseCanExecuteChanged(); }
        protected void RaiseCloseCommandCanExecuteChanged() { this._CloseCommand.RaiseCanExecuteChanged(); }

        private void VerifyDoOKCommand()
        {
            if (!CanDoOkCommand())
                return;
            DoOKCommand();
        }
        protected virtual void DoOKCommand() { }
        protected virtual bool CanDoOkCommand()
        {
            return true;
        }

        private void VerifyDoCloseCommand()
        {
            if (!CanDoCloseCommand())
                return;
            DoCloseCommand();
        }
        protected virtual void DoCloseCommand()
        {
            this.RaiseRequestClose(false);
        }
        protected virtual bool CanDoCloseCommand()
        {
            return true;
        }

        #endregion

    }
}