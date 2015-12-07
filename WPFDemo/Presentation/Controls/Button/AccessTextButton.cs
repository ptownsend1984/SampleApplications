using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFDemo.Presentation.Controls.Button
{
    public class AccessTextButton : System.Windows.Controls.Button
    {

        #region Static Members

        static AccessTextButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AccessTextButton), new FrameworkPropertyMetadata(typeof(AccessTextButton)));
        }

        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register(
            "TextWrapping", typeof(TextWrapping), typeof(AccessTextButton), new UIPropertyMetadata(TextWrapping.NoWrap)
            );
        public static readonly DependencyProperty TextTrimmingProperty =
            DependencyProperty.Register(
            "TextTrimming", typeof(TextTrimming), typeof(AccessTextButton), new UIPropertyMetadata(TextTrimming.None)
            );
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(
            "TextAlignment", typeof(TextAlignment), typeof(AccessTextButton), new UIPropertyMetadata(TextAlignment.Left)
            );

        #endregion

        #region Properties

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }
        public TextTrimming TextTrimming
        {
            get { return (TextTrimming)GetValue(TextTrimmingProperty); }
            set { SetValue(TextTrimmingProperty, value); }
        }
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        #endregion

    }
}