using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFDemo.Presentation.EventManagers
{
    /// <summary>
    /// Weak event manager implementation for the PreviewKeyDown event
    /// </summary>
    /// <remarks>Based on implementation of LostFocusEventManager</remarks>
    public class PreviewKeyDownEventManager : System.Windows.WeakEventManager
    {

		private static PreviewKeyDownEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(PreviewKeyDownEventManager);
				PreviewKeyDownEventManager PreviewKeyDownEventManager = (PreviewKeyDownEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (PreviewKeyDownEventManager == null)
				{
					PreviewKeyDownEventManager = new PreviewKeyDownEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, PreviewKeyDownEventManager);
				}
				return PreviewKeyDownEventManager;
			}
		}
		/// <summary>Adds the provided listener to the list of listeners on the provided source.</summary>
		public static void AddListener(DependencyObject source, IWeakEventListener listener)
		{
			if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            PreviewKeyDownEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}
		/// <summary>Removes the specified listener from the list of listeners on the provided source.</summary>
		public static void RemoveListener(DependencyObject source, IWeakEventListener listener)
		{
			if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            PreviewKeyDownEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}
		protected override void StartListening(object source)
		{
            var Element = source as FrameworkElement;
            if(Element == null)
                return;

            Element.PreviewKeyDown += OnPreviewKeyDown;
		}
		protected override void StopListening(object source)
		{
            var Element = source as FrameworkElement;
            if (Element == null)
                return;

            Element.PreviewKeyDown -= OnPreviewKeyDown;
		}
        private void OnPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs args)
        {
            base.DeliverEvent(sender, args);
        }
        private PreviewKeyDownEventManager() { }

    }
}
