using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Reflection.Helper2.Wrappers
{
    public abstract class InfoWrapper<T>
    {

        #region Global Variables

        private readonly T _info;

        private readonly DelegateCommand _DebuggerBreakCommand;

        #endregion

        #region Properties

        public T Info { get { return _info; } }

        public ICommand DebuggerBreakCommand { get { return _DebuggerBreakCommand; } }

        #endregion

        #region Constructor

        public InfoWrapper(T info)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            
            this._info = info;

            _DebuggerBreakCommand = new DelegateCommand(DoDebuggerBreakCommand);
        }

        #endregion

        #region Methods

        private void DoDebuggerBreakCommand()
        {
            System.Diagnostics.Debugger.Break();
        }

        #endregion

    }
}