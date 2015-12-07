using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XAMLMagicks.ViewModels
{
    public class UpDownDemoViewModel : NotificationObject
    {

        #region Global Variables

        private int _Width;

        #endregion

        #region Properties

        public int Width
        {
            get { return _Width; }
            set
            {
                if (_Width == value) return;
                _Width = value;
                OnPropertyChanged("Width");
            }
        }

        #endregion

        #region Constructor

        public UpDownDemoViewModel()
        {
            _Width = 300;
        }

        #endregion

    }
}