using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using WPFDemo.Presentation.Controls.Adorner;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Input;
using WPFDemo.Presentation.EventManagers;

namespace WPFDemo.Presentation.Behaviors
{
    public static class AdornerExtensions
    {

        #region Static Properties

        private static readonly DependencyProperty AdornerLayerCacheProperty = DependencyProperty.RegisterAttached(
            "AdornerLayerCache", typeof(AdornerLayer), typeof(AdornerExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None)
            );
        private static readonly object AdornerLayerCacheLock = new object();

        public static readonly DependencyProperty ShowMouseOverAdornerProperty = DependencyProperty.RegisterAttached(
            "ShowMouseOverAdorner", typeof(bool), typeof(AdornerExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, ShowMouseOverAdorner_PropertyChanged)
            );
        public static readonly DependencyProperty MouseOverAdornerBrushProperty = DependencyProperty.RegisterAttached(
            "MouseOverAdornerBrush", typeof(Brush), typeof(AdornerExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, MouseOverAdornerBrush_PropertyChanged)
            );
        public static readonly DependencyProperty MouseOverAdornerPenProperty = DependencyProperty.RegisterAttached(
            "MouseOverAdornerPen", typeof(Pen), typeof(AdornerExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, MouseOverAdornerPen_PropertyChanged)
            );
        private static readonly DependencyProperty MouseOverAdornerProperty = DependencyProperty.RegisterAttached(
            "MouseOverAdorner", typeof(RectangleAdorner), typeof(AdornerExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
          );

        public static readonly DependencyProperty ShowContentAdornerProperty = DependencyProperty.RegisterAttached(
            "ShowContentAdorner", typeof(bool), typeof(AdornerExtensions),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, ShowContentAdorner_PropertyChanged)
            );
        public static readonly DependencyProperty ContentAdornerStyleProperty = DependencyProperty.RegisterAttached(
            "ContentAdornerStyle", typeof(Style), typeof(AdornerExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, ContentAdornerStyle_PropertyChanged)
          );
        private static readonly DependencyProperty ContentAdornerProperty = DependencyProperty.RegisterAttached(
            "ContentAdorner", typeof(ContentAdorner), typeof(AdornerExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
          );
        private static readonly DependencyProperty ContentAdornerDataContextWeakEventListenerProperty = DependencyProperty.RegisterAttached(
            "ContentAdornerDataContextWeakEventListener", typeof(IWeakEventListener), typeof(AdornerExtensions),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
          );

        #endregion

        #region Static Methods

        private static AdornerLayer GetAdornerLayerCache(Visual visual)
        {
            return (AdornerLayer)visual.GetValue(AdornerLayerCacheProperty);
            //return AdornerLayer.GetAdornerLayer(visual);
        }
        private static void SetAdornerLayerCache(Visual visual, AdornerLayer value)
        {
            visual.SetValue(AdornerLayerCacheProperty, value);
        }
        private static AdornerLayer InitializeAdornerLayerCache(Visual visual)
        {
            var Layer = GetAdornerLayerCache(visual);
            if (Layer == null)
            {
                lock (AdornerLayerCacheLock)
                {
                    Layer = GetAdornerLayerCache(visual);
                    if (Layer == null)
                    {
                        Layer = AdornerLayer.GetAdornerLayer(visual);
                        if (Layer == null)
                            throw new ArgumentException("No adorner layer.", "layer");

                        SetAdornerLayerCache(visual, Layer);
                    }
                }
            }
            return Layer;
        }

        public static bool GetShowMouseOverAdorner(FrameworkElement element)
        {
            return (bool)element.GetValue(ShowMouseOverAdornerProperty);
        }
        public static void SetShowMouseOverAdorner(FrameworkElement element, bool value)
        {
            element.SetValue(ShowMouseOverAdornerProperty, value);
        }
        public static Brush GetMouseOverAdornerBrush(FrameworkElement element)
        {
            return (Brush)element.GetValue(MouseOverAdornerBrushProperty);
        }
        public static void SetMouseOverAdornerBrush(FrameworkElement element, Brush value)
        {
            element.SetValue(MouseOverAdornerBrushProperty, value);
        }
        public static Pen GetMouseOverAdornerPen(FrameworkElement element)
        {
            return (Pen)element.GetValue(MouseOverAdornerPenProperty);
        }
        public static void SetMouseOverAdornerPen(FrameworkElement element, Pen value)
        {
            element.SetValue(MouseOverAdornerPenProperty, value);
        }
        private static RectangleAdorner GetMouseOverAdorner(FrameworkElement element)
        {
            return (RectangleAdorner)element.GetValue(MouseOverAdornerProperty);
        }
        private static void SetMouseOverAdorner(FrameworkElement element, RectangleAdorner value)
        {
            element.SetValue(MouseOverAdornerProperty, value);
        }

        private static void ShowMouseOverAdorner_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                return;

            Element.PreviewMouseMove -= ShowMouseOverAdorner_Element_PreviewMouseMove;
            Element.MouseLeave -= ShowMouseOverAdorner_Element_MouseLeave;
            if ((bool)e.NewValue)
            {
                Element.MouseEnter += ShowMouseOverAdorner_Element_PreviewMouseMove;
                Element.MouseLeave += ShowMouseOverAdorner_Element_MouseLeave;
            }
            else
            {
                RemoveMouseOverAdorner(Element);
            }
        }
        private static void MouseOverAdornerBrush_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                return;

            var Adorner = GetMouseOverAdorner(Element) as RectangleAdorner;
            if (Adorner == null)
                return;

            Adorner.Brush = e.NewValue as Brush;
        }
        private static void MouseOverAdornerPen_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                return;

            var Adorner = GetMouseOverAdorner(Element) as RectangleAdorner;
            if (Adorner == null)
                return;

            Adorner.Pen = e.NewValue as Pen;
        }
        private static void ShowMouseOverAdorner_Element_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var Element = sender as FrameworkElement;
            if (Element == null)
                return;

            var AdornerLayer = InitializeAdornerLayerCache(Element);
            if (AdornerLayer == null)
                return;

            var Adorner = GetMouseOverAdorner(Element);
            if (Adorner == null)
            {
                Adorner = new RectangleAdorner(Element)
                {
                    IsHitTestVisible = false,
                    Brush = GetMouseOverAdornerBrush(Element),
                    Pen = GetMouseOverAdornerPen(Element)
                };
                SetMouseOverAdorner(Element, Adorner);
            }

            if (Adorner.Parent != AdornerLayer)
                AdornerLayer.Add(Adorner);
        }
        private static void ShowMouseOverAdorner_Element_MouseLeave(object sender, MouseEventArgs e)
        {
            var Element = sender as FrameworkElement;
            if (Element == null)
                return;

            RemoveMouseOverAdorner(Element);
        }
        private static void RemoveMouseOverAdorner(FrameworkElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            var AdornerLayer = InitializeAdornerLayerCache(element);
            if (AdornerLayer == null)
                return;

            var Adorner = GetMouseOverAdorner(element);
            if (Adorner == null)
                return;

            AdornerLayer.Remove(Adorner);
            SetMouseOverAdorner(element, null);
        }

        public static bool GetShowContentAdorner(FrameworkElement element)
        {
            return (bool)element.GetValue(ShowContentAdornerProperty);
        }
        public static void SetShowContentAdorner(FrameworkElement element, bool value)
        {
            element.SetValue(ShowContentAdornerProperty, value);
        }
        public static Style GetContentAdornerStyle(FrameworkElement element)
        {
            return (Style)element.GetValue(ContentAdornerStyleProperty);
        }
        public static void SetContentAdornerStyle(FrameworkElement element, Style value)
        {
            element.SetValue(ContentAdornerStyleProperty, value);
        }
        internal static ContentAdorner GetContentAdorner(FrameworkElement element)
        {
            return (ContentAdorner)element.GetValue(ContentAdornerProperty);
        }
        internal static void SetContentAdorner(FrameworkElement element, ContentAdorner value)
        {
            element.SetValue(ContentAdornerProperty, value);
        }
        private static IWeakEventListener GetContentAdornerDataContextWeakEventListener(FrameworkElement element)
        {
            return (IWeakEventListener)element.GetValue(ContentAdornerDataContextWeakEventListenerProperty);
        }
        private static void SetContentAdornerDataContextWeakEventListener(FrameworkElement element, IWeakEventListener value)
        {
            element.SetValue(ContentAdornerDataContextWeakEventListenerProperty, value);
        }

        private static void ShowContentAdorner_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                return;

            var Listener = GetContentAdornerDataContextWeakEventListener(Element);

            if ((bool)e.NewValue)
            {
                var AdornerLayer = InitializeAdornerLayerCache(Element);
                if (AdornerLayer == null)
                    return;

                var ContentAdorner = GetContentAdorner(Element);
                if (ContentAdorner == null)
                {
                    ContentAdorner = new ContentAdorner(Element);
                    ContentAdorner.Style = GetContentAdornerStyle(Element);
                    SetContentAdorner(Element, ContentAdorner);
                }
                if (ContentAdorner.Parent != AdornerLayer)
                    AdornerLayer.Add(ContentAdorner);

                ContentAdorner.DataContext = Element.DataContext;
                if (Listener == null)
                    Listener = new ContentAdornerDataContextWeakEventListener();

                SetContentAdornerDataContextWeakEventListener(Element, Listener);
                DataContextChangedEventManager.AddListener(Element, Listener);
            }
            else
            {
                if (Listener != null)
                {
                    DataContextChangedEventManager.RemoveListener(Element, Listener);
                    SetContentAdornerDataContextWeakEventListener(Element, null);
                }
                RemoveContentAdorner(Element);
            }
        }
        private static void ContentAdornerStyle_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var Element = o as FrameworkElement;
            if (Element == null)
                return;

            var Adorner = GetContentAdorner(Element);
            if (Adorner == null)
                return;

            Adorner.ContentControl.Style = (Style)e.NewValue;
        }
        private static void RemoveContentAdorner(FrameworkElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            var AdornerLayer = InitializeAdornerLayerCache(element);
            if (AdornerLayer == null)
                return;

            var Adorner = GetContentAdorner(element);
            if (Adorner == null)
                return;

            AdornerLayer.Remove(Adorner);
            SetContentAdorner(element, null);
        }

        #endregion

    }

    internal class ContentAdornerDataContextWeakEventListener : IWeakEventListener
    {

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType != typeof(DataContextChangedEventManager))
                return false;

            var Element = sender as FrameworkElement;
            if (Element == null)
                return true;

            var ContentAdorner = AdornerExtensions.GetContentAdorner(Element);
            if (ContentAdorner == null)
                return true;

            ContentAdorner.DataContext = Element.DataContext;

            return true;
        }

        #endregion
    }
}