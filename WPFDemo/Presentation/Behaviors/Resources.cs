using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Markup;
using WPFDemo.Common.Extensions;
using System.Reflection;

namespace WPFDemo.Presentation.Behaviors
{
    public static class Resources
    {

        #region Static Properties

        /// <summary>
        /// Replaces the Text/Content property with a label resource entry.
        /// </summary>
        /// <remarks>
        /// Performs type checking to get the property to set. For custom user controls, apply the ContentPropertyAttribute.
        /// The resource will not appear in the designer however. Be sure to set the resource AFTER setting the text to appear in the designer.
        /// </remarks>        
        public static DependencyProperty ResourceLabelNameProperty = DependencyProperty.RegisterAttached(
            "ResourceLabelName", typeof(string), typeof(Resources),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender, ClientLabelKey_PropertyChanged)
            );

        #endregion

        #region Static Methods

        public static string GetResourceLabelName(FrameworkElement element)
        {
            return (string)element.GetValue(ResourceLabelNameProperty);
        }
        public static void SetResourceLabelName(FrameworkElement element, string Value)
        {
            element.SetValue(ResourceLabelNameProperty, Value);
        }

        private static void ClientLabelKey_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                throw new ArgumentException("Type mismatch", "o");

            bool UseAccessText = false;
            PropertyInfo ContentProperty = null;
            var ElementType = Element.GetType();
            if (ElementType.TypeIs(typeof(Window)))
            {
                ContentProperty = ElementType.GetProperty("Title", BindingFlags.Public | BindingFlags.Instance);
            }
            else if (ElementType.TypeIs(typeof(System.Windows.Controls.GroupBox)) || ElementType.TypeIs(typeof(System.Windows.Controls.TabItem)))
            {
                ContentProperty = ElementType.GetProperty("Header", BindingFlags.Public | BindingFlags.Instance);
            }
            else if (ElementType.TypeIs(typeof(System.Windows.Controls.TextBlock)))
            {
                ContentProperty = ElementType.GetProperty("Text", BindingFlags.Public | BindingFlags.Instance);
            }
            else if (ElementType.TypeIs(typeof(System.Windows.Controls.Button)))
            {
                ContentProperty = ElementType.GetProperty("Content", BindingFlags.Public | BindingFlags.Instance);

                //Swapping out button content with a style seems to ignore the 
                //built-in AccessKey functionality
                var ContentPresenter = Element.FindVisualChild<System.Windows.Controls.ContentPresenter>();
                UseAccessText = ContentPresenter != null && ContentPresenter.RecognizesAccessKey;
            }
            else if (Attribute.IsDefined(ElementType, typeof(ContentPropertyAttribute), true))
            {
                var Attr = Attribute.GetCustomAttribute(ElementType, typeof(ContentPropertyAttribute), true) as ContentPropertyAttribute;
                var PropertyInfo = ElementType.GetProperty(Attr.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (PropertyInfo != null && PropertyInfo.CanRead && PropertyInfo.CanWrite)
                {
                    ContentProperty = PropertyInfo;
                }
            }
            if (ContentProperty != null)
            {
                object CurrentValue = ContentProperty.GetValue(Element, null);
                string Content;
                if (CurrentValue != null)
                    Content = CurrentValue.ToString();
                else
                    Content = string.Empty;
                string Resource = WPFDemo.Presentation.Properties.Resources.ResourceManager.GetString(Convert.ToString(e.NewValue));
                if (string.IsNullOrEmpty(Content) || !string.IsNullOrEmpty(Resource))
                {
                    if (!UseAccessText)
                        ContentProperty.SetValue(Element, Resource, null);
                    else
                    {
                        var AccessText = new System.Windows.Controls.AccessText();
                        AccessText.Text = Resource;
                        ContentProperty.SetValue(Element, AccessText, null);
                    }
                }
            }
#if DEBUG
            else
                throw new InvalidOperationException("Cannot find text property to set on " + ElementType.Name);
#endif
        }

        #endregion

    }
}