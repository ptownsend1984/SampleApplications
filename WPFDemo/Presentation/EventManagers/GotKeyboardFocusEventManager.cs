using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFDemo.Presentation.EventManagers
{
    /// <summary>
    /// Weak event manager implementation for the GotKeyboardFocus event
    /// </summary>
    /// <remarks>Based on implementation of LostFocusEventManager</remarks>
    public class GotKeyboardFocusEventManager : System.Windows.WeakEventManager
    {

		private static GotKeyboardFocusEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(GotKeyboardFocusEventManager);
				GotKeyboardFocusEventManager GotKeyboardFocusEventManager = (GotKeyboardFocusEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (GotKeyboardFocusEventManager == null)
				{
					GotKeyboardFocusEventManager = new GotKeyboardFocusEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, GotKeyboardFocusEventManager);
				}
				return GotKeyboardFocusEventManager;
			}
		}
		/// <summary>Adds the provided listener to the list of listeners on the provided source.</summary>
		public static void AddListener(DependencyObject source, IWeakEventListener listener)
		{
			if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            GotKeyboardFocusEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}
		/// <summary>Removes the specified listener from the list of listeners on the provided source.</summary>
		public static void RemoveListener(DependencyObject source, IWeakEventListener listener)
		{
			if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            GotKeyboardFocusEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}
		protected override void StartListening(object source)
		{
            var Element = source as FrameworkElement;
            if(Element == null)
                return;

            Element.GotKeyboardFocus += OnGotKeyboardFocus;
		}
		protected override void StopListening(object source)
		{
            var Element = source as FrameworkElement;
            if (Element == null)
                return;

            Element.GotKeyboardFocus -= OnGotKeyboardFocus;
		}
        private void OnGotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs args)
        {
            base.DeliverEvent(sender, args);
        }
        private GotKeyboardFocusEventManager() { }

    }
}
