using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;

namespace WPFDemo.Presentation.Controls.ListBox
{
    /// <summary>
    /// Improved checked list box.  Supports selection.
    /// </summary>
    public class CheckedListBoxEx : System.Windows.Controls.ListBox
    {

        #region Static Members

        static CheckedListBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckedListBoxEx), new FrameworkPropertyMetadata(typeof(CheckedListBoxEx)));

            EventManager.RegisterClassHandler(typeof(CheckedListBoxEx), PreviewKeyDownEvent, new System.Windows.Input.KeyEventHandler(OnPreviewKeyDown));
        }

        #endregion

        #region Constructor

        public CheckedListBoxEx()
        {
        }

        private static void OnPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var Sender = sender as CheckedListBoxEx;
            if (Sender == null)
                return;

            if (e.Key == System.Windows.Input.Key.Space)
            {
                if (!Sender.HasItems)
                    return;

                var SelectedContainers = Sender.Items.
                    Cast<object>()
                    .Select(o => Sender.ItemContainerGenerator.ContainerFromItem(o) as CheckedListBoxExItem)
                    .Where(o => o != null && o.IsCheckBoxEnabled && o.IsSelected)
                    .ToArray();
                if (!SelectedContainers.Any())
                    return;

                if (SelectedContainers.Any(o => o.IsChecked) && SelectedContainers.Any(o => !o.IsChecked))
                {
                    foreach (var Item in SelectedContainers.Where(o => o.IsChecked))
                        Item.IsChecked = false;
                }
                else
                {
                    foreach (var Item in SelectedContainers)
                        Item.IsChecked = !Item.IsChecked;
                }
                e.Handled = true;
            }
        }

        #endregion

        #region Methods

        protected override System.Windows.DependencyObject GetContainerForItemOverride()
        {
            return new CheckedListBoxExItem();
        }
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is CheckedListBoxExItem);
        }

        #endregion

    }
}