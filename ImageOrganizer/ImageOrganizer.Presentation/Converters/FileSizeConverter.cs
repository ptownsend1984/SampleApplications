using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;

namespace ImageOrganizer.Presentation.Converters
{
    public class FileSizeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Value = System.Convert.ToInt64(value);
            if (Value < 0x400)
                return Value.ToString(CultureInfo.InvariantCulture) + " bytes";
            else if (Value < 0x100000)
            {
                var KB = Value / 0x400;
                var Remainer = Value % 0x400;
                return Math.Round((double)KB + (double)Remainer / (double)0x400, 2).ToString(CultureInfo.InvariantCulture) + " KB";
            }
            else
            {
                var MB = Value / 0x100000;
                var Remainer = Value % 0x100000;
                return Math.Round((double)MB + (double)Remainer / (double)0x100000, 2).ToString(CultureInfo.InvariantCulture) + " MB";                
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}