using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;

namespace WPFDemo.Presentation.Converters
{
    /// <summary>
    /// Converter wrapper to be able to bind a value to a converter parameter which does not change.  Inspired by http://stackoverflow.com/a/8858815.
    /// </summary>
    /// <remarks>Cannot be a markup extension since it's a dependency object.</remarks>
    public class BindableParameterConverter : Freezable, IValueConverter
    {

        #region Static Members

        public readonly static DependencyProperty ConverterProperty = DependencyProperty.Register(
            "Converter", typeof(IValueConverter), typeof(BindableParameterConverter), new FrameworkPropertyMetadata(null));
        public readonly static DependencyProperty ConverterParameterProperty = DependencyProperty.Register(
            "ConverterParameter", typeof(object), typeof(BindableParameterConverter), new FrameworkPropertyMetadata(null));

        #endregion

        #region Properties

        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(ConverterProperty); }
            set { SetValue(ConverterProperty, value); }
        }
        public object ConverterParameter
        {
            get { return GetValue(ConverterParameterProperty); }
            set { SetValue(ConverterParameterProperty, value); }
        }

        #endregion

        #region Methods

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Converter = this.Converter;
            if (Converter == null)
                return DependencyProperty.UnsetValue;

            return Converter.Convert(value, targetType, this.ConverterParameter, culture);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Converter = this.Converter;
            if (Converter == null)
                return DependencyProperty.UnsetValue;

            return Converter.ConvertBack(value, targetType, this.ConverterParameter, culture);
        }

        protected override Freezable CreateInstanceCore()
        {
            return new BindableParameterConverter();
        }

        #endregion

    }
}