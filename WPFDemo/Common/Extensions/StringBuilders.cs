using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WPFDemo.Common.Extensions
{
    public static class StringBuilders
    {

        #region Subclasses


        #endregion

        #region Constants


        #endregion

        #region Static Members

        public static void AppendTab(this System.Text.StringBuilder s, string Message)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            s.Append(Message);
            s.Append("\t");
        }
        public static void AppendInNewLine(this System.Text.StringBuilder s, string Message)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            if (!s.IsEmpty())
            {
                s.Append(Environment.NewLine);
            }
            s.Append(Message);
        }
        public static bool IsEmpty(this System.Text.StringBuilder s)
        {
            return s.Length == 0;
        }

        #endregion

    }
}