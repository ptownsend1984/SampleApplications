using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WPFDemo.Presentation.Converters
{
    public class ReverseBooleanToVisibilityLayoutConverter : ValueConverterMarkupExtension<ReverseBooleanToVisibilityLayoutConverter>
    {

        #region IValueConverter Members

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Value = (bool)value;
            if (Value)
                return System.Windows.Visibility.Hidden;
            else
                return System.Windows.Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Value = (System.Windows.Visibility)value;
            return Value == System.Windows.Visibility.Hidden;
        }

        #endregion

    }
}