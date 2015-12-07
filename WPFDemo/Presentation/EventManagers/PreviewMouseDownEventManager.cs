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
    public class PreviewMouseDownEventManager : System.Windows.WeakEventManager
    {

		private static PreviewMouseDownEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(PreviewMouseDownEventManager);
				PreviewMouseDownEventManager PreviewMouseDownEventManager = (PreviewMouseDownEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (PreviewMouseDownEventManager == null)
				{
					PreviewMouseDownEventManager = new PreviewMouseDownEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, PreviewMouseDownEventManager);
				}
				return PreviewMouseDownEventManager;
			}
		}
		/// <summary>Adds the provided listener to the list of listeners on the provided source.</summary>
		public static void AddListener(DependencyObject source, IWeakEventListener listener)
		{
			if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            PreviewMouseDownEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}
		/// <summary>Removes the specified listener from the list of listeners on the provided source.</summary>
		public static void RemoveListener(DependencyObject source, IWeakEventListener listener)
		{
			if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            PreviewMouseDownEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}
		protected override void StartListening(object source)
		{
            var Element = source as FrameworkElement;
            if(Element == null)
                return;

            Element.PreviewMouseDown += OnPreviewMouseDown;
		}
		protected override void StopListening(object source)
		{
            var Element = source as FrameworkElement;
            if (Element == null)
                return;

            Element.PreviewMouseDown -= OnPreviewMouseDown;
		}
        private void OnPreviewMouseDown(object sender, System.Windows.Input.MouseEventArgs args)
        {
            base.DeliverEvent(sender, args);
        }
        private PreviewMouseDownEventManager() { }

    }
}
