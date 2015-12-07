using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WPFDemo.Presentation.Converters
{
    public class CharacterCodeConverter : ValueConverterMarkupExtension<CharacterCodeConverter>
    {

        #region IValueConverter Members

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Value = System.Convert.ToString(value);
            if (string.IsNullOrEmpty(Value))
                return string.Empty;
            int Code;
            if (!int.TryParse(Value, out Code))
                return string.Empty;
            return System.Convert.ToChar(Code).ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}