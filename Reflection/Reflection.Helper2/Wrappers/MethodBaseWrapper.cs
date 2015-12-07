using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Reflection.Helper2.Wrappers
{
    public abstract class MethodBaseWrapper<T> : MemberInfoWrapper<T>
        where T : MethodBase
    {

        #region Properties


        #endregion

        #region Constructor

        public MethodBaseWrapper(TypeWrapper parentType, T info)
            : base(parentType, info)
        {

        }

        #endregion

        #region Methods


        #endregion

    }
}