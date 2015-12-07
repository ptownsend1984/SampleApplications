using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFDemo.Common.Extensions;

namespace WPFDemo.ViewModels
{
    public class DelegateCommand<T> : ICommand where T : class
    {

        #region Subclasses


        #endregion

        #region Constants


        #endregion

        #region Static Members


        #endregion

        #region Global Variables

        private readonly Action<T> ExecuteMethod;
        private readonly Func<T, bool> CanExecuteMethod;

        #endregion

        #region Properties


        #endregion

        #region Events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Constructor

        public DelegateCommand(Action<T> ExecuteMethod)
            : this(ExecuteMethod, null) { }
        public DelegateCommand(Action<T> ExecuteMethod, Func<T, bool> CanExecuteMethod)
        {
            this.ExecuteMethod = ExecuteMethod;
            this.CanExecuteMethod = CanExecuteMethod;
        }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods

        bool ICommand.CanExecute(object parameter)
        {
            if (this.CanExecuteMethod != null)
                return this.CanExecuteMethod(parameter as T);
            else
                return true;
        }
        void ICommand.Execute(object parameter)
        {
            if (this.ExecuteMethod != null)
                this.ExecuteMethod(parameter as T);
        }

        public void RaiseCanExecuteChanged()
        {
            this.RaiseEvent(this.CanExecuteChanged);
        }

        #endregion

    }
}