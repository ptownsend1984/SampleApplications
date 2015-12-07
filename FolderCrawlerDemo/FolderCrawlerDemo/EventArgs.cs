using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FolderCrawlerDemo
{
    public class EventArgs<T> : EventArgs
    {
        public T Value { get; private set; }

        [System.Diagnostics.DebuggerStepThrough]
        public EventArgs(T Value)
        {
            this.Value = Value;
        }
    }
}