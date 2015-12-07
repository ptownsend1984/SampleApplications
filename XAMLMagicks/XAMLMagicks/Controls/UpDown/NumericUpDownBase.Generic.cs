using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using XAMLMagicks.Controls.Spinner;

namespace XAMLMagicks.Controls.UpDown
{
    /// <summary>
    /// Generic base class for a numeric UpDown control.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [TemplatePart(Name = UpDownBase.PART_TextBox, Type = typeof(System.Windows.Controls.Primitives.TextBoxBase))]
    [TemplatePart(Name = UpDownBase.PART_Spinner, Type = typeof(SpinnerBase))]
    public abstract class NumericUpDownBase<T> : UpDownBase where T : struct, IConvertible
    {

        #region Static Members

        /// <summary>
        /// Property to set the value of the control.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(T), typeof(NumericUpDownBase<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueProperty_PropertyChanged, ValueProperty_CoerceValue)
            );
        /// <summary>
        /// Property to set the amount to increment when pressing the up and down buttons.
        /// </summary>
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
            "Increment", typeof(T), typeof(NumericUpDownBase<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );
        /// <summary>
        /// Property to set the maximum value of the control.
        /// </summary>
        public static readonly DependencyProperty MaximumValueProperty = DependencyProperty.Register(
            "MaximumValue", typeof(T), typeof(NumericUpDownBase<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, MaximumValueProperty_PropertyChanged, MaximumValueProperty_CoerceMaximumValue)
            );
        /// <summary>
        /// Property to set the minimum value of the control.
        /// </summary>
        public static readonly DependencyProperty MinimumValueProperty = DependencyProperty.Register(
            "MinimumValue", typeof(T), typeof(NumericUpDownBase<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, MinimumValueProperty_PropertyChanged, MinimumValueProperty_CoerceMinimumValue)
            );

        /// <summary>
        /// Property to control whether to select all the text box text when it receives focus.
        /// </summary>
        public static readonly DependencyProperty SelectAllOnFocusProperty = DependencyProperty.Register(
            "SelectAllOnFocus", typeof(bool), typeof(NumericUpDownBase<T>), new UIPropertyMetadata(true)
            );

        private static void ValueProperty_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var UpDown = o as NumericUpDownBase<T>;
            if (UpDown == null)
                return;

            UpDown.OnValueChanged(e);
        }
        private static object ValueProperty_CoerceValue(DependencyObject o, object baseValue)
        {
            var UpDown = o as NumericUpDownBase<T>;
            if (UpDown == null)
                return null;

            return UpDown.CoerceValue((T)baseValue);
        }

        private static void MaximumValueProperty_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var UpDown = o as NumericUpDownBase<T>;
            if (UpDown == null)
                return;

            UpDown.OnMaximumValueChanged(e);
        }
        private static object MaximumValueProperty_CoerceMaximumValue(DependencyObject o, object baseValue)
        {
            var UpDown = o as NumericUpDownBase<T>;
            if (UpDown == null)
                return null;

            return UpDown.CoerceMaximumValue((T)baseValue);
        }
        private static void MinimumValueProperty_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var UpDown = o as NumericUpDownBase<T>;
            if (UpDown == null)
                return;

            UpDown.OnMinimumValueChanged(e);
        }
        private static object MinimumValueProperty_CoerceMinimumValue(DependencyObject o, object baseValue)
        {
            var UpDown = o as NumericUpDownBase<T>;
            if (UpDown == null)
                return null;

            return UpDown.CoerceMinimumValue((T)baseValue);
        }

        #endregion

        #region Global Variables

        private readonly DelegateCommand DefaultUpCommand;
        private readonly DelegateCommand DefaultDownCommand;

        private System.Windows.Controls.Primitives.TextBoxBase _TextBox;
        private SpinnerBase _Spinner;

        #endregion

        #region Properties

        public T Value
        {
            get { return (T)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public T Increment
        {
            get { return (T)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }
        public T MaximumValue
        {
            get { return (T)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }
        public T MinimumValue
        {
            get { return (T)GetValue(MinimumValueProperty); }
            set { SetValue(MinimumValueProperty, value); }
        }

        public bool SelectAllOnFocus
        {
            get { return (bool)GetValue(SelectAllOnFocusProperty); }
            set { SetValue(SelectAllOnFocusProperty, value); }
        }

        public System.Windows.Controls.Primitives.TextBoxBase TextBox
        {
            get { return _TextBox; }
            set
            {
                if (_TextBox != null)
                {
                    _TextBox.GotFocus -= TextBox_GotFocus;
                    _TextBox.PreviewKeyDown -= TextBox_PreviewKeyDown;
                }
                _TextBox = value;
                if (_TextBox != null)
                {
                    _TextBox.GotFocus += TextBox_GotFocus;
                    _TextBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                }
            }
        }

        public SpinnerBase Spinner
        {
            get { return _Spinner; }
            private set
            {
                if (_Spinner != null)
                {
                    _Spinner.UpCommand = null;
                    _Spinner.DownCommand = null;
                }
                _Spinner = value;
                if (_Spinner != null)
                {
                    _Spinner.UpCommand = this.DefaultUpCommand;
                    _Spinner.DownCommand = this.DefaultDownCommand;
                }
            }
        }

        #endregion

        #region Constructor

        protected NumericUpDownBase()
        {
            this.DefaultUpCommand = new DelegateCommand(DoDefaultUpCommand, CanDoDefaultUpCommand);
            this.DefaultDownCommand = new DelegateCommand(DoDefaultDownCommand, CanDoDefaultDownCommand);
        }

        #endregion

        #region Event Handlers

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var TextBox = sender as System.Windows.Controls.Primitives.TextBoxBase;
            if (TextBox == null)
                return;
            if (!this.SelectAllOnFocus)
                return;
            TextBox.SelectAll();
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
                return;

            switch (e.Key)
            {
                case Key.Up:
                    if (this.CanDoDefaultUpCommand())
                    {
                        this.DoIncrement();
                        e.Handled = true;
                    }
                    break;
                case Key.Down:
                    if (this.CanDoDefaultDownCommand())
                    {
                        this.DoDecrement();
                        e.Handled = true;
                    }
                    break;
            }
        }

        #endregion

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.TextBox = this.GetTemplateChild(PART_TextBox) as System.Windows.Controls.Primitives.TextBoxBase;
            this.Spinner = this.GetTemplateChild(PART_Spinner) as SpinnerBase;
        }
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            var TextBox = this.TextBox;
            if (TextBox != null)
                TextBox.Focus();
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Handled)
                return;

            if (e.Delta < 0 && this.CanDoDefaultDownCommand())
            {
                this.DoDecrement();
                e.Handled = true;
            }
            else if (e.Delta > 0 && this.CanDoDefaultUpCommand())
            {
                this.DoIncrement();
                e.Handled = true;
            }
        }

        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            this.DefaultUpCommand.RaiseCanExecuteChanged();
            this.DefaultDownCommand.RaiseCanExecuteChanged();
        }
        protected abstract T CoerceValue(T BaseValue);

        protected virtual void OnMaximumValueChanged(DependencyPropertyChangedEventArgs e) { }
        protected abstract T CoerceMaximumValue(T BaseValue);
        protected virtual void OnMinimumValueChanged(DependencyPropertyChangedEventArgs e) { }
        protected abstract T CoerceMinimumValue(T BaseValue);

        protected virtual void DoIncrement() { }
        protected virtual void DoDecrement() { }

        private void DoDefaultUpCommand()
        {
            if (!CanDoDefaultUpCommand())
                return;

            DoIncrement();
        }
        protected virtual bool CanDoDefaultUpCommand()
        {
            return !this.IsReadOnly;
        }

        private void DoDefaultDownCommand()
        {
            if (!CanDoDefaultDownCommand())
                return;

            DoDecrement();
        }
        protected virtual bool CanDoDefaultDownCommand()
        {
            return !this.IsReadOnly;
        }

        protected override void OnIsReadOnlyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsReadOnlyChanged(e);

            this.DefaultUpCommand.RaiseCanExecuteChanged();
            this.DefaultDownCommand.RaiseCanExecuteChanged();
        }

        #endregion

    }
}