using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FolderCrawlerDemo.Extensions
{
    [System.Diagnostics.DebuggerStepThrough()]
    public static class Events
    {

        public static void RaiseEvent(this object o, EventHandler Handler)
        {
            var HandlerLock = Handler;
            if (HandlerLock != null)
            {
                HandlerLock(o, EventArgs.Empty);
            }
        }
        public static void RaiseEvent<T>(this object o, EventHandler<EventArgs<T>> Handler, T args)
        {
            var HandlerLock = Handler;
            if (HandlerLock != null)
            {
                HandlerLock(o, new EventArgs<T>(args));
            }
        }

    }
}