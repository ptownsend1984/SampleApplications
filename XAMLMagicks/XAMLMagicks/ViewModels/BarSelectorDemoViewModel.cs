using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using XAMLMagicks.Models;

namespace XAMLMagicks.ViewModels
{
    public class BarSelectorDemoViewModel : NotificationObject
    {

        #region Global Variables

        private readonly ObservableCollection<SelectableItem> _SelectableItems;

        #endregion

        #region Properties

        public IList<SelectableItem> SelectableItems
        {
            get { return _SelectableItems; }
        }

        #endregion

        #region Constructor

        public BarSelectorDemoViewModel()
        {
            _SelectableItems = new ObservableCollection<SelectableItem>();
        }

        #endregion

    }
}