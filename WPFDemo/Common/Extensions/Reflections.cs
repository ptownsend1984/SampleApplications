using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace WPFDemo.Common.Extensions
{
    public static class Reflections
    {

        /// <summary>
        /// Extract a property name out of an expression
        /// </summary>
        /// <typeparam name="T">Property Type</typeparam>
        /// <param name="PropertyExpression">Property Expression</param>
        /// <returns></returns>
        /// <remarks>Lets you use the built-in Visual Studio renaming with impunity</remarks>
        public static string GetPropertyName<T>(this Expression<Func<T>> PropertyExpression)
        {
            if (PropertyExpression == null)
                throw new ArgumentNullException("PropertyExpression");
            var MemberExpression = PropertyExpression.Body as MemberExpression;
            if (MemberExpression == null)
                throw new ArgumentException("Invalid Member Expression", "PropertyExpression");
            var PropertyInfo = MemberExpression.Member as PropertyInfo;
            if (PropertyInfo == null)
                throw new ArgumentException("Not a property method", "PropertyExpression");
            var GetMethod = PropertyInfo.GetGetMethod(true);
            if (GetMethod.IsStatic)
                throw new ArgumentException("Cannot use a static property", "PropertyExpression");

            return MemberExpression.Member.Name;
        }

        public static bool TypeIs(this Type Type, Type ParentType)
        {
            return Type.Equals(ParentType) || Type.IsSubclassOf(ParentType);
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static bool TypeIs(this Type Type, Assembly ParentTypeAssembly, string ParentTypeName)
        {
            if (Type == null || ParentTypeAssembly == null)
                return false;
            return TypeIs(Type, ParentTypeAssembly.GetType(ParentTypeName));
        }

    }
}