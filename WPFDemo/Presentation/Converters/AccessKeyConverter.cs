using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WPFDemo.Presentation.Converters
{
    public class AccessKeyConverter : ValueConverterMarkupExtension<AccessKeyConverter>
    {

        #region IValueConverter Members

        private const string UNDERSCORE = "_";

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //Value is a string
            //Parameter is the index of the letter to turn into an access key.  Default is first letter (0).
            var Value = System.Convert.ToString(value);
            if (string.IsNullOrEmpty(Value))
                return Value;
            int AccessIndex;
            if (parameter != null && parameter is IConvertible)
                AccessIndex = System.Convert.ToInt32(parameter);
            else
                AccessIndex = 0;

            //Underscores are the equivalent to & in WPF.
            //If the access index is negative, place it at the front.
            //If the access index is past the length, place it on the last letter.
            if (AccessIndex < 0)
                AccessIndex = 0;
            else if (AccessIndex >= Value.Length)
                AccessIndex = Value.Length - 1;

            return Value.Insert(AccessIndex, UNDERSCORE);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}