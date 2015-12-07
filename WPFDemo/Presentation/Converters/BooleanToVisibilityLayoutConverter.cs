using System;

namespace WPFDemo.Presentation.Converters
{
    public class BooleanToVisibilityLayoutConverter : ValueConverterMarkupExtension<BooleanToVisibilityLayoutConverter>
    {
        #region IValueConverter Members

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Value = (bool)value;
            if (Value)
                return System.Windows.Visibility.Visible;
            else
                return System.Windows.Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Value = (System.Windows.Visibility)value;
            return Value == System.Windows.Visibility.Visible;
        }

        #endregion
    }
}