using System;
using System.Globalization;

namespace WPFDemo.Presentation.Converters
{
    /// <summary>
    /// For binding a decimal property to a string field
    /// </summary>
    public class DecimalStringConverter : ValueConverterMarkupExtension<DecimalStringConverter>
    {

        #region IValueConverter Members

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal DecValue = System.Convert.ToDecimal(value);
            return DecValue.ToString(CultureInfo.CurrentCulture);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string Value = value.ToString();
                decimal DecValue;
                if (!decimal.TryParse(Value, out DecValue))
                    DecValue = decimal.Zero;
                return DecValue;
            }
            else
                return decimal.Zero;
        }

        #endregion
    }
}