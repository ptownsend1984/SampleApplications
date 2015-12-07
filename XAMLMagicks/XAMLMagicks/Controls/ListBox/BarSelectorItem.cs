using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace XAMLMagicks.Controls.ListBox
{
    public class BarSelectorItem : ListBoxItem
    {

        #region Static Members

        static BarSelectorItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BarSelectorItem), new FrameworkPropertyMetadata(typeof(BarSelectorItem)));
        }

        public static DependencyProperty HighlightedBrushProperty = DependencyProperty.Register(
            "HighlightedBrush", typeof(Brush), typeof(BarSelectorItem), new PropertyMetadata(null, HighlightProperty_PropertyChanged)
            );
        public static DependencyProperty NotHighlightedBrushProperty = DependencyProperty.Register(
            "NotHighlightedBrush", typeof(Brush), typeof(BarSelectorItem), new PropertyMetadata(null, HighlightProperty_PropertyChanged)
            );

        public static void HighlightProperty_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Item = o as BarSelectorItem;
            if (Item == null)
                return;

            Item.OnHighlightChanged();
        }

        #endregion

        #region Properties

        public Brush HighlightedBrush
        {
            get { return (Brush)GetValue(HighlightedBrushProperty); }
            set { SetValue(HighlightedBrushProperty, value); }
        }
        public Brush NotHighlightedBrush
        {
            get { return (Brush)GetValue(NotHighlightedBrushProperty); }
            set { SetValue(NotHighlightedBrushProperty, value); }
        }

        #endregion

        #region Events

        public event EventHandler HighlightChanged;

        #endregion

        #region Constructor

        public BarSelectorItem()
        {

        }

        #endregion

        #region Methods

        protected virtual void OnHighlightChanged()
        {
            var Handler = this.HighlightChanged;
            if (Handler != null)
                Handler(this, EventArgs.Empty);
        }

        #endregion

    }
}