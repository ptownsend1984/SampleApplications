using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Globalization;
using ImageOrganizer.Common.Extensions;

namespace ImageOrganizer.ViewModels
{
    [Serializable]
    public class DataAnnotationSupport : IDataErrorInfo
    {

        #region Global Variables

        private readonly object Instance;

        #endregion

        #region Properties

        public bool IsEnabled { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Cancel to prevent the error from appearing
        /// </summary>
        public event EventHandler<PropertyErrorEventArgs> PropertyError;

        #endregion

        #region Constructor

        public DataAnnotationSupport(object Instance)
        {
            if (Instance == null)
                throw new ArgumentNullException("Instance");
            this.Instance = Instance;
            this.IsEnabled = true;
        }

        #endregion

        #region Methods

        #region IDataErrorInfo Members

        public string Error
        {
            get { return this[string.Empty]; }
        }

        public string this[string PropertyName]
        {
            get
            {
                if (!this.IsEnabled)
                    return string.Empty;

                //Inspired by WAFFramework's DataErrorInfoSupport
                var ErrorMessages = new System.Text.StringBuilder();
                var ValidationResults = new List<ValidationResult>();

                if (string.IsNullOrEmpty(PropertyName))
                    Validator.TryValidateObject(this.Instance, new ValidationContext(this.Instance, null, null), ValidationResults, true);
                else
                {
                    var Property = this.Instance.GetType().GetProperty(PropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (Property == null || !Property.CanRead)
                        throw new ArgumentException(PropertyName + " is not a valid property name.", PropertyName);
                    Validator.TryValidateProperty(Property.GetValue(this.Instance, null), new ValidationContext(this.Instance, null, null) { MemberName = PropertyName }, ValidationResults);
                }

                var ErrorMessage = new System.Text.StringBuilder();
                foreach (var Result in ValidationResults)
                    ErrorMessage.AppendInNewLine(Result.ErrorMessage);
                return ErrorMessage.ToString();
            }
        }
        private bool OnPropertyError(string PropertyName, ValidationAttribute Attr)
        {
            PropertyErrorEventArgs e = new PropertyErrorEventArgs(PropertyName, Attr);
            var Handler = PropertyError;
            if (Handler != null)
                Handler(this, e);
            return !e.Cancel;
        }

        #endregion

        #endregion

    }

    public class PropertyErrorEventArgs : CancelEventArgs
    {
        public readonly string PropertyName;
        public readonly ValidationAttribute Attr;

        public PropertyErrorEventArgs(string PropertyName, ValidationAttribute Attr)
        {
            this.PropertyName = PropertyName;
            this.Attr = Attr;
        }
    }

}
