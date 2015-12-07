using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace XAMLMagicks
{
    public static class Extensions
    {

        #region Subclasses


        #endregion

        #region Constants


        #endregion

        #region Static Global Variables


        #endregion

        #region Static Properties


        #endregion

        #region Static Methods

        /// <summary>
        /// Extract a property name out of an expression
        /// </summary>
        /// <typeparam name="T">Property Type</typeparam>
        /// <param name="PropertyExpression">Property Expression</param>
        /// <returns></returns>
        /// <remarks>Lets you use the built-in Visual Studio renaming with impunity</remarks>
        [System.Diagnostics.DebuggerStepThrough]
        public static string GetPropertyName<T>(this Expression<Func<T>> PropertyExpression)
        {
            if (PropertyExpression == null)
                throw new ArgumentNullException("PropertyExpression");
            var MemberExpression = PropertyExpression.Body as MemberExpression;
            if (MemberExpression == null)
                throw new ArgumentException("Invalid Member Expression", "PropertyExpression");
#if DEBUG
            var PropertyInfo = MemberExpression.Member as PropertyInfo;
            if (PropertyInfo == null)
                throw new ArgumentException("Not a property method", "PropertyExpression");
            var GetMethod = PropertyInfo.GetGetMethod(true);
            if (GetMethod.IsStatic)
                throw new ArgumentException("Cannot use a static property", "PropertyExpression");
#endif
            return MemberExpression.Member.Name;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static bool TypeIs(this Type Type, Type ParentType)
        {
            if (Type == null || ParentType == null)
                return false;
            return Type.Equals(ParentType) || Type.IsSubclassOf(ParentType);
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static bool TypeIs(this Type Type, Assembly ParentTypeAssembly, string ParentTypeName)
        {
            if (Type == null || ParentTypeAssembly == null)
                return false;
            return TypeIs(Type, ParentTypeAssembly.GetType(ParentTypeName));
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static bool HasInterface(this Type Type, string InterfaceName)
        {
            return HasInterface(Type, InterfaceName, false);
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static bool HasInterface(this Type Type, string InterfaceName, bool IgnoreCase)
        {
            if (Type == null || string.IsNullOrEmpty(InterfaceName))
                return false;
            return Type.GetInterface(InterfaceName, IgnoreCase) != null;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static object CreateGeneric(this Type GenericType, Type InnerType, params object[] ConstructorArgs)
        {
#if DEBUG
            if (!GenericType.IsGenericType)
                throw new ArgumentException(GenericType.Name + " is not a generic type");
#endif
            System.Type SpecificType = GenericType.MakeGenericType(new Type[] { InnerType });
            return Activator.CreateInstance(SpecificType, ConstructorArgs);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static T FindVisualChild<T>(this DependencyObject obj) where T : DependencyObject
        {
            return FindVisualChildEx<T>(obj, null);
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static T FindVisualChildEx<T>(this DependencyObject obj, Func<T, bool> WhereFunc) where T : DependencyObject
        {
            //http://msdn.microsoft.com/en-us/library/bb613579.aspx
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (
                    child != null && child is T &&
                    (WhereFunc == null || WhereFunc((T)obj))
                    )
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static T FindVisualParent<T>(this DependencyObject obj) where T : DependencyObject
        {
            var Parent = VisualTreeHelper.GetParent(obj);
            if (Parent == null)
                return null;
            else if (Parent is T)
                return Parent as T;
            else
                return FindVisualParent<T>(Parent);
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static DependencyObject FindVisualParent(this DependencyObject obj, string TypeName)
        {
            var Parent = VisualTreeHelper.GetParent(obj);
            if (Parent == null)
                return null;
            else if (Parent.GetType().Name.Equals(TypeName))
                return Parent;
            else
                return FindVisualParent(Parent, TypeName);
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject obj) where T : DependencyObject
        {
            List<T> Items = new List<T>();
            FindVisualChildrenRecurse<T>(obj, Items);
            return Items;
        }
        [System.Diagnostics.DebuggerStepThrough]
        private static void FindVisualChildrenRecurse<T>(DependencyObject obj, IList<T> Collection) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    Collection.Add((T)child);
                else
                {
                    FindVisualChildrenRecurse<T>(child, Collection);
                }
            }
        }
        [System.Diagnostics.DebuggerStepThrough]
        public static IEnumerable<T> FindAllVisualChildren<T>(this DependencyObject obj) where T : DependencyObject
        {
            List<T> Items = new List<T>();
            FindAllVisualChildrenRecurse<T>(obj, Items);
            return Items;
        }
        [System.Diagnostics.DebuggerStepThrough]
        private static void FindAllVisualChildrenRecurse<T>(DependencyObject obj, IList<T> Collection) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    Collection.Add((T)child);
                FindAllVisualChildrenRecurse<T>(child, Collection);
            }
        }


        #endregion

    }
}