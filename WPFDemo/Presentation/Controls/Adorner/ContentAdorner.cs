using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;

namespace WPFDemo.Presentation.Controls.Adorner
{
    public class ContentAdorner : System.Windows.Documents.Adorner
    {

        #region Static Members


        #endregion

        #region Global Variables

        private readonly ContentPresenter _ContentControl;

        #endregion

        #region Properties

        internal ContentPresenter ContentControl
        {
            get { return _ContentControl; }
        }
        public new FrameworkElement AdornedElement
        {
            get { return base.AdornedElement as FrameworkElement; }
        }

        #endregion

        #region Events


        #endregion

        #region Constructor

        public ContentAdorner(FrameworkElement adornedElement)
            : base(adornedElement)
        {
            _ContentControl = new ContentPresenter();
            _ContentControl.SizeChanged += ContentControl_SizeChanged;

            base.AddLogicalChild(_ContentControl);
            base.AddVisualChild(_ContentControl);

            this.DataContextChanged += this_DataContextChanged;
        }

        #endregion

        #region Event Handlers

        private void this_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.ContentControl.DataContext = e.NewValue;
        }

        private void ContentControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.InvalidateMeasure();
        }

        #endregion

        #region Methods

        protected override Size MeasureOverride(Size constraint)
        {
            this.ContentControl.Measure(constraint);
            return this.ContentControl.DesiredSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            var DesiredSize = this.ContentControl.DesiredSize;
            switch (this.ContentControl.HorizontalAlignment)
            {
                case HorizontalAlignment.Stretch:
                    DesiredSize.Width = this.AdornedElement.ActualWidth;
                    break;
            }
            switch (this.ContentControl.VerticalAlignment)
            {
                case VerticalAlignment.Stretch:
                    DesiredSize.Height = this.AdornedElement.ActualHeight;
                    break;
            }
            this.ContentControl.Arrange(new Rect(0, 0, DesiredSize.Width, DesiredSize.Height));
            return finalSize;
        }
        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }
        protected override System.Windows.Media.Visual GetVisualChild(int index)
        {
            return this.ContentControl;
        }
        protected override IEnumerator LogicalChildren
        {
            get
            {
                return (new FrameworkElement[] { this.ContentControl }).GetEnumerator();
            }
        }

        #endregion

    }
}