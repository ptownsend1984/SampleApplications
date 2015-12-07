using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WPFDemo.Presentation.Converters
{
    public class AppendStringConverter : ValueConverterMarkupExtension<AppendStringConverter>
    {

        #region IValueConverter Members

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            if (parameter == null)
                return value;

            //Append the converter parameter to the end of the supplied value
            return System.Convert.ToString(value) + System.Convert.ToString(parameter);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}