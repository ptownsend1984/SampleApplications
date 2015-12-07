using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ImageOrganizer.Common.Extensions
{
    public static class WPFs
    {

        #region Static Methods
        
        public static T FindVisualChild<T>(this DependencyObject obj) where T : DependencyObject
        {
            //http://msdn.microsoft.com/en-us/library/bb613579.aspx
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        public static T FindVisualParent<T>(this DependencyObject obj) where T : DependencyObject
        {
            var Parent = VisualTreeHelper.GetParent(obj);
            if (Parent == null)
                return null;
            else if (Parent is T)
                return Parent as T;
            else
                return FindVisualParent<T>(Parent);
        }

        /// <summary>
        /// Causes the object to scroll into view centered.
        /// </summary>
        /// <param name="listBox">ListBox instance.</param>
        /// <param name="item">Object to scroll.</param>
        /// <remarks>http://blogs.msdn.com/b/delay/archive/2009/03/30/don-t-let-the-gotchas-getcha-adding-the-scrollintoviewcentered-method-to-wpf-s-listbox.aspx</remarks>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Deliberately targeting ListBox.")]
        public static void ScrollIntoViewCentered(this ListBox listBox, object item)
        {
            System.Diagnostics.Debug.Assert(!VirtualizingStackPanel.GetIsVirtualizing(listBox),
                "VirtualizingStackPanel.IsVirtualizing must be disabled for ScrollIntoViewCentered to work.");

            // Get the container for the specified item
            var container = listBox.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
            if (null != container)
            {
                if (ScrollViewer.GetCanContentScroll(listBox))
                {
                    // Get the parent IScrollInfo
                    var scrollInfo = VisualTreeHelper.GetParent(container) as IScrollInfo;
                    if (null != scrollInfo)
                    {
                        // Need to know orientation, so parent must be a known type
                        var stackPanel = scrollInfo as StackPanel;
                        var virtualizingStackPanel = scrollInfo as VirtualizingStackPanel;
                        System.Diagnostics.Debug.Assert((null != stackPanel) || (null != virtualizingStackPanel),
                            "ItemsPanel must be a StackPanel or VirtualizingStackPanel for ScrollIntoViewCentered to work.");

                        // Get the container's index
                        var index = listBox.ItemContainerGenerator.IndexFromContainer(container);

                        // Center the item by splitting the extra space
                        if (((null != stackPanel) && (Orientation.Horizontal == stackPanel.Orientation)) ||
                            ((null != virtualizingStackPanel) && (Orientation.Horizontal == virtualizingStackPanel.Orientation)))
                        {
                            scrollInfo.SetHorizontalOffset(index - Math.Floor(scrollInfo.ViewportWidth / 2));
                        }
                        else
                        {
                            scrollInfo.SetVerticalOffset(index - Math.Floor(scrollInfo.ViewportHeight / 2));
                        }
                    }
                }
                else
                {
                    // Get the bounds of the item container
                    var rect = new Rect(new Point(), container.RenderSize);

                    // Find constraining parent (either the nearest ScrollContentPresenter or the ListBox itself)
                    FrameworkElement constrainingParent = container;
                    do
                    {
                        constrainingParent = VisualTreeHelper.GetParent(constrainingParent) as FrameworkElement;
                    } while ((null != constrainingParent) &&
                             (listBox != constrainingParent) &&
                             !(constrainingParent is ScrollContentPresenter));

                    if (null != constrainingParent)
                    {
                        // Inflate rect to fill the constraining parent
                        rect.Inflate(
                            Math.Max((constrainingParent.ActualWidth - rect.Width) / 2, 0),
                            Math.Max((constrainingParent.ActualHeight - rect.Height) / 2, 0));
                    }

                    // Bring the (inflated) bounds into view
                    container.BringIntoView(rect);
                }
            }
        }

        #endregion

    }
}