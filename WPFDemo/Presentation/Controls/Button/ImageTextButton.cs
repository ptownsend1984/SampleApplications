using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace WPFDemo.Presentation.Controls.Button
{
    public class ImageTextButton : System.Windows.Controls.Button
    {

        #region Static Members

        static ImageTextButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageTextButton), new FrameworkPropertyMetadata(typeof(ImageTextButton)));
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(
            "Image", typeof(ImageSource), typeof(ImageTextButton), new UIPropertyMetadata(null)
            );
        public static readonly DependencyProperty ImagePlacementProperty =
            DependencyProperty.Register(
            "ImagePlacement", typeof(ControlPlacement), typeof(ImageTextButton), new UIPropertyMetadata(ControlPlacement.Left)
            );
        public static readonly DependencyProperty ImageMaxWidthProperty =
            DependencyProperty.Register(
            "ImageMaxWidth", typeof(double), typeof(ImageTextButton), new UIPropertyMetadata(double.PositiveInfinity)
            );
        public static readonly DependencyProperty ImageMaxHeightProperty =
            DependencyProperty.Register(
            "ImageMaxHeight", typeof(double), typeof(ImageTextButton), new UIPropertyMetadata(double.PositiveInfinity)
            );

        public static readonly DependencyProperty ImageMarginProperty =
            DependencyProperty.Register(
            "ImageMargin", typeof(Thickness), typeof(ImageTextButton), new UIPropertyMetadata(new Thickness(0, 0, 0, 0))
            );
        public static readonly DependencyProperty ImageHorizontalAlignmentProperty =
            DependencyProperty.Register(
            "ImageHorizontalAlignment", typeof(HorizontalAlignment), typeof(ImageTextButton), new UIPropertyMetadata(HorizontalAlignment.Center)
            );
        public static readonly DependencyProperty ImageVerticalAlignmentProperty =
            DependencyProperty.Register(
            "ImageVerticalAlignment", typeof(VerticalAlignment), typeof(ImageTextButton), new UIPropertyMetadata(VerticalAlignment.Center)
            );

        #endregion

        #region Properties

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public ControlPlacement ImagePlacement
        {
            get { return (ControlPlacement)GetValue(ImagePlacementProperty); }
            set { SetValue(ImagePlacementProperty, value); }
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

        public Thickness ImageMargin
        {
            get { return (Thickness)GetValue(ImageMarginProperty); }
            set { SetValue(ImageMarginProperty, value); }
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

        #endregion

    }
}