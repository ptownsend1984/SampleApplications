using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace WPFDemo.Presentation.Controls.Button
{
    public class ImageButton : System.Windows.Controls.Button
    {

        #region Static Members

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(
            "Image", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null)
            );
        public static readonly DependencyProperty MouseOverEffectProperty =
            DependencyProperty.Register(
            "MouseOverEffect", typeof(System.Windows.Media.Effects.BitmapEffect), typeof(ImageButton), new UIPropertyMetadata(null)
            );
        public static readonly DependencyProperty TextOverlayProperty =
             DependencyProperty.Register(
            "TextOverlay", typeof(string), typeof(ImageButton), new UIPropertyMetadata(null)
            );
        public static readonly DependencyProperty TextOverlayVisibilityProperty =
             DependencyProperty.Register(
            "TextOverlayVisibility", typeof(System.Windows.Visibility), typeof(ImageButton), new UIPropertyMetadata(System.Windows.Visibility.Visible)
            );

        public static readonly DependencyProperty TextOverlayHorizontalAlignmentProperty =
             DependencyProperty.Register(
            "TextOverlayHorizontalAlignment", typeof(System.Windows.HorizontalAlignment), typeof(ImageButton), new UIPropertyMetadata(System.Windows.HorizontalAlignment.Center)
            );
        public static readonly DependencyProperty TextOverlayVerticalAlignmentProperty =
             DependencyProperty.Register(
            "TextOverlayVerticalAlignment", typeof(System.Windows.VerticalAlignment), typeof(ImageButton), new UIPropertyMetadata(System.Windows.VerticalAlignment.Center)
            );


        #endregion

        #region Properties

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public System.Windows.Media.Effects.BitmapEffect MouseOverEffect
        {
            get { return (System.Windows.Media.Effects.BitmapEffect)GetValue(MouseOverEffectProperty); }
            set { SetValue(MouseOverEffectProperty, value); }
        }
        public string TextOverlay
        {
            get { return (string)GetValue(TextOverlayProperty); }
            set { SetValue(TextOverlayProperty, value); }
        }
        public System.Windows.Visibility TextOverlayVisibility
        {
            get { return (System.Windows.Visibility)GetValue(TextOverlayVisibilityProperty); }
            set { SetValue(TextOverlayVisibilityProperty, value); }
        }
        public System.Windows.HorizontalAlignment TextOverlayHorizontalAlignment
        {
            get { return (System.Windows.HorizontalAlignment)GetValue(TextOverlayHorizontalAlignmentProperty); }
            set { SetValue(TextOverlayHorizontalAlignmentProperty, value); }
        }
        public System.Windows.VerticalAlignment TextOverlayVerticalAlignment
        {
            get { return (System.Windows.VerticalAlignment)GetValue(TextOverlayVerticalAlignmentProperty); }
            set { SetValue(TextOverlayVerticalAlignmentProperty, value); }
        }

        #endregion

        #region Constructor

        public ImageButton()
        {
        }

        #endregion

    }
}