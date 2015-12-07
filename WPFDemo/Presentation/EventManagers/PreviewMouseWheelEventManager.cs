using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFDemo.Presentation.EventManagers
{
    /// <summary>
    /// Weak event manager implementation for the PreviewMouseWheel event
    /// </summary>
    /// <remarks>Based on implementation of LostFocusEventManager</remarks>
    public class PreviewMouseWheelEventManager : System.Windows.WeakEventManager
    {

        private static PreviewMouseWheelEventManager CurrentManager
        {
            get
            {
                Type typeFromHandle = typeof(PreviewMouseWheelEventManager);
                PreviewMouseWheelEventManager PreviewMouseWheelEventManager = (PreviewMouseWheelEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
                if (PreviewMouseWheelEventManager == null)
                {
                    PreviewMouseWheelEventManager = new PreviewMouseWheelEventManager();
                    WeakEventManager.SetCurrentManager(typeFromHandle, PreviewMouseWheelEventManager);
                }
                return PreviewMouseWheelEventManager;
            }
        }
        /// <summary>Adds the provided listener to the list of listeners on the provided source.</summary>
        public static void AddListener(DependencyObject source, IWeakEventListener listener)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            PreviewMouseWheelEventManager.CurrentManager.ProtectedAddListener(source, listener);
        }
        /// <summary>Removes the specified listener from the list of listeners on the provided source.</summary>
        public static void RemoveListener(DependencyObject source, IWeakEventListener listener)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            PreviewMouseWheelEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
        }
        protected override void StartListening(object source)
        {
            var Element = source as FrameworkElement;
            if (Element == null)
                return;

            Element.PreviewMouseWheel += OnPreviewMouseWheel;
        }
        protected override void StopListening(object source)
        {
            var Element = source as FrameworkElement;
            if (Element == null)
                return;

            Element.PreviewMouseWheel -= OnPreviewMouseWheel;
        }
        private void OnPreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs args)
        {
            base.DeliverEvent(sender, args);
        }
        private PreviewMouseWheelEventManager() { }

    }
}