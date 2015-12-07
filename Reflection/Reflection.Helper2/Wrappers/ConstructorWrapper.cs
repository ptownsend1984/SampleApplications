using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Reflection.Helper2.Wrappers
{
    public class ConstructorWrapper : MethodBaseWrapper<ConstructorInfo>
    {

        #region Properties


        #endregion

        #region Constructor

        public ConstructorWrapper(TypeWrapper parentType, ConstructorInfo info)
            : base(parentType, info)
        {

        }

        #endregion

        #region Methods


        #endregion

    }
}