using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WPFDemo.Presentation.Converters
{
    public class ShortestDayOfWeekNameConverter : ValueConverterMarkupExtension<ShortestDayOfWeekNameConverter>
    {

        #region IValueConverter Members

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            DayOfWeek DayOfWeek;
            try
            {
                if (value is DayOfWeek)
                    DayOfWeek = (DayOfWeek)value;
                else
                {
                    DayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), System.Convert.ToString(value));
                }
            }
            catch (FormatException)
            {
                return null;
            }
            return culture.DateTimeFormat.GetShortestDayName(DayOfWeek);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}