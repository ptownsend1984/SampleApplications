using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Documents;
using ImageOrganizer.Common.Extensions;

namespace ImageOrganizer.Presentation.Behaviors
{
    public static class ControlExtensions
    {

        #region Static Properties

        public static readonly DependencyProperty IsControlSizeBoundProperty = DependencyProperty.RegisterAttached(
            "IsControlSizeBound", typeof(bool), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, IsControlSizeBound_PropertyChanged)
            );
        public static readonly DependencyProperty OneWayControlWidthProperty = DependencyProperty.RegisterAttached(
            "OneWayControlWidth", typeof(double), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );
        public static readonly DependencyProperty OneWayControlHeightProperty = DependencyProperty.RegisterAttached(
            "OneWayControlHeight", typeof(double), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );

        public static readonly DependencyProperty LaunchHyperlinkProperty = DependencyProperty.RegisterAttached(
            "LaunchHyperlink", typeof(bool), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, LaunchHyperlink_PropertyChanged)
            );

        public static readonly DependencyProperty IsAutoCenterItemProperty = DependencyProperty.RegisterAttached(
            "IsAutoCenterItem", typeof(bool), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, IsAutoCenterItem_PropertyChanged)
            );

        public static readonly DependencyProperty BubbleMouseWheelProperty = DependencyProperty.RegisterAttached(
            "BubbleMouseWheel", typeof(bool), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, BubbleMouseWheelProperty_PropertyChanged)
            );
        public static readonly DependencyProperty BubbleMouseWheelRequireControlProperty = DependencyProperty.RegisterAttached(
            "BubbleMouseWheelRequireControl", typeof(bool), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None)
            );

        #endregion

        #region Static Methods

        public static bool GetIsControlSizeBound(Control element)
        {
            return (bool)element.GetValue(IsControlSizeBoundProperty);
        }
        public static void SetIsControlSizeBound(Control element, bool Value)
        {
            element.SetValue(IsControlSizeBoundProperty, Value);
        }
        public static double GetOneWayControlWidth(Control element)
        {
            return (double)element.GetValue(OneWayControlWidthProperty);
        }
        public static void SetOneWayControlWidth(Control element, double Value)
        {
            element.SetValue(OneWayControlWidthProperty, Value);
        }
        public static double GetOneWayControlHeight(Control element)
        {
            return (double)element.GetValue(OneWayControlHeightProperty);
        }
        public static void SetOneWayControlHeight(Control element, double Value)
        {
            element.SetValue(OneWayControlHeightProperty, Value);
        }

        public static bool GetBubbleMouseWheel(FrameworkElement Element)
        {
            return (bool)Element.GetValue(BubbleMouseWheelProperty);
        }
        public static void SetBubbleMouseWheel(FrameworkElement Element, bool Value)
        {
            Element.SetValue(BubbleMouseWheelProperty, Value);
        }
        public static bool GetBubbleMouseWheelRequireControl(FrameworkElement Element)
        {
            return (bool)Element.GetValue(BubbleMouseWheelRequireControlProperty);
        }
        public static void SetBubbleMouseWheelRequireControl(FrameworkElement Element, bool Value)
        {
            Element.SetValue(BubbleMouseWheelRequireControlProperty, Value);
        }

        public static bool GetLaunchHyperlink(Hyperlink element)
        {
            return (bool)element.GetValue(LaunchHyperlinkProperty);
        }
        public static void SetLaunchHyperlink(Hyperlink element, bool Value)
        {
            element.SetValue(LaunchHyperlinkProperty, Value);
        }

        public static bool GetIsAutoCenterItem(ListBox element)
        {
            return (bool)element.GetValue(IsAutoCenterItemProperty);
        }
        public static void SetIsAutoCenterItem(ListBox element, bool Value)
        {
            element.SetValue(IsAutoCenterItemProperty, Value);
        }

        private static void IsControlSizeBound_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as Control;
            if (Element == null)
                return;

            var IsSet = (bool)e.NewValue;            

            Element.SizeChanged -= Element_IsControlSizeBound_SizeChanged;
            if (IsSet)
            {
                SetOneWayControlWidth(Element, Element.ActualWidth);
                SetOneWayControlHeight(Element, Element.ActualHeight);
                Element.SizeChanged += Element_IsControlSizeBound_SizeChanged;
            }

        }
        private static void Element_IsControlSizeBound_SizeChanged(object sender, EventArgs e)
        {
            var Element = sender as Control;
            if (Element == null)
                return;
     
            //Due to some weirdness with the scrollviewer, subtracting 4 prevents the program
            //from taking full CPU usage when maximized.
            SetOneWayControlWidth(Element, Element.ActualWidth - Element.Padding.Left - Element.Padding.Right - 4d);
            SetOneWayControlHeight(Element, Element.ActualHeight - Element.Padding.Top - Element.Padding.Bottom - 4d);
        }

        private static void LaunchHyperlink_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as Hyperlink;
            if (Element == null)
                return;

            var IsSet = (bool)e.NewValue;

            Element.RequestNavigate -= Element_RequestNavigate;
            if (IsSet)
            {
                Element.RequestNavigate += Element_RequestNavigate;
            }
        }

        private static void Element_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        private static void IsAutoCenterItem_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as ListBox;
            if (Element == null)
                return;

            var IsSet = (bool)e.NewValue;

            Element.SelectionChanged -= IsAutoCenterItem_ListBox_SelectedItemChanged;
            if (IsSet)
            {
                Element.SelectionChanged += IsAutoCenterItem_ListBox_SelectedItemChanged;
            }
        }
        private static void IsAutoCenterItem_ListBox_SelectedItemChanged(object sender, SelectionChangedEventArgs e)
        {
            var ListBox = sender as ListBox;
            if (ListBox == null)
                return;
            ListBox.ScrollIntoViewCentered(ListBox.SelectedItem);
        }

        private static void BubbleMouseWheelProperty_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (bool)e.NewValue;
            Element.PreviewMouseWheel -= BubbleMouseWheelProperty_Element_PreviewMouseWheel;
            if (Value)
            {
                Element.PreviewMouseWheel += BubbleMouseWheelProperty_Element_PreviewMouseWheel;
            }
        }
        private static void BubbleMouseWheelProperty_Element_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var Element = sender as FrameworkElement;
            if (Element == null)
                return;

            var RequireControl = GetBubbleMouseWheelRequireControl(Element);
            if (RequireControl && (Keyboard.Modifiers & ModifierKeys.Control) == 0)
                return;

            //Relaunch the mouse wheel event
            e.Handled = true;
            var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            e2.RoutedEvent = UIElement.MouseWheelEvent;
            Element.RaiseEvent(e2);
        }

        #endregion

    }

}