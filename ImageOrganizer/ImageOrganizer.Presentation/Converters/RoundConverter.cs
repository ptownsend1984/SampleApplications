using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;

namespace ImageOrganizer.Presentation.Converters
{
    public class RoundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int Parameter;
            if (parameter == null || !int.TryParse(System.Convert.ToString(parameter), out Parameter))
                Parameter = 2;

            return Math.Round(System.Convert.ToDouble(value), Parameter).ToString("f2", CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}