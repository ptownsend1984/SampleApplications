using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Media;

namespace WPFDemo.Presentation.Converters
{
    public class ImageBoundsConverter : ValueConverterMarkupExtension<ImageBoundsConverter>
    {

        #region IValueConverter Members

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Image = value as ImageSource;
            if (Image == null || Image.Width < 0.001d || Image.Height < 0.001d)
                return new System.Windows.Rect(0d, 0d, 1d, 1d);
            return new System.Windows.Rect(0d, 0d, Image.Width, Image.Height);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}