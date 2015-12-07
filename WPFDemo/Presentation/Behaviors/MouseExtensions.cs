using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WPFDemo.Presentation.EventManagers;

namespace WPFDemo.Presentation.Behaviors
{
    public static class MouseExtensions
    {

        #region Static Properties

        public static readonly DependencyProperty MiddleClickCommandProperty = DependencyProperty.RegisterAttached(
            "MiddleClickCommand", typeof(ICommand), typeof(MouseExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, MiddleClickCommand_PropertyChanged)
            );
        private static readonly DependencyProperty MiddleClickCommandWeakEventListenerProperty = DependencyProperty.RegisterAttached(
          "MiddleClickCommandWeakEventListener", typeof(IWeakEventListener), typeof(MouseExtensions ),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
          );
        
        public static readonly DependencyProperty MiddleClickCommandParameterProperty = DependencyProperty.RegisterAttached(
            "MiddleClickCommandParameter", typeof(object), typeof(MouseExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
            );

        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached(
            "DoubleClickCommand", typeof(ICommand), typeof(MouseExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, DoubleClickCommand_PropertyChanged)
            );
        private static readonly DependencyProperty DoubleClickCommandWeakEventListenerProperty = DependencyProperty.RegisterAttached(
          "DoubleClickCommandWeakEventListener", typeof(IWeakEventListener), typeof(MouseExtensions),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
          );

        public static readonly DependencyProperty DoubleClickCommandParameterProperty = DependencyProperty.RegisterAttached(
            "DoubleClickCommandParameter", typeof(object), typeof(MouseExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
            );

        #endregion

        #region Static Methods

        public static ICommand GetMiddleClickCommand(FrameworkElement element)
        {
            return (ICommand)element.GetValue(MiddleClickCommandProperty);
        }
        public static void SetMiddleClickCommand(FrameworkElement element, ICommand Value)
        {
            element.SetValue(MiddleClickCommandProperty, Value);
        }
        private static IWeakEventListener GetMiddleClickCommandWeakEventListener(FrameworkElement element)
        {
            return (IWeakEventListener)element.GetValue(MiddleClickCommandWeakEventListenerProperty);
        }
        private static void SetMiddleClickCommandWeakEventListener(FrameworkElement element, IWeakEventListener Value)
        {
            element.SetValue(MiddleClickCommandWeakEventListenerProperty, Value);
        }

        public static object GetMiddleClickCommandParameter(FrameworkElement element)
        {
            return element.GetValue(MiddleClickCommandParameterProperty);
        }
        public static void SetMiddleClickCommandParameter(FrameworkElement element, object Value)
        {
            element.SetValue(MiddleClickCommandParameterProperty, Value);
        }

        private static void MiddleClickCommand_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (ICommand)e.NewValue;
            var Listener = GetMiddleClickCommandWeakEventListener(Element);
            if (Value != null)
            {
                if (Listener == null)
                    Listener = new MiddleClickCommandEventListener();

                SetMiddleClickCommandWeakEventListener(Element, Listener);
                PreviewMouseDownEventManager.AddListener(Element, Listener);
            }
            else if (Listener != null)
            {
                SetMiddleClickCommandWeakEventListener(Element, null);
                PreviewMouseDownEventManager.RemoveListener(Element, Listener);
            }
        }

        public static ICommand GetDoubleClickCommand(System.Windows.Controls.Control element)
        {
            return (ICommand)element.GetValue(DoubleClickCommandProperty);
        }
        public static void SetDoubleClickCommand(System.Windows.Controls.Control element, ICommand Value)
        {
            element.SetValue(DoubleClickCommandProperty, Value);
        }
        private static IWeakEventListener GetDoubleClickCommandWeakEventListener(System.Windows.Controls.Control element)
        {
            return (IWeakEventListener)element.GetValue(DoubleClickCommandWeakEventListenerProperty);
        }
        private static void SetDoubleClickCommandWeakEventListener(System.Windows.Controls.Control element, IWeakEventListener Value)
        {
            element.SetValue(DoubleClickCommandWeakEventListenerProperty, Value);
        }

        public static object GetDoubleClickCommandParameter(System.Windows.Controls.Control element)
        {
            return element.GetValue(DoubleClickCommandParameterProperty);
        }
        public static void SetDoubleClickCommandParameter(System.Windows.Controls.Control element, object Value)
        {
            element.SetValue(DoubleClickCommandParameterProperty, Value);
        }

        private static void DoubleClickCommand_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as System.Windows.Controls.Control;
            if (Element == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (ICommand)e.NewValue;
            var Listener = GetDoubleClickCommandWeakEventListener(Element);
            if (Value != null)
            {
                if (Listener == null)
                    Listener = new DoubleClickCommandEventListener();

                SetDoubleClickCommandWeakEventListener(Element, Listener);
                DoubleClickEventManager.AddListener(Element, Listener);
            }
            else if (Listener != null)
            {
                SetDoubleClickCommandWeakEventListener(Element, null);
                DoubleClickEventManager.RemoveListener(Element, Listener);
            }
        }
        #endregion

    }

    internal class MiddleClickCommandEventListener : IWeakEventListener
    {

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs args)
        {
            if (managerType != typeof(PreviewMouseDownEventManager))
                return false;

            var Element = sender as FrameworkElement;
            if (Element == null)
                return true;
            var e = args as System.Windows.Input.MouseEventArgs;
            if (e == null)
                return true;
            
            var Command = MouseExtensions.GetMiddleClickCommand(Element);
            if (Command == null)
                return true;
            if (e.MiddleButton != MouseButtonState.Pressed)
                return true;

            e.Handled = true;
            var CommandParameter = MouseExtensions.GetMiddleClickCommandParameter(Element);
            if (Command.CanExecute(CommandParameter))
                Command.Execute(CommandParameter);

            return true;
        }

        #endregion
    }
    internal class DoubleClickCommandEventListener : IWeakEventListener
    {

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs args)
        {
            if (managerType != typeof(DoubleClickEventManager))
                return false;

            var Element = sender as System.Windows.Controls.Control;
            if (Element == null)
                return true;
            var e = args as System.Windows.Input.MouseButtonEventArgs;
            if (e == null)
                return true;

            var Command = MouseExtensions.GetDoubleClickCommand(Element);
            if (Command == null)
                return true;
            if (e.LeftButton != MouseButtonState.Pressed)
                return true;

            e.Handled = true;
            var CommandParameter = MouseExtensions.GetDoubleClickCommandParameter(Element);
            if (Command.CanExecute(CommandParameter))
                Command.Execute(CommandParameter);

            return true;
        }

        #endregion
    }

}