using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Reflection.Helper2.Wrappers
{
    public class PropertyWrapper : MemberInfoWrapper<PropertyInfo>
    {

        #region Properties        

        #endregion

        #region Constructor

        public PropertyWrapper(TypeWrapper parentType, PropertyInfo info)
            : base(parentType, info)
        {

        }

        #endregion

        #region Methods


        #endregion

    }
}