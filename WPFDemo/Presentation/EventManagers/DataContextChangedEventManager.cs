using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using WPFDemo.Presentation.Common;

namespace WPFDemo.Presentation.EventManagers
{
    public class DataContextChangedEventManager : System.Windows.WeakEventManager
    {
        private static DataContextChangedEventManager CurrentManager
        {
            get
            {
                Type typeFromHandle = typeof(DataContextChangedEventManager);
                DataContextChangedEventManager DataContextChangedEventManager = (DataContextChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
                if (DataContextChangedEventManager == null)
                {
                    DataContextChangedEventManager = new DataContextChangedEventManager();
                    WeakEventManager.SetCurrentManager(typeFromHandle, DataContextChangedEventManager);
                }
                return DataContextChangedEventManager;
            }
        }
        /// <summary>Adds the provided listener to the list of listeners on the provided source.</summary>
        public static void AddListener(DependencyObject source, IWeakEventListener listener)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            DataContextChangedEventManager.CurrentManager.ProtectedAddListener(source, listener);
        }
        /// <summary>Removes the specified listener from the list of listeners on the provided source.</summary>
        public static void RemoveListener(DependencyObject source, IWeakEventListener listener)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            DataContextChangedEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
        }
        protected override void StartListening(object source)
        {
            var Element = source as FrameworkElement;
            if (Element == null)
                return;

            Element.DataContextChanged += OnDataContextChanged;
        }
        protected override void StopListening(object source)
        {
            var Element = source as FrameworkElement;
            if (Element == null)
                return;

            Element.DataContextChanged -= OnDataContextChanged;
        }
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            base.DeliverEvent(sender, new DependencyPropertyChangedEventArgsEx(args.Property, args.OldValue, args.NewValue));
        }
        private DataContextChangedEventManager() { }

    }
}