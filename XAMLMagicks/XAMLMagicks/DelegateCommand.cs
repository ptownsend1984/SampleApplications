using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace XAMLMagicks
{
    public class DelegateCommand : ICommand
    {

        #region Global Variables

        private readonly Action ExecuteMethod;
        private readonly Func<bool> CanExecuteMethod;

        #endregion

        #region Events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Constructor

        [System.Diagnostics.DebuggerStepThrough]
        public DelegateCommand(Action ExecuteMethod)
            : this(ExecuteMethod, null) { }
        [System.Diagnostics.DebuggerStepThrough]
        public DelegateCommand(Action ExecuteMethod, Func<bool> CanExecuteMethod)
        {
            this.ExecuteMethod = ExecuteMethod;
            this.CanExecuteMethod = CanExecuteMethod;
        }

        #endregion

        #region Methods

        bool ICommand.CanExecute(object parameter)
        {
            if (this.CanExecuteMethod != null)
                return this.CanExecuteMethod();
            else
                return true;
        }
        void ICommand.Execute(object parameter)
        {
            if (this.ExecuteMethod != null)
                this.ExecuteMethod();
        }

        [System.Diagnostics.DebuggerStepThrough]
        public void RaiseCanExecuteChanged()
        {
            var Handler = this.CanExecuteChanged;
            if (Handler != null)
                Handler(this, EventArgs.Empty);            
        }

        #endregion

    }
}