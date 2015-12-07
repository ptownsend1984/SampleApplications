using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Reflection.Helper2.Wrappers
{
    public class MethodWrapper : MethodBaseWrapper<MethodInfo>
    {

        #region Properties


        #endregion

        #region Constructor

        public MethodWrapper(TypeWrapper parentType, MethodInfo info)
            : base(parentType, info)
        {

        }

        #endregion

        #region Methods


        #endregion

    }
}