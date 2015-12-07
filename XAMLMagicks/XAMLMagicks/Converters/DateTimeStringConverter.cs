using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;

namespace XAMLMagicks.Converters
{
    public class DateTimeStringConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var Format = System.Convert.ToString(parameter);
                var Value = System.Convert.ToDateTime(value);
                return Value.ToString(Format, CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var Value = System.Convert.ToString(value);
                return DateTime.Parse(Value, CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
                return DateTime.Now;
            }
        }

        #endregion

    }
}