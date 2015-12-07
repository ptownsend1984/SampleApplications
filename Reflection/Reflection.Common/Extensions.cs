using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Reflection
{
    public static class Extensions
    {

        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            if (source == null)
                return false;

            return source.IndexOf(value, comparisonType) > -1;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new ObservableCollection<T>(source);
        }

    }
}