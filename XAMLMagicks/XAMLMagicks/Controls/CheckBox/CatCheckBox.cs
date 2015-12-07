using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace XAMLMagicks.Controls.CheckBox
{
    public class CatCheckBox : System.Windows.Controls.CheckBox
    {

        #region Static Members

        static CatCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CatCheckBox), new FrameworkPropertyMetadata(typeof(CatCheckBox)));
        }

        public static readonly DependencyProperty CatMaxWidthProperty =
            DependencyProperty.Register(
            "CatMaxWidth", typeof(double), typeof(CatCheckBox), new UIPropertyMetadata(0d)
            );
        public static readonly DependencyProperty CatMaxHeightProperty =
            DependencyProperty.Register(
            "CatMaxHeight", typeof(double), typeof(CatCheckBox), new UIPropertyMetadata(0d)
            );

        #endregion

        #region Properties

        public double CatMaxWidth
        {
            get { return (double)GetValue(CatMaxWidthProperty); }
            set { SetValue(CatMaxWidthProperty, value); }
        }
        public double CatMaxHeight
        {
            get { return (double)GetValue(CatMaxHeightProperty); }
            set { SetValue(CatMaxHeightProperty, value); }
        }

        #endregion

        #region Constructor

        public CatCheckBox() { }

        #endregion

    }
}