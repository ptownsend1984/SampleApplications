using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace WPFDemo.Presentation.Controls.Button
{
    public class ImageRadioButton : System.Windows.Controls.RadioButton
    {

        #region Static Members

        static ImageRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageRadioButton), new FrameworkPropertyMetadata(typeof(ImageRadioButton)));
        }

        public static readonly DependencyProperty ContentLocationProperty =
            DependencyProperty.Register(
            "ContentLocation", typeof(ControlPlacement), typeof(ImageRadioButton), new UIPropertyMetadata(ControlPlacement.Bottom)
            );

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(
            "Image", typeof(ImageSource), typeof(ImageRadioButton), new UIPropertyMetadata(null)
            );
        public static readonly DependencyProperty ImageMarginProperty =
            DependencyProperty.Register(
            "ImageMargin", typeof(Thickness), typeof(ImageRadioButton), new UIPropertyMetadata(new Thickness(0, 0, 0, 0))
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
            "ImageHorizontalAlignment", typeof(HorizontalAlignment), typeof(ImageRadioButton), new UIPropertyMetadata(HorizontalAlignment.Center)
            );
        public static readonly DependencyProperty ImageVerticalAlignmentProperty =
            DependencyProperty.Register(
            "ImageVerticalAlignment", typeof(VerticalAlignment), typeof(ImageRadioButton), new UIPropertyMetadata(VerticalAlignment.Stretch)
            );

        public static readonly DependencyProperty MouseOverEffectProperty =
            DependencyProperty.Register(
            "MouseOverEffect", typeof(System.Windows.Media.Effects.BitmapEffect), typeof(ImageRadioButton), new UIPropertyMetadata(null)
            );

        public static readonly DependencyProperty IsCheckedDisplayModeProperty =
            DependencyProperty.Register(
            "IsCheckedDisplayMode", typeof(IsCheckedDisplayMode), typeof(ImageRadioButton), new UIPropertyMetadata(IsCheckedDisplayMode.IsCheckedBackground)
            );
        public static readonly DependencyProperty IsCheckedBackgroundProperty =
            DependencyProperty.Register(
            "IsCheckedBackground", typeof(System.Windows.Media.Brush), typeof(ImageRadioButton), new UIPropertyMetadata(null)
            );
        public static readonly DependencyProperty IsCheckedImageProperty =
            DependencyProperty.Register(
            "IsCheckedImage", typeof(ImageSource), typeof(ImageRadioButton), new UIPropertyMetadata(null)
            );

        #endregion

        #region Properties

        public ControlPlacement ContentLocation
        {
            get { return (ControlPlacement)GetValue(ContentLocationProperty); }
            set { SetValue(ContentLocationProperty, value); }
        }

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public Thickness ImageMargin
        {
            get { return (Thickness)GetValue(ImageMarginProperty); }
            set { SetValue(ImageMarginProperty, value); }
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
        public HorizontalAlignment ImageHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(ImageHorizontalAlignmentProperty); }
            set { SetValue(ImageHorizontalAlignmentProperty, value); }
        }
        public VerticalAlignment ImageVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(ImageVerticalAlignmentProperty); }
            set { SetValue(ImageVerticalAlignmentProperty, value); }
        }

        public System.Windows.Media.Effects.BitmapEffect MouseOverEffect
        {
            get { return (System.Windows.Media.Effects.BitmapEffect)GetValue(MouseOverEffectProperty); }
            set { SetValue(MouseOverEffectProperty, value); }
        }

        public IsCheckedDisplayMode IsCheckedDisplayMode
        {
            get { return (IsCheckedDisplayMode)GetValue(IsCheckedDisplayModeProperty); }
            set { SetValue(IsCheckedDisplayModeProperty, value); }
        }
        public System.Windows.Media.Brush IsCheckedBackground
        {
            get { return (System.Windows.Media.Brush)GetValue(IsCheckedBackgroundProperty); }
            set { SetValue(IsCheckedBackgroundProperty, value); }
        }
        public ImageSource IsCheckedImage
        {
            get { return (ImageSource)GetValue(IsCheckedImageProperty); }
            set { SetValue(IsCheckedImageProperty, value); }
        }

        #endregion

    }

    [Flags]
    public enum IsCheckedDisplayMode
    {
        IsCheckedBackground = 1,
        IsCheckedImage = 2,
    }
}