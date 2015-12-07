using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Reflection.Helper2.Types
{
    public class FieldsModel
    {

        public int publicInt = 0;
        internal int internalInt = 0;

#pragma warning disable 414
        private int privateInt = 0;
        private static int privateStaticInt = 0;
#pragma warning restore 414

    }
}