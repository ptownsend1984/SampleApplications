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
    public static class KeyboardExtensions
    {

        #region Static Properties

        public static readonly DependencyProperty EnterDownCommandProperty = DependencyProperty.RegisterAttached(
            "EnterDownCommand", typeof(ICommand), typeof(KeyboardExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, EnterDownCommand_PropertyChanged)
            );
        private static readonly DependencyProperty EnterDownCommandWeakEventListenerProperty = DependencyProperty.RegisterAttached(
          "EnterDownCommandWeakEventListener", typeof(IWeakEventListener), typeof(MouseExtensions),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
          );

        public static readonly DependencyProperty EnterDownCommandParameterProperty = DependencyProperty.RegisterAttached(
            "EnterDownCommandParameter", typeof(object), typeof(KeyboardExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
            );

        public static readonly DependencyProperty F5CommandProperty = DependencyProperty.RegisterAttached(
            "F5Command", typeof(ICommand), typeof(KeyboardExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, F5Command_PropertyChanged)
            );
        private static readonly DependencyProperty F5CommandWeakEventListenerProperty = DependencyProperty.RegisterAttached(
        "F5CommandWeakEventListener", typeof(IWeakEventListener), typeof(MouseExtensions),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
        );
        
        public static readonly DependencyProperty F5CommandParameterProperty = DependencyProperty.RegisterAttached(
            "F5CommandParameter", typeof(object), typeof(KeyboardExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
            );

        #endregion

        #region Static Methods

        public static ICommand GetEnterDownCommand(FrameworkElement element)
        {
            return (ICommand)element.GetValue(EnterDownCommandProperty);
        }
        public static void SetEnterDownCommand(FrameworkElement element, ICommand Value)
        {
            element.SetValue(EnterDownCommandProperty, Value);
        }
        private static IWeakEventListener GetEnterDownCommandWeakEventListener(FrameworkElement element)
        {
            return (IWeakEventListener)element.GetValue(EnterDownCommandWeakEventListenerProperty);
        }
        private static void SetEnterDownCommandWeakEventListener(FrameworkElement element, IWeakEventListener Value)
        {
            element.SetValue(EnterDownCommandWeakEventListenerProperty, Value);
        }
        
        public static object GetEnterDownCommandParameter(FrameworkElement element)
        {
            return (object)element.GetValue(EnterDownCommandParameterProperty);
        }
        public static void SetEnterDownCommandParameter(FrameworkElement element, object Value)
        {
            element.SetValue(EnterDownCommandParameterProperty, Value);
        }

        private static void EnterDownCommand_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (ICommand)e.NewValue;
            var Listener = GetEnterDownCommandWeakEventListener(Element);
            if (Value != null)
            {
                if (Listener == null)
                    Listener = new EnterDownCommandEventListener();

                SetEnterDownCommandWeakEventListener(Element, Listener);
                PreviewKeyDownEventManager.AddListener(Element, Listener);
            }
            else if (Listener != null)
            {
                SetEnterDownCommandWeakEventListener(Element, null);
                PreviewKeyDownEventManager.RemoveListener(Element, Listener);
            }
        }

        public static ICommand GetF5Command(FrameworkElement element)
        {
            return (ICommand)element.GetValue(F5CommandProperty);
        }
        public static void SetF5Command(FrameworkElement element, ICommand Value)
        {
            element.SetValue(F5CommandProperty, Value);
        }
        private static IWeakEventListener GetF5CommandWeakEventListener(FrameworkElement element)
        {
            return (IWeakEventListener)element.GetValue(F5CommandWeakEventListenerProperty);
        }
        private static void SetF5CommandWeakEventListener(FrameworkElement element, IWeakEventListener Value)
        {
            element.SetValue(F5CommandWeakEventListenerProperty, Value);
        }
        
        public static object GetF5CommandParameter(FrameworkElement element)
        {
            return (object)element.GetValue(F5CommandParameterProperty);
        }
        public static void SetF5CommandParameter(FrameworkElement element, object Value)
        {
            element.SetValue(F5CommandParameterProperty, Value);
        }

        private static void F5Command_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (ICommand)e.NewValue;
            var Listener = GetF5CommandWeakEventListener(Element);
            if (Value != null)
            {
                if (Listener == null)
                    Listener = new EnterDownCommandEventListener();

                SetF5CommandWeakEventListener(Element, Listener);
                PreviewKeyDownEventManager.AddListener(Element, Listener);
            }
            else if (Listener != null)
            {
                SetF5CommandWeakEventListener(Element, null);
                PreviewKeyDownEventManager.RemoveListener(Element, Listener);
            }

        }

        #endregion

    }
    internal class EnterDownCommandEventListener : IWeakEventListener
    {

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

            var Command = KeyboardExtensions.GetEnterDownCommand(Element);
            if (Command == null)
                return true;
            if (e.Key != Key.Enter)
                return true;

            e.Handled = true;
            var CommandParameter = KeyboardExtensions.GetEnterDownCommandParameter(Element);
            if (Command.CanExecute(CommandParameter))
                Command.Execute(CommandParameter);

            return true;
        }

        #endregion
    }
    internal class F5CommandEventListener : IWeakEventListener
    {

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

            var Command = KeyboardExtensions.GetEnterDownCommand(Element);
            if (Command == null)
                return true;
            if (e.Key != Key.F5)
                return true;

            e.Handled = true;
            var CommandParameter = KeyboardExtensions.GetEnterDownCommandParameter(Element);
            if (Command.CanExecute(CommandParameter))
                Command.Execute(CommandParameter);

            return true;
        }

        #endregion
    }

}