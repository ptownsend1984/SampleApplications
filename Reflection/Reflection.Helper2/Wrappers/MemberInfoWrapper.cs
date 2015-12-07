using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;

namespace Reflection.Helper2.Wrappers
{
    public abstract class MemberInfoWrapper<T> : InfoWrapper<T>
        where T : MemberInfo
    {

        #region Global Variables

        private readonly TypeWrapper _parentType;

        #endregion

        #region Properties

        public TypeWrapper ParentType { get { return _parentType; } }

        public bool IsOnParentType { get { return this.ParentType != null && this.Info.DeclaringType.Equals(this.ParentType.Info); } }

        #endregion

        #region Events


        #endregion

        #region Constructor

        public MemberInfoWrapper(TypeWrapper parentType, T info)
            : base(info)
        {
            this._parentType = parentType;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Info.ToString();
        }
        public override bool Equals(object obj)
        {            
            var op2 = obj as MemberInfoWrapper<T>;
            return op2 != null && op2.Info.Equals(this.Info);
        }
        public override int GetHashCode()
        {
            return this.Info.GetHashCode();
        }

        #endregion

    }
}