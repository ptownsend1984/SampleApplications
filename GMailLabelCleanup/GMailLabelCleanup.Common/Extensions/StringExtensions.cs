using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GMailLabelCleanup.Common.Extensions
{
    public static class StringExtensions
    {

        public static string IfNotEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s) ? s : string.Empty;
        }

        public static string TrimIfNotEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s) ? s.Trim() : string.Empty;
        }

    }
}