using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFDemo.Presentation.EventManagers
{
    /// <summary>
    /// Weak event manager implementation for the PreviewMouseDown event
    /// </summary>
    /// <remarks>Based on implementation of LostFocusEventManager</remarks>
    public class PreviewMouseMoveEventManager : System.Windows.WeakEventManager
    {

        private static PreviewMouseMoveEventManager CurrentManager
        {
            get
            {
                Type typeFromHandle = typeof(PreviewMouseMoveEventManager);
                PreviewMouseMoveEventManager PreviewMouseMoveEventManager = (PreviewMouseMoveEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
                if (PreviewMouseMoveEventManager == null)
                {
                    PreviewMouseMoveEventManager = new PreviewMouseMoveEventManager();
                    WeakEventManager.SetCurrentManager(typeFromHandle, PreviewMouseMoveEventManager);
                }
                return PreviewMouseMoveEventManager;
            }
        }
        /// <summary>Adds the provided listener to the list of listeners on the provided source.</summary>
        public static void AddListener(DependencyObject source, IWeakEventListener listener)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            PreviewMouseMoveEventManager.CurrentManager.ProtectedAddListener(source, listener);
        }
        /// <summary>Removes the specified listener from the list of listeners on the provided source.</summary>
        public static void RemoveListener(DependencyObject source, IWeakEventListener listener)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            PreviewMouseMoveEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
        }
        protected override void StartListening(object source)
        {
            var Element = source as FrameworkElement;
            if (Element == null)
                return;

            Element.PreviewMouseMove += OnPreviewMouseMove;
        }
        protected override void StopListening(object source)
        {
            var Element = source as FrameworkElement;
            if (Element == null)
                return;

            Element.PreviewMouseMove -= OnPreviewMouseMove;
        }
        private void OnPreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs args)
        {
            base.DeliverEvent(sender, args);
        }
        private PreviewMouseMoveEventManager() { }

    }
}