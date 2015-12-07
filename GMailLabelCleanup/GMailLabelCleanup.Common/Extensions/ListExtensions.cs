using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GMailLabelCleanup.Common.Extensions
{
    public static class ListExtensions
    {

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            return new HashSet<T>(items);
        }

        public static IReadOnlyDictionary<T, U> AsReadOnly<T, U>(this IDictionary<T, U> dictionary)
        {
            return new ReadOnlyDictionary<T, U>(dictionary);
        }

    }
}