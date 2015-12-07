using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Reflection.Helper
{

    public class HelperUtility : MarshalByRefObject, ICommonClass
    {

        public override string ToString()
        {
            return "I'm a helper!";
        }

    }
}