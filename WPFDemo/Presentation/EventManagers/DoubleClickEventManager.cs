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
    public class DoubleClickEventManager : System.Windows.WeakEventManager
    {

		private static DoubleClickEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(DoubleClickEventManager);
				DoubleClickEventManager DoubleClickEventManager = (DoubleClickEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (DoubleClickEventManager == null)
				{
					DoubleClickEventManager = new DoubleClickEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, DoubleClickEventManager);
				}
				return DoubleClickEventManager;
			}
		}
		/// <summary>Adds the provided listener to the list of listeners on the provided source.</summary>
		public static void AddListener(DependencyObject source, IWeakEventListener listener)
		{
			if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            DoubleClickEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}
		/// <summary>Removes the specified listener from the list of listeners on the provided source.</summary>
		public static void RemoveListener(DependencyObject source, IWeakEventListener listener)
		{
			if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            DoubleClickEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}
		protected override void StartListening(object source)
		{
            var Element = source as System.Windows.Controls.Control;
            if(Element == null)
                return;

            Element.MouseDoubleClick += OnMouseDoubleClick;
		}
		protected override void StopListening(object source)
		{
            var Element = source as System.Windows.Controls.Control;
            if (Element == null)
                return;

            Element.MouseDoubleClick -= OnMouseDoubleClick;
		}
        private void OnMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs args)
        {
            base.DeliverEvent(sender, args);
        }
        private DoubleClickEventManager() { }

    }
}
