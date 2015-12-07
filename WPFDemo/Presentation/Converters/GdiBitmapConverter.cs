using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using WPFDemo.Common.Extensions;

namespace WPFDemo.Presentation.Converters
{
    public class GdiBitmapConverter : ValueConverterMarkupExtension<GdiBitmapConverter>
    {

        #region IValueConverter Members

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Value = value as System.Drawing.Bitmap;
            if (Value == null)
                return null;
            return Value.ToBitmapSource();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}