using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using WPFDemo.Common.Extensions;
using System.Windows.Input;
using System.Windows.Controls;
using WPFDemo.Presentation.EventManagers;

namespace WPFDemo.Presentation.Behaviors
{
    public static class ControlExtensions
    {

        #region Static Properties

        public static readonly DependencyProperty IsPasswordBoundProperty = DependencyProperty.RegisterAttached(
            "IsPasswordBound", typeof(bool), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, IsPasswordBoundProperty_PropertyChanged)
            );
        public static readonly DependencyProperty PasswordValueProperty = DependencyProperty.RegisterAttached(
            "PasswordValue", typeof(string), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, PasswordValueProperty_PropertyChanged) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.LostFocus }
            );
        private static readonly DependencyProperty PasswordValueWeakEventListenerProperty = DependencyProperty.RegisterAttached(
          "PasswordValueWeakEventListener", typeof(IWeakEventListener), typeof(ControlExtensions),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
          );
        internal static readonly DependencyProperty IsPasswordValueChangingProperty = DependencyProperty.RegisterAttached(
          "IsPasswordValueChanging", typeof(bool), typeof(ControlExtensions),
          new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None)
          );

        public static readonly DependencyProperty BubbleMouseWheelProperty = DependencyProperty.RegisterAttached(
            "BubbleMouseWheel", typeof(bool), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, BubbleMouseWheelProperty_PropertyChanged)
            );
        private static readonly DependencyProperty BubbleMouseWheelWeakEventListenerProperty = DependencyProperty.RegisterAttached(
          "BubbleMouseWheelWeakEventListener", typeof(IWeakEventListener), typeof(ControlExtensions),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
          );

        public static readonly DependencyProperty DoDragMoveProperty = DependencyProperty.RegisterAttached(
            "DoDragMove", typeof(bool), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, DoDragMove_PropertyChanged)
            );
        public static readonly DependencyProperty UseDoDragMoveDragDistanceProperty = DependencyProperty.RegisterAttached(
            "UseDoDragMoveDragDistance", typeof(bool), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None)
            );
        internal static readonly DependencyProperty DoDragMoveStartPointProperty = DependencyProperty.RegisterAttached(
            "DoDragMoveStartPoint", typeof(Point), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(new Point(0d, 0d), FrameworkPropertyMetadataOptions.None)
            );
        private static readonly DependencyProperty DoDragMoveMouseDownWeakEventListenerProperty = DependencyProperty.RegisterAttached(
          "DoDragMoveMouseDownWeakEventListener", typeof(IWeakEventListener), typeof(ControlExtensions),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
          );
        private static readonly DependencyProperty DoDragMoveMouseMoveWeakEventListenerProperty = DependencyProperty.RegisterAttached(
          "DoDragMoveMouseMoveWeakEventListener", typeof(IWeakEventListener), typeof(ControlExtensions),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
          );

        public static DependencyProperty UseBindableCaretIndexProperty = DependencyProperty.RegisterAttached(
            "UseBindableCaretIndex", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false, UseBindableCaretIndex_PropertyChanged));

        public static DependencyProperty BindableCaretIndexProperty = DependencyProperty.RegisterAttached(
            "BindableCaretIndex", typeof(int), typeof(ControlExtensions), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, BindableCaretIndex_PropertyChanged));

        private static DependencyProperty SettingBindableCaretIndexProperty = DependencyProperty.RegisterAttached(
            "SettingBindableCaretIndex", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false));

        public static DependencyProperty SquareModeProperty = DependencyProperty.RegisterAttached(
            "SquareMode", typeof(SquareMode), typeof(ControlExtensions), new PropertyMetadata(SquareMode.None, SquareMode_PropertyChanged));

        public static readonly DependencyProperty CenterSelectedItemOnLoadProperty = DependencyProperty.RegisterAttached(
            "CenterSelectedItemOnLoad", typeof(bool), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, CenterSelectedItemOnLoad_PropertyChanged)
            );

        public static readonly DependencyProperty ScrollToNewItemProperty = DependencyProperty.RegisterAttached(
            "ScrollToNewItem", typeof(bool), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, ScrollToNewItem_PropertyChanged)
            );
        private static readonly DependencyProperty ScrollToNewItemWeakEventListenerProperty = DependencyProperty.RegisterAttached(
          "ScrollToNewItemWeakEventListener", typeof(IWeakEventListener), typeof(ControlExtensions),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
          );
        public static readonly DependencyProperty ScrollToNewItemFocusSpecificElementNameProperty = DependencyProperty.RegisterAttached(
            "ScrollToNewItemFocusSpecificElementName", typeof(string), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
            );

        public static readonly DependencyProperty ItemContainersGeneratedCommandProperty = DependencyProperty.RegisterAttached(
            "ItemContainersGeneratedCommand", typeof(ICommand), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, ItemContainersGeneratedCommand_PropertyChanged)
            );
        public static readonly DependencyProperty ItemContainersGeneratedCommandParameterProperty = DependencyProperty.RegisterAttached(
            "ItemContainersGeneratedCommandParameter", typeof(object), typeof(ControlExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, ItemContainersGeneratedCommand_PropertyChanged)
            );
        private static readonly DependencyProperty ItemContainersGeneratedCommandWeakEventListenerProperty = DependencyProperty.RegisterAttached(
          "ItemContainersGeneratedCommandWeakEventListener", typeof(IWeakEventListener), typeof(ControlExtensions),
          new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
          );

        #endregion

        #region Static Methods

        public static bool GetIsPasswordBound(PasswordBox PasswordBox)
        {
            return (bool)PasswordBox.GetValue(IsPasswordBoundProperty);
        }
        public static void SetIsPasswordBound(PasswordBox PasswordBox, bool Value)
        {
            PasswordBox.SetValue(IsPasswordBoundProperty, Value);
        }
        public static string GetPasswordValue(PasswordBox PasswordBox)
        {
            return (string)PasswordBox.GetValue(PasswordValueProperty);
        }
        public static void SetPasswordValue(PasswordBox PasswordBox, string Value)
        {
            PasswordBox.SetValue(PasswordValueProperty, Value);
        }
        private static IWeakEventListener GetPasswordValueWeakEventListener(PasswordBox PasswordBox)
        {
            return (IWeakEventListener)PasswordBox.GetValue(PasswordValueWeakEventListenerProperty);
        }
        private static void SetPasswordValueWeakEventListener(PasswordBox PasswordBox, IWeakEventListener Value)
        {
            PasswordBox.SetValue(PasswordValueWeakEventListenerProperty, Value);
        }
        internal static bool GetIsPasswordValueChanging(PasswordBox PasswordBox)
        {
            return (bool)PasswordBox.GetValue(IsPasswordValueChangingProperty);
        }
        internal static void SetIsPasswordValueChanging(PasswordBox PasswordBox, bool Value)
        {
            PasswordBox.SetValue(IsPasswordValueChangingProperty, Value);
        }

        public static bool GetBubbleMouseWheel(FrameworkElement Element)
        {
            return (bool)Element.GetValue(BubbleMouseWheelProperty);
        }
        public static void SetBubbleMouseWheel(FrameworkElement Element, bool Value)
        {
            Element.SetValue(BubbleMouseWheelProperty, Value);
        }
        private static IWeakEventListener GetBubbleMouseWheelWeakEventListenerProperty(FrameworkElement Element)
        {
            return (IWeakEventListener)Element.GetValue(BubbleMouseWheelWeakEventListenerProperty);
        }
        private static void SetBubbleMouseWheelWeakEventListenerProperty(FrameworkElement Element, IWeakEventListener Value)
        {
            Element.SetValue(BubbleMouseWheelWeakEventListenerProperty, Value);
        }

        public static bool GetDoDragMove(FrameworkElement Element)
        {
            return (bool)Element.GetValue(DoDragMoveProperty);
        }
        public static void SetDoDragMove(FrameworkElement Element, bool Value)
        {
            Element.SetValue(DoDragMoveProperty, Value);
        }
        public static bool GetUseDoDragMoveDragDistance(FrameworkElement Element)
        {
            return (bool)Element.GetValue(UseDoDragMoveDragDistanceProperty);
        }
        public static void SetUseDoDragMoveDragDistance(FrameworkElement Element, bool Value)
        {
            Element.SetValue(UseDoDragMoveDragDistanceProperty, Value);
        }
        internal static Point GetDoDragMoveStartPoint(FrameworkElement Element)
        {
            return (Point)Element.GetValue(DoDragMoveStartPointProperty);
        }
        internal static void SetDoDragMoveStartPoint(FrameworkElement Element, Point Value)
        {
            Element.SetValue(DoDragMoveStartPointProperty, Value);
        }
        private static IWeakEventListener GetDoDragMoveMouseDownWeakEventListenerProperty(FrameworkElement Element)
        {
            return (IWeakEventListener)Element.GetValue(DoDragMoveMouseDownWeakEventListenerProperty);
        }
        private static void SetDoDragMoveMouseDownWeakEventListenerProperty(FrameworkElement Element, IWeakEventListener Value)
        {
            Element.SetValue(DoDragMoveMouseDownWeakEventListenerProperty, Value);
        }
        private static IWeakEventListener GetDoDragMoveMouseMoveWeakEventListenerProperty(FrameworkElement Element)
        {
            return (IWeakEventListener)Element.GetValue(DoDragMoveMouseMoveWeakEventListenerProperty);
        }
        private static void SetDoDragMoveMouseMoveWeakEventListenerProperty(FrameworkElement Element, IWeakEventListener Value)
        {
            Element.SetValue(DoDragMoveMouseMoveWeakEventListenerProperty, Value);
        }

        private static void IsPasswordBoundProperty_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var PasswordBox = o as PasswordBox;
            if (PasswordBox == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (bool)e.NewValue;
            var Listener = GetPasswordValueWeakEventListener(PasswordBox);
            if (Value)
            {
                if (Listener == null)
                    Listener = new PasswordChangedEventListener();

                SetPasswordValueWeakEventListener(PasswordBox, Listener);
                PasswordChangedEventManager.AddListener(PasswordBox, Listener);
            }
            else if (Listener != null)
            {
                SetPasswordValueWeakEventListener(PasswordBox, null);
                PasswordChangedEventManager.RemoveListener(PasswordBox, Listener);
            }
        }
        private static void PasswordValueProperty_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var PasswordBox = o as PasswordBox;
            if (PasswordBox == null)
                throw new ArgumentException("Type mismatch", "o");

            var IsBound = GetIsPasswordBound(PasswordBox);
            if (!IsBound)
                return;
            var IsChanging = ControlExtensions.GetIsPasswordValueChanging(PasswordBox);
            if (IsChanging)
                return;

            ControlExtensions.SetIsPasswordValueChanging(PasswordBox, true);
            PasswordBox.Password = Convert.ToString(e.NewValue);
            ControlExtensions.SetIsPasswordValueChanging(PasswordBox, false);
        }

        private static void BubbleMouseWheelProperty_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (bool)e.NewValue;
            var Listener = GetBubbleMouseWheelWeakEventListenerProperty(Element);
            if (Value)
            {
                if (Listener == null)
                    Listener = new BubbleMouseWheelEventListener();

                SetBubbleMouseWheelWeakEventListenerProperty(Element, Listener);
                PreviewMouseWheelEventManager.AddListener(Element, Listener);
            }
            else if (Listener != null)
            {
                SetBubbleMouseWheelWeakEventListenerProperty(Element, null);
                PreviewMouseWheelEventManager.RemoveListener(Element, Listener);
            }
        }

        private static void DoDragMove_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (bool)e.NewValue;
            var DownListener = GetDoDragMoveMouseDownWeakEventListenerProperty(Element);
            var MoveListener = GetDoDragMoveMouseMoveWeakEventListenerProperty(Element);
            if (Value)
            {
                if (MoveListener == null)
                    MoveListener = new DoDragMoveMouseMoveEventListener();
                if (DownListener == null)
                    DownListener = new DoDragMoveMouseDownEventListener();

                SetDoDragMoveMouseMoveWeakEventListenerProperty(Element, MoveListener);
                SetDoDragMoveMouseDownWeakEventListenerProperty(Element, DownListener);
                PreviewMouseMoveEventManager.AddListener(Element, MoveListener);
                PreviewMouseDownEventManager.AddListener(Element, DownListener);
            }
            else
            {
                if (MoveListener != null)
                {
                    SetDoDragMoveMouseMoveWeakEventListenerProperty(Element, null);
                    PreviewMouseMoveEventManager.RemoveListener(Element, MoveListener);
                }
                if (DownListener != null)
                {
                    SetDoDragMoveMouseDownWeakEventListenerProperty(Element, null);
                    PreviewMouseDownEventManager.RemoveListener(Element, DownListener);
                }
            }
        }

        public static bool GetCenterSelectedItemOnLoad(ListBox ListBox)
        {
            return (bool)ListBox.GetValue(CenterSelectedItemOnLoadProperty);
        }
        public static void SetCenterSelectedItemOnLoad(ListBox ListBox, bool Value)
        {
            ListBox.SetValue(CenterSelectedItemOnLoadProperty, Value);
        }
        private static void CenterSelectedItemOnLoad_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var ListBox = o as ListBox;
            if (ListBox == null)
                throw new ArgumentException("Type mismatch", "o");

            ListBox.Loaded -= CenterSelectedItemOnLoad_ListBox_Loaded;
            if ((bool)e.NewValue == false)
                return;

            ListBox.Loaded += CenterSelectedItemOnLoad_ListBox_Loaded;
        }
        private static void CenterSelectedItemOnLoad_ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            var ListBox = sender as ListBox;
            if (ListBox == null)
                return;

            try
            {
                var SelectedItem = ListBox.SelectedItem;
                if (SelectedItem == null)
                    return;

                ListBox.ScrollIntoView(SelectedItem);
            }
            finally
            {
                ListBox.Loaded -= CenterSelectedItemOnLoad_ListBox_Loaded;
            }
        }

        public static bool GetScrollToNewItem(ItemsControl ItemsControl)
        {
            return (bool)ItemsControl.GetValue(ScrollToNewItemProperty);
        }
        public static void SetScrollToNewItem(ItemsControl ItemsControl, bool Value)
        {
            ItemsControl.SetValue(ScrollToNewItemProperty, Value);
        }
        private static IWeakEventListener GetScrollToNewItemWeakEventListener(ItemsControl Element)
        {
            return (IWeakEventListener)Element.GetValue(ScrollToNewItemWeakEventListenerProperty);
        }
        private static void SetScrollToNewItemWeakEventListener(ItemsControl Element, IWeakEventListener Value)
        {
            Element.SetValue(ScrollToNewItemWeakEventListenerProperty, Value);
        }
        public static string GetScrollToNewItemFocusSpecificElementName(ItemsControl ItemsControl)
        {
            return (string)ItemsControl.GetValue(ScrollToNewItemFocusSpecificElementNameProperty);
        }
        public static void SetScrollToNewItemFocusSpecificElementName(ItemsControl ItemsControl, string Value)
        {
            ItemsControl.SetValue(ScrollToNewItemFocusSpecificElementNameProperty, Value);
        }
        private static void ScrollToNewItem_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var ItemsControl = o as ItemsControl;
            if (ItemsControl == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (bool)e.NewValue;
            var StatusListener = GetScrollToNewItemWeakEventListener(ItemsControl);
            if (Value)
            {
                if (StatusListener == null)
                    StatusListener = new ScrollToNewItemEventListener();

                SetScrollToNewItemWeakEventListener(ItemsControl, StatusListener);
                ItemContainerGeneratorStatusChangedEventManager.AddListener(ItemsControl, StatusListener);
            }
            else
            {
                if (StatusListener != null)
                {
                    SetScrollToNewItemWeakEventListener(ItemsControl, null);
                    ItemContainerGeneratorStatusChangedEventManager.RemoveListener(ItemsControl, StatusListener);
                }
            }
        }

        public static ICommand GetItemContainersGeneratedCommand(ItemsControl ItemsControl)
        {
            return (ICommand)ItemsControl.GetValue(ItemContainersGeneratedCommandProperty);
        }
        public static void SetItemContainersGeneratedCommand(ItemsControl ItemsControl, ICommand Value)
        {
            ItemsControl.SetValue(ItemContainersGeneratedCommandProperty, Value);
        }
        public static object GetItemContainersGeneratedCommandParameter(ItemsControl ItemsControl)
        {
            return (object)ItemsControl.GetValue(ItemContainersGeneratedCommandParameterProperty);
        }
        public static void SetItemContainersGeneratedCommandParameter(ItemsControl ItemsControl, object Value)
        {
            ItemsControl.SetValue(ItemContainersGeneratedCommandParameterProperty, Value);
        }
        private static IWeakEventListener GetItemContainersGeneratedCommandWeakEventListener(ItemsControl Element)
        {
            return (IWeakEventListener)Element.GetValue(ItemContainersGeneratedCommandWeakEventListenerProperty);
        }
        private static void SetItemContainersGeneratedCommandWeakEventListener(ItemsControl Element, IWeakEventListener Value)
        {
            Element.SetValue(ItemContainersGeneratedCommandWeakEventListenerProperty, Value);
        }
        private static void ItemContainersGeneratedCommand_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var ItemsControl = o as ItemsControl;
            if (ItemsControl == null)
                throw new ArgumentException("Type mismatch", "o");

            var Value = (ICommand)e.NewValue;
            var StatusListener = GetItemContainersGeneratedCommandWeakEventListener(ItemsControl);
            if (Value != null)
            {
                if (StatusListener == null)
                    StatusListener = new ItemContainerGeneratedCommandEventListener();

                SetItemContainersGeneratedCommandWeakEventListener(ItemsControl, StatusListener);
                ItemContainerGeneratorStatusChangedEventManager.AddListener(ItemsControl, StatusListener);
            }
            else
            {
                if (StatusListener != null)
                {
                    SetItemContainersGeneratedCommandWeakEventListener(ItemsControl, null);
                    ItemContainerGeneratorStatusChangedEventManager.RemoveListener(ItemsControl, StatusListener);
                }
            }
        }

        private static void UseBindableCaretIndex_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var TextBox = o as TextBox;
            if (TextBox == null)
                return;

            TextBox.TextChanged -= TextBox_TextChanged;
            TextBox.PreviewMouseDown -= TextBox_PreviewMouseUp;
            TextBox.PreviewKeyDown -= TextBox_PreviewKeyUp;

            var NewValue = (bool)e.NewValue;
            if (!NewValue)
                return;

            SetSettingBindableCaretIndex(TextBox, true);
            SetBindableCaretIndex(TextBox, TextBox.CaretIndex);
            SetSettingBindableCaretIndex(TextBox, false);

            TextBox.TextChanged += TextBox_TextChanged;
            TextBox.PreviewMouseUp += TextBox_PreviewMouseUp;
            TextBox.PreviewKeyUp += TextBox_PreviewKeyUp;
        }

        private static void BindableCaretIndex_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var TextBox = o as TextBox;
            if (TextBox == null)
                return;

            var IsSetting = GetSettingBindableCaretIndex(TextBox);
            if (IsSetting)
                return;

            SetSettingBindableCaretIndex(TextBox, true);
            TextBox.CaretIndex = (int)e.NewValue;
            SetSettingBindableCaretIndex(TextBox, false);
        }
        private static void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var TextBox = sender as TextBox;
            if (TextBox == null)
                return;

            var IsSetting = GetSettingBindableCaretIndex(TextBox);
            if (IsSetting)
                return;

            SetSettingBindableCaretIndex(TextBox, true);
            SetBindableCaretIndex(TextBox, TextBox.CaretIndex);
            SetSettingBindableCaretIndex(TextBox, false);
        }
        private static void TextBox_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var TextBox = sender as TextBox;
            if (TextBox == null)
                return;

            SetSettingBindableCaretIndex(TextBox, true);
            SetBindableCaretIndex(TextBox, TextBox.CaretIndex);
            SetSettingBindableCaretIndex(TextBox, false);
        }
        private static void TextBox_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var TextBox = sender as TextBox;
            if (TextBox == null)
                return;

            SetSettingBindableCaretIndex(TextBox, true);
            SetBindableCaretIndex(TextBox, TextBox.CaretIndex);
            SetSettingBindableCaretIndex(TextBox, false);
        }

        public static bool GetUseBindableCaretIndex(TextBox o)
        {
            return (bool)o.GetValue(UseBindableCaretIndexProperty);
        }
        public static void SetUseBindableCaretIndex(TextBox o, bool Value)
        {
            o.SetValue(UseBindableCaretIndexProperty, Value);
        }
        public static int GetBindableCaretIndex(TextBox o)
        {
            return (int)o.GetValue(BindableCaretIndexProperty);
        }
        public static void SetBindableCaretIndex(TextBox o, int Value)
        {
            o.SetValue(BindableCaretIndexProperty, Value);
        }
        public static bool GetSettingBindableCaretIndex(TextBox o)
        {
            return (bool)o.GetValue(SettingBindableCaretIndexProperty);
        }
        public static void SetSettingBindableCaretIndex(TextBox o, bool Value)
        {
            o.SetValue(SettingBindableCaretIndexProperty, Value);
        }

        public static SquareMode GetSquareMode(FrameworkElement o)
        {
            return (SquareMode)o.GetValue(SquareModeProperty);
        }
        public static void SetSquareMode(FrameworkElement o, SquareMode Value)
        {
            o.SetValue(SquareModeProperty, Value);
        }
        private static void SquareMode_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                return;

            var OldValue = (SquareMode)e.OldValue;
            if (OldValue != SquareMode.None)
                ClearSquareModeBinding(Element, OldValue);

            var NewValue = (SquareMode)e.NewValue;
            if (NewValue != SquareMode.None)
                SetSquareModeBinding(Element, NewValue);
        }
        private static void SetSquareModeBinding(FrameworkElement element, SquareMode squareMode)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            //http://blog.wpfwonderland.com/2010/10/04/sizeable-yet-square-content-control/
            System.Windows.Data.Binding Binding;
            switch (squareMode)
            {
                case SquareMode.WidthMatchesActualHeight:
                    //Force the element's width to match its actual height.
                    Binding = new System.Windows.Data.Binding();
                    Binding.Path = new PropertyPath(FrameworkElement.ActualHeightProperty.Name);
                    Binding.RelativeSource = new System.Windows.Data.RelativeSource(System.Windows.Data.RelativeSourceMode.Self);

                    element.SetBinding(FrameworkElement.WidthProperty, Binding);
                    break;
                case SquareMode.HeightMatchesActualWidth:
                    //Force the element's height to match its actual width.
                    Binding = new System.Windows.Data.Binding();
                    Binding.Path = new PropertyPath(FrameworkElement.ActualWidthProperty.Name);
                    Binding.RelativeSource = new System.Windows.Data.RelativeSource(System.Windows.Data.RelativeSourceMode.Self);

                    element.SetBinding(FrameworkElement.HeightProperty, Binding);
                    break;
            }
        }
        private static void ClearSquareModeBinding(FrameworkElement element, SquareMode squareMode)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            switch (squareMode)
            {
                case SquareMode.WidthMatchesActualHeight:
                    System.Windows.Data.BindingOperations.ClearBinding(element, FrameworkElement.WidthProperty);
                    break;
                case SquareMode.HeightMatchesActualWidth:
                    System.Windows.Data.BindingOperations.ClearBinding(element, FrameworkElement.HeightProperty);
                    break;
            }
        }

        #endregion

    }

    internal class PasswordChangedEventListener : IWeakEventListener
    {

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs args)
        {
            if (managerType != typeof(PasswordChangedEventManager))
                return false;

            var PasswordBox = sender as PasswordBox;
            if (PasswordBox == null)
                return true;

            var IsBound = ControlExtensions.GetIsPasswordBound(PasswordBox);
            if (!IsBound)
                return true;
            var IsChanging = ControlExtensions.GetIsPasswordValueChanging(PasswordBox);
            if (IsChanging)
                return true;

            ControlExtensions.SetIsPasswordValueChanging(PasswordBox, true);
            ControlExtensions.SetPasswordValue(PasswordBox, PasswordBox.Password);
            ControlExtensions.SetIsPasswordValueChanging(PasswordBox, false);

            return true;
        }

        #endregion
    }
    internal class BubbleMouseWheelEventListener : IWeakEventListener
    {

        #region IWeakEventListener Members

        // http://josheinstein.com/blog/index.php/2010/08/wpf-nested-scrollviewer-listbox-scrolling/
        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs args)
        {
            if (managerType != typeof(PreviewMouseWheelEventManager))
                return false;

            var Element = sender as FrameworkElement;
            if (Element == null)
                return true;

            var e = args as MouseWheelEventArgs;
            if (e == null)
                return true;

            //Relaunch the mouse wheel event
            e.Handled = true;
            var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            e2.RoutedEvent = UIElement.MouseWheelEvent;
            Element.RaiseEvent(e2);

            return true;
        }

        #endregion
    }
    internal class DoDragMoveMouseMoveEventListener : IWeakEventListener
    {

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs args)
        {
            if (managerType != typeof(PreviewMouseMoveEventManager))
                return false;

            var Element = sender as FrameworkElement;
            if (Element == null)
                return true;

            var e = args as MouseEventArgs;
            if (e == null)
                return true;

            if (e.LeftButton != MouseButtonState.Pressed)
                return true;

            var StartPoint = ControlExtensions.GetDoDragMoveStartPoint(Element);
            var CurrentPoint = e.GetPosition(Element);
            var UseDoDragMoveDragDistance = ControlExtensions.GetUseDoDragMoveDragDistance(Element);
            if (UseDoDragMoveDragDistance)
            {
                var IsXDrag = Math.Abs(CurrentPoint.X - StartPoint.X) < SystemParameters.MinimumHorizontalDragDistance;
                var IsYDrag = Math.Abs(CurrentPoint.Y - StartPoint.Y) < SystemParameters.MinimumVerticalDragDistance;
                if (!IsXDrag && !IsYDrag)
                    return true;
            }

            Window OwnerWindow = null;
            if (Element is Window)
                //This really isn't recommended.  It should ideally be placed on a small and obvious element of the window.
                OwnerWindow = Element as Window;
            else
            {
                OwnerWindow = Element.FindVisualParent<Window>();
            }

            if (OwnerWindow != null)
            {
                OwnerWindow.DragMove();
            }

            return true;
        }

        #endregion
    }
    internal class DoDragMoveMouseDownEventListener : IWeakEventListener
    {

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs args)
        {
            if (managerType != typeof(PreviewMouseDownEventManager))
                return false;

            var Element = sender as FrameworkElement;
            if (Element == null)
                return true;

            var e = args as MouseButtonEventArgs;
            if (e == null)
                return true;

            if (e.LeftButton != MouseButtonState.Pressed)
                return true;

            ControlExtensions.SetDoDragMoveStartPoint(Element, e.GetPosition(Element));
            return true;
        }

        #endregion
    }
    internal class ScrollToNewItemEventListener : IWeakEventListener
    {

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs args)
        {
            if (managerType != typeof(ItemContainerGeneratorStatusChangedEventManager))
                return false;

            var Generator = sender as ItemContainerGenerator;
            if (Generator == null || Generator.Status != System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                return true;

            var GeneratorHost = Generator.GetHost();
            if (GeneratorHost == null || GeneratorHost.Items.Count == 0)
                return true;

            var NewItem = GeneratorHost.Items[GeneratorHost.Items.Count - 1];
            if (NewItem == null)
                return true;

            if (GeneratorHost is ListBox)
                (GeneratorHost as ListBox).ScrollIntoView(NewItem);

            var Container = Generator.ContainerFromItem(NewItem) as UIElement;
            if (Container == null)
                return true;

            Container.Dispatcher.BeginInvoke(new Action<UIElement, string>(WPFs.SetElementFocus), System.Windows.Threading.DispatcherPriority.Loaded, Container, ControlExtensions.GetScrollToNewItemFocusSpecificElementName(GeneratorHost));
            return true;
        }

        #endregion
    }
    internal class ItemContainerGeneratedCommandEventListener : IWeakEventListener
    {

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs args)
        {
            if (managerType != typeof(ItemContainerGeneratorStatusChangedEventManager))
                return false;

            var Generator = sender as ItemContainerGenerator;
            if (Generator == null || Generator.Status != System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                return true;

            var GeneratorHost = Generator.GetHost();
            if (GeneratorHost == null || GeneratorHost.Items.Count == 0)
                return true;

            var Command = ControlExtensions.GetItemContainersGeneratedCommand(GeneratorHost);
            if (Command == null)
                return true;

            var Parameter = ControlExtensions.GetItemContainersGeneratedCommandParameter(GeneratorHost);

            if (Command.CanExecute(Parameter))
                Command.Execute(Parameter);

            return true;
        }

        #endregion
    }

    public enum SquareMode
    {
        None = 0,
        WidthMatchesActualHeight,
        HeightMatchesActualWidth
    }
}