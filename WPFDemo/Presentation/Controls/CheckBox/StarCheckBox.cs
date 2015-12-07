using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFDemo.Presentation.Controls.CheckBox
{
    public class StarCheckBox : System.Windows.Controls.CheckBox
    {

        #region Static Members

        static StarCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StarCheckBox), new FrameworkPropertyMetadata(typeof(StarCheckBox)));
        }

        public static readonly DependencyProperty StarMaxWidthProperty =
            DependencyProperty.Register(
            "StarMaxWidth", typeof(double), typeof(StarCheckBox), new UIPropertyMetadata(0d)
            );
        public static readonly DependencyProperty StarMaxHeightProperty =
            DependencyProperty.Register(
            "StarMaxHeight", typeof(double), typeof(StarCheckBox), new UIPropertyMetadata(0d)
            );

        #endregion

        #region Properties

        public double StarMaxWidth
        {
            get { return (double)GetValue(StarMaxWidthProperty); }
            set { SetValue(StarMaxWidthProperty, value); }
        }
        public double StarMaxHeight
        {
            get { return (double)GetValue(StarMaxHeightProperty); }
            set { SetValue(StarMaxHeightProperty, value); }
        }

        #endregion

        #region Events


        #endregion

        #region Constructor

        public StarCheckBox() { }

        #endregion

        #region Event Handlers


        #endregion

        #region Methods


        #endregion

    }
}