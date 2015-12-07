using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Reflection.Helper2.Types
{
    public class MethodsModel
    {

        public int x;
        public int y;

        public System.Drawing.Point CreatePoint()
        {
            return new System.Drawing.Point(x, y);
        }

        internal System.Drawing.Point CreatePoint(int x)
        {
            return new System.Drawing.Point(x, y);
        }

        internal System.Drawing.Point CreatePoint(int x, int y)
        {
            return new System.Drawing.Point(x, y);
        }

    }
}