using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFDemo.Presentation.Controls.CheckBox
{
    public class AccessTextCheckBox : System.Windows.Controls.CheckBox
    {

        #region Static Members

        static AccessTextCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AccessTextCheckBox), new FrameworkPropertyMetadata(typeof(AccessTextCheckBox)));
        }

        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register(
            "TextWrapping", typeof(TextWrapping), typeof(AccessTextCheckBox), new UIPropertyMetadata(TextWrapping.NoWrap)
            );
        public static readonly DependencyProperty TextTrimmingProperty =
            DependencyProperty.Register(
            "TextTrimming", typeof(TextTrimming), typeof(AccessTextCheckBox), new UIPropertyMetadata(TextTrimming.None)
            );
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(
            "TextAlignment", typeof(TextAlignment), typeof(AccessTextCheckBox), new UIPropertyMetadata(TextAlignment.Left)
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