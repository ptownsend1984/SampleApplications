using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XAMLMagicks.Models
{
    public class SelectableItem : NotificationObject
    {

        #region Global Variables

        private bool _IsSelected;
        private string _DisplayName;

        #endregion

        #region Properties

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected == value) return;
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        public string DisplayName
        {
            get { return _DisplayName; }
            set
            {
                if (_DisplayName == value) return;
                _DisplayName = value;
                OnPropertyChanged("DisplayName");
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return this.DisplayName;
        }

        #endregion

    }
}