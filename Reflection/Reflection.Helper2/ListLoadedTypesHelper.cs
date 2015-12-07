using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Reflection.Helper2.Wrappers;

namespace Reflection.Helper2
{
    public class ListLoadedTypesHelper
    {

        #region Methods

        public IEnumerable<TypeWrapper> ListLoadedTypes(AppDomain appDomain)
        {            
            if (appDomain == null)
                throw new ArgumentNullException("appDomain");

            return appDomain.GetAssemblies().SelectMany(o => o.GetTypes().Select(t => new TypeWrapper(null, t)));
        }

        #endregion

    }
}