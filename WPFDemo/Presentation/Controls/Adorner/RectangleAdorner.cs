using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace WPFDemo.Presentation.Controls.Adorner
{
    public class RectangleAdorner : System.Windows.Documents.Adorner
    {

        #region Static Members

        public static readonly DependencyProperty BrushProperty = DependencyProperty.Register(
            "Brush", typeof(Brush), typeof(RectangleAdorner),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, Visual_PropertyChanged)
            );
        public static readonly DependencyProperty PenProperty = DependencyProperty.Register(
            "Pen", typeof(Pen), typeof(RectangleAdorner),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, Visual_PropertyChanged)
            );
        private static void Visual_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Adorner = o as RectangleAdorner;
            if (Adorner == null)
                return;

            Adorner.InvalidateVisual();
        }

        #endregion

        #region Properties

        public Brush Brush
        {
            get { return (Brush)this.GetValue(BrushProperty); }
            set { this.SetValue(BrushProperty, value); }
        }
        public Pen Pen
        {
            get { return (Pen)this.GetValue(PenProperty); }
            set { this.SetValue(PenProperty, value); }
        }

        #endregion

        #region Constructor

        public RectangleAdorner(UIElement element)
            : base(element)
        {
        }

        #endregion

        #region Methods

        protected override void OnRender(DrawingContext drawingContext)
        {
            var Rectangle = new Rect(new Point(0, 0), this.DesiredSize);
            drawingContext.DrawRectangle(this.Brush, this.Pen, Rectangle);
        }

        #endregion

    }
}