using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Reflection.Helper2.Wrappers
{
    public class EventWrapper : MemberInfoWrapper<EventInfo>
    {

        #region Properties


        #endregion

        #region Constructor

        public EventWrapper(TypeWrapper parentType, EventInfo info)
            : base(parentType, info)
        {

        }

        #endregion

        #region Methods


        #endregion

    }
}