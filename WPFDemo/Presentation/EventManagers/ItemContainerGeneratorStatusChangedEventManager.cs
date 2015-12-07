using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFDemo.Presentation.EventManagers
{
    public class ItemContainerGeneratorStatusChangedEventManager : System.Windows.WeakEventManager
    {

        private static ItemContainerGeneratorStatusChangedEventManager CurrentManager
        {
            get
            {
                Type typeFromHandle = typeof(ItemContainerGeneratorStatusChangedEventManager);
                ItemContainerGeneratorStatusChangedEventManager ItemContainerGeneratorStatusChangedEventManager = (ItemContainerGeneratorStatusChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
                if (ItemContainerGeneratorStatusChangedEventManager == null)
                {
                    ItemContainerGeneratorStatusChangedEventManager = new ItemContainerGeneratorStatusChangedEventManager();
                    WeakEventManager.SetCurrentManager(typeFromHandle, ItemContainerGeneratorStatusChangedEventManager);
                }
                return ItemContainerGeneratorStatusChangedEventManager;
            }
        }
        /// <summary>Adds the provided listener to the list of listeners on the provided source.</summary>
        public static void AddListener(DependencyObject source, IWeakEventListener listener)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            var ItemsControl = source as System.Windows.Controls.ItemsControl;
            if (ItemsControl == null)
                throw new ArgumentException("Type mismatch.", "source");

            ItemContainerGeneratorStatusChangedEventManager.CurrentManager.ProtectedAddListener(ItemsControl.ItemContainerGenerator, listener);
        }
        /// <summary>Removes the specified listener from the list of listeners on the provided source.</summary>
        public static void RemoveListener(DependencyObject source, IWeakEventListener listener)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (listener == null)
                throw new ArgumentNullException("listener");
            var ItemsControl = source as System.Windows.Controls.ItemsControl;
            if (ItemsControl == null)
                throw new ArgumentException("Type mismatch.", "source");

            ItemContainerGeneratorStatusChangedEventManager.CurrentManager.ProtectedRemoveListener(ItemsControl.ItemContainerGenerator, listener);
        }
        protected override void StartListening(object source)
        {
            var ItemContainerGenerator = source as System.Windows.Controls.ItemContainerGenerator;
            if (ItemContainerGenerator == null)
                return;

            ItemContainerGenerator.StatusChanged += OnItemContainerGeneratorStatusChanged;
        }
        protected override void StopListening(object source)
        {
            var ItemContainerGenerator = source as System.Windows.Controls.ItemContainerGenerator;
            if (ItemContainerGenerator == null)
                return;

            ItemContainerGenerator.StatusChanged -= OnItemContainerGeneratorStatusChanged;
        }
        private void OnItemContainerGeneratorStatusChanged(object sender, EventArgs args)
        {
            base.DeliverEvent(sender, args);
        }
        private ItemContainerGeneratorStatusChangedEventManager() { }

    }
}