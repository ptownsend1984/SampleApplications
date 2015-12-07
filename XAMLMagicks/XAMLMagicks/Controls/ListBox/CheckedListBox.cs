using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace XAMLMagicks.Controls.ListBox
{
    public class CheckedListBox : System.Windows.Controls.ItemsControl
    {

        #region Static Members

        static CheckedListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckedListBox), new FrameworkPropertyMetadata(typeof(CheckedListBox)));
        }

        #endregion

        #region Properties

        #endregion

        #region Constructor

        public CheckedListBox()
        {
        }

        #endregion

        #region Methods

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CheckedListBoxItem();
        }
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is CheckedListBoxItem;
        }
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        #endregion

    }
}