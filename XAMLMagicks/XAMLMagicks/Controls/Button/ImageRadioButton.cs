using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace XAMLMagicks.Controls.Button
{
    public class ImageRadioButton : System.Windows.Controls.RadioButton
    {

        #region Static Members

        static ImageRadioButton()
        {
            // Enable theming
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageRadioButton), new FrameworkPropertyMetadata(typeof(ImageRadioButton)));
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(
            "Image", typeof(ImageSource), typeof(ImageRadioButton), new UIPropertyMetadata(null)
            );
        public static readonly DependencyProperty ImageMaxWidthProperty =
            DependencyProperty.Register(
            "ImageMaxWidth", typeof(double), typeof(ImageRadioButton), new UIPropertyMetadata(double.PositiveInfinity)
            );
        public static readonly DependencyProperty ImageMaxHeightProperty =
            DependencyProperty.Register(
            "ImageMaxHeight", typeof(double), typeof(ImageRadioButton), new UIPropertyMetadata(double.PositiveInfinity)
            );
        public static readonly DependencyProperty ImageHorizontalAlignmentProperty =
             DependencyProperty.Register(
            "ImageHorizontalAlignment", typeof(System.Windows.HorizontalAlignment), typeof(ImageRadioButton), new UIPropertyMetadata(System.Windows.HorizontalAlignment.Stretch)
            );
        public static readonly DependencyProperty ImageVerticalAlignmentProperty =
             DependencyProperty.Register(
            "ImageVerticalAlignment", typeof(System.Windows.VerticalAlignment), typeof(ImageRadioButton), new UIPropertyMetadata(System.Windows.VerticalAlignment.Stretch)
            );
        public static readonly DependencyProperty MouseOverEffectProperty =
            DependencyProperty.Register(
            "MouseOverEffect", typeof(System.Windows.Media.Effects.BitmapEffect), typeof(ImageRadioButton), new UIPropertyMetadata(null)
            );
        public static readonly DependencyProperty IsCheckedBackgroundProperty =
            DependencyProperty.Register(
            "IsCheckedBackground", typeof(System.Windows.Media.Brush), typeof(ImageRadioButton), new UIPropertyMetadata(SystemColors.ControlDarkBrush)
            );
        public static readonly DependencyProperty TextOverlayProperty =
             DependencyProperty.Register(
            "TextOverlay", typeof(string), typeof(ImageRadioButton), new UIPropertyMetadata(null)
            );
        public static readonly DependencyProperty TextOverlayVisibilityProperty =
             DependencyProperty.Register(
            "TextOverlayVisibility", typeof(System.Windows.Visibility), typeof(ImageRadioButton), new UIPropertyMetadata(System.Windows.Visibility.Visible)
            );

        public static readonly DependencyProperty TextOverlayHorizontalAlignmentProperty =
             DependencyProperty.Register(
            "TextOverlayHorizontalAlignment", typeof(System.Windows.HorizontalAlignment), typeof(ImageRadioButton), new UIPropertyMetadata(System.Windows.HorizontalAlignment.Center)
            );
        public static readonly DependencyProperty TextOverlayVerticalAlignmentProperty =
             DependencyProperty.Register(
            "TextOverlayVerticalAlignment", typeof(System.Windows.VerticalAlignment), typeof(ImageRadioButton), new UIPropertyMetadata(System.Windows.VerticalAlignment.Center)
            );

        public static readonly DependencyProperty BottomTextProperty =
             DependencyProperty.Register(
            "BottomText", typeof(string), typeof(ImageRadioButton), new UIPropertyMetadata(null)
            );
        public static readonly DependencyProperty BottomTextVisibilityProperty =
             DependencyProperty.Register(
            "BottomTextVisibility", typeof(System.Windows.Visibility), typeof(ImageRadioButton), new UIPropertyMetadata(System.Windows.Visibility.Visible)
            );
        public static readonly DependencyProperty BottomTextMarginProperty =
             DependencyProperty.Register(
            "BottomTextMargin", typeof(System.Windows.Thickness), typeof(ImageRadioButton), new UIPropertyMetadata(new System.Windows.Thickness(0))
            );

        public static readonly DependencyProperty BottomTextHorizontalAlignmentProperty =
             DependencyProperty.Register(
            "BottomTextHorizontalAlignment", typeof(System.Windows.HorizontalAlignment), typeof(ImageRadioButton), new UIPropertyMetadata(System.Windows.HorizontalAlignment.Center)
            );
        public static readonly DependencyProperty BottomTextVerticalAlignmentProperty =
             DependencyProperty.Register(
            "BottomTextVerticalAlignment", typeof(System.Windows.VerticalAlignment), typeof(ImageRadioButton), new UIPropertyMetadata(System.Windows.VerticalAlignment.Center)
            );


        #endregion

        #region Properties

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public double ImageMaxWidth
        {
            get { return (double)GetValue(ImageMaxWidthProperty); }
            set { SetValue(ImageMaxWidthProperty, value); }
        }
        public double ImageMaxHeight
        {
            get { return (double)GetValue(ImageMaxHeightProperty); }
            set { SetValue(ImageMaxHeightProperty, value); }
        }
        public System.Windows.HorizontalAlignment ImageHorizontalAlignment
        {
            get { return (System.Windows.HorizontalAlignment)GetValue(ImageHorizontalAlignmentProperty); }
            set { SetValue(ImageHorizontalAlignmentProperty, value); }
        }
        public System.Windows.VerticalAlignment ImageVerticalAlignment
        {
            get { return (System.Windows.VerticalAlignment)GetValue(ImageVerticalAlignmentProperty); }
            set { SetValue(ImageVerticalAlignmentProperty, value); }
        }
        public System.Windows.Media.Effects.BitmapEffect MouseOverEffect
        {
            get { return (System.Windows.Media.Effects.BitmapEffect)GetValue(MouseOverEffectProperty); }
            set { SetValue(MouseOverEffectProperty, value); }
        }
        public System.Windows.Media.Brush IsCheckedBackground
        {
            get { return (System.Windows.Media.Brush)GetValue(IsCheckedBackgroundProperty); }
            set { SetValue(IsCheckedBackgroundProperty, value); }
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

        public string BottomText
        {
            get { return (string)GetValue(BottomTextProperty); }
            set { SetValue(BottomTextProperty, value); }
        }
        public System.Windows.Visibility BottomTextVisibility
        {
            get { return (System.Windows.Visibility)GetValue(BottomTextVisibilityProperty); }
            set { SetValue(BottomTextVisibilityProperty, value); }
        }
        public System.Windows.Thickness BottomTextMargin
        {
            get { return (System.Windows.Thickness)GetValue(BottomTextMarginProperty); }
            set { SetValue(BottomTextMarginProperty, value); }
        }
        public System.Windows.HorizontalAlignment BottomTextHorizontalAlignment
        {
            get { return (System.Windows.HorizontalAlignment)GetValue(BottomTextHorizontalAlignmentProperty); }
            set { SetValue(BottomTextHorizontalAlignmentProperty, value); }
        }
        public System.Windows.VerticalAlignment BottomTextVerticalAlignment
        {
            get { return (System.Windows.VerticalAlignment)GetValue(BottomTextVerticalAlignmentProperty); }
            set { SetValue(BottomTextVerticalAlignmentProperty, value); }
        }

        #endregion

        #region Constructor

        public ImageRadioButton()
        {
        }

        #endregion

    }
}