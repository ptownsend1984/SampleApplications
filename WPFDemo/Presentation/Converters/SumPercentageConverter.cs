using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WPFDemo.Presentation.Converters
{
    public class SumPercentageConverter : ValueMultiConverterMarkupExtension<SumPercentageConverter>
    {

        #region IMultiValueConverter Members

        public override object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //Takes a series of values.
            //The last is the total.
            //The rest are summed together to be divided into the total, and a percentage formatted string is returned.
            //Ex: {12, 14, 100} => (12 + 14) / 100 => 26%

            //If nothing provided, return 0%
            if (values == null || !values.Any() || !values.All(o => o is IConvertible))
                return (0d).ToString("p");

            //If the last number is almost zero, return 0%
            var Total = System.Convert.ToDouble(values.Last());
            if (Math.Abs(Total) < 0.0001d)
                return (0d).ToString("p");

            //Add the first n-1 numbers
            var Sum = 0d;
            for (int i = 0; i < values.Length - 1; i++)
                Sum += System.Convert.ToDouble(values[i]);

            //If the sum and total are almost equal, return 100%
            if (Math.Abs(Sum - Total) < 0.0001d)
                return (1d).ToString("p");

            //Return the percentage
            return (Sum / Total).ToString("p");
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion

    }
}