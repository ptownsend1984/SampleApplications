using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace ImageOrganizer.Presentation.Collections
{
    public class SyncObservableCollection<T> : ObservableCollection<T>
    {

        #region Static Members


        #endregion

        #region Global Variables

        private readonly Dispatcher Dispatcher;

        #endregion

        #region Constructor

        public SyncObservableCollection()
        {
            this.Dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
        }

        #endregion

        #region Methods

        protected override void ClearItems()
        {
            this.Dispatcher.Invoke(new Action(base.ClearItems));
        }
        protected override void InsertItem(int index, T item)
        {
            this.Dispatcher.Invoke(new Action<int, T>(base.InsertItem), index, item);
        }
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            this.Dispatcher.Invoke(new Action<int, int>(base.MoveItem), oldIndex, newIndex);
        }
        protected override void RemoveItem(int index)
        {
            this.Dispatcher.Invoke(new Action<int>(base.RemoveItem), index);
        }
        protected override void SetItem(int index, T item)
        {
            this.Dispatcher.Invoke(new Action<int, T>(base.SetItem), index, item);
        }

        #endregion

    }
}