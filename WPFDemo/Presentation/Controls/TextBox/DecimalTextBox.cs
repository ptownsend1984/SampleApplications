using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Text.RegularExpressions;
using System.Globalization;
using WPFDemo.Common.Extensions;

namespace WPFDemo.Presentation.Controls.TextBox
{
    public class DecimalTextBox : IntegerTextBox
    {

        #region Static Members

        public static readonly DependencyProperty AutoFillDecimalPlacesProperty = DependencyProperty.Register(
            "AutoFillDecimalPlaces", typeof(bool), typeof(DecimalTextBox),
            new PropertyMetadata(true, AutoFillDecimalPlaces_PropertyChanged));

        public static readonly DependencyProperty PrecisionProperty = DependencyProperty.Register(
            "Precision", typeof(uint), typeof(DecimalTextBox),
            new PropertyMetadata((uint)15, Precision_PropertyChanged));
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", typeof(uint), typeof(DecimalTextBox),
            new PropertyMetadata((uint)5, Scale_PropertyChanged));

        private static void AutoFillDecimalPlaces_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Sender = o as DecimalTextBox;
            if (Sender == null)
                return;

            var NewValue = (bool)e.NewValue;
            if (!NewValue && Sender.SessionTextChanged)
                Sender.SessionTextChanged = false;
        }

        private static void Precision_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var NewValue = (uint)e.NewValue;
            if (NewValue > Structs.MAXPRECISION)
                throw new ArgumentException(string.Format("Precision must be less than {0} due to decimal data type constraints.", Structs.MAXPRECISION + 1), "precision");
        }
        private static void Scale_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var NewValue = (uint)e.NewValue;
            if (NewValue > Structs.MAXPRECISION)
                throw new ArgumentException(string.Format("Scale must be less than {0} due to decimal data type constraints.", Structs.MAXPRECISION + 1), "scale");
        }

        #endregion

        #region Properties

        public bool AutoFillDecimalPlaces
        {
            get { return (bool)GetValue(AutoFillDecimalPlacesProperty); }
            set { SetValue(AutoFillDecimalPlacesProperty, value); }
        }

        public uint Precision
        {
            get { return (uint)GetValue(PrecisionProperty); }
            set { SetValue(PrecisionProperty, value); }
        }
        public uint Scale
        {
            get { return (uint)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        #endregion

        #region Methods

        private bool SessionTextChanged;
        protected override void OnTextChanged(System.Windows.Controls.TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (!this.IsFocused || SessionTextChanged || !this.AutoFillDecimalPlaces)
                return;

            SessionTextChanged = true;
        }
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (SessionTextChanged)
                SessionTextChanged = false;
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            //Perform AutoFillDecimalPlaces before base.OnLostFocus()
            //to update the text before the LostFocus binding update occurs.
            if (SessionTextChanged)
            {
                SessionTextChanged = false;

                var Scale = this.Scale;
                var Text = this.Text;
                if (Scale == (uint)0 || string.IsNullOrEmpty(Text))
                {
                    this.Text = ToPrecisionString(decimal.Zero, Scale);

                    //WPF thing: update the binding source, if any
                    this.UpdateBindingExpressionSource(TextProperty);
                    return;
                }

                decimal Temp;
                if (!this.TryGetDecimalValue(Text, out Temp))
                    return;

                //CheckTextFormat forces Precision >= ValuePrecision and Scale >= ValueScale
                this.Text = ToPrecisionString(Temp, Scale);

                //WPF thing: update the binding source, if any
                this.UpdateBindingExpressionSource(TextProperty);
            }

            base.OnLostFocus(e);
        }

        protected override bool CheckTextFormat(string Value)
        {
            //Entire text is being replaced
            if (this.SelectionLength == this.TextLength)
            {
                if (!string.IsNullOrEmpty(Value))
                {
                    if (Value.Length == 1 && (Value[0] == '-' || Value[0] == '.'))
                        return true;
                }
                return IsValidDecimalFormat(Value);
            }

            //Replace selection with Value
            string EntireText = this.Text;
            if (this.SelectionLength > 0)
            {
                EntireText = EntireText.Remove(this.SelectionStart, this.SelectionLength);
            }
            EntireText = EntireText.Insert(Math.Min(this.SelectionStart, EntireText.Length), Value);
            return IsValidDecimalFormat(EntireText);
        }

        protected virtual bool IsValidDecimalFormat(string Value)
        {
            decimal Temp;
            if (!this.TryGetDecimalValue(Value, out Temp))
                return false;

            //Check the precision and scale.
            //Note that is isn't really intended to be changed on the fly.
            //Otherwise coercing the text value may be in order.
            var Precision = this.Precision;
            var Scale = this.Scale;

            if (Precision == 0 && Scale == 0)
                return true;
            return Temp.IsValidPrecision(Precision, Scale);
        }

        /// <summary>
        /// Execute an overload of Decimal.TryParse for this class type.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual bool TryGetDecimalValue(string text, out decimal value)
        {
            return decimal.TryParse(text, out value);
        }
        /// <summary>
        /// Returns the precision string for this class type.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        protected virtual string ToPrecisionString(decimal value, uint scale)
        {
            return value.ToPrecisionString(scale);
        }

        #endregion

    }
}