using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;

namespace Reflection.Helper2.Wrappers
{
    public class TypeWrapper : MemberInfoWrapper<Type>
    {

        #region Global Variables

        #endregion

        #region Properties

        public string AssemblyQualifiedName { get { return Info.AssemblyQualifiedName; } }
        public string Name { get { return Info.Name; } }

        public IEnumerable<TypeWrapper> AllBaseClasses { get { return GetAllBaseClasses(); } }
        public IEnumerable<TypeWrapper> AllInterfaces { get { return GetAllInterfaces(); } }

        public IEnumerable<ConstructorWrapper> AllConstructors { get { return GetAllConstructors(); } }
        public IEnumerable<EventWrapper> AllEvents { get { return GetAllEvents(); } }
        public IEnumerable<FieldWrapper> AllFields { get { return GetAllFields(); } }
        public IEnumerable<PropertyWrapper> AllProperties { get { return GetAllProperties(); } }
        public IEnumerable<MethodWrapper> AllMethods { get { return GetAllMethods(); } }

        #endregion

        #region Constructor

        public TypeWrapper(TypeWrapper parentType, Type type)
            : base(parentType, type)
        {
        }
        public TypeWrapper(Type type)
            : this(null, type)
        {
        }

        #endregion

        #region Methods

        private IEnumerable<TypeWrapper> GetAllBaseClasses()
        {
            Type baseType = this.Info.BaseType;
            if (baseType == null)
                yield break;
            do
            {
                yield return new TypeWrapper(this, baseType);
                baseType = baseType.BaseType;
            }
            while (baseType != null);
        }
        private IEnumerable<TypeWrapper> GetAllInterfaces()
        {
            return this.Info.GetInterfaces().Select(o => new TypeWrapper(this, o));
        }

        private IEnumerable<ConstructorWrapper> GetAllConstructors()
        {
            return this.Info.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Select(o => new ConstructorWrapper(this, o));
        }

        private IEnumerable<EventWrapper> GetAllEvents()
        {
            return this.Info.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Select(o => new EventWrapper(this, o));
        }

        private IEnumerable<FieldWrapper> GetAllFields()
        {
            return this.Info.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Select(o => new FieldWrapper(this, o));
        }

        private IEnumerable<PropertyWrapper> GetAllProperties()
        {
            return this.Info.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Select(o => new PropertyWrapper(this, o));
        }

        private IEnumerable<MethodWrapper> GetAllMethods()
        {
            return this.Info.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Select(o => new MethodWrapper(this, o));
        }

        #endregion

    }
}