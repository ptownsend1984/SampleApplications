using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace FolderCrawlerDemo
{
    public abstract class UINotificationObject : NotificationObject
    {

        public virtual System.Windows.Window Owner
        {
            get;
            set;
        }
        public virtual string FormText
        {
            get;
            set;
        }

        protected object Invoke(Delegate method, params object[] args)
        {
            var Owner = this.Owner;
            if (Owner == null)
                return method.DynamicInvoke(args);
            else
            {
                return Owner.Dispatcher.Invoke(method, args);
            }
        }

        private int _UseWaitCursor;
        public bool UseWaitCursor
        {
            get { return _UseWaitCursor > 0; }
            set
            {
                if (value)
                    Interlocked.Increment(ref _UseWaitCursor);
                else
                    Interlocked.Decrement(ref _UseWaitCursor);
                OnPropertyChanged(() => this.UseWaitCursor);
            }
        }

    }
}