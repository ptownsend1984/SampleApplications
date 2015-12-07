using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ImageOrganizer.Common.Extensions
{
    public static class Strings
    {

        #region Static Methods

        public static bool IsValidFileName(this string Value)
        {
            if (string.IsNullOrEmpty(Value))
                return false;
            else
            {
                var InvalidPathChars = System.IO.Path.GetInvalidFileNameChars();
                return !Value.Any((c) => InvalidPathChars.Contains(c));
            }
        }

        #endregion

    }
}