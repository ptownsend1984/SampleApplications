using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFDemo.Presentation.Controls.DateTimePicker
{
    public partial class DateTimePickerWrapper : UserControl
    {

        #region Static Members

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
            "Value", typeof(DateTime), typeof(DateTimePickerWrapper), new FrameworkPropertyMetadata(new DateTime(1900, 1, 1), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Value_PropertyChanged)
            );
        private static readonly DependencyProperty IsSettingValueProperty =
            DependencyProperty.Register(
            "IsSettingValue", typeof(bool), typeof(DateTimePickerWrapper), new PropertyMetadata(false)
            );

        public static readonly DependencyProperty MinDateProperty =
            DependencyProperty.Register(
            "MinDate", typeof(DateTime), typeof(DateTimePickerWrapper), new FrameworkPropertyMetadata(new DateTime(1900, 1, 1), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, MinDate_PropertyChanged)
            );
        public static readonly DependencyProperty MaxDateProperty =
            DependencyProperty.Register(
            "MaxDate", typeof(DateTime), typeof(DateTimePickerWrapper), new FrameworkPropertyMetadata(new DateTime(2079, 1, 1), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, MaxDate_PropertyChanged)
            );
        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.Register(
            "Format", typeof(System.Windows.Forms.DateTimePickerFormat), typeof(DateTimePickerWrapper), new PropertyMetadata(System.Windows.Forms.DateTimePickerFormat.Long, Format_PropertyChanged)
            );
        public static readonly DependencyProperty CustomFormatProperty =
            DependencyProperty.Register(
            "CustomFormat", typeof(string), typeof(DateTimePickerWrapper), new PropertyMetadata(null, CustomFormat_PropertyChanged)
            );
        public static readonly DependencyProperty ShowUpDownProperty =
            DependencyProperty.Register(
            "ShowUpDown", typeof(bool), typeof(DateTimePickerWrapper), new PropertyMetadata(false, ShowUpDown_PropertyChanged)
            );

        public static readonly DependencyProperty ShowCheckBoxProperty =
            DependencyProperty.Register(
            "ShowCheckBox", typeof(bool), typeof(DateTimePickerWrapper), new PropertyMetadata(false, ShowCheckBox_PropertyChanged)
            );
        public static readonly DependencyProperty CheckedProperty =
            DependencyProperty.Register(
            "Checked", typeof(bool), typeof(DateTimePickerWrapper), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Checked_PropertyChanged)
            );
        private static readonly DependencyProperty IsCheckingProperty =
            DependencyProperty.Register(
            "IsChecking", typeof(bool), typeof(DateTimePickerWrapper), new PropertyMetadata(false)
            );

        //public static readonly DependencyProperty Property =
        //    DependencyProperty.Register(
        //    "", typeof(), typeof(DateTimePickerWrapper), new PropertyMetadata(, )
        //    );

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DateTimePickerWrapper)
            );

        private static void Value_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var DateTimePickerWrapper = o as DateTimePickerWrapper;
            if (DateTimePickerWrapper == null)
                throw new ArgumentException("Type mismatch", "o");

            var IsSettingValue = DateTimePickerWrapper.IsSettingValue;
            if (IsSettingValue)
                return;

            DateTimePickerWrapper.IsSettingValue = true;
            DateTimePickerWrapper.DateTimePicker.Value = (DateTime)e.NewValue;
            DateTimePickerWrapper.IsSettingValue = false;
        }

        private static void MinDate_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var DateTimePickerWrapper = o as DateTimePickerWrapper;
            if (DateTimePickerWrapper == null)
                throw new ArgumentException("Type mismatch", "o");

            DateTimePickerWrapper.DateTimePicker.MinDate = (DateTime)e.NewValue;
        }
        private static void MaxDate_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var DateTimePickerWrapper = o as DateTimePickerWrapper;
            if (DateTimePickerWrapper == null)
                throw new ArgumentException("Type mismatch", "o");

            DateTimePickerWrapper.DateTimePicker.MaxDate = (DateTime)e.NewValue;
        }
        private static void Format_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var DateTimePickerWrapper = o as DateTimePickerWrapper;
            if (DateTimePickerWrapper == null)
                throw new ArgumentException("Type mismatch", "o");

            DateTimePickerWrapper.DateTimePicker.Format = (System.Windows.Forms.DateTimePickerFormat)e.NewValue;
        }
        private static void CustomFormat_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var DateTimePickerWrapper = o as DateTimePickerWrapper;
            if (DateTimePickerWrapper == null)
                throw new ArgumentException("Type mismatch", "o");

            DateTimePickerWrapper.DateTimePicker.CustomFormat = (string)e.NewValue;
        }
        private static void ShowUpDown_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var DateTimePickerWrapper = o as DateTimePickerWrapper;
            if (DateTimePickerWrapper == null)
                throw new ArgumentException("Type mismatch", "o");

            DateTimePickerWrapper.DateTimePicker.ShowUpDown = (bool)e.NewValue;
        }

        private static void ShowCheckBox_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var DateTimePickerWrapper = o as DateTimePickerWrapper;
            if (DateTimePickerWrapper == null)
                throw new ArgumentException("Type mismatch", "o");

            DateTimePickerWrapper.DateTimePicker.ShowCheckBox = (bool)e.NewValue;
        }
        private static void Checked_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var DateTimePickerWrapper = o as DateTimePickerWrapper;
            if (DateTimePickerWrapper == null)
                throw new ArgumentException("Type mismatch", "o");

            var IsChecking = DateTimePickerWrapper.IsChecking;
            if (IsChecking)
                return;

            DateTimePickerWrapper.IsChecking = true;
            DateTimePickerWrapper.DateTimePicker.Checked = (bool)e.NewValue;
            DateTimePickerWrapper.IsChecking = false;
        }

        //private static void _PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        //{
        //    var DateTimePickerWrapper = o as DateTimePickerWrapper;
        //    if (DateTimePickerWrapper == null)
        //        throw new ArgumentException("Type mismatch", "o");

        //}

        #endregion

        #region Properties

        public System.Windows.Forms.DateTimePicker DateTimePicker { get { return _DateTimePicker; } }

        public DateTime Value
        {
            get { return (DateTime)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        private bool IsSettingValue
        {
            get { return (bool)GetValue(IsSettingValueProperty); }
            set { SetValue(IsSettingValueProperty, value); }
        }
        public DateTime MinDate
        {
            get { return (DateTime)GetValue(MinDateProperty); }
            set { SetValue(MinDateProperty, value); }
        }
        public DateTime MaxDate
        {
            get { return (DateTime)GetValue(MaxDateProperty); }
            set { SetValue(MaxDateProperty, value); }
        }
        public System.Windows.Forms.DateTimePickerFormat Format
        {
            get { return (System.Windows.Forms.DateTimePickerFormat)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }
        public string CustomFormat
        {
            get { return (string)GetValue(CustomFormatProperty); }
            set { SetValue(CustomFormatProperty, value); }
        }
        public bool ShowUpDown
        {
            get { return (bool)GetValue(ShowUpDownProperty); }
            set { SetValue(ShowUpDownProperty, value); }
        }

        public bool ShowCheckBox
        {
            get { return (bool)GetValue(ShowCheckBoxProperty); }
            set { SetValue(ShowCheckBoxProperty, value); }
        }
        public bool Checked
        {
            get { return (bool)GetValue(CheckedProperty); }
            set { SetValue(CheckedProperty, value); }
        }
        private bool IsChecking
        {
            get { return (bool)GetValue(IsCheckingProperty); }
            set { SetValue(IsCheckingProperty, value); }
        }

        //public  
        //{
        //    get { return ()GetValue(); }
        //    set { SetValue(, value); }
        //}

        #endregion

        #region Events

        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        #endregion

        #region Constructor

        public DateTimePickerWrapper()
        {
            InitializeComponent();

            this.DateTimePicker.ValueChanged += DateTimePicker_ValueChanged;
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!this.IsSettingValue)
            {
                this.IsSettingValue = true;
                this.Value = this.DateTimePicker.Value;
                this.IsSettingValue = false;
            }

            if (!this.IsChecking)
            {
                this.IsChecking = true;
                this.Checked = this.DateTimePicker.Checked;
                this.IsChecking = false;
            }

            this.RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
        }

        #endregion

    }
}
