using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WPFDemo.Presentation.Converters
{
    public class BooleanMultiConverter : ValueMultiConverterMarkupExtension<BooleanMultiConverter>
    {

        #region IMultiValueConverter Members

        public override object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length == 0)
                return false;

            //Returns false if any one value converts to false
            //or if something unconvertible comes in.
            foreach (object Value in values)
            {
                if (Value is IConvertible)
                {
                    if (!System.Convert.ToBoolean(Value))
                        return false;
                }
                else
                    return false;
            }
            return true;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            var Values = new object[targetTypes.Length];
            for (int i = 0; i < Values.Length; i++)
                Values[i] = (bool)value;
            return Values;
        }

        #endregion

    }
}