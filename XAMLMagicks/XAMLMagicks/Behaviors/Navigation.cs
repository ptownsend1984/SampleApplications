using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace XAMLMagicks.Behaviors
{
    public static class Navigation
    {

        /// <summary>
        /// If true, the enter key will process as a tab press instead.
        /// </summary>
        public static readonly DependencyProperty ProcessEnterAsTabProperty = DependencyProperty.RegisterAttached(
            "ProcessEnterAsTab", typeof(bool), typeof(Navigation),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, ProcessEnterAsTab_PropertyChanged)
            );

        public static bool GetProcessEnterAsTab(FrameworkElement element)
        {
            return (bool)element.GetValue(ProcessEnterAsTabProperty);
        }
        public static void SetProcessEnterAsTab(FrameworkElement element, bool Value)
        {
            element.SetValue(ProcessEnterAsTabProperty, Value);
        }
        private static void ProcessEnterAsTab_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (bool)e.NewValue;
            Element.PreviewKeyDown -= ProcessEnterAsTab_Element_PreviewKeyDown;
            if (Value)
            {
                Element.PreviewKeyDown += ProcessEnterAsTab_Element_PreviewKeyDown;
            }
        }
        private static void ProcessEnterAsTab_Element_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var Element = sender as FrameworkElement;
            if (Element == null)
                return;

            //Ignore keys that are not the enter key
            if (e.Key != Key.Enter)
                return;

            //Ignore shift
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                return;

            var OriginalElement = e.OriginalSource as FrameworkElement;
            if (OriginalElement != null)
                OriginalElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            else
                Element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

            e.Handled = true;
        }

        /// <summary>
        /// If true, all text will be selected when the element receives focus.
        /// </summary>
        public static readonly DependencyProperty SelectAllOnKeyboardFocusProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnKeyboardFocus", typeof(bool), typeof(Navigation),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, SelectAllOnKeyboardFocus_PropertyChanged)
            );
        public static bool GetSelectAllOnKeyboardFocus(TextBoxBase element)
        {
            return (bool)element.GetValue(SelectAllOnKeyboardFocusProperty);
        }
        public static void SetSelectAllOnKeyboardFocus(TextBoxBase element, bool Value)
        {
            element.SetValue(SelectAllOnKeyboardFocusProperty, Value);
        }
        private static void SelectAllOnKeyboardFocus_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var TextBoxBase = o as TextBoxBase;
            if (TextBoxBase == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (bool)e.NewValue;
            TextBoxBase.GotKeyboardFocus -= SelectAllOnKeyboardFocus_Element_GotKeyboardFocus;
            if (Value)
            {
                TextBoxBase.GotKeyboardFocus += SelectAllOnKeyboardFocus_Element_GotKeyboardFocus;
            }
        }
        private static void SelectAllOnKeyboardFocus_Element_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            var TextBoxBase = sender as TextBoxBase;
            if (TextBoxBase == null)
                return;

            TextBoxBase.SelectAll();
        }

        /// <summary>
        /// If true, this element will receive a focus call when it loads the first time.
        /// </summary>
        public static readonly DependencyProperty InitialFocusProperty = DependencyProperty.RegisterAttached(
            "InitialFocus", typeof(bool), typeof(Navigation),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, InitialFocus_PropertyChanged)
            );
        /// <summary>
        /// If using initial focus, specify a type of sub element for focusing fallback.
        /// </summary>
        public static readonly DependencyProperty InitialFocusSpecificElementNameProperty = DependencyProperty.RegisterAttached(
            "InitialFocusSpecificElementName", typeof(string), typeof(Navigation),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
            );

        public static bool GetInitialFocus(FrameworkElement element)
        {
            return (bool)element.GetValue(InitialFocusProperty);
        }
        public static void SetInitialFocus(FrameworkElement element, bool Value)
        {
            element.SetValue(InitialFocusProperty, Value);
        }
        public static string GetInitialFocusSpecificElementName(FrameworkElement element)
        {
            return (string)element.GetValue(InitialFocusSpecificElementNameProperty);
        }
        public static void SetInitialFocusSpecificElementName(FrameworkElement element, string Value)
        {
            element.SetValue(InitialFocusSpecificElementNameProperty, Value);
        }

        private static void InitialFocus_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                throw new ArgumentException("Type mismatch", "o");

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(Element))
                return;

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
                SetElementFocus(Element, GetInitialFocusSpecificElementName(Element));
            }
            finally
            {
                Element.Loaded -= Element_InitialFocus_Loaded;
            }
        }
        internal static void SetElementFocus(FrameworkElement Element, string SpecificElementName)
        {
            if (Element == null)
                throw new ArgumentNullException("Element");
            if (Element.Focusable)
            {
                Element.Focus();
                Keyboard.Focus(Element);
            }
            else
            {
                //If the supplied element isn't focusable,
                //try and find a child that is.
                //Optionally, supply a child name to specify a specific control to hit
                UIElement FirstFocusableChild = null;
                if (string.IsNullOrEmpty(SpecificElementName))
                    FirstFocusableChild = Element.FindAllVisualChildren<UIElement>()
                        .Where((o) => o.Focusable)
                        .FirstOrDefault();
                else
                    FirstFocusableChild = Element.FindAllVisualChildren<FrameworkElement>()
                        .Where((o) => o.Focusable && o.Name == SpecificElementName)
                        .FirstOrDefault();
                if (FirstFocusableChild != null)
                {
                    FirstFocusableChild.Focus();
                    Keyboard.Focus(FirstFocusableChild);
                }
            }
        }

    }
}