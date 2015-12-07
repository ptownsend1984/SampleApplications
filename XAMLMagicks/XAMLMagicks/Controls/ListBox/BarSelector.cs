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
    public class BarSelector : System.Windows.Controls.ListBox
    {

        #region Static Members

        static BarSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BarSelector), new FrameworkPropertyMetadata(typeof(BarSelector)));
        }

        public static DependencyProperty NoSelectionHighlightBrushProperty = DependencyProperty.Register(
            "NoSelectionHighlightBrush", typeof(Brush), typeof(BarSelector), new PropertyMetadata(null, HighlightProperty_PropertyChanged)
            );
        public static void HighlightProperty_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Item = o as BarSelector;
            if (Item == null)
                return;

            Item.ApplyHighlightBrushes();
        }

        #endregion

        #region Global Variables


        #endregion

        #region Properties

        public Brush NoSelectionHighlightBrush
        {
            get { return (Brush)GetValue(NoSelectionHighlightBrushProperty); }
            set { SetValue(NoSelectionHighlightBrushProperty, value); }
        }

        #endregion

        #region Events


        #endregion

        #region Constructor

        public BarSelector()
        {
            this.ItemContainerGenerator.StatusChanged += (s, e) =>
            {
                if (this.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                    this.ApplyHighlightBrushes();
            };
        }

        #endregion

        #region Event Handlers

        private void BarSelectorItem_HighlightChanged(object sender, EventArgs e)
        {
            ApplyHighlightBrushes();
        }

        #endregion

        #region Methods

        protected override void PrepareContainerForItemOverride(System.Windows.DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var Item = this.ItemContainerGenerator.ContainerFromItem(item) as BarSelectorItem;
            if (Item != null)
            {
                Item.HighlightChanged += BarSelectorItem_HighlightChanged;
            }
        }

        protected override void ClearContainerForItemOverride(System.Windows.DependencyObject element, object item)
        {
            var Item = this.ItemContainerGenerator.ContainerFromItem(item) as BarSelectorItem;
            if (Item != null)
            {
                Item.HighlightChanged -= BarSelectorItem_HighlightChanged;
            }

            base.ClearContainerForItemOverride(element, item);
        }
        protected override System.Windows.DependencyObject GetContainerForItemOverride()
        {
            return new BarSelectorItem();
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            ApplyHighlightBrushes();
        }
        private void ApplyHighlightBrushes()
        {
            if (this.SelectedIndex < 0 && this.NoSelectionHighlightBrush != null)
            {
                foreach (var Item in this.Items)
                {
                    var Container = this.ItemContainerGenerator.ContainerFromItem(Item) as BarSelectorItem;
                    if (Container == null)
                        continue;
                    Container.Background = this.NoSelectionHighlightBrush;
                }
                return;
            }

            for (int i = 0; i <= this.SelectedIndex; i++)
            {
                var Item = this.Items[i];
                var Container = this.ItemContainerGenerator.ContainerFromItem(Item) as BarSelectorItem;
                if (Container == null)
                    continue;
                Container.Background = Container.HighlightedBrush;
            }
            for (int i = this.SelectedIndex + 1; i < this.Items.Count; i++)
            {
                var Item = this.Items[i];
                var Container = this.ItemContainerGenerator.ContainerFromItem(Item) as BarSelectorItem;
                if (Container == null)
                    continue;
                Container.Background = Container.NotHighlightedBrush;
            }
        }

        #endregion

    }
}