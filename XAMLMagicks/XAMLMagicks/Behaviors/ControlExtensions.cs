using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace XAMLMagicks.Behaviors
{
    public static class ControlExtensions
    {

        public static readonly DependencyProperty BubbleMouseWheelProperty = DependencyProperty.RegisterAttached(
            "BubbleMouseWheel", typeof(bool), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, BubbleMouseWheelProperty_PropertyChanged)
            );
        public static bool GetBubbleMouseWheel(FrameworkElement Element)
        {
            return (bool)Element.GetValue(BubbleMouseWheelProperty);
        }
        public static void SetBubbleMouseWheel(FrameworkElement Element, bool Value)
        {
            Element.SetValue(BubbleMouseWheelProperty, Value);
        }
        private static void BubbleMouseWheelProperty_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (bool)e.NewValue;
            Element.PreviewMouseWheel -= BubbleMouseWheelProperty_Element_PreviousMouseWheel;
            if (Value)
            {
                Element.PreviewMouseWheel += BubbleMouseWheelProperty_Element_PreviousMouseWheel;
            }
        }
        private static void BubbleMouseWheelProperty_Element_PreviousMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var Element = sender as FrameworkElement;
            if (Element == null)
                return;

            //Relaunch the mouse wheel event
            e.Handled = true;
            var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            e2.RoutedEvent = UIElement.MouseWheelEvent;
            Element.RaiseEvent(e2);
        }

    }
}