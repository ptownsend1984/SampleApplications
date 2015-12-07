using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace WPFDemo.Presentation.Converters
{
    public class ByteArrayImageSourceConverter : ValueConverterMarkupExtension<ByteArrayImageSourceConverter>
    {

        #region IValueConverter Members

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Bytes = value as byte[];
            if (Bytes == null || Bytes.Length == 0)
                return null;

            try
            {
                ImageSource Image;
                var Stream = new System.IO.MemoryStream(Bytes);
                var Decoder = BitmapDecoder.Create(Stream, BitmapCreateOptions.None, BitmapCacheOption.Default);
                if (Decoder.Frames.Count == 0)
                    return null;
                Image = Decoder.Frames[0];
                if (Image.CanFreeze && !Image.IsFrozen)
                    Image.Freeze();
                return Image;
            }
            catch { return null; }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}