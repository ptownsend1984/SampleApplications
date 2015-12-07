using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPFDemo.Presentation.Converters
{
    /// <summary>
    /// Provides a generic base class to use IValueConverter implementations directly in the XAML markup
    /// instead of declaring a static resource.
    /// <para>The converter returned is a static instance of the type.</para>
    /// <para>Usage: {Binding ...., Converter={xmlnamespace:YourConverterClassName}}</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValueConverterMarkupExtension<T> : MarkupExtension, IValueConverter where T : class, IValueConverter, new()
    {
        #region MarkupExtension Overrides

        private static T _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
            {
                _converter = new T();
            }
            return _converter;
        }

        #endregion MarkupExtension Overrides

        #region IValueConverter Members

        public abstract object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);

        #endregion
    }
}
