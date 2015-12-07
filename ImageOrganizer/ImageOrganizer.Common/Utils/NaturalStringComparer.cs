using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImageOrganizer.Common.Contracts.Images;

namespace ImageOrganizer.Common.Utils
{
    public class NaturalStringComparer : IComparer, IComparer<string>, IComparer<System.IO.FileInfo>
    {
        public int Compare(object x, object y)
        {
            if (x is System.IO.FileInfo)
                return this.Compare((System.IO.FileInfo)x, (System.IO.FileInfo)y);
            else if (x is string)
                return this.Compare((string)x, (string)y);
            else if (x is IImageViewModel)
                return this.Compare((IImageViewModel)x, (IImageViewModel)y);
            else
                throw new NotSupportedException();
        }
        public int Compare(string x, string y)
        {
            return NativeMethods.StrCmpLogicalW(x, y);
        }
        public int Compare(System.IO.FileInfo x, System.IO.FileInfo y)
        {
            if (x == null || y == null)
                return 0;
            return this.Compare(x.Name, y.Name);
        }
        public int Compare(IImageViewModel x, IImageViewModel y)
        {
            if (x == null || y == null)
                return 0;
            return this.Compare(x.FileInfo, y.FileInfo);
        }
    }
}