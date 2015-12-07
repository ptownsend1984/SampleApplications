using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Reflection.Helper2.Wrappers
{
    public class FieldWrapper : MemberInfoWrapper<FieldInfo>
    {

        #region Properties


        #endregion

        #region Constructor

        public FieldWrapper(TypeWrapper parentType, FieldInfo info)
            : base(parentType, info)
        {

        }

        #endregion

        #region Methods


        #endregion

    }
}