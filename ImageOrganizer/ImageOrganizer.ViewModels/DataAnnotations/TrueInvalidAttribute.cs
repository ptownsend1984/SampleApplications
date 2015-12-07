using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ImageOrganizer.ViewModels.DataAnnotations
{
    /// <summary>
    /// Returns invalid if the value is true
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TrueInvalidAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        public string PropertyName { get; private set; }

        /// <summary>
        /// Specify a boolean property by name to cross-check
        /// </summary>
        /// <param name="PropertyName"></param>
        public TrueInvalidAttribute(string PropertyName)
        {
            this.PropertyName = PropertyName;
        }
        /// <summary>
        /// This should only be used on boolean properties
        /// </summary>
        public TrueInvalidAttribute()
        {
        }

        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(this.PropertyName))
                return base.IsValid(value, validationContext);
            else
            {
                PropertyInfo Property = null;
                var PropertyNameInfo = validationContext.ObjectInstance.GetType().GetProperty(this.PropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (PropertyNameInfo != null && PropertyNameInfo.CanRead && PropertyNameInfo.PropertyType == typeof(bool))
                    Property = PropertyNameInfo;
                if (Property == null)
                    return base.IsValid(value, validationContext);
                else
                {
                    var Value = (bool)Property.GetValue(validationContext.ObjectInstance, null);
                    if (!this.IsValid(Value))
                        return new System.ComponentModel.DataAnnotations.ValidationResult(this.ErrorMessageString);
                    else
                        return null;
                }
            }
        }
        public override bool IsValid(object value)
        {
            return !Convert.ToBoolean(value);
        }

    }
}