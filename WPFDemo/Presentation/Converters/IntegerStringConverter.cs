using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;

namespace WPFDemo.Presentation.Converters
{
    /// <summary>
    /// For binding an integer property to a string field
    /// </summary>
    public class IntegerStringConverter : ValueConverterMarkupExtension<IntegerStringConverter>
    {

        #region IValueConverter Members

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Value = System.Convert.ToInt32(value);
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
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