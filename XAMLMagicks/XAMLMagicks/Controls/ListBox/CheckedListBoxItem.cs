using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace XAMLMagicks.Controls.ListBox
{
    public class CheckedListBoxItem : System.Windows.Controls.ContentControl
    {

        #region Static Members

        static CheckedListBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckedListBoxItem), new FrameworkPropertyMetadata(typeof(CheckedListBoxItem)));
        }

        #endregion

        #region Constructor

        public CheckedListBoxItem() { }

        #endregion

    }
}