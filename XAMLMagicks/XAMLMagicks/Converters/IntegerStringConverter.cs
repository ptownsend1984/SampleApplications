using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;

namespace XAMLMagicks.Converters
{
    public class IntegerStringConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Value = System.Convert.ToInt32(value);
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var Value = System.Convert.ToString(value);
                int intValue;
                if (!int.TryParse(Value, out intValue))
                    intValue = 0;
                return intValue;
            }
            else
                return 0;
        }

        #endregion

    }
}