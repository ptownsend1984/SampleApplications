using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace XAMLMagicks.Behaviors
{
    public static class KeyboardExtensions
    {

        public static readonly DependencyProperty EnterDownCommandProperty = DependencyProperty.RegisterAttached(
            "EnterDownCommand", typeof(ICommand), typeof(KeyboardExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, EnterDownCommand_PropertyChanged)
            );
        public static readonly DependencyProperty EnterDownCommandParameterProperty = DependencyProperty.RegisterAttached(
            "EnterDownCommandParameter", typeof(object), typeof(KeyboardExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
            );
        public static ICommand GetEnterDownCommand(FrameworkElement element)
        {
            return (ICommand)element.GetValue(EnterDownCommandProperty);
        }
        public static void SetEnterDownCommand(FrameworkElement element, ICommand Value)
        {
            element.SetValue(EnterDownCommandProperty, Value);
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
            Element.PreviewKeyDown -= EnterDownCommand_Element_PreviewKeyDown;
            if (Value != null)
            {
                Element.PreviewKeyDown += EnterDownCommand_Element_PreviewKeyDown;
            }
        }
        private static void EnterDownCommand_Element_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var Element = sender as FrameworkElement;
            if (Element == null)
                return;

            //Ignore non-enter
            if (e.Key != Key.Enter)
                return;

            //Get the command
            var Command = GetEnterDownCommand(Element);
            if (Command == null)
                return;

            var Parameter = GetEnterDownCommandParameter(Element);
            if (Command.CanExecute(Parameter))
                Command.Execute(Parameter);

            e.Handled = true;
        }

    }
}