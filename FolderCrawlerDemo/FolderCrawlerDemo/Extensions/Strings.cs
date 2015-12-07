using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FolderCrawlerDemo.Extensions
{
    public static class Strings
    {

        [System.Diagnostics.DebuggerStepThrough]
        public static Uri ToUri(this string Value)
        {
            if (string.IsNullOrEmpty(Value))
                return null;
            return new Uri(Value);
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static Uri TryToUri(this string Value)
        {
            try
            {
                return ToUri(Value);
            }
            catch { return null; }
        }

    }
}