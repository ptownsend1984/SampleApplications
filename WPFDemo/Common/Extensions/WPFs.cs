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
using System.Windows.Input;

namespace WPFDemo.Common.Extensions
{
    [System.Diagnostics.DebuggerStepThrough]
    public static class WPFs
    {

        #region Static Methods

        public static void Execute(this ICommand Command)
        {
            if (Command == null)
                return;
            if (Command.CanExecute(null))
                Command.Execute(null);
        }
        public static bool CanExecute(this ICommand Command)
        {
            if (Command == null)
                return false;
            return Command.CanExecute(null);
        }

        public static System.Windows.MessageBoxButton ToMessageBoxButton(this System.Windows.Forms.MessageBoxButtons MessageBoxButtons)
        {
            switch (MessageBoxButtons)
            {
                case System.Windows.Forms.MessageBoxButtons.AbortRetryIgnore:
                    return MessageBoxButton.YesNoCancel;
                case System.Windows.Forms.MessageBoxButtons.OK:
                    return MessageBoxButton.OK;
                case System.Windows.Forms.MessageBoxButtons.OKCancel:
                    return MessageBoxButton.OKCancel;
                case System.Windows.Forms.MessageBoxButtons.RetryCancel:
                    return MessageBoxButton.YesNo;
                case System.Windows.Forms.MessageBoxButtons.YesNo:
                    return MessageBoxButton.YesNo;
                case System.Windows.Forms.MessageBoxButtons.YesNoCancel:
                    return MessageBoxButton.YesNoCancel;
                default:
                    return MessageBoxButton.OK;
            }
        }
        public static System.Windows.MessageBoxImage ToMessageBoxImage(this System.Windows.Forms.MessageBoxIcon MessageBoxIcon)
        {
            switch (MessageBoxIcon)
            {
                case System.Windows.Forms.MessageBoxIcon.Information:
                    return MessageBoxImage.Information;
                case System.Windows.Forms.MessageBoxIcon.Error:
                    return MessageBoxImage.Stop;
                case System.Windows.Forms.MessageBoxIcon.Exclamation:
                    return MessageBoxImage.Warning;
                case System.Windows.Forms.MessageBoxIcon.None:
                    return MessageBoxImage.None;
                case System.Windows.Forms.MessageBoxIcon.Question:
                    return MessageBoxImage.Question;
                default:
                    return MessageBoxImage.None;
            }
        }
        public static System.Windows.MessageBoxResult ToMessageBoxResult(this System.Windows.Forms.DialogResult DialogResult)
        {
            switch (DialogResult)
            {
                case System.Windows.Forms.DialogResult.Abort:
                    return MessageBoxResult.No;
                case System.Windows.Forms.DialogResult.Cancel:
                    return MessageBoxResult.Cancel;
                case System.Windows.Forms.DialogResult.Ignore:
                    return MessageBoxResult.None;
                case System.Windows.Forms.DialogResult.No:
                    return MessageBoxResult.No;
                case System.Windows.Forms.DialogResult.None:
                    return MessageBoxResult.None;
                case System.Windows.Forms.DialogResult.OK:
                    return MessageBoxResult.OK;
                case System.Windows.Forms.DialogResult.Retry:
                    return MessageBoxResult.Yes;
                case System.Windows.Forms.DialogResult.Yes:
                    return MessageBoxResult.Yes;
                default:
                    return MessageBoxResult.None;
            }
        }

        public static T FindVisualChild<T>(this DependencyObject obj) where T : DependencyObject
        {
            return FindVisualChildEx<T>(obj, null);
        }
        public static T FindVisualChildEx<T>(this DependencyObject obj, Func<T, bool> WhereFunc) where T : DependencyObject
        {
            //http://msdn.microsoft.com/en-us/library/bb613579.aspx
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (
                    child != null && child is T &&
                    (WhereFunc == null || WhereFunc((T)obj))
                    )
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
        public static DependencyObject FindVisualParent(this DependencyObject obj, string TypeName)
        {
            var Parent = VisualTreeHelper.GetParent(obj);
            if (Parent == null)
                return null;
            else if (Parent.GetType().Name.Equals(TypeName))
                return Parent;
            else
                return FindVisualParent(Parent, TypeName);
        }
        /// <summary>
        /// Find the visual children of type T.  Does not recurse under these children.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject obj) where T : DependencyObject
        {
            return FindVisualChildren<T>(obj, true);
        }
        /// <summary>
        /// Find the visual children of type T.  Will recurse to the end.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindAllVisualChildren<T>(this DependencyObject obj) where T : DependencyObject
        {
            return FindVisualChildren<T>(obj, false);
        }
        private static IEnumerable<T> FindVisualChildren<T>(this DependencyObject obj, bool stopRecurseOnHit) where T : DependencyObject
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                //The previous incarnation of FindVisualChildren<T> would stop its recursion hunting when it first
                //encountered the child of type T.
                //The previous incarnation of FindAllVisualChildren<T> would take it and continue recursion.
                var child = VisualTreeHelper.GetChild(obj, i);
                bool isHit = false;
                if (child is T)
                {
                    yield return child as T;
                    isHit = true;
                }

                //Thus, if we are not stopping the recursion on it
                //OR we are stopping but it wasn't a hit,
                //then continue.
                if (!stopRecurseOnHit || !isHit)
                {
                    foreach (var GrandChild in FindVisualChildren<T>(child, stopRecurseOnHit))
                        yield return GrandChild;
                }
            }
        }

        public static IEnumerable<T> FindInputBindings<T>(this UIElement Element)
            where T : InputBinding
        {
            var Current = Element as FrameworkElement;
            while (Current != null)
            {
                foreach (InputBinding Item in Current.InputBindings)
                    if (Item is T)
                        yield return Item as T;
                Current = Current.Parent as FrameworkElement;
            }
        }
        public static IEnumerable<KeyBinding> FindKeyBindings(this UIElement Element)
        {
            return FindInputBindings<KeyBinding>(Element);
        }
        public static IEnumerable<MouseBinding> FindMouseBindings(this UIElement Element)
        {
            return FindInputBindings<MouseBinding>(Element);
        }

        public static void SetElementFocus(this UIElement element)
        {
            SetElementFocus(element, null);
        }
        public static void SetElementFocus(this UIElement element, string specificElementName)
        {
            if (element == null)
                throw new ArgumentNullException("Element");

            //If a specific name is given, attempt to find a child with that name.
            //If one is found, either focus on it or its first focusable child.
            //If none of these are focusable, exit out.
            //If no specific name is given,
            //attempt to set focus on the given element or its first focusable child.
            UIElement FocusElement = null;

            if (!string.IsNullOrEmpty(specificElementName))
            {
                var NamedChild = element.FindAllVisualChildren<FrameworkElement>().Where(o => o.Name == specificElementName).FirstOrDefault();
                if (NamedChild.Focusable)
                    FocusElement = NamedChild;
                else
                    FocusElement = NamedChild.FindAllVisualChildren<UIElement>().Where(o => o.Focusable).FirstOrDefault();
            }
            else
            {
                if (element.Focusable)
                    FocusElement = element;
                else
                    FocusElement = element.FindAllVisualChildren<UIElement>().Where(o => o.Focusable).FirstOrDefault();
            }
            if (FocusElement == null)
                return;

            FocusElement.Focus();
            Keyboard.Focus(FocusElement);
        }

        /// <summary>
        /// Calls UpdateSource on the binding expression for a dependency object and property.  Does nothing if there is no binding.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool UpdateBindingExpressionSource(this DependencyObject o, DependencyProperty property)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (property == null)
                throw new ArgumentNullException("property");

            var BindingExpression = System.Windows.Data.BindingOperations.GetBindingExpression(o, property);
            if (BindingExpression != null)
            {
                BindingExpression.UpdateSource();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Calls UpdateTarget on the binding expression for a dependency object and property.  Does nothing if there is no binding.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool UpdateBindingExpressionTarget(this DependencyObject o, DependencyProperty property)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (property == null)
                throw new ArgumentNullException("property");

            var BindingExpression = System.Windows.Data.BindingOperations.GetBindingExpression(o, property);
            if (BindingExpression != null)
            {
                BindingExpression.UpdateTarget();
                return true;
            }
            else
                return false;
        }

        public static System.Windows.Media.Color ToMediaColor(this System.Drawing.Color Color)
        {
            return System.Windows.Media.Color.FromArgb(Color.A, Color.R, Color.G, Color.B);
        }

        public static BitmapImage ToBitmapImage(this byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                return null;

            var Image = new BitmapImage();
            Image.BeginInit();
            using (var Stream = new System.IO.MemoryStream(imageBytes))
            {
                Image.CacheOption = BitmapCacheOption.OnLoad;
                Image.StreamSource = Stream;
                Image.EndInit();
            }
            return Image;
        }

        public static System.Windows.Controls.ItemsControl GetHost(this System.Windows.Controls.ItemContainerGenerator generator)
        {
            if (generator == null)
                throw new ArgumentNullException("generator");

            var HostProperty = typeof(System.Windows.Controls.ItemContainerGenerator).GetProperty("Host", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (HostProperty == null || !HostProperty.CanRead)
                return null;

            return HostProperty.GetValue(generator, null) as System.Windows.Controls.ItemsControl;
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.Bitmap"/> into a WPF <see cref="BitmapSource"/>.
        /// http://stackoverflow.com/questions/94456/load-a-wpf-bitmapimage-from-a-system-drawing-bitmap
        /// </summary>
        /// <remarks>Uses GDI to do the conversion. Hence the call to the marshalled DeleteObject.
        /// </remarks>
        /// <param name="source">The source bitmap.</param>
        /// <returns>A BitmapSource</returns>
        public static BitmapSource ToBitmapSource(this System.Drawing.Bitmap source)
        {
            BitmapSource bitSrc = null;

            var hBitmap = source.GetHbitmap();

            try
            {
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitSrc = null;
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return bitSrc;
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr hObject);

        #endregion

    }
}