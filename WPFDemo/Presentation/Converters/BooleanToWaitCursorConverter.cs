using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WPFDemo.Presentation.Converters
{
    public class BooleanToWaitCursorConverter : ValueConverterMarkupExtension<BooleanToWaitCursorConverter>
    {

        #region IValueConverter Members

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Value = (bool)value;
            if (Value)
                return System.Windows.Input.Cursors.Wait;
            else
                return System.Windows.Input.Cursors.Arrow;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}