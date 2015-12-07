using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ComponentModel;

namespace ImageOrganizer.Common.Extensions
{
    public static class Events
    {

        #region Static Members

        [DebuggerStepThrough()]
        public static void RaiseEvent(this object o, EventHandler Handler)
        {
            var HandlerLock = Handler;
            if (HandlerLock != null)
            {
                HandlerLock(o, EventArgs.Empty);
            }
        }
        [DebuggerStepThrough()]
        public static CancelEventArgs RaiseCancelEvent(this object o, CancelEventHandler Handler, bool Cancel)
        {
            var e = new CancelEventArgs(Cancel);
            var HandlerLock = Handler;
            if (HandlerLock != null)
            {
                HandlerLock(o, e);
            }
            return e;
        }
        [DebuggerStepThrough()]
        public static CancelEventArgs RaiseCancelEvent(this object o, CancelEventHandler Handler)
        {
            return RaiseCancelEvent(o, Handler, false);
        }
        [DebuggerStepThrough()]
        public static void RaiseAction(this object o, Action Action)
        {
            if (Action != null)
            {
                Action();
            }
        }

        #endregion

    }
}