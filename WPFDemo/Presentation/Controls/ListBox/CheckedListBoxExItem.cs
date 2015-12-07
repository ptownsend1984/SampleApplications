using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using WPFDemo.Common.Extensions;

namespace WPFDemo.Presentation.Controls.ListBox
{
    [TemplatePart(Name = PART_BulletChrome, Type = typeof(Microsoft.Windows.Themes.BulletChrome))]
    public class CheckedListBoxExItem : ListBoxItem
    {

        #region Constants

        private const string PART_BulletChrome = "PART_BulletChrome";

        #endregion

        #region Static Members

        static CheckedListBoxExItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckedListBoxExItem), new FrameworkPropertyMetadata(typeof(CheckedListBoxExItem)));

            EventManager.RegisterClassHandler(typeof(CheckedListBoxExItem), MouseDownEvent, new System.Windows.Input.MouseButtonEventHandler(OnPreviewMouseDownEvent));
            EventManager.RegisterClassHandler(typeof(CheckedListBoxExItem), MouseDoubleClickEvent, new System.Windows.Input.MouseButtonEventHandler(OnPreviewMouseDoubleClick));
        }

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
            "IsChecked", typeof(bool), typeof(CheckedListBoxExItem),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsChecked_PropertyChanged)
            );
        public static readonly DependencyProperty IsCheckBoxEnabledProperty = DependencyProperty.Register(
            "IsCheckBoxEnabled", typeof(bool), typeof(CheckedListBoxExItem),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None, IsCheckBoxEnabled_PropertyChanged)
            );

        public static readonly DependencyProperty ClickSelectModeProperty = DependencyProperty.Register(
            "ClickSelectMode", typeof(CheckedListBoxExClickSelectMode), typeof(CheckedListBoxExItem),
            new FrameworkPropertyMetadata(CheckedListBoxExClickSelectMode.DoubleClick, FrameworkPropertyMetadataOptions.None, CheckedListBoxExClickSelectMode_PropertyChanged)
            );

        private static void IsChecked_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Item = o as CheckedListBoxExItem;
            if (Item == null)
                return;

            Item.OnIsCheckedChanged();
        }
        private static void IsCheckBoxEnabled_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Item = o as CheckedListBoxExItem;
            if (Item == null)
                return;

            Item.OnIsCheckBoxEnabledChanged();
        }
        private static void CheckedListBoxExClickSelectMode_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Item = o as CheckedListBoxExItem;
            if (Item == null)
                return;

            Item.OnClickSelectModeChanged();
        }

        public static readonly RoutedEvent IsCheckedChangedEvent = EventManager.RegisterRoutedEvent(
            "IsCheckedChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CheckedListBoxExItem)
            );
        public static readonly RoutedEvent IsCheckBoxEnabledChangedEvent = EventManager.RegisterRoutedEvent(
            "IsCheckBoxEnabledChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CheckedListBoxExItem)
            );
        public static readonly RoutedEvent ClickSelectModeChangedEvent = EventManager.RegisterRoutedEvent(
            "ClickSelectModeChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CheckedListBoxExItem)
            );

        private static void OnPreviewMouseDownEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var Sender = sender as CheckedListBoxExItem;
            if (Sender == null || e.LeftButton != System.Windows.Input.MouseButtonState.Pressed || !Sender.IsCheckBoxEnabled)
                return;

            switch (Sender.ClickSelectMode)
            {
                case CheckedListBoxExClickSelectMode.SingleClick:
                    Sender.IsChecked = !Sender.IsChecked;
                    break;
            }
        }
        private static void OnPreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var Sender = sender as CheckedListBoxExItem;
            if (Sender == null || e.LeftButton != System.Windows.Input.MouseButtonState.Pressed || !Sender.IsCheckBoxEnabled)
                return;

            switch (Sender.ClickSelectMode)
            {
                case CheckedListBoxExClickSelectMode.DoubleClick:
                    Sender.IsChecked = !Sender.IsChecked;
                    break;
            }
        }

        #endregion

        #region Global Variables

        private Microsoft.Windows.Themes.BulletChrome _BulletChrome;

        #endregion

        #region Properties

        private Microsoft.Windows.Themes.BulletChrome BulletChrome
        {
            get { return _BulletChrome; }
            set
            {
                if (_BulletChrome != null)
                    DetachBulletChrome(_BulletChrome);
                _BulletChrome = value;
                if (_BulletChrome != null)
                    AttachBulletChrome(_BulletChrome);
            }
        }
        private void AttachBulletChrome(Microsoft.Windows.Themes.BulletChrome bulletChrome)
        {
            if (bulletChrome == null)
                throw new ArgumentNullException("bulletChrome");

            bulletChrome.MouseDown += BulletChrome_MouseDown;
        }

        private void DetachBulletChrome(Microsoft.Windows.Themes.BulletChrome bulletChrome)
        {
            if (bulletChrome == null)
                throw new ArgumentNullException("bulletChrome");

            bulletChrome.MouseDown -= BulletChrome_MouseDown;
        }

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public bool IsCheckBoxEnabled
        {
            get { return (bool)GetValue(IsCheckBoxEnabledProperty); }
            set { SetValue(IsCheckBoxEnabledProperty, value); }
        }
        public CheckedListBoxExClickSelectMode ClickSelectMode
        {
            get { return (CheckedListBoxExClickSelectMode)GetValue(ClickSelectModeProperty); }
            set { SetValue(ClickSelectModeProperty, value); }
        }

        #endregion

        #region Events

        public event RoutedEventHandler IsCheckedChanged
        {
            add { AddHandler(IsCheckedChangedEvent, value); }
            remove { RemoveHandler(IsCheckedChangedEvent, value); }
        }
        public event RoutedEventHandler IsCheckBoxEnabledChanged
        {
            add { AddHandler(IsCheckBoxEnabledChangedEvent, value); }
            remove { RemoveHandler(IsCheckBoxEnabledChangedEvent, value); }
        }
        public event RoutedEventHandler ClickSelectModeChanged
        {
            add { AddHandler(ClickSelectModeChangedEvent, value); }
            remove { RemoveHandler(ClickSelectModeChangedEvent, value); }
        }

        #endregion

        #region Event Handlers

        private void BulletChrome_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!this.IsCheckBoxEnabled)
                return;

            this.IsChecked = !this.IsChecked;

            //If the owning listbox is in single selection mode, select the item
            var ListBox = this.FindVisualParent<System.Windows.Controls.ListBox>();
            if (ListBox != null && ListBox.SelectionMode == SelectionMode.Single)
                this.IsSelected = true;

            //Focus on the item
            this.Focus();
            e.Handled = true;
        }

        #endregion

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.BulletChrome = this.GetTemplateChild("PART_BulletChrome") as Microsoft.Windows.Themes.BulletChrome;
        }

        protected virtual void OnIsCheckedChanged()
        {
            this.RaiseEvent(new RoutedEventArgs(IsCheckedChangedEvent));
        }
        protected virtual void OnIsCheckBoxEnabledChanged()
        {
            this.RaiseEvent(new RoutedEventArgs(IsCheckBoxEnabledChangedEvent));
        }
        protected virtual void OnClickSelectModeChanged()
        {
            this.RaiseEvent(new RoutedEventArgs(ClickSelectModeChangedEvent));
        }

        #endregion

    }

    public enum CheckedListBoxExClickSelectMode
    {
        None = 0,
        SingleClick = 1,
        DoubleClick = 2
    }
}