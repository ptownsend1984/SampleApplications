using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using WPFDemo.Common.Extensions;
using WPFDemo.Presentation.EventManagers;

namespace WPFDemo.Presentation.Behaviors
{
    public static class Navigation
    {

        #region Static Properties

        public static readonly DependencyProperty ProcessEnterAsTabProperty = DependencyProperty.RegisterAttached(
            "ProcessEnterAsTab", typeof(bool), typeof(Navigation),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, ProcessEnterAsTab_PropertyChanged)
            );
        private static readonly DependencyProperty ProcessEnterAsTabWeakEventListenerProperty = DependencyProperty.RegisterAttached(
           "ProcessEnterAsTabWeakEventListener", typeof(IWeakEventListener), typeof(Navigation),
           new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
           );
        
        public static readonly DependencyProperty SelectAllOnKeyboardFocusProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnKeyboardFocus", typeof(bool), typeof(Navigation),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, SelectAllOnKeyboardFocus_PropertyChanged)
            );
        private static readonly DependencyProperty SelectAllOnKeyboardFocusWeakEventListenerProperty = DependencyProperty.RegisterAttached(
           "SelectAllOnKeyboardFocusWeakEventListener", typeof(IWeakEventListener), typeof(Navigation),
           new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
           );

        public static readonly DependencyProperty InitialFocusProperty = DependencyProperty.RegisterAttached(
            "InitialFocus", typeof(bool), typeof(Navigation),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, InitialFocus_PropertyChanged)
            );

        #endregion

        #region Static Methods

        public static bool GetProcessEnterAsTab(FrameworkElement element)
        {
            return (bool)element.GetValue(ProcessEnterAsTabProperty);
        }
        public static void SetProcessEnterAsTab(FrameworkElement element, bool Value)
        {
            element.SetValue(ProcessEnterAsTabProperty, Value);
        }
        private static IWeakEventListener GetProcessEnterAsTabWeakEventListener(FrameworkElement element)
        {
            return (IWeakEventListener)element.GetValue(ProcessEnterAsTabWeakEventListenerProperty);
        }
        private static void SetProcessEnterAsTabWeakEventListener(FrameworkElement element, IWeakEventListener Value)
        {
            element.SetValue(ProcessEnterAsTabWeakEventListenerProperty, Value);
        }

        public static bool GetSelectAllOnKeyboardFocus(TextBoxBase element)
        {
            return (bool)element.GetValue(SelectAllOnKeyboardFocusProperty);
        }
        public static void SetSelectAllOnKeyboardFocus(TextBoxBase element, bool Value)
        {
            element.SetValue(SelectAllOnKeyboardFocusProperty, Value);
        }
        private static IWeakEventListener GetSelectAllOnKeyboardFocusWeakEventListener(FrameworkElement element)
        {
            return (IWeakEventListener)element.GetValue(SelectAllOnKeyboardFocusWeakEventListenerProperty);
        }
        private static void SetSelectAllOnKeyboardFocusWeakEventListener(FrameworkElement element, IWeakEventListener Value)
        {
            element.SetValue(SelectAllOnKeyboardFocusWeakEventListenerProperty, Value);
        }

        public static bool GetInitialFocus(FrameworkElement element)
        {
            return (bool)element.GetValue(InitialFocusProperty);
        }
        public static void SetInitialFocus(FrameworkElement element, bool Value)
        {
            element.SetValue(InitialFocusProperty, Value);
        }

        private static void ProcessEnterAsTab_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (bool)e.NewValue;
            var Listener = GetProcessEnterAsTabWeakEventListener(Element);
            if (Value)
            {
                if (Listener == null)
                    Listener = new ProcessEnterAsTabEventListener();

                SetProcessEnterAsTabWeakEventListener(Element, Listener);
                PreviewKeyDownEventManager.AddListener(Element, Listener);
            }
            else if (Listener != null)
            {
                SetProcessEnterAsTabWeakEventListener(Element, null);
                PreviewKeyDownEventManager.RemoveListener(Element, Listener);
            }
        }

        private static void SelectAllOnKeyboardFocus_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var TextBoxBase = o as TextBoxBase;
            if (TextBoxBase == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (bool)e.NewValue;
            var Listener = GetSelectAllOnKeyboardFocusWeakEventListener(TextBoxBase);
            if (Value)
            {
                if (Listener == null)
                    Listener = new SelectAllOnKeyboardFocusEventListener();

                SetSelectAllOnKeyboardFocusWeakEventListener(TextBoxBase, Listener);
                GotKeyboardFocusEventManager.AddListener(TextBoxBase, Listener);
            }
            else if (Listener != null)
            {
                SetSelectAllOnKeyboardFocusWeakEventListener(TextBoxBase, null);
                GotKeyboardFocusEventManager.RemoveListener(TextBoxBase, Listener);
            }
         }

        private static void InitialFocus_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                throw new ArgumentException("Type mismatch", "o");

            Element.Loaded -= Element_InitialFocus_Loaded;
            var Value = (bool)e.NewValue;
            if (Value)
                Element.Loaded += Element_InitialFocus_Loaded;
        }

        static void Element_InitialFocus_Loaded(object sender, RoutedEventArgs e)
        {
            var Element = sender as FrameworkElement;
            if (Element == null)
                throw new ArgumentException("Type mismatch", "sender");
            try
            {
                Element.Focus();
                Keyboard.Focus(Element);
            }
            finally
            {
                Element.Loaded -= Element_InitialFocus_Loaded;
            }
        }
        

        #endregion

    }
    
    internal class ProcessEnterAsTabEventListener : IWeakEventListener
    {
        //Explanation for this type of IWeakEventListener implementation:
        //This uses weak events to prevent memory leaks from holding onto the WPF elements' event handlers
        //To prevent the weak listener from being destroyed, it is saved as an attached property on
        //its owning WPF element.
        //This is fine because the only GC reference to the listener is contained on the element itself,
        //so when the GC collects after the element is zero-ref'd, the listener will be destroyed as well.

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs args)
        {
            if (managerType != typeof(PreviewKeyDownEventManager))
                return false;

            var Element = sender as FrameworkElement;
            if (Element == null)
                return true;
            var e = args as System.Windows.Input.KeyEventArgs;
            if (e == null)
                return true;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                var OriginalElement = e.OriginalSource as FrameworkElement;
                if (OriginalElement != null)
                    OriginalElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                else
                    Element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                return true;
            }
            return true;
        }

        #endregion
    }
    internal class SelectAllOnKeyboardFocusEventListener : IWeakEventListener
    {

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs args)
        {
            if (managerType != typeof(GotKeyboardFocusEventManager))
                return false;

            var TextBoxBase = sender as TextBoxBase;
            if (TextBoxBase == null)
                return true;

            TextBoxBase.SelectAll();           
            return true;
        }

        #endregion
    }
}