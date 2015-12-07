using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace XAMLMagicks.Behaviors
{
    public static class MouseExtensions
    {

        public static readonly DependencyProperty MiddleClickCommandProperty = DependencyProperty.RegisterAttached(
            "MiddleClickCommand", typeof(ICommand), typeof(MouseExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, MiddleClickCommand_PropertyChanged)
            );
        public static readonly DependencyProperty MiddleClickCommandParameterProperty = DependencyProperty.RegisterAttached(
            "MiddleClickCommandParameter", typeof(object), typeof(MouseExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
            );

        public static ICommand GetMiddleClickCommand(FrameworkElement element)
        {
            return (ICommand)element.GetValue(MiddleClickCommandProperty);
        }
        public static void SetMiddleClickCommand(FrameworkElement element, ICommand Value)
        {
            element.SetValue(MiddleClickCommandProperty, Value);
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
            Element.PreviewMouseDown -= MiddleClickCommand_Element_PreviewMouseDown;
            if (Value != null)
            {
                Element.PreviewMouseDown += MiddleClickCommand_Element_PreviewMouseDown;
            }
        }
        private static void MiddleClickCommand_Element_PreviewMouseDown(object sender, MouseEventArgs e)
        {
            var Element = sender as FrameworkElement;
            if (Element == null)
                return;

            if (e.MiddleButton != MouseButtonState.Pressed)
                return;

            var Command = GetMiddleClickCommand(Element);
            if (Command == null)
                return;

            var Parameter = GetMiddleClickCommandParameter(Element);

            if (Command.CanExecute(Parameter))
                Command.Execute(Parameter);

            e.Handled = true;
        }

    }
}