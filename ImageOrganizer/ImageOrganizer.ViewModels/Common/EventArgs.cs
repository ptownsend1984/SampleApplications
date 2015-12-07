using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ImageOrganizer.ViewModels.Common
{
    public class RequestCloseEventArgs : EventArgs
    {
        private readonly bool? _DialogResult;

        public bool? DialogResult { get { return _DialogResult; } }

        public RequestCloseEventArgs(bool? DialogResult)
        {
            this._DialogResult = DialogResult;
        }
    }

}