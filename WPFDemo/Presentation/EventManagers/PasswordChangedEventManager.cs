using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFDemo.Presentation.EventManagers
{
    /// <summary>
    /// Weak event manager implementation for the PasswordChanged event
    /// </summary>
    /// <remarks>Based on implementation of LostFocusEventManager</remarks>
    public class PasswordChangedEventManager : System.Windows.WeakEventManager
    {

        private static PasswordChangedEventManager CurrentManager
        {
            get
            {
                Type typeFromHandle = typeof(PasswordChangedEventManager);
                PasswordChangedEventManager PasswordChangedEventManager = (PasswordChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
                if (PasswordChangedEventManager == null)
                {
                    PasswordChangedEventManager = new PasswordChangedEventManager();
                    WeakEventManager.SetCurrentManager(typeFromHandle, PasswordChangedEventManager);
                }
                return PasswordChangedEventManager;
            }
        }
        /// <summary>Adds the provided listener to the list of listeners on the provided source.</summary>
        public static void AddListener(DependencyObject source, IWeakEventListener listener)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            PasswordChangedEventManager.CurrentManager.ProtectedAddListener(source, listener);
        }
        /// <summary>Removes the specified listener from the list of listeners on the provided source.</summary>
        public static void RemoveListener(DependencyObject source, IWeakEventListener listener)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            PasswordChangedEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
        }
        protected override void StartListening(object source)
        {
            var Element = source as System.Windows.Controls.PasswordBox;
            if (Element == null)
                return;

            Element.PasswordChanged += Element_PasswordChanged;
        }
        protected override void StopListening(object source)
        {
            var Element = source as System.Windows.Controls.PasswordBox;
            if (Element == null)
                return;

            Element.PasswordChanged -= Element_PasswordChanged;
        }
        private void Element_PasswordChanged(object sender, System.Windows.RoutedEventArgs args)
        {
            base.DeliverEvent(sender, args);
        }
        private PasswordChangedEventManager() { }

    }
}