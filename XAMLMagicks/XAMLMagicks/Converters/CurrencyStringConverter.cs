using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;

namespace XAMLMagicks.Converters
{
    public class CurrencyStringConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal DecValue = System.Convert.ToDecimal(value);
            return DecValue.ToString("c2");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string Value = value.ToString();
                decimal DecValue;
                if (!decimal.TryParse(Value, System.Globalization.NumberStyles.Currency, CultureInfo.CurrentCulture, out DecValue))
                    DecValue = decimal.Zero;
                return DecValue;
            }
            else
                return decimal.Zero;
        }

        #endregion
    }
}